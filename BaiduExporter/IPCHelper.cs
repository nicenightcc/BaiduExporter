using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading.Tasks;

namespace BaiduExporter
{
    public class IPCHelper
    {
        private class IPCObject : MarshalByRefObject
        {
            public void Send(string data)
            {
                IPCHandler.Receive(data);
            }
        }
        public class IPCEventArgs : EventArgs
        {
            public string Data { get; set; }
        }
        private class IPCHandler
        {
            private IPCHandler() { }
            public event EventHandler<IPCEventArgs> DataReceived;
            private static IPCHandler handler = null;
            public static IPCHandler GetHandler()
            {
                if (handler == null)
                    handler = new IPCHandler();
                return handler;
            }
            public static void Receive(string data)
            {
                handler.DataReceived(handler, new IPCEventArgs { Data = data });
            }
        }
        private IChannel channel = null;
        private IPCObject myObj = null;
        public event EventHandler<IPCEventArgs> DataReceived
        {
            add { IPCHandler.GetHandler().DataReceived += value; }
            remove { IPCHandler.GetHandler().DataReceived -= value; }
        }
        public string port;
        public string url;
        public IPCHelper(string port, string url)
        {
            this.port = port; this.url = url;
        }
        public IPCHelper Server()
        {
            Task.Factory.StartNew(() =>
            {
                channel = new IpcChannel(port);
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(IPCObject), url, WellKnownObjectMode.Singleton);
            });
            return this;
        }
        public IPCHelper Client()
        {
            channel = new IpcChannel();
            ChannelServices.RegisterChannel(channel, false);
            myObj = (IPCObject)Activator.GetObject(typeof(IPCObject), $"ipc://{port}/{url}");
            return this;
        }
        public void Send(string data)
        {
            if (myObj != null)
                myObj.Send(data);
        }
    }
}
