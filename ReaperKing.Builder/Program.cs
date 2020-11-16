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
    class Program
    {
        static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        #region Required Arguments & Options
        // ReSharper disable UnassignedGetOnlyAutoProperty

        [Argument(order: 0)]
        [Required]
        public string SiteAssemblyName { get; }

        [Option(LongName = "assembly-path")]
        public string AssemblyPath { get; set; } = "";
        
        [Option(LongName = "environment")]
        [Required]
        public string EnvironmentName { get; }

        // ReSharper restore UnassignedGetOnlyAutoProperty
        #endregion
        #region Options
        [Option]
        public string DeploymentPath { get; } = "public";

        [Option(LongName = "project")]
        public string ProjectFilename { get; } = "project";
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
            
            log.LogInformation("Site singleton is being created");
            Site site = Activator.CreateInstance(siteClass) as Site;
            site.Initialize(project, log);
            
            // Add the assembly to RazorLight's metadata references
            var metadataReferences = site.GetRazor().Handler.Options.AdditionalMetadataReferences;
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
        
        public Assembly LoadAssemblyInCustomSearchPath(object sender, ResolveEventArgs args)
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

        Type GetSiteClassFromAssembly(Assembly siteAssembly)
        {
            foreach (Type type in siteAssembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(SiteAttribute), true).Length > 0)
                {
                    return type;
                }
            }

            return null;
        }

        Project SetProjectEnvironment(Project project, string environmentName)
        {
            string filename = $"{ProjectFilename}.{environmentName}.yaml";
            Project environment = ParsingUtils.ReadYamlFile<Project>(filename);

            if (!String.IsNullOrEmpty(environment.Inherits) && environment.Inherits != "base")
            {
                project = SetProjectEnvironment(project, environment.Inherits);
            }

            project = ParsingUtils.ReadYamlFile<Project>(filename, project);
            
            if (project.Build.AddRunBeforeCmds != null && project.Build.AddRunBeforeCmds.Length > 0)
            {
                project.Build.RunBefore.AddRange(project.Build.AddRunBeforeCmds);
                project.Build.AddRunBeforeCmds = null;
            }
            
            if (project.Build.AddDefines != null && project.Build.AddDefines.Length > 0)
            {
                project.Build.Define.AddRange(project.Build.AddDefines);
                project.Build.AddDefines = null;
            }
            
            return project;
        }
    }
}