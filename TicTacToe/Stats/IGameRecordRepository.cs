// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Stats
{
    public interface IGameRecordRepository : IDisposable
    {
        void SaveRecord(GamePlayRecord record);
        IEnumerable<GamePlayRecord> GetLastRecords(int maxCount);
        IDictionary<string, GamePlayStatistics> GetStatistics();
        void ClearHistory();

    }
}
