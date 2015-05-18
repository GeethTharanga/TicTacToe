using System;
using System.Net.Sockets;
using T3Network.Util;
using TicTacToe.Core.Agent;

namespace T3Network
{
    public class NetworkClient : IDisposable 
    {
        private string ip;
        private TcpClient cl;

        public bool IsConnected { get; private set; }

        public PlayingAgent Agent { get; private set; }

        public NetworkClient(string ip)
        {
            this.ip = ip;
        }

        public void Connect()
        {
            int port = Config.ServerPort;
            cl = new TcpClient();
            cl.BeginConnect(ip, port, this.Connected, null);
        }

        private void Connected(IAsyncResult result)
        {
            Console.WriteLine("connected to server");
        }

        public void Dispose()
        {
            if(cl != null)
            {
                cl.Close();
            }
        }
    }
}