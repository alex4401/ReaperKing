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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using McMaster.Extensions.CommandLineUtils;

using ReaperKing.Core;
using ReaperKing.Core.Configuration;

namespace ReaperKing.Builder
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed partial class Program
    {
        static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        #region Required Arguments & Options
        // ReSharper disable UnassignedGetOnlyAutoProperty

        [Argument(order: 0)]
        [Required]
        public string SiteAssemblyName { get; }

        // ReSharper restore UnassignedGetOnlyAutoProperty
        #endregion
        #region Options
        [Option(LongName = "assembly-path")]
        public string AssemblyPath { get; set; } = "";

        [Option(LongName = "environment")]
        public string EnvironmentName { get; } = "default";
        
        [Option]
        public string DeploymentPath { get; } = "public";

        [Option(LongName = "project")]
        public string ProjectFilename { get; } = "project";

        [Option(LongName = "skip-pre-build")]
        public bool SkipPreBuild { get; } = false;
        #endregion

        private ILogger Log { get; set; }
        private ProjectConfigurationManager Config { get; set; }
        private Site SiteObject { get; set; }
        private ImmutableRuntimeConfiguration RuntimeConfig { get; set; }
        private BuildConfiguration BuildConfig { get; set; }
        
        
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        void OnExecute()
        {
            // Initialize a logging factory and get a logger for ourselves
            ApplicationLogging.Initialize();
            Log = ApplicationLogging.Factory.CreateLogger("ReaperKing.Builder");

            // Clean up arguments
            AssemblyPath = PathUtils.EnsureRooted(AssemblyPath);
            Log.LogInformation("Reaper King building tool");

            // Load the project assembly and find the Site class.
            Assembly siteAssembly;
            Type siteType;
            
            Log.LogInformation("Static configuration assembly is now being loaded");
            AllowAssembliesFromUserPath();
            (siteAssembly, siteType) = GetSiteBuildRecipeType();
            
            // Initialize a configuration manager
            Config = new ProjectConfigurationManager(ApplicationLogging.Factory);
            Config.InjectProperty<BuildConfiguration>("rk.build");
            Config.SchemaManager.ImportFromAssembly(siteAssembly);

            // Set the ImmutableRuntimeConfiguration early.
            RuntimeConfig = new()
            {
                ContentRoot = new FileInfo(ProjectFilename).Directory?.FullName,
                AssemblyRoot = AssemblyPath,
                DeploymentPath = PathUtils.EnsureRooted("public"),
            };
            Config.Override<ImmutableRuntimeConfiguration>(RuntimeConfig);
            
            // Create an instance of the build recipe class.
            Log.LogInformation("Static Site Configuration object is being created");
            var instance = Activator.CreateInstance(siteType, Config, ApplicationLogging.Factory);
            if (!(instance is Site site))
            {
                Log.LogCritical("Static Configuration instance is not valid.");
                return;
            }
            SiteObject = site;

            // Load the project configuration from a file
            Log.LogInformation("Project configuration is now being loaded");
            LoadProjectConfig();
            
            // Add the assembly to RazorLight's metadata references
            AddTypeMetadataToRazor(siteAssembly);

            ProceedBakeSite();
            Log.LogInformation("Site has been successfully built.");
        }
    }
}