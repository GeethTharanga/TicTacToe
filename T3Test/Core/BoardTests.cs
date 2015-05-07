using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe.Core;

namespace T3Test.Core
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void TestInitialization()
        {
            Board b = new Board();
            Assert.AreEqual(Status.NotStarted,b.CurrentStatus);

            Assert.AreEqual( CellType.Clear,b[0, 0]);
            Assert.AreEqual(CellType.Clear,b[1, 2]);

            b.StartGame(Player.Player1);
            Assert.AreEqual(Status.TurnP1,b.CurrentStatus);

        }

        [TestMethod]
        public void TestAsigning()
        {
            Board b = new Board();
            b.StartGame(Player.Player1);
            
            b[1, 1] = CellType.Player1;

            Assert.AreEqual( CellType.Player1,b[1, 1]);
            Assert.AreEqual(Status.TurnP2,b.CurrentStatus);
            Assert.AreEqual(1,b.NumCheckedCells);

            Assert.IsTrue(b.IsGameInProgress);
            Assert.IsFalse(b.IsGameEnded);

            try
            {
                b[1, 1] = CellType.Player2;
                Assert.Fail("No exception - overwrite");
            }
            catch (InvalidOperationException )
            {      }

            try
            {
                b[3, 1] = CellType.Player2;
                Assert.Fail("No exception - out of range");
            }
            catch (ArgumentOutOfRangeException )
            {}

            try
            {
                b[2, 1] = CellType.Player1;
                Assert.Fail("No exception - wrong player");
            }
            catch (InvalidOperationException)
            { }

            //cancelling
            b.CancelGame();
            Assert.IsTrue(b.IsGameEnded);
            Assert.IsFalse(b.IsGameInProgress);
            Assert.AreEqual(Status.Cancelled, b.CurrentStatus);
            try
            {
                b[0, 2] = CellType.Player2;
                Assert.Fail("No exception - cancelled game");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void TestWinning()
        {
            Board b = new Board();
            b.StartGame(Player.Player2);

            Assert.IsTrue(b.IsGameInProgress);
            b[1, 1] = CellType.Player2;
            b[1, 2] = CellType.Player1;
            Assert.IsTrue(b.IsGameInProgress);
            b[0, 0] = CellType.Player2;
            b[0, 2] = CellType.Player1;
            Assert.IsTrue(b.IsGameInProgress);
            Assert.IsFalse(b.IsGameEnded);
            b[2, 2] = CellType.Player2;

            Assert.IsFalse(b.IsGameInProgress);
            Assert.IsTrue(b.IsGameEnded);
            Assert.AreEqual(Status.WonP2, b.CurrentStatus);

            try
            {
                b[2, 0] = CellType.Player1;
                Assert.Fail("No exception- doing moves after winning");
            }
            catch (InvalidOperationException)
            { }
        }

        [TestMethod]
        public void TestTie()
        {
            Board b = new Board();
            b.StartGame(Player.Player2);

            b[0, 0] = CellType.Player2;
            b[0, 1] = CellType.Player1;
            b[0, 2] = CellType.Player2;
            b[1, 1] = CellType.Player1;
            b[1, 0] = CellType.Player2;
            b[2, 0] = CellType.Player1;
            b[1, 2] = CellType.Player2;
            b[2, 2] = CellType.Player1;

            Assert.IsTrue(b.IsGameInProgress);
            b[2, 1] = CellType.Player2;
            Assert.IsTrue(b.IsGameEnded);
            Assert.IsFalse(b.IsGameInProgress);
            Assert.AreEqual(Status.Tie, b.CurrentStatus);
            
        }

    }
}
