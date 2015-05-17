// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe.UI
{
    public class ConsoleUI : IBasicUI 
    {
        public event EventHandler UIOnClose;



        public void UIRefresh(Core.Board board, Core.Player thisPlayer)
        {
            Console.WriteLine("Player: {0}, status :{1}",thisPlayer, board.CurrentStatus);
            for(int i=0; i<3;i++)
            {
                for(int j=0; j<3; j++)
                {
                    Console.Write(board[i,j].ToString());
                }
                Console.WriteLine();
            }
        }

        public void UIStart()
        {
            Thread t = new Thread(ProcessKeys);
            t.Start();
        }
        public void ProcessKeys()
        {
            while (true)
            {
                int row = int.Parse(Console.ReadLine());
                int col = int.Parse(Console.ReadLine());
                UIOnMove(this, new CellArgs { Col = col, Row = row });
            }
        }
        public void UIClose()
        {
            
        }


        public event CellMoveEventHandler UIOnMove;
    }
}
