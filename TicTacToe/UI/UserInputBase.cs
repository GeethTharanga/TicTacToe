using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core;
using TicTacToe.Core.Agent;

namespace TicTacToe.UI
{
    public abstract class UserInputBase
    {
        protected readonly Board board;
        protected readonly Player thisPlayer;

        public UserInputBase(Board board,Player player)
        {
            this.board = board;
            this.thisPlayer = player;
        }

        public abstract void Refresh();

        public event EventHandler OnCancel;
        public event TTTMoveEventHandler OnMove;

        protected void CancelGame()
        {
            OnCancel(this, new EventArgs());
        }
        protected void DeclareMove(int row,int col)
        {
            var evt = new TTTMoveEventArgs(row, col, thisPlayer);
            OnMove(this, evt);
        }
    }
}
