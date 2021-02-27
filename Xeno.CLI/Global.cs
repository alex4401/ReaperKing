using Microsoft.Extensions.Logging;

namespace Xeno.CLI
{
    internal sealed class XenoCli
    {
        internal static XenoCli Instance { get; private set; }

        internal static void TryInitialize(Program program)
            => Instance = new(program);

        internal Program Main { get; }
        internal ILogger Log { get; }
        internal bool IsVerbose { get; }
        
        private XenoCli(Program program)
        {
            Main = program;
            
            IsVerbose = Main.Verbose;
            ApplicationLogging.Initialize(IsVerbose);
            Log = ApplicationLogging.Factory.CreateLogger("Xeno.CLI");
        }
    }
}