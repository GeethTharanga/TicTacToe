using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{

    public class GameState
    {
        public GameState Status { get; private set; }
        public CellType[,] Board { get; private set; }
    }
}
