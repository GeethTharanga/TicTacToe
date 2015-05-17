using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe.Core;
using TicTacToe.UI;

namespace TicTacToe
{
    public partial class SimpleGUIForm : Form, IBasicUI 
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        Thread uiThread;

        public SimpleGUIForm()
        {
            InitializeComponent();
            InitUI();
        }

        Dictionary<Tuple<int, int>, Label> cellMap;

        private void InitUI()
        {
            logger.Info("Adding grid in the ui");
            cellMap = new Dictionary<Tuple<int, int>, Label>();
            for(int i=0; i <3 ;i++)
                for(int j=0;j<3;j++)
                {
                    Label l = new Label { Text = "Clear" ,AutoSize=false, Width=cellPanel.Width/4, TextAlign= ContentAlignment.MiddleCenter,BorderStyle= BorderStyle.Fixed3D};
                    cellPanel.Controls.Add(l);
                    l.Click += (s, e) => { UIOnMove(this, new CellArgs { Row = i, Col = j }); };
                    cellMap[Tuple.Create(i,j)]=l;
                }
        }

        public event EventHandler UIOnClose;


        public void UIRefresh(Core.Board board, Core.Player thisPlayer)
        {

            logger.Info("UI Update requested");
            lblPlayer.Text = thisPlayer.ToString();
            lblTurn.Text = board.CurrentStatus.ToString();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    cellMap[Tuple.Create(i, j)].Text = board[i, j].ToString();
                }
            
        }


        public void UIStart()
        {
            uiThread = new Thread( () => {
            this.ShowDialog();});
        }

        public void UIClose()
        {
            this.Close();
            
        }


        public event CellMoveEventHandler UIOnMove;
    }
}
