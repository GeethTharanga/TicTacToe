using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe.Core.Agent;
using TicTacToe.Core;
using System.Threading;
using TicTacToe.Core.Agent.AI;

namespace T3Test.Core.Agent.AI
{
    /// <summary>
    /// Summary description for AIAgent
    /// </summary>
    [TestClass]
    public class AIAgentTests
    {
        TTTMoveEventArgs arg = null;
        bool cancelled = false;
        object sender = null;

        public AIAgentTests()
        {

        }

        [TestMethod]
        public void TestRandomAgent()
        {
            AIAgent p1 = new AIRandomAgent(Player.Player1), p2 = new AIRandomAgent(Player.Player2);
            p1.ThinkDuration = p2.ThinkDuration = 100;
            TestAgents(p1, p2);
        }

        private void TestAgents(PlayingAgent p1, PlayingAgent p2)
        {
            cancelled = false;
            arg = null;
            sender=null;

            Board board = new Board();
            Random r = new Random();

            Player starter = r.Next(2) > 0 ? Player.Player1 : Player.Player2;
            board.StartGame(starter);

            EventWaitHandle eventOccured = new AutoResetEvent(false);


            Action<object, EventArgs> cancel = (object s, EventArgs e) => { sender = s; cancelled = true; eventOccured.Set(); };
            Action<object, TTTMoveEventArgs> reset = (object s, TTTMoveEventArgs e) => { sender = s; arg = e; eventOccured.Set();  };
            p1.OnCancelGame += new EventHandler(cancel) ;
            p2.OnCancelGame += new EventHandler(cancel);
            p1.OnMove += new TTTMoveEventHandler(reset);
            p2.OnMove += new TTTMoveEventHandler(reset);


            //start
            
            p1.InformStart(starter);
            p2.InformStart(starter);
            while (board.IsGameInProgress)
            {
                eventOccured.WaitOne();
                if(cancelled)
                {
                    Assert.Fail("{0} cancelled", sender);
                }
                Assert.IsNotNull(arg);

                Console.WriteLine(arg);

                CellType cell = arg.player == Player.Player1 ? CellType.Player1 : CellType.Player2;
                board[arg.row,arg.col] = cell;
                Console.WriteLine("Status: {0}",board.CurrentStatus);

                PlayingAgent otherPlayer = p1 == sender ? p2 : p1;
                otherPlayer.InformMove(arg.row, arg.col, cell);
                arg = null; sender = null;
            }
            Assert.IsTrue(board.IsGameEnded);
            Assert.IsFalse(board.CurrentStatus == Status.Cancelled);

            Console.WriteLine();
        }
    }
}
