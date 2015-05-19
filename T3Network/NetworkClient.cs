using NLog;
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

        public NetworkAgent Agent { get; private set; }

        public event EventHandler OnConnect;
        public event EventHandler<string> OnError;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public NetworkClient(string ip,Player player)
        {
            logger.Info("Creating new Network client: {0} {1}", ip, player);
            this.ip = ip;
            this.player = player;
        }

        public void Connect()
        {
            logger.Info("Connecting...");
            int port = Config.ServerPort;
            cl = new TcpClient();
            cl.BeginConnect(ip, port, this.Connected, null);
        }

        private void Connected(IAsyncResult result)
        {
            logger.Info("Connected/Error");
            try
            {
                cl.EndConnect(result);
                logger.Info("Connected to host");
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
                logger.Error("Error connecting", ex);
                if(OnError != null)
                {
                    OnError(this, ex.Message);
                }
            }
        }

        public void Dispose()
        {
            logger.Info("Disposing client");
            if(cl != null)
            {
                cl.Close();
            }
        }
    }
}