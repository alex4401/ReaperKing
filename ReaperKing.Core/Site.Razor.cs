using System;
using RazorLight;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        [Obsolete("Use the RazorEngine property instead.")]
        public RazorLightEngine GetRazor() => RazorEngine;

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
    }
}