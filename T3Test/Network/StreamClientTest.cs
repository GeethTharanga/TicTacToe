using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe.Core;
using T3Network.Util;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;

namespace T3Test.Network
{
    [TestClass]
    public class StreamClientTest
    {

        NetMessage test;

        [TestMethod]
        public void StreamRead()
        {
            MemoryStream mem = new MemoryStream();
            IFormatter fmt = new BinaryFormatter();

            NetMessage msg = new NetMessage { MessageType = NetMessageType.Connect };
            msg.ConnectData = new NetMessage.ConnectMessageData { AssignedPlayer = Player.Player2 };
            mem.WriteByte(0xff);
            fmt.Serialize(mem, msg);

            msg = new NetMessage { MessageType = NetMessageType.Start };
            msg.StartData = new NetMessage.StartMessageData { IsStarter = false };
            mem.WriteByte(0xff);
            fmt.Serialize(mem, msg);

            msg = new NetMessage { MessageType = NetMessageType.Move };
            msg.MoveData = new NetMessage.MoveMessageData { Col = 1, Row = 2 };
            mem.WriteByte(0xff);
            fmt.Serialize(mem, msg);

            mem.Seek(0, SeekOrigin.Begin);

            MemoryStream temp = new MemoryStream();
            StreamClient cl = new StreamClient(mem, temp);

            var evt = new AutoResetEvent(false);

            cl.MessageReceived += (s, e) => { test = e; evt.Set(); Thread.Sleep(100); };
            cl.StartListening();

            evt.WaitOne();
            Assert.AreEqual(NetMessageType.Connect,test.MessageType);

            evt.Reset();
            evt.WaitOne();
            Assert.AreEqual(NetMessageType.Start,test.MessageType);

            
            evt.Reset();
            evt.WaitOne();
            Assert.AreEqual(NetMessageType.Move ,test.MessageType);

            cl.CancelListening();
        }

        [TestMethod]
        public void WriteTest()
        {
            MemoryStream input = new MemoryStream(), output = new MemoryStream(); ;

            StreamClient cl = new StreamClient(input, output);

            NetMessage msg = new NetMessage { MessageType = NetMessageType.Connect };
            msg.ConnectData = new NetMessage.ConnectMessageData { AssignedPlayer = Player.Player2 };
            cl.SendMessage(msg);

            msg = new NetMessage { MessageType = NetMessageType.Start };
            msg.StartData = new NetMessage.StartMessageData { IsStarter = false };
            cl.SendMessage(msg);

            msg = new NetMessage { MessageType = NetMessageType.Move };
            msg.MoveData = new NetMessage.MoveMessageData { Col = 1, Row = 2 };
            cl.SendMessage(msg);

            input = output;
            output = new MemoryStream();
            input.Seek(0, SeekOrigin.Begin);
            cl = new StreamClient(input, output);

            var evt = new AutoResetEvent(false);

            cl.MessageReceived += (s, e) => { test = e; evt.Set(); Thread.Sleep(100); };
            cl.StartListening();

            evt.WaitOne();
            Assert.AreEqual(NetMessageType.Connect, test.MessageType);

            evt.Reset();
            evt.WaitOne();
            Assert.AreEqual(NetMessageType.Start, test.MessageType);


            evt.Reset();
            evt.WaitOne();
            Assert.AreEqual(NetMessageType.Move, test.MessageType);

            cl.CancelListening();

        }

    }
}