using System.Collections.Generic;
using System.Linq;
using ProjectReaperKing.Data;
using ProjectReaperKing.Data.ARK;
using SiteBuilder.Core;

namespace ProjectReaperKing.Pages.WikiTools
{
    public class WikiToolsContentProvider : ISiteContentProvider
    {
        public WikiToolsContentProvider()
        { }

        public void BuildContent(SiteContext ctx)
        {
            using (ctx.AddOptionalTemplateDirectory("templates/wiki"))
            {
                var creatureStats = new CreatureStats();
                ctx.BuildPage(creatureStats);
            }
        }
    }
}