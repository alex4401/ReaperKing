using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CodeAnalysis;
using SharpYaml.Serialization;
using ShellProgressBar;
using SiteBuilder.Core;

namespace SiteBuilder
{
    class Program
    {
        private static readonly ProgressBarOptions BarOptions = Site.BarOptions;
        
        static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        #region Required Arguments & Options
        // ReSharper disable UnassignedGetOnlyAutoProperty
        
        [Argument(order: 0)]
        [Required]
        public string AssemblyName { get; }
        
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
            using (var pbar = new ProgressBar(2, "Building site", BarOptions))
            {
                Site site;
                Project project;
                
                using (var pbar2 = pbar.Spawn(1, "Loading project information", BarOptions))
                {
                    project = ParsingUtils.ReadYamlFile<Project>(ProjectFilename + ".yaml");
                    project = SetProjectEnvironment(project, EnvironmentName);
                    pbar2.Tick("Project of the site loaded");
                }
                
                using (var pbar2 = pbar.Spawn(2, "Loading .NET assembly", BarOptions))
                {
                    var assemblyPath = Path.Join(Environment.CurrentDirectory, AssemblyName);
                    var siteAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                    var siteClass = GetSiteClassFromAssembly(siteAssembly);
                    pbar2.Tick("Initializing site info");

                    site = Activator.CreateInstance(siteClass) as Site;
                    site.Initialize(project);

                    var metadataReference = MetadataReference.CreateFromFile(assemblyPath);
                    site.GetRazor().Handler.Options.AdditionalMetadataReferences.Add(metadataReference);
                        
                    pbar2.Tick(".NET assembly of the site loaded");
                }

                pbar.Tick("Building site content");
                site.Build(pbar);

                pbar.Tick();
            }

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