using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicTacToe.Core;
using TicTacToe.Core.Agent;
using TicTacToe.Core.Agent.AI;

namespace T3WPFGui
{
    /// <summary>
    /// Interaction logic for GameSelectWindow.xaml
    /// </summary>
    public partial class GameSelectWindow : Window
    {


        public bool NotSelectedOption
        {
            get { return (bool)GetValue(NotSelectedOptionProperty); }
            set { SetValue(NotSelectedOptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotSelectedOption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotSelectedOptionProperty =
            DependencyProperty.Register("NotSelectedOption", typeof(bool), typeof(GameSelectWindow), new PropertyMetadata(0));


        
        public GameSelectWindow()
        {
            InitializeComponent();
        }



        public void StartGame(PlayingAgent otherPlayer, Player thisPlayer)
        {
            MessageBox.Show("new game" + otherPlayer);
        }

        private void NewHostGame(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Not implemented");
        }
        private void NewAIGame(object sender, ExecutedRoutedEventArgs e)
        {
            PlayingAgent ai = new AIRandomAgent(Player.Player2);
            StartGame(ai, Player.Player1);
        }
        private void NewJoinGame(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Not implemented");
        }
    }
}
