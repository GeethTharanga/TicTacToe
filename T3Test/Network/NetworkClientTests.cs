using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using T3Network;

namespace T3Test.Network
{
    [TestClass]
    public class NetworkClientTests
    {
        bool flag1, flag2;


        [TestMethod]
        public void TestNetworkConnect()
        {
            NetworkClient cl = new NetworkClient("127.0.0.1", TicTacToe.Core.Player.Player2);
            NetworkListener listener = new NetworkListener(TicTacToe.Core.Player.Player1);

            cl.OnConnect += (s, e) => { flag1 = true; };
            listener.OnConnect += (s,e) => { flag2=true;};
            
            flag1 = flag2 = false;

            listener.StartListening();
            cl.Connect();

            Thread.Sleep(200);

            listener.Dispose();

            Assert.IsTrue(cl.IsConnected);
            Assert.IsTrue(listener.IsConnected);
            Assert.IsNotNull(cl.Agent);
            Assert.IsNotNull(listener.Agent);
            Assert.IsTrue(flag1);
            Assert.IsTrue(flag2);
        }

        [TestMethod]
        public void TestListenerCancel()
        {
            flag1 = flag2 = false;
            NetworkListener listener = new NetworkListener(TicTacToe.Core.Player.Player1);
            listener.OnConnect += (s, e) => { flag1 = true; };
            listener.StartListening();
            Thread.Sleep(100);
            listener.Dispose();

            Assert.IsFalse(flag1);
            Assert.IsNull(listener.Agent);
            Assert.IsFalse(listener.IsConnected);

            NetworkClient cl = new NetworkClient("127.0.0.1", TicTacToe.Core.Player.Player2);
            cl.OnConnect += (s, e) => { flag2 = true; };
            cl.Connect();
            Thread.Sleep(100);
            Assert.IsFalse(flag2);
            Assert.IsNull(cl.Agent);
            Assert.IsFalse(cl.IsConnected);
        }

        [TestMethod]
        public void TestClientCancel()
        {
            flag1 = flag2 = false;
            NetworkClient cl = new NetworkClient("www.9gag.com", TicTacToe.Core.Player.Player1);
            cl.OnConnect += (s, e) => { flag1 = true; };

            cl.Connect();
            cl.Dispose();

            Assert.IsFalse(flag1);
            Assert.IsNull(cl.Agent);
            Assert.IsFalse(cl.IsConnected);
        }

        [TestMethod]
        public void TestClientError()
        {
            flag1 = flag2 = false;
            NetworkClient cl = new NetworkClient("999.222.333.111", TicTacToe.Core.Player.Player1);
            cl.OnConnect += (s, e) => { flag1 = true; };
            cl.OnError += (s, e) => { flag2 = true; };
            cl.Connect();
            Thread.Sleep(200);


            Assert.IsFalse(flag1);
            Assert.IsTrue(flag2);
            Assert.IsNull(cl.Agent);
            Assert.IsFalse(cl.IsConnected);

            cl.Dispose();
        }
    }
}
