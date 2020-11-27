using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;
using RazorLight;

using ReaperKing.Core.Razor;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        public Project ProjectConfig { get; }
        public string ContentRoot => ProjectConfig.ContentDirectory;
        public string AssemblyRoot => ProjectConfig.AssemblyDirectory;
        public string DeploymentPath => ProjectConfig.Paths.Deployment;
        
        /**
         * A logger factory to be used by modules and other classes
         * dependent on the Site.
         */
        public ILoggerFactory LogFactory { get; }
        
        /**
         * A logger instance in the site's domain.
         */
        protected ILogger Log { get; }

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

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public Site(Project project, ILoggerFactory loggerFactory)
            : this(typeof(Site), project, loggerFactory)
        { }

        protected Site(Type selfType, Project project, ILoggerFactory loggerFactory)
        {
            (ProjectConfig, LogFactory) = (project, loggerFactory);
            Log = LogFactory.CreateLogger(selfType.FullName);
            
            _razorProject = new RazorScopedFilesystemProject(Path.Join(ContentRoot, "templates"));
            RazorEngine = new RazorLightEngineBuilder()
                .UseProject(_razorProject)
                .UseMemoryCachingProvider()
                .Build();
        }

        /**
         * Checks if a constant is defined in project configuration.
         */
        [Obsolete("Going forward, this will be replaced with methods directly on the Project Config.")]
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
        /**
         * Mounts a directory in a template inclusion namespace of choice.
         * 
         * Mounted root is expected to exist and method will throw otherwise.
         * Use TryAddTemplateIncludeNamespace if you need a "safe" version.
         */
        public void AddTemplateIncludeNamespace(string ns, string root)
        {
            _razorProject.Mount(new RazorIncludePathInfo
            {
                Namespace = ns,
                RealRoot = PathUtils.EnsureRooted(root),
            });
        }

        /**
         * Mounts a directory in a template inclusion namespace of choice
         * if the directory exists.
         * 
         * Mounted root is not expected to exist. If success should be
         * ensured, use AddTemplateIncludeNamespace instead.
         */
        public void TryAddTemplateIncludeNamespace(string ns, string root)
        {
            _razorProject.MountUnsafe(new RazorIncludePathInfo
            {
                Namespace = ns,
                RealRoot = PathUtils.EnsureRooted(root),
            });
        }

        /**
         * Mounts a directory in the default template inclusion namespace.
         * 
         * Mounted root is expected to exist and method will throw otherwise.
         * Use TryAddTemplateDefaultIncludePath if you need a "safe" version.
         */
        public void AddTemplateDefaultIncludePath(string root)
        {
            _razorProject.Mount(PathUtils.EnsureRooted(root));
        }

        /**
         * Mounts a directory in the default template inclusion namespace
         * if the directory exists.
         * 
         * Mounted root is not expected to exist. If success should be
         * ensured, use AddTemplateDefaultIncludePath instead.
         */
        public void TryAddTemplateDefaultIncludePath(string root)
        {
            _razorProject.MountUnsafe(PathUtils.EnsureRooted(root));
        }
        
        /**
         * Removes all mounts attached to a specific namespace.
         */
        public void RemoveTemplateNamespace(string ns)
        {
            _razorProject.DestroyNamespace(ns);
        }

        /**
         * Removes a specific mount (described by path) attached to
         * a specific namespace.
         */
        public void RemoveTemplateNamespace(string ns, string path)
        {
            _razorProject.DestroyNamespace(ns, path);
        }

        /**
         * Removes a specific mount (described by path) attached to
         * the default namespace.
         */
        public void RemoveTemplateDefaultIncludePath(string root)
        {
            _razorProject.DestroyNamespace("", PathUtils.EnsureRooted(root));
        }
        #endregion
    }
}