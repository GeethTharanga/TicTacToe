using System;
using System.Net.Sockets;
using T3Network.Util;
using TicTacToe.Core;
using TicTacToe.Core.Agent;

namespace T3Network
{
    public class NetworkClient : IDisposable 
    {
        private string ip;
        private TcpClient cl;
        private Player player;

        public bool IsConnected { get; private set; }

        public PlayingAgent Agent { get; private set; }

        public event EventHandler OnConnect;
        public event EventHandler<string> OnError;

        public NetworkClient(string ip,Player player)
        {
            this.ip = ip;
            this.player = player;
        }

        public void Connect()
        {
            int port = Config.ServerPort;
            cl = new TcpClient();
            cl.BeginConnect(ip, port, this.Connected, null);
        }

        private void Connected(IAsyncResult result)
        {
            try
            {
                cl.EndConnect(result);
                var stream = cl.GetStream();

                Agent = new NetworkAgent(player, stream, stream);
                IsConnected = true;

                if (OnConnect != null)
                {
                    OnConnect(this, new EventArgs());
                }
            }
            catch(SocketException  ex)
            {
                if(OnError != null)
                {
                    OnError(this, ex.Message);
                }
            }
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