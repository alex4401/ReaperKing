using ReaperKing.Core;

namespace ReaperKing.Generation.Tools
{
    public class ToolsContentProvider : ISiteContentProvider
    {
        public void BuildContent(SiteContext ctx)
        {
            using (ctx.AddOptionalTemplateDirectory("templates/wiki"))
            {
                
                ctx.BuildPage(new LegacyCreatureStats());
                ctx.BuildPage(new CreatureStats());
                ctx.BuildPage(new DvJsonFilterGenerator());
                ctx.BuildPage(new ColorTable());
            }
        }
    }
}