using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiduExporter
{
    public partial class MainForm : Form
    {
        private ChromiumWebBrowser webBrowser1 = null;
        //private string weburl = "http://localhost:6900/";
        private string weburl = "https://nicenight.cc/app/baidu";
        private string localurl = "file:///" + Environment.CurrentDirectory.Replace("\\", "/") + "/";
        private string webui = "AriaNg";
        private Aria2Helper aria2;
        private InternalWebServer server;
        private Notify notify;
        private Config config;
        private LogForm log;

        public MainForm()
        {
            InitializeComponent();

            config = new Config();
            if (config.GetInt("height") > 0)
                this.Height = config.GetInt("height");
            if (config.GetInt("width") > 0)
                this.Width = config.GetInt("width");
            if (!string.IsNullOrEmpty(config.Get("webui")))
                this.webui = config.Get("webui");

            server = new InternalWebServer(weburl);
            aria2 = new Aria2Helper();
            this.Disposed += server.Dispose;
            this.Disposed += aria2.Dispose;

            if (!string.IsNullOrEmpty(config.Get("aria2c")))
                aria2.Config = JsonConvert.DeserializeObject<Aria2Config>(config.Get("aria2c"));

            if (config.Get("aria2") != "false")
                aria2.Start();
            //server.Start();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs ea)
        {
            timer1.Enabled = false;

            notify = new Notify(this);
            if (config.Get("notify") == "false")
                notify.Enabled = false;
            notify.AddMenuItem("打开下载目录", () => { Process.Start("explorer.exe", this.aria2.Config.__dir); });
            notify.AddMenuItem(notify.Enabled ? "关闭通知" : "开启通知", (s) =>
            {
                notify.Enabled = !notify.Enabled;
                s.Text = notify.Enabled ? "关闭通知" : "开启通知";
            });
            notify.AddMenuItem(aria2.Enabled ? "关闭Aria2" : "开启Aria2", (s) =>
            {
                aria2.Enabled = !aria2.Enabled;
                s.Text = aria2.Enabled ? "关闭Aria2" : "开启Aria2";
                if (aria2.Enabled) aria2.Start(); else aria2.Stop();
            });
            notify.AddMenuItem("设置下载目录", () =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowDialog();
                this.aria2.Config.__dir = dialog.SelectedPath;
                aria2.Stop();
                aria2.Start();
            });
            notify.AddMenuItem("显示日志", () =>
            {
                if (log == null)
                    log = new LogForm();
                aria2.DataReceived += log.Log;
                log.Show();
            });
            notify.AddMenuItem("退出", () => { closing = true; Application.Exit(); });

            var tabPage_home = new BrowserTabPage { Name = "tabPage_home", Text = "主页", Url = "http://pan.baidu.com", MainPage = true };
            this.tabControl1.TabPages.Add(tabPage_home);
            this.webBrowser1 = tabPage_home.Browser;
            webBrowser1.FrameLoadEnd += (s, e) => { SetPageScript(webBrowser1); };
            tabPage_home.Load();

            var tabPage_aria = new BrowserTabPage { Name = "tabPage_aria", Text = "下载", Url = localurl + "/resource/" + webui + "/index.html", MainPage = true };
            this.tabControl1.TabPages.Add(tabPage_aria);
            tabPage_aria.Browser.TitleChanged += (a, e) => { this.Invoke(() => { this.Text = e.Title; }); };
            tabPage_aria.Load();
        }

        private async Task<string> GetCookies()
        {
            var cookieManager = Cef.GetGlobalCookieManager();
            var cookies = await cookieManager.VisitAllCookiesAsync();
            return JsonConvert.SerializeObject(cookies);
        }

        private void SetPageScript(ChromiumWebBrowser browser)
        {
            this.Invoke(() =>
            {
                browser.GetBrowser().MainFrame.EvaluateScriptAsync("script=document.createElement('script');script.id='chrome';script.src='" + weburl + "/chrome/chrome.js';document.body.appendChild(script);").Wait();
            });

            Task.Run(() =>
            {
                var cookies = GetCookies().Result;
                this.Invoke(() => browser.GetBrowser().MainFrame.EvaluateScriptAsync("window.cookies=" + cookies + ";").Wait());
            });
        }

        private void btn_open_Click(object sender, EventArgs ea)
        {
            if (txt_url.Text.Length == 0) return;
            this.txt_url.SelectionStart = 0;
            if (!txt_url.Text.StartsWith("http")) txt_url.Text = "http://" + txt_url.Text;

            var tabPage = new BrowserTabPage { Name = "tabPage_share" + new Random().Next().ToString(), Text = "打开链接", Url = txt_url.Text };
            this.tabControl1.TabPages.Add(tabPage);
            var browser = tabPage.Browser;
            browser.TitleChanged += (s, e) =>
            {
                this.Invoke(() => { tabPage.Text = e.Title.Length < 10 ? e.Title : e.Title.Substring(0, 10); });
            };
            browser.FrameLoadEnd += (s, e) => { SetPageScript(browser); };
            browser.AddressChanged += (s, e) =>
            {
                this.Invoke(() =>
                {
                    if (this.tabControl1.SelectedTab == tabPage)
                    {
                        this.txt_url.Text = e.Address;
                        this.txt_url.SelectionStart = 0;
                    }
                });
            };
            tabPage.Load();
            this.tabControl1.SelectedTab = tabPage;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            var tabPage = this.tabControl1.SelectedTab as BrowserTabPage;
            if (!tabPage.MainPage)
                this.tabControl1.TabPages.Remove(tabPage);
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            var tabPage = this.tabControl1.SelectedTab as BrowserTabPage;
            tabPage.Browser.Reload(true);
        }

        private void btn_debug_Click(object sender, EventArgs e)
        {
            var tabPage = this.tabControl1.SelectedTab as BrowserTabPage;
            tabPage.Browser.GetBrowser().ShowDevTools();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabPage = this.tabControl1.SelectedTab as BrowserTabPage;
            if (tabPage.MainPage)
            {
                this.btn_close.Enabled = false;
                this.txt_url.Text = "";
            }
            else
            {
                this.btn_close.Enabled = true;
                this.txt_url.Text = tabPage.Browser.Address;
            }
        }

        private bool closing = false;
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !closing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                aria2.Stop();
                server.Stop();
                config.Set("height", this.Height);
                config.Set("width", this.Width);
                config.Set("aria2c", JsonConvert.SerializeObject(this.aria2.Config));
                config.Set("notify", notify.Enabled);
                config.Set("aria2", aria2.Enabled);
                config.Set("webui", this.webui);
                config.Save();
            }
        }
    }
}
