using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe.Core.Agent.AI;
using TicTacToe.Core;
using System.Threading;

namespace T3Test.Core
{
    [TestClass]
    public class GameManagerTests
    {
        [TestMethod]
        public void TestGameManager()
        {
            AIAgent p1 = new AIRandomAgent(Player.Player1), p2 = new AIRandomAgent(Player.Player2);
            p1.ThinkDuration = p2.ThinkDuration = 10;
            GameManager manager = new GameManager(p1, p2);
            manager.GameStartDelay = 5;

            AutoResetEvent ev = new AutoResetEvent(false);
            bool completed = false;
            manager.OnGameEnd += (s, e) => { ev.Set(); completed=true;};
            manager.StartGame();

            ev.WaitOne(1000);

            Assert.IsTrue(completed);
            
        }
    }
}
