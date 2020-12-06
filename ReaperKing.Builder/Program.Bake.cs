using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;

namespace ReaperKing.Builder
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed partial class Program
    {
        private void ProceedBakeSite()
        {
            Log.LogInformation("Executing pre-build tasks");
            {
                var prebuildCmds = BuildConfig.BeforeBuild;
                if (prebuildCmds.Count > 0)
                {
                    if (!SkipPreBuild)
                    {
                        Log.LogInformation("Executing commands scheduled to run before build");
                        foreach (var cmd in prebuildCmds)
                        {
                            Log.LogInformation(cmd);
                            var exitCode = ShellHelper.Run(cmd);

                            if (exitCode != 0)
                            {
                                Environment.Exit(exitCode);
                            }
                        }
                    }
                    else
                    {
                        Log.LogWarning("The pre-build tasks have been requested to be skipped.");
                    }
                }

                SiteObject.PreBuild();
            }
            
            Log.LogInformation("Building site content");
            SiteObject.Build();
            
            Log.LogInformation("Executing post-build tasks");
            SiteObject.PostBuild();
        }
    }
}