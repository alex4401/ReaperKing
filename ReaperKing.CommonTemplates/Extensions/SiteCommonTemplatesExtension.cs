using System;
using System.IO;
using System.Reflection;
using ReaperKing.Core;

namespace ReaperKing.CommonTemplates.Extensions
{
    public static class SiteCommonTemplatesExtension
    {
        private const string Namespace = "ReaperKing.CommonTemplates";
        private const string RealDirectory = "ReaperKing.CommonTemplates";
        
        public static void EnableCommonTemplates(this Site site)
        {
            site.AddTemplateIncludeNamespace(Namespace, site.GetInternalResourcePath(RealDirectory));
        }
    }
}