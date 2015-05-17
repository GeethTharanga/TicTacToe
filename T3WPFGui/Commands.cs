// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

namespace T3WPFGui
{
    using System.Windows.Input;

    public static class Commands
    {
        public static readonly RoutedUICommand MarkCell = new RoutedUICommand("Mark Cell", "MarkCell", typeof(MainWindow));

        public static readonly RoutedUICommand AIGame = new RoutedUICommand("New AI Game", "AIGame", typeof(GameSelectWindow));
        public static readonly RoutedUICommand HostGame = new RoutedUICommand("Host Game", "HostGame", typeof(GameSelectWindow));
        public static readonly RoutedUICommand JoinGame = new RoutedUICommand("Join Game", "JoinGame", typeof(GameSelectWindow));
    }
}