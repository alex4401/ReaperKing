using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using ReaperKing.Core;

namespace Larvae
{
    internal sealed class BakeCommand : BaseCommandWithSite
    {
        [Option("--skip-pre-build")]
        internal bool SkipPreBuild { get; }
        
        public override int Execute()
        {
            LoadRecipeAssembly();
            
            Config.Override<ImmutableRuntimeConfiguration>(new()
            {
                ContentRoot = Environment.CurrentDirectory,
                AssemblyRoot = RecipeAssemblyPath,
                DeploymentPath = PathUtils.EnsureRooted("public"),
            });
            InstantiateBakeRecipe();
            LoadProjectConfiguration();
            AddTypeMetadataToRazor(BakeRecipeAssembly);

            Bake();
            Log.LogInformation("Site has been successfully built.");

            return 0;
        }

        internal void Bake()
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
                            int exitCode = ShellHelper.Run(cmd);
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

                BakeRecipe.PreBuild();
            }
            
            Log.LogInformation("Building site content");
            BakeRecipe.Build();
            
            Log.LogInformation("Executing post-build tasks");
            BakeRecipe.PostBuild();
        }
    }
}