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
    /// Interaction logic for GameSelectWindow.xaml
    /// </summary>
    public partial class GameSelectWindow : Window
    {
        #region UI Dependency Properties

        public bool NotSelectedOption
        {
            get { return (bool)GetValue(NotSelectedOptionProperty); }
            set { SetValue(NotSelectedOptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotSelectedOption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotSelectedOptionProperty =
            DependencyProperty.Register("NotSelectedOption", typeof(bool), typeof(GameSelectWindow), new PropertyMetadata(true));



        public bool IsHostingGame
        {
            get { return (bool)GetValue(IsHostingGameProperty); }
            set { SetValue(IsHostingGameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsHostingGame.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHostingGameProperty =
            DependencyProperty.Register("IsHostingGame", typeof(bool), typeof(GameSelectWindow), new PropertyMetadata(false));



        public bool IsJoiningGame
        {
            get { return (bool)GetValue(IsJoiningGameProperty); }
            set { SetValue(IsJoiningGameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsJoiningGame.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsJoiningGameProperty =
            DependencyProperty.Register("IsJoiningGame", typeof(bool), typeof(GameSelectWindow), new PropertyMetadata(false));



        #endregion UI Dependency Properties

        public GameSelectWindow()
        {
            InitializeComponent();
        }

        private void StartGame(PlayingAgent otherAgent, Player thisPlayer)
        {
            MainWindow wndMain = new MainWindow();
            var uiHandler = new UIBase(wndMain);
            var human = new HumanAgent(thisPlayer, uiHandler);

            PlayingAgent p1, p2;
            if (thisPlayer == Player.Player1)
            {
                p1 = human;
                p2 = otherAgent;
            }
            else
            {
                p1 = otherAgent;
                p2 = human;
            }

            GameManager manager = new GameManager(p1, p2);
            manager.StartGame();
            wndMain.ShowDialog();
        }

        private void NewHostGame(object sender, ExecutedRoutedEventArgs e)
        {
            NotSelectedOption = false;
            IsHostingGame = true;
            MessageBox.Show("Not implemented");
        }

        private void NewAIGame(object sender, ExecutedRoutedEventArgs e)
        {
            PlayingAgent ai = new AIRandomAgent(Player.Player2);
            StartGame(ai, Player.Player1);
        }

        private void NewJoinGame(object sender, ExecutedRoutedEventArgs e)
        {
            NotSelectedOption = false;
            IsJoiningGame = true;
            MessageBox.Show("Not implemented");
        }

        private void CancelJoinGame(object sender, ExecutedRoutedEventArgs e)
        {
            NotSelectedOption = true;
            IsJoiningGame = false ;
            MessageBox.Show("Not implemented");
        }

        private void CancelHostGame(object sender, ExecutedRoutedEventArgs e)
        {
            NotSelectedOption = true ;
            IsHostingGame = false;
            MessageBox.Show("Not implemented");
        }

        private void CanStartJoining(object sender, CanExecuteRoutedEventArgs e)
        {
            var address = (string)e.Parameter;
            e.CanExecute = !string.IsNullOrWhiteSpace(address);
        }
    }
}