﻿using Servicedog.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servicedog.Analysers
{
    public abstract class Analyser
    {
        protected IReceiver _receiver;

        public Analyser(IReceiver receiver)
        {
            _receiver = receiver;
        }
        
        /// <summary>
        /// Start listening for new messages and analyse them. All in another Thread.
        /// </summary>
        /// <param name="cancel"></param>
        public void StartAnalysing(CancellationToken cancel)
        {
            try
            {
                _receiver.AddRoutingKeys(PrefixesToSubscribeTo());

                Task.Run(() =>
                {
                    while (true)
                    {
                        Tuple<int,string, string> message = _receiver.NextMessage();//this is a blocking call
                        Analyse(message);
                    }
                }, cancel);
            }
            catch (TaskCanceledException)
            {
                //do nothing, just exit.
            }
            catch (Exception)
            {
                //TODO: need to analyse the behavior of _receiver,NextMessage(). It is still unclear for me how zeromq works.
                throw;
            }
        }

        public abstract void Analyse(Tuple<int,string, string> message);
        public abstract IEnumerable<string> PrefixesToSubscribeTo();
    }
}