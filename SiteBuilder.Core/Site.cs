using System;
using System.Collections.Generic;
using System.IO;
using RazorLight;
using RazorLight.Caching;
using ShellProgressBar;
using SiteBuilder.Core.Razor;

namespace SiteBuilder.Core
{
    public abstract partial class Site
    {
        public static readonly ProgressBarOptions BarOptions = new ProgressBarOptions
        {
            ProgressCharacter = '─',
            ProgressBarOnBottom = true,
            CollapseWhenFinished = false
        };
        
        internal static Site Instance = null;
        private RazorScopedFilesystemProject _razorProject = null;
        private RazorLightEngine _razorEngine = null;
        public string DeploymentPath => ProjectConfig.Site.DeploymentDirectory;
        public Project ProjectConfig;

        public virtual void Initialize(Project project)
        {
            Instance = this;
            ProjectConfig = project;
            
            _razorProject = new RazorScopedFilesystemProject(new string[]
            {
                Environment.CurrentDirectory + "/templates",
            });
            _razorEngine = new RazorLightEngineBuilder()
                .UseProject(_razorProject)
                .UseMemoryCachingProvider()
                .Build();
        }

        public virtual void Build(ProgressBar pbar)
        {
            var prebuildCmds = ProjectConfig.Build.RunBefore;
            var nvResources = ProjectConfig.Site.NonVersionedResources;
            
            if (prebuildCmds.Length > 0)
            {
                pbar.MaxTicks += 1;

                var message = "Running pre-build commands";
                using (var pbar2 = pbar.Spawn(prebuildCmds.Length, message, BarOptions))
                {
                    foreach (var cmd in prebuildCmds)
                    {
                        pbar2.Tick($"{message}: {cmd}");
                        var exitCode = ShellHelper.Run(cmd);

                        if (exitCode != 0)
                        {
                            Environment.Exit(exitCode);
                        }
                    }
                }

                pbar.Tick();
            }

            if (nvResources.Length > 0)
            {
                pbar.MaxTicks += 1;

                var message = "Copying non-versioned resources";
                using (var pbar2 = pbar.Spawn(nvResources.Length, message, BarOptions))
                {
                    foreach (var file in nvResources)
                    {
                        pbar2.Tick($"{message}: {file}");
                        CopyResource(file, file);
                    }
                }

                pbar.Tick();
            }
        }
    }
}