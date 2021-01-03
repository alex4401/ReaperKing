using Microsoft.Extensions.Logging;

namespace ReaperKing.Larvae
{
    internal sealed class Larvae
    {
        internal static Larvae Instance { get; private set; }

        internal static void TryInitialize(Program program)
            => Instance = new(program);

        internal Program Main { get; }
        internal ILogger Log { get; }
        internal bool IsVerbose { get; }
        
        private Larvae(Program program)
        {
            Main = program;
            
            IsVerbose = Main.Verbose;
            ApplicationLogging.Initialize(IsVerbose);
            Log = ApplicationLogging.Factory.CreateLogger("Larvae");
        }
    }
}