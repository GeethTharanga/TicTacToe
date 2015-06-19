using System.Windows;
using System.Windows.Input;
using T3DBStatRepository;
using T3Network;
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


        private NetworkListener listener;
        private NetworkClient client;

        public GameSelectWindow()
        {
            InitializeComponent();
        }

        private void StartNewGame(PlayingAgent otherAgent, Player thisPlayer)
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

            GameManager manager = new GameManager(p1, p2,new DBRecordRepository());
            manager.StartGame();
            this.Visibility = System.Windows.Visibility.Hidden;
            wndMain.ShowDialog();
            ResetWindowState();
        }

        private void JoinGame(RemoteStartingAgent netAgent, Player thisPlayer)
        {
            MainWindow wndMain = new MainWindow();
            var uiHandler = new UIBase(wndMain);
            var human = new HumanAgent(thisPlayer, uiHandler);

            GameManager manager = new GameManager(netAgent, human, new DBRecordRepository());
            this.Visibility = System.Windows.Visibility.Hidden;
            wndMain.ShowDialog();

            ResetWindowState();
            
        }

        private void NewHostGame(object sender, ExecutedRoutedEventArgs e)
        {
            NotSelectedOption = false;
            IsHostingGame = true;
            listener = new NetworkListener(Player.Player2);
            listener.OnConnect += listener_OnConnect;
            listener.StartListening();
        }

        void listener_OnConnect(object sender, System.EventArgs e)
        {
            Dispatcher.Invoke(() =>
           {
               if (IsHostingGame)
               {
                   NetworkAgent agent = listener.Agent;
                   this.StartNewGame(agent, Player.Player1);
               }
           });
        }

        private void NewAIGame(object sender, ExecutedRoutedEventArgs e)
        {
            AIMode mode = (AIMode)e.Parameter;
            PlayingAgent ai;
            if (mode == AIMode.Easy)
            {
                ai = new AIRandomAgent(Player.Player2);
            }
            else
            {
                ai = new AIHardAgent(Player.Player2);
            }
            StartNewGame(ai, Player.Player1);
        }

        private void NewJoinGame(object sender, ExecutedRoutedEventArgs e)
        {
            NotSelectedOption = false;
            IsJoiningGame = true;
            var addr = (string) e.Parameter;
            client = new NetworkClient(addr, Player.Player1);
            client.OnConnect += client_OnConnect;
            client.OnError += client_OnError;
            client.Connect();
        }

        void client_OnError(object sender, string e)
        {
            Dispatcher.Invoke(() =>
           {
               MessageBox.Show("Unable to join game\nError: " + e, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
               ResetWindowState();
           });
        }

        void client_OnConnect(object sender, System.EventArgs e)
        {
            RemoteStartingAgent agent = client.Agent;
            Dispatcher.Invoke(() =>
            {
                JoinGame(agent, Player.Player2);
            });
        }

        private void CancelJoinGame(object sender, ExecutedRoutedEventArgs e)
        {
            ResetWindowState();
            client.Dispose();
            client = null;
        }

        private void CancelHostGame(object sender, ExecutedRoutedEventArgs e)
        {
            ResetWindowState();
            listener.Dispose();
            listener = null;
        }

        private void CanStartJoining(object sender, CanExecuteRoutedEventArgs e)
        {
            var address = (string)e.Parameter;
            e.CanExecute = !string.IsNullOrWhiteSpace(address);
        }

        private void ResetWindowState()
        {
            IsHostingGame = IsJoiningGame = false;
            NotSelectedOption = true;
            this.Visibility = System.Windows.Visibility.Visible;
        }

        private void ViewStatistics_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
            new StatisticsWindow().ShowDialog();
            this.Visibility = System.Windows.Visibility.Visible;
        }
    }
}