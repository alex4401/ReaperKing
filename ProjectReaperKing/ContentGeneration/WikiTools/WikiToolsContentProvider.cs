using System.Collections.Generic;
using System.Linq;
using ProjectReaperKing.Data;
using ProjectReaperKing.Data.ARK;
using SiteBuilder.Core;

namespace ProjectReaperKing.ContentGeneration.WikiTools
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