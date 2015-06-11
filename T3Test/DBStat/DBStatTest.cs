using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using T3DBStatRepository;
using TicTacToe.Stats;

namespace T3Test.DBStat
{
    [TestClass]
    public class DBStatTest
    {
        [TestMethod]
        public void DBTest1()
        {
            DBRecordRepository rep = new DBRecordRepository();
            rep.ClearHistory();

            rep.SaveRecord(new GamePlayRecord { Opponent ="HUMAN", Result = GamePlayResult.Won, Time = DateTime.Now });
            rep.SaveRecord(new GamePlayRecord { Opponent = "EASY", Result = GamePlayResult.Tied, Time = DateTime.Now.AddMinutes(1) });
            rep.SaveRecord(new GamePlayRecord { Opponent = "HARD", Result = GamePlayResult.Tied, Time = DateTime.Now.AddMinutes(10) });

            rep.Dispose();
 
            rep = new DBRecordRepository();

            var results = rep.GetLastRecords(10);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("HARD", results.ToArray()[0].Opponent);

        }
    }
}
