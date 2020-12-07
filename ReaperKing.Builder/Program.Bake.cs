/*!
 * This file is a part of Reaper King, and the project's repository may be
 * found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
 * the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

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