// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;

namespace TicTacToe.Core.Agent.AI
{
    public class AIRandomAgent : AIAgent
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Random rand = new Random();
        public AIRandomAgent(Player player)
            : base(player)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
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
            var cell = possibleCells[rand.Next(count)];
            var move = new TTTMoveEventArgs(cell.Item1, cell.Item2, ThisPlayer);
            return move;
        }
    }
}