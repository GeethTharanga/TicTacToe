// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Stats
{
    interface IGameRecordRepository
    {
        void SaveRecord(GamePlayRecord record);
        IEnumerable<GamePlayRecord> GetLastRecords(int maxCount);
        IDictionary<GamePlayOpponent, GamePlayStatistics> GetStatistics();
        void ClearHistory();
    }
}
