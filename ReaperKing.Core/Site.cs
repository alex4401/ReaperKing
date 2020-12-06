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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;
using RazorLight;

using ReaperKing.Core.Razor;

namespace ReaperKing.Core
{
    [RkConfigurable(new[]
    {
        typeof(WebConfiguration),
        typeof(ResourcesConfiguration),
        typeof(ImmutableRuntimeConfiguration),
    })]
    public abstract partial class Site
    {
        public ProjectConfigurationManager ProjectConfig { get; }
        public ImmutableRuntimeConfiguration ImmutableConfig { get; }
        public WebConfiguration WebConfig { get; }
        
        public string ContentRoot => ImmutableConfig.ContentRoot;
        public string AssemblyRoot => ImmutableConfig.AssemblyRoot;
        public string DeploymentPath => ImmutableConfig.DeploymentPath;
        
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
        public Site(ProjectConfigurationManager project,
                    ILoggerFactory loggerFactory)
            : this(typeof(Site), project, loggerFactory)
        { }

        protected Site(Type selfType,
                       ProjectConfigurationManager project,
                       ILoggerFactory loggerFactory)
        {
            (ProjectConfig, LogFactory) = (project, loggerFactory);
            Log = LogFactory.CreateLogger(selfType.FullName);
            
            ImmutableConfig = project.Get<ImmutableRuntimeConfiguration>();
            WebConfig = project.Get<WebConfiguration>();
            
            _razorProject = new RazorScopedFilesystemProject(Path.Join(ContentRoot, "templates"));
            RazorEngine = new RazorLightEngineBuilder()
                .UseProject(_razorProject)
                .UseMemoryCachingProvider()
                .Build();
        }

        #region Building virtuals
        /**
         * Prepares for content building. This method is executed
         * before Build().
         */
        public virtual void PreBuild()
        {
            ResourcesConfiguration resourceConfig = ProjectConfig.Get<ResourcesConfiguration>();
            var nvResources = resourceConfig.CopyNonVersioned;

            if (nvResources.Count > 0)
            {
                Log.LogInformation("Copying non-versioned resources");
                foreach (var file in nvResources)
                {
                    CopyResource(file, file);
                }
            }
            
            // Notify modules that the configuration is now available.
            foreach (RkModule module in _modules)
            {
                module.AcceptConfiguration(ProjectConfig);
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