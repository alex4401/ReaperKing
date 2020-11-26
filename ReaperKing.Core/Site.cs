using System;
using System.IO;
using Microsoft.Extensions.Logging;
using RazorLight;

using ReaperKing.Core.Razor;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        public Project ProjectConfig { get; }
        public ILogger Log { get; }
        public string ContentRoot => ProjectConfig.ContentDirectory;
        public string AssemblyRoot => ProjectConfig.AssemblyDirectory;
        public string DeploymentPath => ProjectConfig.Paths.Deployment;

        /**
         * RazorLight project instance.
         * Scoped filesystem project is used to provide namespace
         * support and allow overlapping mounts.
         */
        private readonly RazorScopedFilesystemProject _razorProject;
        
        /**
         * Instance of the RazorLight engine.
         */
        public RazorLightEngine RazorEngine { get; }

        public Site(Project project, ILogger logger)
        {
            ProjectConfig = project;
            Log = logger;
            
            _razorProject = new RazorScopedFilesystemProject(Path.Join(ContentRoot, "templates"));
            RazorEngine = new RazorLightEngineBuilder()
                .UseProject(_razorProject)
                .UseMemoryCachingProvider()
                .Build();
        }

        /**
         * Checks if a constant is defined in project configuration.
         */
        public bool IsProjectConstantDefined(string id)
        {
            return ProjectConfig.Build.Define != null && ProjectConfig.Build.Define.Contains(id);
        }

        #region Building virtuals
        /**
         * Prepares for content building. This method is executed
         * before Build().
         */
        public virtual void PreBuild()
        {
            var nvResources = ProjectConfig.Resources.CopyNonVersioned;

            if (nvResources.Length > 0)
            {
                Log.LogInformation("Copying non-versioned resources");
                foreach (var file in nvResources)
                {
                    CopyResource(file, file);
                }
            }
        }

        /**
         * Builds the site content.
         */
        public virtual void Build()
        { }

        /**
         * Wraps up content building process. This method is executed
         * after Build().
         */
        public virtual void PostBuild()
        { }
        #endregion
        
        #region Razor template namespacing management
        public void AddTemplateIncludeNamespace(string ns, string root)
        {
            _razorProject.Mount(new RazorIncludePathInfo
            {
                Namespace = ns,
                RealRoot = PathUtils.EnsureRooted(root),
            });
        }

        public void TryAddTemplateIncludeNamespace(string ns, string root)
        {
            _razorProject.MountUnsafe(new RazorIncludePathInfo
            {
                Namespace = ns,
                RealRoot = PathUtils.EnsureRooted(root),
            });
        }

        public void AddTemplateDefaultIncludePath(string root)
        {
            _razorProject.Mount(PathUtils.EnsureRooted(root));
        }

        public void TryAddTemplateDefaultIncludePath(string root)
        {
            _razorProject.MountUnsafe(PathUtils.EnsureRooted(root));
        }

        public void RemoveTemplateNamespace(string ns)
        {
            _razorProject.DestroyNamespace(ns);
        }

        public void RemoveTemplateNamespace(string ns, string path)
        {
            _razorProject.DestroyNamespace(ns, path);
        }

        public void RemoveTemplateDefaultIncludePath(string root)
        {
            _razorProject.DestroyNamespace("", PathUtils.EnsureRooted(root));
        }
        #endregion
    }
}