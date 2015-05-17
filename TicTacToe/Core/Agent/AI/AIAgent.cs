// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Util;

namespace TicTacToe.Core.Agent.AI
{
    public abstract class AIAgent : PlayingAgent
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public AIAgent(Player thisPlayer):base(thisPlayer)
        {
            AgentName = "AI";
            logger.Info("Creating random AI agent for : {0}", thisPlayer);
        }

        public override Task InformStart(bool firstMove)
        {
            board.StartGame(firstMove ? ThisPlayer : OtherPlayer);
            if (firstMove)
            {
                StartMakingMove();
            }

            return Task.FromResult(true);
        }

        public override Task InformCancel()
        {
            board.CancelGame();
            return Task.FromResult(true);
        }

        public override Task InformMove(int row, int col, CellType move)
        {
            board[row, col] = move;
            if(board.IsGameInProgress)
            {
                StartMakingMove();
            }
            return Task.FromResult(true);
        }

        private void StartMakingMove()
        {
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var move = FindMove();
                        Thread.Sleep(Config.AIThinkDuration);
                        logger.Info("AI Making move {0} . {1},{2}", ThisPlayer, move.row,move.col);
                        DeclareMove(move);
                    }
                    catch (Exception e)
                    {
                        logger.Error("Exception while finding move", e);
                        CancelGame();
                    }
                });
        }

        protected abstract TTTMoveEventArgs FindMove();
    }
}
