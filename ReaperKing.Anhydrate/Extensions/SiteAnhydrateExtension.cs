using System;
using ReaperKing.Core;

namespace ReaperKing.Anhydrate.Extensions
{
    public static class SiteAnhydrateExtension
    {
        private const string Namespace = "ReaperKing.Anhydrate";
        private const string RealDirectory = "ReaperKing.Anhydrate";
        
        public static void EnableAnhydrateTemplates(this Site site)
        {
            string selfDir = site.GetInternalResourcePath(RealDirectory);
            // TODO: Make this more generic.
            foreach (string define in site.ProjectConfig.Build.Define)
            {
                if (define.StartsWith("ANHYDRATE_PATH"))
                {
                    selfDir = define.Split('=', 2)[1];
                    break;
                }
            }
            
            site.AddTemplateIncludeNamespace(Namespace, selfDir);
        }
    }
}