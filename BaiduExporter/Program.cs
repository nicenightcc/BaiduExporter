using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace BaiduExporter
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew = true;
            var filename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            using (Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out createdNew))
            {
                if (createdNew)
                {
                    /**
                    * 当前用户是管理员的时候，直接启动应用程序
                    * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
                    */
                    //获得当前登录的Windows用户标示
                    System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                    System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                    //判断当前登录用户是否为管理员
                    if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                    {
                        //如果是管理员，则直接运行
                        new StartUp().Start();
                    }
                    else
                    {
                        //创建启动对象
                        var p = new System.Diagnostics.Process();
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.WorkingDirectory = new FileInfo(filename).DirectoryName;
                        p.StartInfo.FileName = filename;
                        //设置启动动作,确保以管理员身份运行
                        p.StartInfo.Verb = "runas";
                        try { p.Start(); } catch { }
                        //退出
                    }
                }
                else
                {
                    try
                    {
                        var client = new IPCHelper("aria2channel", "aria2gui").Client();
                        if (args.Length > 0)
                        {
                            client.Send(string.Join("\n", args));
                        }
                        else
                        {
                            client.Send("show");
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            Environment.Exit(0);
            return;
        }
    }
}
