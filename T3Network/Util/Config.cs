// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T3Network.Util
{
    static class Config
    {
        public static int ServerPort
        {
            get
            {
                int port = Properties.Settings.Default.ServerPort;
                return port;
            }
        }
    }
}
