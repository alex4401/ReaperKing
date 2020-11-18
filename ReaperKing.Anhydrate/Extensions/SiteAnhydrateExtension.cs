using ReaperKing.Core;

namespace ReaperKing.Anhydrate.Extensions
{
    public static class SiteAnhydrateExtension
    {
        private const string Namespace = "ReaperKing.Anhydrate";
        private const string RealDirectory = "ReaperKing.Anhydrate";
        
        public static void EnableAnhydrateTemplates(this Site site)
        {
            site.AddTemplateIncludeNamespace(Namespace, site.GetInternalResourcePath(RealDirectory));
        }
    }
}