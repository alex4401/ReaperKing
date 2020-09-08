using ProjectReaperKing.Data;
using ProjectReaperKing.Data.ARK;
using ProjectReaperKing.Models;
using SiteBuilder.Core;

namespace ProjectReaperKing.Pages.ARK
{
    public class ModHomeGenerator : IPageGenerator
    {
        private readonly ModInfo _arkMod;

        public ModHomeGenerator(ModInfo arkMod)
        {
            _arkMod = arkMod;
        }

        public PageGenerationResult Generate(Site site, string parentUri)
        {
            return new PageGenerationResult()
            {
                Name = "index",
                Template = "mods/home.cshtml",
                Model = new ModHomeModel
                {
                    Super = new BaseModel
                    {
                        SiteName = _arkMod.Name,
                        DisplayTitle = $"{_arkMod.Name}, interactive spawning maps",
                        RootUri = parentUri,
                    },
                    
                    ModInfo = _arkMod,
                    Maps = DataManagerARK.Instance.LoadedMaps,
                },
            };
        }
    }
}