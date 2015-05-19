using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using T3Network;
using TicTacToe.Core.Agent.AI;
using System.Threading;
using TicTacToe.Core;

namespace T3Test.Network
{
    [TestClass]
    public class NetworkAgentTests
    {
        [TestMethod]
        public void TestNetAgent()
        {
            NetworkAgent host, guest;
            AIRandomAgent hostAi, guestAi;

            NetworkListener listener = new NetworkListener(TicTacToe.Core.Player.Player2);
            NetworkClient client = new NetworkClient("localhost", TicTacToe.Core.Player.Player1);

            client.OnError += (s, e) => { Console.WriteLine(e); };

            listener.StartListening();
            client.Connect();
            Thread.Sleep(100);
            Assert.IsTrue(client.IsConnected && listener.IsConnected);

            host = listener.Agent;
            guest = client.Agent;

            hostAi = new AIRandomAgent(TicTacToe.Core.Player.Player1);
            guestAi = new AIRandomAgent(TicTacToe.Core.Player.Player2);

            hostAi.ThinkDuration = guestAi.ThinkDuration = 1000;

            GameManager hostManager, guestManager;
            hostManager = new GameManager(hostAi, host);  //host
            guestManager = new GameManager(guest, guestAi);  //guest

            hostManager.StartGame();
            //guestManager.StartGame();

            hostAi.OnMove += (s, e) => { Console.WriteLine("hostAI " + e); };
            host.OnMove += (s, e) => { Console.WriteLine("host " + e); };
            guestAi.OnMove += (s, e) => { Console.WriteLine("guestAI " + e); };
            guest.OnMove += (s, e) => { Console.WriteLine("guest " + e); };

            Thread.Sleep(10000);
        }
    }
}
