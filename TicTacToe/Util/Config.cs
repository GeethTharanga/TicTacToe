using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Properties;

namespace TicTacToe.Util
{
    public static class Config
    {
        public static int AIThinkDuration
        {
            get
            {
                return Settings.Default.AIThinkDuration ;
            }
        }
    }
}
