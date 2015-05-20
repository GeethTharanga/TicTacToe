// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using T3Network.Util;
using TicTacToe.Core;

namespace T3Network
{
    public class NetworkListener : IDisposable
    {
        public NetworkAgent Agent  { get; private set; }
        public bool IsConnected { get; private set; }

        private Player player;
        private TcpListener listener;

        public event EventHandler OnConnect;

        private bool isDisposed;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public NetworkListener(Player player)
        {
            logger.Info("Creating Network Listener");
            this.player = player;
            isDisposed = false;
        }

        public void StartListening()
        {
            logger.Info("Starting to listen");
            listener = new TcpListener(IPAddress.Any, Config.ServerPort);
            listener.Start();
            listener.BeginAcceptTcpClient(this.Connected, null);

        }

        private void Connected(IAsyncResult result)
        {
            //if listener is closed
            if(isDisposed)
            {
                return;
            }

            logger.Info("Connected");

            var client = listener.EndAcceptTcpClient(result);
            IsConnected = true;
            var stream=client.GetStream();
            Agent = new NetworkAgent(player, stream, stream);
            listener.Server.Dispose();

            if (OnConnect != null)
            {
                OnConnect(this, new EventArgs());
            }
        }

        public void Dispose()
        {
            logger.Info("Disposing");
            if(listener != null)
            {
                isDisposed = true; 
                listener.Stop();
            }
        }
    }
}
