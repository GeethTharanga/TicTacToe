// Copyright (c) 2015 Geeth Tharanga
// Under the MIT licence - See licence.txt

using NLog;
using System.IO;
using System.Threading.Tasks;
using T3Network.Util;
using T3Test.Network;
using TicTacToe.Core;
using TicTacToe.Core.Agent;

namespace T3Network
{
    public sealed class NetworkAgent : RemoteStartingAgent
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private Stream input, output;

        private StreamClient cl;
        private bool isStreamClosed;

        public NetworkAgent(Player player, Stream input, Stream output)
            : base(player)
        {
            logger.Info("Creating network agent, {0}, in: {1}, out {2}", player, input, output);
            this.input = input;
            this.output = output;
            cl = new StreamClient(input, output);
            cl.MessageReceived += cl_MessageReceived;
            cl.StreamClosed += cl_StreamClosed;
            cl.StartListening();
        }

        void cl_StreamClosed(object sender, System.EventArgs e)
        {
            isStreamClosed = true;
            if(!board.IsGameEnded)
            {
                board.CancelGame();
                this.CancelGame();
            }
        }

        private void cl_MessageReceived(object sender, Util.NetMessage msg)
        {
            logger.Info("Received message : {0}", msg);
            switch (msg.MessageType)
            {
                case NetMessageType.Connect:
                    break;

                case NetMessageType.Start:
                    {
                        var starter = msg.StartData.Starter;
                        DeclareRemoteStart(starter);
                        board.StartGame(starter);
                    }
                    break;

                case NetMessageType.Move:
                    {
                        var move = new TTTMoveEventArgs(msg.MoveData.Row, msg.MoveData.Col, ThisPlayer);
                        DeclareMove(move);
                    }
                    break;

                case NetMessageType.Cancel:
                    CancelGame();
                    break;

                case NetMessageType.Disconnect:
                    cl.CancelListening();
                    CancelGame();
                    break;

                default:
                    break;
            }
        }

        public override Task InformStart(Player starter)
        {
            if (!isStreamClosed)
            {
                NetMessage msg = new NetMessage { MessageType = NetMessageType.Start };
                msg.StartData = new NetMessage.StartMessageData { Starter = starter };
                cl.SendMessage(msg);
                board.StartGame(starter);
            }
            return Task.FromResult(new object());
        }

        public override Task InformMove(int row, int col, TicTacToe.Core.CellType move)
        {
            if (!isStreamClosed)
            {
                NetMessage msg = new NetMessage { MessageType = NetMessageType.Move };
                msg.MoveData = new NetMessage.MoveMessageData { Col = col, Row = row };
                cl.SendMessage(msg);
                board[row, col] = move;
            }
            return Task.FromResult(new object());
        }

        public override Task InformCancel()
        {
            if (!isStreamClosed)
            {
                cl.SendMessage(new Util.NetMessage { MessageType = Util.NetMessageType.Cancel });
                cl.CancelListening();
            }
            return Task.FromResult(new object());
        }
    }
}