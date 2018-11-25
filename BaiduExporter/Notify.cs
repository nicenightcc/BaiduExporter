using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BaiduExporter
{
    public class Notify
    {
        private readonly static DateTime startTime = DateTime.Now;
        private Dictionary<string, int> queue = new Dictionary<string, int>();
        private NotifyIcon notifyIcon1;
        private Form form;
        public bool Enabled = true;
        public ContextMenuStrip ContextMenuStrip => notifyIcon1.ContextMenuStrip;

        public Notify(Form form)
        {
            this.form = form;
            notifyIcon1 = new NotifyIcon();
            notifyIcon1.Icon = form.Icon;
            this.notifyIcon1.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon1.Visible = true;
            notifyIcon1.MouseClick += ShowForm;
            notifyIcon1.MouseDoubleClick += ShowForm;
            form.Disposed += (s, e) => { notifyIcon1.Visible = false; notifyIcon1.Dispose(); };
            InitIPC();
        }

        private void ShowForm(object sender = null, MouseEventArgs e = null)
        {
            if (e != null && e.Button != MouseButtons.Left) return;
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
            form.Show();
            form.BringToFront();
            form.Focus();
        }

        public void AddMenuItem(string text, Action<ToolStripMenuItem> onClick)
        {
            var item = new ToolStripMenuItem(text);
            item.Click += (s, e) => { form.Invoke(onClick, s as ToolStripMenuItem); };
            this.notifyIcon1.ContextMenuStrip.Items.Add(item);
        }

        public void AddMenuItem(string text, Action onClick)
        {
            var item = new ToolStripMenuItem(text);
            item.Click += (s, e) => { form.Invoke(onClick); };
            this.notifyIcon1.ContextMenuStrip.Items.Add(item);
        }

        private void InitIPC()
        {
            var ipc = new IPCHelper("aria2channel", "aria2gui").Server();
            ipc.DataReceived += (s, e) =>
            {
                Console.WriteLine(e.Data);
                if (e.Data == "show")
                {
                    ShowForm();
                }
                else
                {
                    if (DateTime.Now - startTime < TimeSpan.FromSeconds(5)) return;
                    var args = e.Data.Split('\n');
                    if (args.Length < 4) return;
                    var file = new FileInfo(args[3]).Name;
                    var id = args[1];
                    var msg = "";
                    switch (args[0])
                    {
                        case "__on_bt_download_complete":
                            msg = "BT任务下载完成：" + file;
                            break;

                        case "__on_download_complete":
                            try { File.Delete(args[3] + ".aria2"); } catch { }
                            msg = "下载完成：" + file;
                            break;

                        case "__on_download_error":
                            msg = "下载出错：" + file;
                            break;

                        case "__on_download_pause":
                            break;

                        case "__on_download_start":
                            if (!queue.Keys.Contains(id))
                            {
                                queue.Add(id, 1);
                                msg = "开始下载：" + file;
                            }
                            break;

                        case "__on_download_stop":
                            break;
                    }
                    if (!this.Enabled || string.IsNullOrEmpty(msg)) return;
                    this.notifyIcon1.ShowBalloonTip(3000, "Aria2", msg, ToolTipIcon.Info);
                }
            };
        }
    }
}
