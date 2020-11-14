using System.Collections.Generic;
using System.Linq;
using ReaperKing.StaticConfig.Data;
using ReaperKing.StaticConfig.Data.ARK;
using ReaperKing.Core;

namespace ReaperKing.StaticConfig.ContentGeneration.WikiTools
{
    public class WikiToolsContentProvider : ISiteContentProvider
    {
        public WikiToolsContentProvider()
        { }

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