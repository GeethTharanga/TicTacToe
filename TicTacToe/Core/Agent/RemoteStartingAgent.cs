// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core.Agent
{
    public class RemoteStartArgs  
    {
        public RemoteStartArgs(Player starter)
        {
            Starter = starter;
        }
        public Player Starter { get; private set; }
    }
    public abstract class RemoteStartingAgent : PlayingAgent 
    {
        public RemoteStartingAgent(Player player):base(player)
        {

        }

        public event EventHandler<RemoteStartArgs> OnRemoteStart;

        protected void DeclareRemoteStart(Player starter)
        {
            if(OnRemoteStart != null )
            {
                var arg = new RemoteStartArgs(starter);
                OnRemoteStart(this, arg);
            }
        }
    }
}
