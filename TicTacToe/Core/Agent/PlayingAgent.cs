using System;
using System.Threading.Tasks;

namespace TicTacToe.Core.Agent
{
    public class TTTMoveEventArgs
    {
        public readonly int row, col;
        public readonly Player player;

        public TTTMoveEventArgs(int row, int col, Player player)
        {
            this.row = row;
            this.col = col;
            this.player = player;
        }

        public override string ToString()
        {
            return string.Format("[Move {0} to {1},{2}]", player, row, col);
        }
    }

    public delegate void TTTMoveEventHandler(object sender, TTTMoveEventArgs e);

    public abstract class PlayingAgent
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string AgentName { get; protected set; }

        protected readonly Player player;
        protected readonly Board board;

        public abstract Task InformStart(bool firstMove);

        public abstract Task InformMove(int row, int col, CellType move);

        public abstract Task InformCancel();

        public event TTTMoveEventHandler OnMove;

        public event EventHandler OnCancelGame;

        public Player ThisPlayer
        {
            get { return player; }
        }

        public Player OtherPlayer
        {
            get { return player == Player.Player1 ? Player.Player2 : Player.Player1; }
        }

        public PlayingAgent(Player thisPlayer)
        {
            player = thisPlayer;
            board = new Board();
        }

        protected void DeclareMove(TTTMoveEventArgs move)
        {
            board[move.row, move.col] = move.player == Player.Player1 ? CellType.Player1 : CellType.Player2;
            OnMove(this, move);
        }

        protected void CancelGame()
        {
            logger.Info("{0} Cancelling game.", ThisPlayer);
            if (OnCancelGame != null)
                OnCancelGame(this, new EventArgs());
        }

        public override string ToString()
        {
            return string.Format("[{0} - {1}]", ThisPlayer, base.ToString());
        }
    }
}