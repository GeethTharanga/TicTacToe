using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core;
using TicTacToe.Core.Agent;

namespace TicTacToe.UI
{
    public class CellArgs : EventArgs 
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }

    public delegate void CellMoveEventHandler(object sender,CellArgs e);

    public class UIBase
    {
        public Board GameBoard { get; set; }
        public Player ThisPlayer { get; set; }

        private readonly IBasicUI ui;

        public UIBase(IBasicUI ui)
        {
            this.ui = ui;
            ui.UIOnClose += (s, e) => { CancelGame(); };
            ui.UIOnMove += (s, e) => { DeclareMove(e.Row, e.Col); Refresh(); };
            ui.UIStart();
        }

        public event EventHandler OnCancel;
        public event CellMoveEventHandler OnMove;

        public virtual void Refresh()
        {
            ui.UIRefresh(GameBoard, ThisPlayer);
        }

        //methods to invoke events
        protected void CancelGame()
        {
            OnCancel(this, new EventArgs());
        }
        protected void DeclareMove(int row,int col)
        {
            var evt = new CellArgs { Col = col, Row = row };
            OnMove(this, evt);
        }
    }
}
