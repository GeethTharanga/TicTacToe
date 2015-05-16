using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T3WPFGui
{
    public static class Commands
    {
        public static readonly RoutedUICommand MarkCell = new RoutedUICommand("Mark Cell", "MarkCell", typeof(MainWindow));
    }
}
