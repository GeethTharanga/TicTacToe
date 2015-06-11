// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;

namespace TicTacToe.Core.Agent.AI
{
    public sealed class AIHardAgent : AIAgent
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Random rand = new Random();

        private IEnumerable<IEnumerable<Tuple<int, int>>> BestPositions;
        public AIHardAgent(Player player)
            : base(player)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            CalculateBestPositions();
        }

        private void CalculateBestPositions()
        {
            List<Tuple<int, int>> BestPositions1, BestPositions2, BestPositions3;
            BestPositions1 = new List<Tuple<int, int>>{ Tuple.Create(1, 1)};
            BestPositions2 = new List<Tuple<int, int>> { Tuple.Create(0, 0), Tuple.Create(0, 2), 
                                                Tuple.Create(2, 0), Tuple.Create(2, 2) };
            BestPositions3 = new List<Tuple<int, int>>(){ Tuple.Create(0, 1), Tuple.Create(1, 0), 
                                                Tuple.Create(2, 1), Tuple.Create(1, 2) };
            BestPositions = new List<IEnumerable<Tuple<int, int>>> { BestPositions1, BestPositions2, BestPositions3 };
        }

        protected override TTTMoveEventArgs FindMove()
        {
            //Select a cell randomly
            List<Tuple<int, int>> possibleCells = new List<Tuple<int, int>>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == CellType.Clear)
                    {
                        possibleCells.Add(Tuple.Create(i, j));
                    }
                }
            }
            int count = possibleCells.Count;
            //if no moves - never reacheable
            if (count == 0)
            {
                throw new IndexOutOfRangeException("No more possible moves");
            }

            var winningPossibilities = WinningCombinations.Combinations;

            TTTMoveEventArgs move = null;

            Tuple<int, int> position;

            // if can win, make move
            foreach(var possibiliy in winningPossibilities)
            {
                position = WinningPosition(possibiliy, ThisPlayer);
                if(position != null)
                {
                    return new TTTMoveEventArgs(position.Item1, position.Item2, ThisPlayer);
                }
            }

            // if other can win, block it
            foreach (var possibiliy in winningPossibilities)
            {
                position = WinningPosition(possibiliy, OtherPlayer);
                if (position != null)
                {
                    return new TTTMoveEventArgs(position.Item1, position.Item2, ThisPlayer);
                }
            }

            //select best location
            foreach (var positionSet in BestPositions)
            {
                List<Tuple<int, int>> availablePos = new List<Tuple<int, int>>();
                foreach (var pos in positionSet)
                {
                    if(board[pos.Item1,pos.Item2] == CellType.Clear)
                    {
                        availablePos.Add(pos);
                    }
                }
                if(availablePos.Count > 0)
                {
                    int index = rand.Next(availablePos.Count);
                    position = availablePos[index];
                    move = new TTTMoveEventArgs(position.Item1, position.Item2, ThisPlayer);
                    return move;
                }
            }

            return null;
        }

        private Tuple<int, int> WinningPosition(int[,] combination, Player p)
        {
            Tuple<int, int> position =Tuple.Create(0,0);
            int player = 0, clear = 0;
            for(int i=0; i< 3; i++)
            {
                int x = combination[i, 0];
                int y = combination[i, 1];
                if(board[x,y] == CellType.Clear)
                {
                    clear++;
                    position = Tuple.Create(x, y); 
                }
                else if ((p == Player.Player1 && board[x, y] == CellType.Player1) ||
                    (p == Player.Player2 && board[x, y] == CellType.Player2))
                {
                    player++;
                }
            }
            if( clear == 1 && player == 2)
            {
                return position;
            }
            else
            {
                return null;
            }
        }

        public override string AgentName
        {
            get { return "AI Hard"; }
        }
    }
}