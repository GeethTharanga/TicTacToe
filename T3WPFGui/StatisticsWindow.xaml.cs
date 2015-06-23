using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using T3DBStatRepository;
using T3Network;
using System.Windows.Media;
using TicTacToe.Stats;
using System.Windows.Controls;


namespace T3WPFGui
{
    /// <summary>
    /// Interaction logic for GameSelectWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public class UIResultItem
        {
            public string Date { get; set; }
            public string Opponent { get; set; }
            public string Result { get; set; }
            public Brush  ResultColor { get; set; }
        }

        public class UIResultStatistics
        {
            public string Opponent { get; set; }
            public int Wins { get; set; }
            public int Losses { get; set; }
            public int Ties { get; set; }
        }

        public List<UIResultItem> LastResults
        {
            get { return (List<UIResultItem>)GetValue(LastResultsProperty); }
            set { SetValue(LastResultsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastResults.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastResultsProperty =
            DependencyProperty.Register("LastResults", typeof(List<UIResultItem>), typeof(StatisticsWindow));



        public List<UIResultStatistics> OverallStatistics
        {
            get { return (List<UIResultStatistics>)GetValue(OverallStatisticsProperty); }
            set { SetValue(OverallStatisticsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OverallStatistics.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverallStatisticsProperty =
            DependencyProperty.Register("OverallStatistics", typeof(List<UIResultStatistics>), typeof(StatisticsWindow));




        public StatisticsWindow()
        {
            InitializeComponent();

            var db = new DBRecordRepository();
            var resultColors = new Dictionary<GamePlayResult, Brush>()
            {
                {GamePlayResult.Loss, new SolidColorBrush(Colors.DarkRed) },
                {GamePlayResult.Won , new SolidColorBrush( Colors.Green)},
                {GamePlayResult.Tied, new SolidColorBrush(Colors.Black ) }
            };
            LastResults = db.GetLastRecords(10).Select(r => new UIResultItem
            {
                Date = r.Time.ToString(),
                Result = r.Result.ToString(),
                Opponent = r.Opponent,
                ResultColor = resultColors[r.Result]
            }).ToList();
            List<string> opponentTypes = new List<string> { "Multiplayer", "AI Easy", "AI Hard" };
            var overalls = db.GetStatistics();
            var uiOveralls = new List<UIResultStatistics>();
            foreach (var opp in opponentTypes)
            {
                if(overalls.ContainsKey(opp))
                {
                    var results = overalls[opp];
                    uiOveralls.Add(new UIResultStatistics
                    {
                        Opponent = opp,
                        Losses = results.Losses,
                        Ties = results.Tied,
                        Wins = results.Wins
                    });
                }
                else
                {
                    uiOveralls.Add(new UIResultStatistics { Opponent = opp });
                }
            }
            OverallStatistics = uiOveralls;
            db.Dispose();

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}