using SiteBuilder.Core;

namespace ProjectReaperKing.Pages.WikiTools
{
    public class CreatureStats : IPageGenerator
    {
        public PageGenerationResult Generate(Site site, string parentUri)
        {
            return new PageGenerationResult
            {
                Name = "creature-stats",
                Template = "wiki/creatureStats.cshtml",
                Model = new 
                {
                    Super = new BaseModel
                    {
                        SiteName = "Wiki Tools",
                        DisplayTitle = "{{CreatureStats}}",
                    },
                    
                    Web = site.GetCommonInfo(),
                    BaseUri = parentUri,
                },
            };
        }
    }
}