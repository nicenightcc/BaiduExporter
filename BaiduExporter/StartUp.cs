using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace BaiduExporter
{
    public class StartUp
    {
        public void Start()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            var cefsettings = new CefSettings()
            {
                Locale = "zh-CN",
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.CurrentDirectory, "cache"),
                IgnoreCertificateErrors = true,
                LogSeverity = LogSeverity.Disable,
            };
            //NOTE: The following function will set all three params
            //settings.SetOffScreenRenderingBestPerformanceArgs();
            cefsettings.CefCommandLineArgs.Add("disable-gpu", "1");
            cefsettings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            cefsettings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");
            cefsettings.CefCommandLineArgs.Add("enable-npapi", "1");
            cefsettings.CefCommandLineArgs.Add("register-pepper-plugins", "PepperFlash/pepflashplayer.dll;application/x-shockwave-flash");

            cefsettings.CefCommandLineArgs.Add("disable-gpu-vsync", "1"); //Disable Vsync

            //Disables the DirectWrite font rendering system on windows.
            //Possibly useful when experiencing blury fonts.
            //cefsettings.CefCommandLineArgs.Add("disable-direct-write", "1");

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(cefsettings, performDependencyCheck: true, browserProcessHandler: null);

            Application.Run(new MainForm());
        }
    }
}
