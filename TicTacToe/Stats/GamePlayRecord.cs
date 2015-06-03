// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Stats
{
    public enum GamePlayOpponent
    {
        AIEasy,
        AIHard,
        Human
    }
    public enum GamePlayResult
    {
        Tied,
        Won,
        Loss
    }

    public sealed class GamePlayRecord
    {
        public DateTime Time { get; set; }
        public GamePlayOpponent Opponent { get; set; }
        public GamePlayResult Result { get; set; }
    }

    public sealed class GamePlayStatistics
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Tied { get; set; }
    }

}
