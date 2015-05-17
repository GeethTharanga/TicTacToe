using System.Windows.Input;

namespace T3WPFGui
{
    public static class Commands
    {
        public static readonly RoutedUICommand MarkCell = new RoutedUICommand("Mark Cell", "MarkCell", typeof(MainWindow));

        public static readonly RoutedUICommand AIGame = new RoutedUICommand("New AI Game", "AIGame", typeof(GameSelectWindow));
        public static readonly RoutedUICommand HostGame = new RoutedUICommand("Host Game", "HostGame", typeof(GameSelectWindow));
        public static readonly RoutedUICommand JoinGame = new RoutedUICommand("Join Game", "JoinGame", typeof(GameSelectWindow));
    }
}