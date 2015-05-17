// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core;
using TicTacToe.Core.Agent;

namespace TicTacToe.UI
{

    public interface IBasicUI
    {
        event EventHandler UIOnClose;
        event CellMoveEventHandler UIOnMove;

        void UIRefresh(Board board, Player thisPlayer);
        void UIStart();
        void UIClose();

    }
}
