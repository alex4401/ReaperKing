using ProjectReaperKing.Data;
using ProjectReaperKing.Data.ARK;
using ProjectReaperKing.Models;
using SiteBuilder.Core;

namespace ProjectReaperKing.ContentGeneration.ARK
{
    public class EpicIniGenerator : IPageGenerator
    {
        private readonly ModInfo _arkMod;
        private ModInfo.Revision _revision;

        public EpicIniGenerator(ModInfo arkMod, ModInfo.Revision revision)
        {
            _arkMod = arkMod;
            _revision = revision;
        }

        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult
            {
                Name = "egs",
                Template = "mods/egs.cshtml",
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