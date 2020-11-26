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
        
        void OnExecute()
        {
            AssemblyPath = PathUtils.EnsureRooted(AssemblyPath);
            string projectFilename = ProjectFilename + ".yaml";
            
            var log = ApplicationLogging.Initialize<Program>();
            log.LogInformation("Reaper King building tool");
            
            log.LogInformation("Project configuration is now being loaded");
            Project project = ParsingUtils.ReadYamlFile<Project>(projectFilename);
            project = SetProjectEnvironment(project, EnvironmentName);
            project.ContentDirectory = new FileInfo(projectFilename).Directory?.FullName;
            project.AssemblyDirectory = AssemblyPath;
            
            log.LogInformation("Static configuration assembly is now being loaded");
            if (!String.IsNullOrEmpty(AssemblyPath))
            {
                AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblyInCustomSearchPath;
            }

            var siteAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(SiteAssemblyName));
            var siteClass = GetSiteClassFromAssembly(siteAssembly);
            
            log.LogInformation("Static Site Configuration object is being created");
            var instance = Activator.CreateInstance(siteClass, project, log);
            if (!(instance is Site site))
            {
                log.LogCritical("Static Configuration instance is not valid.");
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
            
            
            log.LogInformation("Building site content");
            using (log.BeginScope("Pre-build tasks"))
            {
                var prebuildCmds = project.Build.RunBefore;
                if (prebuildCmds.Count > 0)
                {
                    if (!SkipPreBuild)
                    {
                        log.LogInformation("Executing commands scheduled to run before build");
                        foreach (var cmd in prebuildCmds)
                        {
                            log.LogInformation(cmd);
                            var exitCode = ShellHelper.Run(cmd);

                            if (exitCode != 0)
                            {
                                Environment.Exit(exitCode);
                            }
                        }
                    }
                    else
                    {
                        log.LogWarning("The pre-build tasks have been requested to be skipped.");
                    }
                }

                site.PreBuild();
            }
            
            using (log.BeginScope("Build tasks"))
            {
                site.Build();
            }
            
            using (log.BeginScope("Post-build tasks"))
            {
                site.PostBuild();
            }
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
                if (type.GetCustomAttributes(typeof(SiteAttribute), true).Length > 0)
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