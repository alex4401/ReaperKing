using System;
using System.IO;
using RazorLight;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        public RazorLightEngine GetRazor() => _razorEngine;

        public void AddTemplateDirectory(string root)
        {
            _razorProject.AddRoot(Path.Join(Environment.CurrentDirectory, root));
        }

        public void AddOptionalTemplateDirectory(string root)
        {
            _razorProject.AddOptionalRoot(Path.Join(Environment.CurrentDirectory, root));
        }

        public void RemoveTemplateDirectory(string root)
        {
            _razorProject.RemoveRoot(Path.Join(Environment.CurrentDirectory, root));
        }
    }
}