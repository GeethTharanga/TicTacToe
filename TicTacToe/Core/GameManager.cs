// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core.Agent;
using TicTacToe.Util;

namespace TicTacToe.Core
{

    public class GameManager
    {
        private PlayingAgent p1, p2;
        private bool host;

        public GameManager(PlayingAgent p1, PlayingAgent p2,bool isHost)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.host = isHost;

            InitBindings();
        }

        public void StartGame()
        {
            int delayPeriod = Config.GameStartDelay;
            Player startPlayer = (new Random().NextDouble()< 0.5) ? Player.Player1: Player.Player2;

            var task = Task.Delay(delayPeriod).ContinueWith((e) =>
            {
                p1.InformStart(startPlayer == Player.Player1);
                p2.InformStart(startPlayer == Player.Player2);
            });
        }

        private void InitBindings()
        {
            this.p1.OnMove += p1_OnMove;
            this.p2.OnMove += p2_OnMove;

            this.p1.OnCancelGame += p1_OnCancelGame;
            this.p2.OnCancelGame += p2_OnCancelGame;
        }

        private void p2_OnCancelGame(object sender, EventArgs e)
        {
            p1.InformCancel();
        }

        private void p1_OnCancelGame(object sender, EventArgs e)
        {
            p2.InformCancel();
        }

        private void p2_OnMove(object sender, TTTMoveEventArgs e)
        {
            p1.InformMove(e.row, e.col, CellType.Player2);
        }

        void p1_OnMove(object sender, TTTMoveEventArgs e)
        {
            p2.InformMove(e.row, e.col, CellType.Player1);
        }

        CellType PlayerToCellType(Player p)
        {
            return p == Player.Player1 ? CellType.Player1 : CellType.Player2;
        }
    }
}
