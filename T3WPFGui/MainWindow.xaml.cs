using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using TicTacToe.Core;
using TicTacToe.UI;

namespace T3WPFGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IBasicUI
    {
        public ObservableCollection<TicTacToeCell> Cells { get; set; }

        Board board;

        public bool GameInProgress
        {
            get { return (bool)GetValue(GameInProgressProperty); }
            set { SetValue(GameInProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GameStarted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameInProgressProperty =
            DependencyProperty.Register("GameStarted", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        public CellType UserCellType
        {
            get { return (CellType)GetValue(UserCellTypeProperty); }
            set { SetValue(UserCellTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserCellType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserCellTypeProperty =
            DependencyProperty.Register("UserCellType", typeof(CellType), typeof(MainWindow), new PropertyMetadata(CellType.X));



        public bool IsUserTurn
        {
            get { return (bool)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("IsUserTurn", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));




        public MainWindow()
        {
            Cells = new ObservableCollection<TicTacToeCell>();
            InitControls();
            InitializeComponent();
            
        }

        private void InitControls()
        {
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TicTacToeCell cell = new TicTacToeCell { Col = j, Row = i, Type = null };
                    Cells.Add(cell);
                }
            }
        }

        public event EventHandler UIOnClose;

        public event TicTacToe.Core.Agent.TTTMoveEventHandler UIOnMove;

        public void UIRefresh(TicTacToe.Core.Board board, TicTacToe.Core.Player thisPlayer)
        {
            throw new NotImplementedException();
        }

        public void UIStart()
        {
            throw new NotImplementedException();
        }

        public void UIClose()
        {
            throw new NotImplementedException();
        }
    }
}
