using ReaperKing.Core;
using ReaperKing.CommonTemplates.Extensions;

namespace ReaperKing.Generation.Tools
{
    public class ToolsContentProvider : ISiteContentProvider
    {
        public void BuildContent(SiteContext ctx)
        {
            using (ctx.TryAddTemplateIncludeNamespace("ARKTools", "templates/tools"))
            using (ctx.TryAddTemplateDefaultIncludePath("templates/tools"))
            {
                ctx.BuildPage(new LegacyCreatureStats());
                ctx.BuildPage(new CreatureStats());
                ctx.BuildPage(new ColorTable());
            }
        }
    }
}