using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using RazorLight;
using RazorLight.Caching;
using ReaperKing.Core.Razor;
using ShellProgressBar;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        internal static Site Instance = null;
        
        public Project ProjectConfig;
        public ILogger Log;
        public string ContentRoot => ProjectConfig.ContentDirectory;
        public string AssemblyRoot => ProjectConfig.AssemblyDirectory;
        public string DeploymentPath => ProjectConfig.Paths.Deployment;
        
        private RazorScopedFilesystemProject _razorProject = null;
        private RazorLightEngine _razorEngine = null;

        public virtual void Initialize(Project project, ILogger logger)
        {
            Instance = this;
            ProjectConfig = project;
            Log = logger;
            
            _razorProject = new RazorScopedFilesystemProject(Path.Join(ContentRoot, "templates"));
            _razorEngine = new RazorLightEngineBuilder()
                .UseProject(_razorProject)
                .UseMemoryCachingProvider()
                .Build();
        }

        public virtual void PreBuild()
        {
            var prebuildCmds = ProjectConfig.Build.RunBefore;
            var nvResources = ProjectConfig.Resources.CopyNonVersioned;

            if (prebuildCmds.Count > 0)
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

            if (nvResources.Length > 0)
            {
                Log.LogInformation("Copying non-versioned resources");
                foreach (var file in nvResources)
                {
                    CopyResource(file, file);
                }
            }
        }

        public virtual void Build()
        { }

        public virtual void PostBuild()
        { }
    }
}