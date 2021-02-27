/*!
 * This file is a part of Xeno, and the project's repository may be found at https://github.com/alex4401/rk.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * http://www.gnu.org/licenses/.
 */

using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

using Xeno.Core;

namespace Xeno.CLI
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