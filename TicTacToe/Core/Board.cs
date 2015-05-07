using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public enum CellType
    {
        Clear=0, Player1, Player2
    }
    public enum Status
    {
        NotStarted, TurnP1, TurnP2, WonP1, WonP2,Tie, Cancelled
    }
    public enum Player
    {
        Player1,Player2
    }

    public class Board
    {
        //logger
        Logger logger = LogManager.GetCurrentClassLogger();
        //attributes
        private CellType[,] cells;
        public Status  CurrentStatus { get; private set; }
        
        //ctor
        public Board()
        {
            logger.Info("Creating new board");
            
            cells = new CellType[3,3];
            CurrentStatus = Status.NotStarted;
        }

        //cell management
        public CellType this[int row,int col]
        {
            get
            {
                if (row < 0 || col < 0 || row >= 3 || col >= 3)
                {
                    throw new ArgumentOutOfRangeException("Invalid Cell");
                }
                else
                {
                    return cells[row, col];
                }
            }
            set
            {
                logger.Info("Trying to set cell {0},{1} to {2}", row, col, value);
                if (row < 0 || col < 0 || row >= 3 || col >= 3)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (cells[row, col] != CellType.Clear)
                {
                    throw new InvalidOperationException("Tried to overwrite cell");
                }
                else if (!IsGameInProgress)
                {
                    throw new InvalidOperationException("Game not in playing state");
                }
                else if (!( (value == CellType.Player1 && Status.TurnP1 == CurrentStatus) ||
                    (value == CellType.Player2 && Status.TurnP2 == CurrentStatus)) )
                {
                    throw new InvalidOperationException("Invalid player");
                }
                else
                {
                    cells[row, col] = value;
                    CurrentStatus = (CurrentStatus == Status.TurnP1) ? Status.TurnP2 : Status.TurnP1;
                    UpdateTableState();
                }
            }
        }

        public void StartGame(Player startPlayer) 
        {
            
            if(CurrentStatus == Status.NotStarted )
            {
                logger.Info("Starting game");
                CurrentStatus = startPlayer == Player.Player1 ? Status.TurnP1 : Status.TurnP2;
            }
            else
            {
                throw new InvalidOperationException("Game already started");
            }
        }
        public void CancelGame()
        {
            CurrentStatus = Status.Cancelled;
        }

        public int NumCheckedCells
        {
            get
            {
                return cells.OfType<CellType>().Count(c=> c!= CellType.Clear);
            }
        }

        public bool IsGameInProgress
        {
            get
            {
                return CurrentStatus == Status.TurnP1 || CurrentStatus == Status.TurnP2;
            }
        }

        public bool IsGameEnded
        {
            get
            {
                var endStates=new[] { Status.WonP1, Status.WonP2,Status.Tie, Status.Cancelled };
                return endStates.Contains(CurrentStatus);
            }
        }

        /// <summary>
        /// Update game status according to last move
        /// To check if someone won or game tied.
        /// </summary>
        private void UpdateTableState()
        {
            if(!IsGameInProgress)
            {
                throw new InvalidOperationException("This method shouldn't be called when game is not playing");
            }

            //for each possible winning combination - line
            foreach (int[,] combination in WinningCombinations.Combinations)
            {
                //find current celltypes in this line
                List<CellType> line = new List<CellType>(3);
                for (int i = 0; i < 3; i++) 
                {
                    int cellRow = combination[i, 0], cellCol = combination[i, 1];
                    CellType cell = cells[cellRow, cellCol];
                    line.Add(cell);
                }

                //declare winner if all cells in the line belongs to same player
                if(line.TrueForAll(c => c== CellType.Player1 ) )
                {
                    CurrentStatus = Status.WonP1;
                }
                else if (line.TrueForAll(c=> c==CellType.Player2) )
                {
                    CurrentStatus = Status.WonP2;
                }

                //mark as a tie if still in progress(not won), and all cells are marked
                if(IsGameInProgress && NumCheckedCells == 9)
                {
                    CurrentStatus = Status.Tie;
                }
            }
        }

    }
}
