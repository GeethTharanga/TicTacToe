// Copyright (c) 2015 Geeth Tharanga
// Under the MIT licence - See licence.txt

using NLog;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using T3Network.Util;

namespace T3Test.Network
{
    public delegate void NetMessageHandler(object sender, NetMessage msg);

    public class StreamClient
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private Stream input, output;
        private IFormatter formatter;
        private Task listener;
        private CancellationTokenSource tokenSource;

        public event NetMessageHandler MessageReceived;
        public event EventHandler StreamClosed;

        public StreamClient(Stream input, Stream output)
        {
            this.input = input;
            this.output = output;

            tokenSource = new CancellationTokenSource();
            formatter = new BinaryFormatter();
        }

        public void StartListening()
        {
            listener = Task.Factory.StartNew(InitListener, tokenSource.Token);
        }

        public void SendMessage(NetMessage msg)
        {
            try
            {
                logger.Info("Trying to send message - {0}", msg.MessageType);
                output.WriteByte(0xff);
                formatter.Serialize(output, msg);
                output.Flush();
            }
            catch (SocketException e)
            {
                logger.Error("Error trying to send message", e);
                throw;
            }
        }

        public void CancelListening()
        {
            try
            {
                tokenSource.Cancel();
                input.Close();
            }
            catch (AggregateException e)
            {
                logger.Error("Error while trying to cancel listening", e);
            }
        }

        private async void InitListener()
        {
            MemoryStream stream = new MemoryStream();

            while (!tokenSource.Token.IsCancellationRequested)
            {
                if (input.CanTimeout)
                {
                    input.ReadTimeout = 100;
                }
                byte[] header = new byte[1];
                while (true)
                {
                    Task<int> task = null ;
                    try
                    {
                        task = input.ReadAsync(header, 0, 1, tokenSource.Token);
                        await task;
                    }
                    catch(Exception ex)
                    {
                        if (ex is ObjectDisposedException || ex is IOException)
                        {
                            logger.Warn("Stream error", ex);
                            if (StreamClosed != null)
                            {
                                StreamClosed(this, new EventArgs());
                            }
                            return;
                        }
                        throw;
                    }
                    if (task.IsCanceled || tokenSource.Token.IsCancellationRequested)
                    {
                        return;
                    }
                    if (task.Result != 0)
                    {
                        break;
                    }
                }

                if (input.CanTimeout)
                {
                    input.ReadTimeout = 500;
                }
                var obj = formatter.Deserialize(input);
                if (MessageReceived != null)
                {
                    MessageReceived(this, (NetMessage)obj);
                }
            }
        }
    }
}