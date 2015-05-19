using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core.Agent
{
    public class RemoteStartArgs  
    {
        public bool StartingMove { get; private set; }
    }
    public abstract class RemoteStartingAgent : PlayingAgent 
    {
        public event EventHandler<RemoteStartArgs> OnRemoteStart;

        protected void DeclareRemoteStart(RemoteStartArgs arg)
        {
            if(OnRemoteStart != null )
            {
                OnRemoteStart(this, arg);
            }
        }
    }
}
