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
        public Board board { get; private set; }
        public void CancelGame(string reason)
        {

        }
    }
}
