using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using T3Network.Util;

namespace T3Network
{
    public class NetworkListener : IDisposable
    {
        public NetworkAgent Agent  { get; private set; }
        public bool IsConnected { get; private set; }


        private TcpListener listener;

        public void StartListening()
        {
            listener = new TcpListener(IPAddress.Any, Config.ServerPort);
            listener.BeginAcceptTcpClient(this.Connected, null);

        }

        private void Connected(IAsyncResult result)
        {
            Console.WriteLine("Connected");
        }



        public void Dispose()
        {
            if(listener != null)
            {
                listener.Server.Dispose();
            }
        }
    }
}
