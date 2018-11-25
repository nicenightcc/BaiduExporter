using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BaiduExporter
{
    public class InternalWebServer : IDisposable
    {
        private string weburl = "http://localhost/";
        private HttpListener httpListener = null;
        private Dictionary<string, string> mimetype = new Dictionary<string, string>()
        {
            { ".aac", "audio/aac" },
            { ".abw", "application/x-abiword" },
            { ".arc", "application/octet-stream" },
            { ".avi", "video/x-msvideo" },
            { ".azw", "application/vnd.amazon.ebook" },
            { ".bin", "application/octet-stream" },
            { ".bz", "application/x-bzip" },
            { ".bz2", "application/x-bzip2" },
            { ".csh", "application/x-csh" },
            { ".css", "text/css" },
            { ".csv", "text/csv" },
            { ".doc", "application/msword" },
            { ".epub", "application/epub+zip" },
            { ".gif", "image/gif" },
            { ".htm", "text/html" },
            { ".html", "text/html" },
            { ".ico", "image/x-icon" },
            { ".ics", "text/calendar" },
            { ".jar", "application/java-archive" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".js", "application/javascript" },
            { ".json", "application/json" },
            { ".mid", "audio/midi" },
            { ".midi", "audio/midi" },
            { ".mpeg", "video/mpeg" },
            { ".mpkg", "application/vnd.apple.installer+xml" },
            { ".odp", "application/vnd.oasis.opendocument.presentation" },
            { ".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { ".odt", "application/vnd.oasis.opendocument.text" },
            { ".oga", "audio/ogg" },
            { ".ogv", "video/ogg" },
            { ".ogx", "application/ogg" },
            { ".pdf", "application/pdf" },
            { ".ppt", "application/vnd.ms-powerpoint" },
            { ".rar", "application/x-rar-compressed" },
            { ".rtf", "application/rtf" },
            { ".sh", "application/x-sh" },
            { ".svg", "image/svg+xml" },
            { ".swf", "application/x-shockwave-flash" },
            { ".tar", "application/x-tar" },
            { ".tif", "image/tiff" },
            { ".tiff", "image/tiff" },
            { ".ttf", "application/x-font-ttf" },
            { ".vsd", "application/vnd.visio" },
            { ".wav", "audio/x-wav" },
            { ".weba", "audio/webm" },
            { ".webm", "video/webm" },
            { ".webp", "image/webp" },
            { ".woff", "application/x-font-woff" },
            { ".woff2", "application/x-font-woff" },
            { ".xhtml", "application/xhtml+xml" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".xml", "application/xml" },
            { ".xul", "application/vnd.mozilla.xul+xml" },
            { ".zip", "application/zip" },
            { ".3gp", "video/3gpp" },
            { ".3g2", "video/3gpp2" },
            { ".7z", "application/x-7z-compressed" },
            { ".manifest", "text/cache-manifest" },
            { ".eot", "font/eot" },
            { ".txt", "text/plain" },
            { ".png", "image/png" },
         };
        public InternalWebServer(string weburl)
        {
            this.weburl = weburl;
        }
        /// <summary>
        /// 启动本地网页服务器
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            try
            {
                //监听端口
                httpListener = new HttpListener();
                httpListener.Prefixes.Add(weburl);
                httpListener.Start();
                httpListener.BeginGetContext(new AsyncCallback(onWebResponse), httpListener);  //开始异步接收request请求
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 网页服务器相应处理
        /// </summary>
        /// <param name="ar"></param>
        private void onWebResponse(IAsyncResult ar)
        {
            try
            {
                HttpListener httpListener = ar.AsyncState as HttpListener;
                HttpListenerContext context = httpListener.EndGetContext(ar);  //接收到的请求context（一个环境封装体）            

                httpListener.BeginGetContext(new AsyncCallback(onWebResponse), httpListener);  //开始 第二次 异步接收request请求

                HttpListenerRequest request = context.Request;  //接收的request数据
                HttpListenerResponse response = context.Response;  //用来向客户端发送回复

                var path = request.Url.LocalPath.Trim(' ', '/', '\\');
                var file = Path.Combine(Environment.CurrentDirectory, path);

                byte[] responseByte = null;    //响应数据
                if (File.Exists(file))
                {
                    //处理请求文件名的后缀
                    string fileExt = Path.GetExtension(file);
                    if (mimetype.ContainsKey(fileExt))
                        response.ContentType = mimetype[fileExt];

                    responseByte = File.ReadAllBytes(file);
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    responseByte = Encoding.UTF8.GetBytes(request.Url.AbsoluteUri + "\n404 Not Found!");
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }

                response.Cookies = request.Cookies; //处理Cookies
                response.ContentEncoding = Encoding.UTF8;
                using (Stream output = response.OutputStream)  //发送回复
                {
                    output.Write(responseByte, 0, responseByte.Length);
                }
            }
            catch { }
        }

        public void Stop()
        {
            if (httpListener != null)
                httpListener.Stop();
            httpListener = null;
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
