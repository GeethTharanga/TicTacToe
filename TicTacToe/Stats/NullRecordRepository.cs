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

        public IDictionary<GamePlayOpponent, GamePlayStatistics> GetStatistics()
        {
            Dictionary<GamePlayOpponent, GamePlayStatistics> ret = new Dictionary<GamePlayOpponent, GamePlayStatistics>();
            GamePlayStatistics stat = new GamePlayStatistics { Losses = 0, Wins = 0 };
            ret[GamePlayOpponent.AIEasy] = stat;
            ret[GamePlayOpponent.AIHard] = stat;
            ret[GamePlayOpponent.Human] = stat;

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
