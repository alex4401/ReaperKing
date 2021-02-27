using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using ReaperKing.Core;

namespace Xeno.CLI
{
    internal abstract class BaseCommand
    {
        protected Program Parent { get; }
        protected CommandLineApplication App { get; private set; }
        protected ILogger Log { get; private set; }

        public int OnExecute(CommandLineApplication app)
        {
            XenoCli.TryInitialize(Parent);
            Log = XenoCli.Instance.Log;
            App = app;
            
            Log.LogInformation($"Xeno.CLI, v{Program.GetVersion()}");
            BeforeExecution();
            return Execute();
        }

        public virtual void BeforeExecution()
        { }

        public abstract int Execute();
    }

}