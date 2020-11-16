using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.ARK.Models;

namespace ReaperKing.Generation.ARK
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
                Model = new ModHomeModel(ctx)
                {
                    SiteName = _arkMod.Name,
                    DisplayTitle = $"{_arkMod.Name}, interactive spawning maps",
                    ModInfo = _arkMod,
                    Maps = DataManagerARK.Instance.LoadedMaps,
                },
            };
        }
    }
}