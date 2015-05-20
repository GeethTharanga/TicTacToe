// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.UI;

namespace TicTacToe.Core.Agent.Human
{
    public sealed class HumanAgent : PlayingAgent 
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private UIBase ui;

        public HumanAgent(Player player,UIBase ui):base(player)
        {
            logger.Info("Setting up human agent- {0}",player);
            ui.ThisPlayer = player;
            ui.GameBoard = board;

            ui.OnCancel += ui_OnCancel;
            ui.OnMove += ui_OnMove;

            this.ui = ui;
        }

        public override Task InformStart(Player starter)
        {
            board.StartGame(starter);
            ui.Refresh();

            return Task.FromResult(true);
        }

        public override Task InformMove(int row, int col, CellType move)
        {
            board[row, col] = move;
            ui.Refresh();
            return Task.FromResult(true);
        }

        public override Task InformCancel()
        {
            board.CancelGame();
            ui.Refresh();
            return Task.FromResult(true);
        }

        void ui_OnMove(object sender, CellArgs e)
        {
            logger.Info("Received move {0} from ui", e);

            Status reqStatus = ThisPlayer == Player.Player1 ? Status.TurnP1 : Status.TurnP2 ;
            if(board.IsGameInProgress && reqStatus == board.CurrentStatus )
            {
                TTTMoveEventArgs arg = new TTTMoveEventArgs(e.Row, e.Col, player);
                DeclareMove(arg);
            }
            else
            {
                logger.Error("Received move ( {0} ) from ui while in state {1}, player : {2}", e, board.CurrentStatus, ThisPlayer);
                throw new InvalidOperationException("Received movefrom ui out of turn");
            }
        }

        void ui_OnCancel(object sender, EventArgs e)
        {
            if (!board.IsGameEnded)
            {
                CancelGame();
            }
        }


    }
}
