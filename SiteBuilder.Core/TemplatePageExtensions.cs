using RazorLight;

namespace SiteBuilder.Core
{
    public static class TemplatePageExtensions
    {
        public static string LinkResource(this ITemplatePage template, string path)
        {
            return TemplateUtils.LinkResource(path);
        }
    }
}