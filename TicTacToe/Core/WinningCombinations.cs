// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public static class WinningCombinations
    {
        public static IEnumerable<int[,]> Combinations
        {
            get
            {
                var comb = new List<int[,]>(8);

                //diagonals
                comb.Add(new int[3, 2] { { 0, 0 }, { 1, 1 }, { 2, 2 } });
                comb.Add(new int[3, 2] { { 0, 2 }, { 1, 1 }, { 2, 0 } });

                //horizontal & vertical
                for (int i = 0; i < 3; i++)
                {
                    //horizontal
                    comb.Add(new int[3, 2] { { i, 0 }, { i, 1 }, { i, 2 } });
                    //vertical
                    comb.Add(new int[3, 2] { { 0, i }, { 1, i }, { 2, i } });
                }
                return comb;
            }
        }
    }
}
