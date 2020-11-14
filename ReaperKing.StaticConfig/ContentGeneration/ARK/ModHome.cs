using ReaperKing.Core;
using ReaperKing.StaticConfig.Data;
using ReaperKing.StaticConfig.Data.ARK;
using ReaperKing.StaticConfig.Models;

namespace ReaperKing.StaticConfig.ContentGeneration.ARK
{
    public class ModHomeGenerator : IPageGenerator
    {
        private readonly ModInfo _arkMod;

        public ModHomeGenerator(ModInfo arkMod)
        {
            _arkMod = arkMod;
        }

        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult()
            {
                Name = "index",
                Template = "mods/home.cshtml",
                Model = new ModHomeModel
                {
                    Super = ctx.AcquireBaseModel(SiteName: _arkMod.Name,
                                                 DisplayTitle: $"{_arkMod.Name}, interactive spawning maps"),
                    
                    ModInfo = _arkMod,
                    Maps = DataManagerARK.Instance.LoadedMaps,
                },
            };
        }
    }
}