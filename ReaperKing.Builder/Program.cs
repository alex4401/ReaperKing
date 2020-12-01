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
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using McMaster.Extensions.CommandLineUtils;

using ReaperKing.Core;

namespace ReaperKing.Builder
{
    sealed class Program
    {
        static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        #region Required Arguments & Options
        // ReSharper disable UnassignedGetOnlyAutoProperty

        [Argument(order: 0)]
        [Required]
        public string SiteAssemblyName { get; }
        
        [Option(LongName = "environment")]
        [Required]
        public string EnvironmentName { get; }

        // ReSharper restore UnassignedGetOnlyAutoProperty
        #endregion
        #region Options
        [Option(LongName = "assembly-path")]
        public string AssemblyPath { get; set; } = "";
        
        [Option]
        public string DeploymentPath { get; } = "public";

        [Option(LongName = "project")]
        public string ProjectFilename { get; } = "project";

        [Option(LongName = "skip-pre-build")]
        public bool SkipPreBuild { get; } = false;
        #endregion

        private ILogger Log { get; set; }
        
        void OnExecute()
        {
            // Initialize a logging factory and get a logger for ourselves
            ApplicationLogging.Initialize();
            Log = ApplicationLogging.Factory.CreateLogger("ReaperKing.Builder");

            // Clean up arguments
            AssemblyPath = PathUtils.EnsureRooted(AssemblyPath);
            string projectFilename = ProjectFilename + ".yaml";
            Log.LogInformation("Reaper King building tool");
            
            // Load the project configuration from a file
            Log.LogInformation("Project configuration is now being loaded");
            Project project = ParsingUtils.ReadYamlFile<Project>(projectFilename);
            project = SetProjectEnvironment(project, EnvironmentName);
            project.ContentDirectory = new FileInfo(projectFilename).Directory?.FullName;
            project.AssemblyDirectory = AssemblyPath;
            
            // Insert a custom assembly resolver if Assembly Path is given.
            if (!String.IsNullOrEmpty(AssemblyPath))
            {
                AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblyInCustomSearchPath;
            }
            
            // Load the project assembly and find the Site class.
            Log.LogInformation("Static configuration assembly is now being loaded");
            var siteAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(SiteAssemblyName));
            var siteClass = GetSiteClassFromAssembly(siteAssembly);

            Log.LogInformation($"Found a build recipe in the assembly: {siteClass.FullName}");
            Log.LogInformation("Static Site Configuration object is being created");
            var instance = Activator.CreateInstance(siteClass, project, ApplicationLogging.Factory);
            if (!(instance is Site site))
            {
                Log.LogCritical("Static Configuration instance is not valid.");
                return;
            }
            
            // Add the assembly to RazorLight's metadata references
            var metadataReferences = site.RazorEngine.Handler.Options.AdditionalMetadataReferences;
            metadataReferences.Add(MetadataReference.CreateFromFile(siteAssembly.Location));
            
            foreach (var otherAssembly in siteAssembly.GetReferencedAssemblies())
            {
                if (otherAssembly.Name != null && otherAssembly.Name.StartsWith("ReaperKing"))
                {
                    var otherAssemblyPath = Assembly.Load(otherAssembly).Location;
                    metadataReferences.Add(MetadataReference.CreateFromFile(otherAssemblyPath));
                }
            }
            
            Log.LogInformation("Executing pre-build tasks");
            {
                var prebuildCmds = project.Build.RunBefore;
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

                site.PreBuild();
            }
            
            Log.LogInformation("Building site content");
            site.Build();
            
            Log.LogInformation("Executing post-build tasks");
            site.PostBuild();
            
            Log.LogInformation("Site has been successfully built.");
        }

        /**
         * Finds an assembly in the directory of the site
         * assembly, if the runtime fails to locate one on
         * its own.
         *
         * Not exactly safe, but given that the Static Config
         * assembly is already loaded and this is a fallback,
         * there's not much to lose.
         */
        private Assembly LoadAssemblyInCustomSearchPath(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            
            if (args != null && !string.IsNullOrEmpty(args.Name))
            {
                var assemblyName = args.Name.Split(new string[] { "," }, StringSplitOptions.None)[0];
                var assemblyPath = Path.Combine(AssemblyPath, $"{assemblyName}.dll");
                if (File.Exists(assemblyPath))
                {
                    result = Assembly.LoadFrom(assemblyPath);
                }
            }

            return result;
        }

        /**
         * Finds a class with the SiteAttribute in an assembly.
         * Only one is permitted per assembly.
         */
        private static Type GetSiteClassFromAssembly(Assembly siteAssembly)
        {
            foreach (Type type in siteAssembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(SiteRecipeAttribute), true).Length > 0)
                {
                    return type;
                }
            }

            return null;
        }

        /**
         * Loads a project environment configuration while taking
         * care of the basing.
         * 
         * Rather expensive, as a single call may parse each
         * tree node (file) twice, but there's not much reason
         * to optimize it at the moment as this method is
         * expected to be rarely executed in program's lifetime.
         */
        private Project SetProjectEnvironment(Project project, string environmentName)
        {
            // Load requested project to check its inheritance.
            string filename = $"{ProjectFilename}.{environmentName}.yaml";
            Project environment = ParsingUtils.ReadYamlFile<Project>(filename);

            // Load base configuration if one is specified
            if (!String.IsNullOrEmpty(environment.Inherits) && environment.Inherits != "base")
            {
                project = SetProjectEnvironment(project, environment.Inherits);
            }

            // Load requested project while overriding existing
            // fields.
            project = ParsingUtils.ReadYamlFile<Project>(filename, project);
            
            // Append pre-build commands
            if (project.Build.AddRunBeforeCmds != null && project.Build.AddRunBeforeCmds.Length > 0)
            {
                project.Build.RunBefore.AddRange(project.Build.AddRunBeforeCmds);
                project.Build.AddRunBeforeCmds = null;
            }
            
            // Append defines
            if (project.Build.AddDefines != null && project.Build.AddDefines.Length > 0)
            {
                project.Build.Define.AddRange(project.Build.AddDefines);
                project.Build.AddDefines = null;
            }
            
            return project;
        }
    }
}