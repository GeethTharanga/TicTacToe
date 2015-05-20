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
        bool completed;
        [TestMethod]
        public void TestGameManager()
        {
            AIAgent p1 = new AIRandomAgent(Player.Player1), p2 = new AIRandomAgent(Player.Player2);
            p1.ThinkDuration = p2.ThinkDuration = 60;
            GameManager manager = new GameManager(p1, p2);
            manager.GameStartDelay = 5;

            AutoResetEvent ev = new AutoResetEvent(false);
            completed = false;
            manager.OnGameEnd += (s, e) => { completed = true; ev.Set(); };
            manager.StartGame();

            ev.WaitOne(2000);

            Assert.IsTrue(completed);
            
        }
    }
}
