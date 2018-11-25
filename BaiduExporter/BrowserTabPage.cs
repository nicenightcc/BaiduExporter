using System.Windows.Forms;

namespace BaiduExporter
{
    public class BrowserTabPage : TabPage
    {
        public string Url { get; set; }
        public CefSharp.WinForms.ChromiumWebBrowser Browser { get; }
        public bool MainPage { get; set; }
        public BrowserTabPage()
        {
            Browser = new CefSharp.WinForms.ChromiumWebBrowser("about:blank");
            Browser.Dock = DockStyle.Fill;
            this.Controls.Add(Browser);
        }
        public void Load(string url = null)
        {
            if (url == null)
                Browser.Load(this.Url);
            else
                Browser.Load(url);
        }
    }
}
