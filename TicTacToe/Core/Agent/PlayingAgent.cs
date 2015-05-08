using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }

    public delegate void TTTMoveEventHandler(object sender, TTTMoveEventArgs e);

    public abstract class PlayingAgent
    {
        public string AgentName { get; protected set; }

        protected Player player;

        public abstract async void InformStart(bool firstMove);
        public abstract async void InformMove(int row, int col, CellType move);
        public abstract async void InformCancel();

        public event TTTMoveEventHandler MakeMove;
        public event EventHandler CancelGame;
        
        
        public PlayingAgent(Player thisPlayer)
        {
            player = thisPlayer;
        }
    }
}
