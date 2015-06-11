using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Stats
{
    public class NullRecordRepository : IGameRecordRepository 
    {
        public void SaveRecord(GamePlayRecord record)
        {
        }

        public IEnumerable<GamePlayRecord> GetLastRecords(int maxCount)
        {
            return new GamePlayRecord[0];
        }

        public IDictionary<string, GamePlayStatistics> GetStatistics()
        {
            Dictionary<string, GamePlayStatistics> ret = new Dictionary<string, GamePlayStatistics>();
            GamePlayStatistics stat = new GamePlayStatistics { Losses = 0, Wins = 0 };
            ret["AI Easy"] = stat;
            ret["Human"] = stat;
            ret["AI Hard"] = stat;

            return ret;
        }

        public void ClearHistory()
        {
        }

        public void Dispose()
        {
        }
    }
}
