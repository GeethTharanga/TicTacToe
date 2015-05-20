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

        public Status GameStatus
        {
            get
            {
                return board.CurrentStatus;
            }
        }

        public event EventHandler OnGameEnd;

        /// <summary>
        /// Create new game manager as host
        /// </summary>
        /// <param name="p1">Player 1</param>
        /// <param name="p2">Player 2</param>
        public GameManager(PlayingAgent p1, PlayingAgent p2)
        {
            logger.Info("Creating game manager  {0}, {1}", p1, p2);
            this.p1 = p1;
            this.p2 = p2;
            this.host = true;

            InitBindings();
            GameStartDelay = Config.GameStartDelay;
            board = new Board();
        }

        /// <summary>
        /// Create a game manager with remote start
        /// </summary>
        /// <param name="p1">Remote agent - player 1</param>
        /// <param name="p2">Player 2</param>
        public GameManager(RemoteStartingAgent p1, PlayingAgent p2)
            : this((PlayingAgent)p1, p2)
        {
            this.host = false;
            p1.OnRemoteStart += RemoteStarted;
        }

        public void StartGame()
        {
            if (!host)
            {
                logger.Error("Tried to start a guest game");
                throw new InvalidOperationException("Tried to start a guest game");
            }
            int delayPeriod = GameStartDelay;
            Player startPlayer = (new Random().NextDouble() < 0.5) ? Player.Player1 : Player.Player2;
            logger.Info("Starting player : {0}", startPlayer);
            var task = Task.Delay(delayPeriod).ContinueWith((e) =>
            {
                p1.InformStart(startPlayer);
                p2.InformStart(startPlayer);
            });
            board.StartGame(startPlayer);
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
            if (!board.IsGameEnded)
            {
                p1.InformCancel();
                board.CancelGame();
                DeclareGameEnded();
            }
        }

        private void p1_OnCancelGame(object sender, EventArgs e)
        {
            if (!board.IsGameEnded)
            {
                p2.InformCancel();
                board.CancelGame();
                DeclareGameEnded();
            }
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
            if (this.IsGameEnded)
            {
                DeclareGameEnded();
            }
        }

        private void RemoteStarted(object sender, RemoteStartArgs e)
        {
            p2.InformStart(e.Starter);
            board.StartGame(e.Starter);
        }

        private CellType PlayerToCellType(Player p)
        {
            return p == Player.Player1 ? CellType.Player1 : CellType.Player2;
        }

        private void DeclareGameEnded()
        {
            if (OnGameEnd != null)
            {
                OnGameEnd(this, new EventArgs());
            }
        }
    }
}