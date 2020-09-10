using System;
using RazorLight;

namespace SiteBuilder.Core
{
    public static class TemplatePageExtensions
    {
        [Obsolete]
        public static string LinkResource(this ITemplatePage template, string path)
        {
            return TemplateUtils.LinkResource(path);
        }
    }
}