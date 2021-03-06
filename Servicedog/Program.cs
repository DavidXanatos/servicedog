﻿using Servicedog.Messaging;
using Servicedog.Messaging.Dispatchers;
using Servicedog.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using ZeroMQ;

namespace Servicedog
{
    class Program
    {
        static void Main(string[] args)
        {
            EnvironmentChecker.EnsureIsAdministrator();

            HostFactory.New(x =>
            {
                x.SetServiceName("servicedog");
                x.SetDescription("Captures and analyses problems that interfere on the communications between local and remote applications.");
                x.Service<ServiceRunner>(sc =>
                {
                    sc.ConstructUsing(() => new ServiceRunner(new MessageDispatcher(),new ProcessTable()));
                    sc.WhenStarted(s => s.Start());
                    sc.WhenStopped(s => s.Stop());
                    sc.WhenShutdown(s => s.Shutdown());
                });

            });

        }


       

    }
}
