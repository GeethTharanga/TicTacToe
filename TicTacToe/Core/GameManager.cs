// Copyright (c) 2015 Geeth Tharanga
// Under the MIT licence - See licence.txt

using NLog;
using System;
using System.Threading.Tasks;
using TicTacToe.Core.Agent;
using TicTacToe.Util;

namespace TicTacToe.Core
{
    public class GameManager
    {
        private PlayingAgent p1, p2;
        private bool host;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private Board board;

        public int GameStartDelay { get; set; }
        public bool IsGameEnded
        {
            get
            {
                return board.IsGameEnded;
            }
        }

        public event EventHandler OnGameEnd;

        public GameManager(PlayingAgent p1, PlayingAgent p2, bool isHost)
        {
            logger.Info("Creating game manager {0}, {1},host: {2}", p1, p2, isHost);
            this.p1 = p1;
            this.p2 = p2;
            this.host = isHost;

            InitBindings();
            GameStartDelay = Config.GameStartDelay;
            board = new Board();
        }

        public void StartGame()
        {
            if (host)
            {
                int delayPeriod = GameStartDelay;
                Player startPlayer = (new Random().NextDouble() < 0.5) ? Player.Player1 : Player.Player2;

                var task = Task.Delay(delayPeriod).ContinueWith((e) =>
                {
                    p1.InformStart(startPlayer == Player.Player1);
                    p2.InformStart(startPlayer == Player.Player2);
                });
                board.StartGame(startPlayer);
            }
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
            board.CancelGame();
            DeclareGameEnded();
        }

        private void p1_OnCancelGame(object sender, EventArgs e)
        {
            p2.InformCancel();
            board.CancelGame();
            DeclareGameEnded();
        }

        private void p2_OnMove(object sender, TTTMoveEventArgs e)
        {
            p1.InformMove(e.row, e.col, CellType.Player2);
            board[e.row, e.col] = CellType.Player2;
            if (this.IsGameEnded)
            {
                DeclareGameEnded();
            }
        }

        private void p1_OnMove(object sender, TTTMoveEventArgs e)
        {
            p2.InformMove(e.row, e.col, CellType.Player1);
            board[e.row, e.col] = CellType.Player1;
            if(this.IsGameEnded)
            {
                DeclareGameEnded();
            }
        }

        private CellType PlayerToCellType(Player p)
        {
            return p == Player.Player1 ? CellType.Player1 : CellType.Player2;
        }

        private void DeclareGameEnded()
        {
            if(OnGameEnd != null)
            {
                OnGameEnd(this, new EventArgs());
            }
        }
    }
}