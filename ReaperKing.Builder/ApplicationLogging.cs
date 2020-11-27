using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ReaperKing.Builder
{
    internal static class ApplicationLogging
    {
        internal static ILoggerFactory Factory;
        
        internal static void Initialize()
        {
            Factory = LoggerFactory.Create(
                builder =>
                {
                    builder.AddSimpleConsole(options =>
                    {
                        options.ColorBehavior = LoggerColorBehavior.Disabled;
                        options.SingleLine = true;
                        options.IncludeScopes = false;
                    });
                });
        }
    }
}