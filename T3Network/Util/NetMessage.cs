// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt


namespace T3Network.Util
{
    using System;
    using TicTacToe.Core;

    public enum NetMessageType
    {
        Connect,
        Start,
        Move,
        Cancel,
        Disconnect
    }

    [Serializable()]
    public class NetMessage
    {
        public NetMessageType MessageType { get; set; }

        public ConnectMessageData ConnectData { get; set; }

        public StartMessageData StartData { get; set; }

        public MoveMessageData MoveData { get; set; }

        [Serializable]
        public class ConnectMessageData
        {
            public Player AssignedPlayer { get; set; }
        }

        [Serializable]
        public class StartMessageData
        {
            public bool IsStarter { get; set; }
        }
        
        [Serializable]
        public class MoveMessageData
        {
            public int Row { get; set; }

            public int Col { get; set; }
        }
    }
}