using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CodeAnalysis;
using ShellProgressBar;
using SiteBuilder.Core;

namespace SiteBuilder
{
    class Program
    {
        private static readonly ProgressBarOptions BarOptions = Site.BarOptions;
        
        static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0)]
        [Required]
        public string AssemblyName { get; }

        [Option]
        public string DeploymentPath { get; } = "public";

        void OnExecute()
        {
            using (var pbar = new ProgressBar(2, "Building site", BarOptions))
            {
                Site site;
                Project project;
                
                using (var pbar2 = pbar.Spawn(1, "Loading project information", BarOptions))
                {
                    project = ParsingUtils.ReadYamlFile<Project>("project.yaml");
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
    }
}