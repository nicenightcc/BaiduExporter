using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BaiduExporter
{
    public class Aria2Helper : IDisposable
    {
        private Process process = null;
        public event EventHandler<string> DataReceived;
        public Aria2Config Config { get; set; }
        private string[] aria2events = new string[] {
           "__on_bt_download_complete",
           "__on_download_complete",
           "__on_download_error",
           "__on_download_pause",
           "__on_download_start",
           "__on_download_stop"
        };
        private List<string> tempfiles = new List<string>();
        public bool Enabled = true;

        public Aria2Helper()
        {
            this.Config = new Aria2Config();
        }

        private void LoadConfig()
        {
            var cfgfile = Path.Combine(Environment.CurrentDirectory, "aria2c.json");
            if (File.Exists(cfgfile))
            {
                using (var reader = new StreamReader(cfgfile))
                {
                    this.Config = JsonConvert.DeserializeObject<Aria2Config>(reader.ReadToEnd());
                }
            }
            else
            {
                this.Config = new Aria2Config();
            }
        }
        private void SaveConfig()
        {
            var cfgfile = Path.Combine(Environment.CurrentDirectory, "aria2c.json");
            using (var writer = new StreamWriter(cfgfile, false))
            {
                writer.Write(JsonConvert.SerializeObject(this.Config));
            }
        }

        public void Start()
        {
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "aria2c.exe")))
            {
                System.Windows.Forms.MessageBox.Show("程序不完整，未找到aria2c.exe");
                System.Windows.Forms.Application.Exit();
            }
            //try { File.Delete(Config.__log); } catch { }
            if (!File.Exists(Config.__save_session))
                File.Create(Config.__save_session).Close();

            if (!File.Exists(Config.__input_file))
                Config.__input_file = "";

            if (!Directory.Exists(Config.__dir))
                Directory.CreateDirectory(Config.__dir);

            var currentfile = Process.GetCurrentProcess().MainModule.FileName;

            var props = Config.GetType().GetProperties();

            foreach (var ev in props.Where(en => aria2events.Contains(en.Name)))
            {
                var file = ev.GetValue(Config)?.ToString();
                if (!string.IsNullOrEmpty(file))
                {
                    using (var writer = new StreamWriter(file, false))
                        writer.Write(string.Format("\"{0}\" {1} %1 %2 %3", currentfile, ev.Name));
                    tempfiles.Add(file);
                }
            }

            var args = new List<string>();

            foreach (var p in props)
            {
                var v = p.GetValue(Config)?.ToString();
                if (!string.IsNullOrEmpty(v))
                    args.Add(string.Format("{0}={1}", p.Name.Substring(2).Replace('_', '-'), v));
            }

            var conf = Path.Combine(Path.GetTempPath(), "aria2c.conf");
            using (var writer = new StreamWriter(conf, false))
                writer.Write(string.Join("\r\n", args));
            tempfiles.Add(conf);

            process = new Process();
            process.StartInfo.Arguments = " --conf-path=\"" + conf + "\"";
            process.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "aria2c.exe");
            //if (Environment.Is64BitOperatingSystem)
            //    p.StartInfo.FileName = "aria2c64.exe";
            //else
            //    p.StartInfo.FileName = "aria2c32.exe";

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputDataReceived);
            process.Exited += (s, e) =>
            {
                foreach (var f in tempfiles)
                {
                    try { File.Delete(f); } catch { }
                }
                tempfiles.Clear();
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Console.WriteLine(e.Data);
                DataReceived?.Invoke(this, e.Data);
            }
        }

        public void Stop()
        {
            //try
            //{
            //    var url = "http://localhost:6800/jsonrpc?method=Aria2.shutdown&id=0";
            //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //    webRequest.Method = "GET";
            //    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            //    StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            //    string retString = sr.ReadToEnd();
            //    while (!p.HasExited)
            //        Thread.Sleep(100);
            //}
            //catch { }
            if (process != null)
            {
                try { process.Kill(); } catch { }
                process.Close();
                process.Dispose();
            }
            process = null;
        }

        public void Dispose(object sender, EventArgs e)
        {
            Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
