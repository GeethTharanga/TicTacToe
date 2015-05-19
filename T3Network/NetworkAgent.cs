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
    public class NetworkAgent : RemoteStartingAgent 
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private Stream input, output;

        private StreamClient cl;

        public NetworkAgent(Player player, Stream input, Stream output)
            : base(player)
        {
            logger.Info("Creating network agent, {0}, in: {1}, out {2}", player, input, output);
            this.input = input;
            this.output = output;
            cl = new StreamClient(input, output);
            cl.MessageReceived += cl_MessageReceived;
            cl.StartListening();
        }

        private void cl_MessageReceived(object sender, Util.NetMessage msg)
        {
            logger.Info("Received message : {0}", msg);
            switch (msg.MessageType)
            {
                case NetMessageType.Connect:
                    break;
                case NetMessageType.Start:
                    DeclareRemoteStart(msg.StartData.IsStarter);
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

        public override Task InformStart(bool firstMove)
        {
            NetMessage msg = new NetMessage { MessageType = NetMessageType.Start };
            msg.StartData = new NetMessage.StartMessageData { IsStarter = firstMove };
            cl.SendMessage(msg);
            return Task.FromResult(new object());
        }

        public override Task InformMove(int row, int col, TicTacToe.Core.CellType move)
        {
            NetMessage msg = new NetMessage { MessageType = NetMessageType.Move };
            msg.MoveData = new NetMessage.MoveMessageData { Col = col, Row = row };
            cl.SendMessage(msg);
            return Task.FromResult(new object());
        }

        public override Task InformCancel()
        {
            cl.SendMessage(new Util.NetMessage { MessageType = Util.NetMessageType.Cancel });
            cl.CancelListening();
            return Task.FromResult(new object());
        }
    }
}