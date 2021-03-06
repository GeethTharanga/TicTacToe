﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TicTacToe.Core;
using TicTacToe.Core.Agent;
using TicTacToe.Core.Agent.AI;
using TicTacToe.Core.Agent.Human;
using TicTacToe.UI;

namespace T3WPFGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IBasicUI
    {
        public ObservableCollection<TicTacToeCell> Cells { get; set; }


        #region Dependency Properties



        public bool IsGameEnded
        {
            get { return (bool)GetValue(IsGameEndedProperty); }
            set { SetValue(IsGameEndedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGameEnded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGameEndedProperty =
            DependencyProperty.Register("IsGameEnded", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        public bool IsGameInProgress
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
            DependencyProperty.Register("UserCellType", typeof(CellType), typeof(MainWindow), new PropertyMetadata(CellType.Clear));

        public bool IsUserTurn
        {
            get { return (bool)GetValue(IsUserTurnProperty); }
            set { SetValue(IsUserTurnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUserTurnProperty =
            DependencyProperty.Register("IsUserTurn", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        public string  DisplayStatus
        {
            get { return (string )GetValue(DisplayStatusProperty); }
            set { SetValue(DisplayStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayStatusProperty =
            DependencyProperty.Register("DisplayStatus", typeof(string), typeof(MainWindow), new PropertyMetadata(""));




        public bool ShowBanner
        {
            get { return (bool)GetValue(ShowBannerProperty); }
            set { SetValue(ShowBannerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowBanner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowBannerProperty =
            DependencyProperty.Register("ShowBanner", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        #endregion


        public MainWindow()
        {
            Cells = new ObservableCollection<TicTacToeCell>();

            InitControls();
            InitializeComponent();
           // SimulateStart();
        }

        //private void SimulateStart()
        //{
        //    PlayingAgent p1 = new AIRandomAgent(Player.Player1), p2;

        //    var uiHandler = new UIBase(this);
        //    var agent = new HumanAgent(Player.Player2, uiHandler);
        //    p2 = agent;

        //    p1.OnMove += (s, e) => { p2.InformMove(e.row, e.col, TicTacToe.Core.CellType.Player1); };
        //    p2.OnMove += (s, e) => { p1.InformMove(e.row, e.col, TicTacToe.Core.CellType.Player2); };

        //    //   p1.OnCancelGame += (s, e) => { MessageBox.Show("cancelled"); };
        //    //    p2.OnCancelGame += (s, e) => { MessageBox.Show("cancelled"); };

        //    agent.InformStart(false);
        //    p1.InformStart(true);
        //}

        private void InitControls()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TicTacToeCell cell = new TicTacToeCell { Col = j, Row = i, Type = CellType.Clear };
                    Cells.Add(cell);
                }
            }

            DisplayStatus = "Waiting for game to start";
            ShowBanner = true;
        }

        public event EventHandler UIOnClose;

        public void UIRefresh(Board board, Player thisPlayer)
        {
            Dispatcher.Invoke(() =>
            {
                UserCellType = thisPlayer == Player.Player1 ? CellType.O : CellType.X;
                IsGameInProgress = board.IsGameInProgress;
                IsGameEnded = board.IsGameEnded;
                IsUserTurn = (board.CurrentStatus == Status.TurnP1 && thisPlayer == Player.Player1)
                             || (board.CurrentStatus == Status.TurnP2 && thisPlayer == Player.Player2);
                UpdateBanner(board, thisPlayer);
                UpdateCells(board, thisPlayer);
            });
        }

        private TicTacToeCell GetCell(int row, int col)
        {
            int pos = row * 3 + col;
            return Cells[pos];
        }

        private void UpdateBanner(Board board,Player thisPlayer)
        {
            switch (board.CurrentStatus)
            {
                case Status.NotStarted:
                    DisplayStatus = "Waiting for game to start";
                    ShowBanner = true;
                    break;
                case Status.TurnP1:
                case Status.TurnP2:
                    {
                        if ((Status.TurnP1 == board.CurrentStatus && thisPlayer == Player.Player1) ||
                            (Status.TurnP2 == board.CurrentStatus && thisPlayer == Player.Player2)) //player turn
                        {
                            DisplayStatus = "Your Turn";
                        }
                        else
                        {
                            DisplayStatus = "Oppotent's Turn";
                        }
                        ShowBanner = false;
                    }
                    break;
                case Status.WonP1:
                case Status.WonP2:
                    {
                        if ((Status.WonP1 == board.CurrentStatus && thisPlayer == Player.Player1) ||
                            (Status.WonP2 == board.CurrentStatus && thisPlayer == Player.Player2)) //player turn
                        {
                            DisplayStatus = "You Win!";
                        }
                        else
                        {
                            DisplayStatus = "You Lost!";
                        }
                        ShowBanner = true;
                    }
                    break;
                case Status.Tie:
                    DisplayStatus = "Game Tied";
                    ShowBanner = true;
                    break;
                case Status.Cancelled:
                    DisplayStatus = "Game was Cancelled";
                    ShowBanner = true;
                    break;
                default:
                    break;
            }

        }

        private void UpdateCells(Board board, Player thisPlayer)
        {

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var cell = GetCell(row, col);
                    switch (board[row, col])
                    {
                        case TicTacToe.Core.CellType.Clear:
                            cell.Type = CellType.Clear;
                            break;

                        case TicTacToe.Core.CellType.Player1:
                            cell.Type = CellType.O;
                            break;

                        case TicTacToe.Core.CellType.Player2:
                            cell.Type = CellType.X;
                            break;

                        default:
                            break;
                    }
                }
            }

            CommandManager.InvalidateRequerySuggested();
        }

        public void UIStart()
        {
            IsGameInProgress = true;
        }

        public void UIClose()
        {
            this.Close();
        }

        private void Cell_Click(object sender, ExecutedRoutedEventArgs e)
        {
            var cell = (TicTacToeCell)e.Parameter;
            var arg = new CellArgs { Row = cell.Row, Col = cell.Col };
            UIOnMove(this, arg);
        }

        private void IsCellClickable(object sender, CanExecuteRoutedEventArgs e)
        {
            var cell = (TicTacToeCell)e.Parameter;
            e.CanExecute = IsGameInProgress && IsUserTurn && cell.Type == CellType.Clear;
        }

        public event CellMoveEventHandler UIOnMove;

        private void WndMain_Closed(object sender, EventArgs e)
        {
            if (UIOnClose != null)
                UIOnClose(this, e);
        }

        private void LinkContinue_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}