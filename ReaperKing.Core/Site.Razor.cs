using System;
using System.IO;
using RazorLight;
using ReaperKing.Core.Razor;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        public RazorLightEngine GetRazor() => _razorEngine;

        [Obsolete("Replaced with AddTemplateDefaultIncludePath")]
        public void AddTemplateDirectory(string root)
        {
            AddTemplateDefaultIncludePath(root);
        }

        [Obsolete("Replaced with TryAddTemplateDefaultIncludePath")]
        public void AddOptionalTemplateDirectory(string root)
        {
            TryAddTemplateDefaultIncludePath(root);
        }

        [Obsolete("Replaced with RemoveTemplateDefaultIncludePath")]
        public void RemoveTemplateDirectory(string root)
        {
            RemoveTemplateDefaultIncludePath(root);
        }

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
    }
}