using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public enum CellType
    {
        Clear, Player1, Player2
    }
    public enum Status
    {
        NotStarted, TurnP1, TurnP2, WonP1, WonP2, Cancelled
    }
    public class Board
    {
        private CellType[,] cells;
        public Status  CurrentStatus { get; private set; }

        public CellType this[int row,int col]
        {
            get { return CellType.Clear; }
            set { }
        }
    }
}
