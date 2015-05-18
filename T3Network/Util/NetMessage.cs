// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt


namespace T3Network.Util
{
    using System;
    using System.Text;
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
            public override string ToString()
            {
                return AssignedPlayer.ToString();
            }
        }

        [Serializable]
        public class StartMessageData
        {
            public bool IsStarter { get; set; }
            public override string ToString()
            {
                return IsStarter.ToString();
            }
        }
        
        [Serializable]
        public class MoveMessageData
        {
            public int Row { get; set; }
            public int Col { get; set; }

            public override string ToString()
            {
                return string.Format("({0},{1})", Row, Col);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Type : {0} .", this.MessageType);
            if(this.ConnectData != null)
            {
                sb.Append("ConnectData : " + this.ConnectData.ToString());
            }
            if (this.MoveData != null)
            {
                sb.Append("MovetData : " + this.MoveData.ToString());
            }
            if (this.StartData != null)
            {
                sb.Append("StartData : " + this.StartData.ToString());
            }

            return sb.ToString();
        }
    }
}