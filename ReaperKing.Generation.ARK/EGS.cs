using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.ARK.Models;

namespace ReaperKing.Generation.ARK
{
    public class EpicIniGenerator : IDocumentGenerator
    {
        private readonly ModInfo _arkMod;
        private ModInfo.Revision _revision;

        public EpicIniGenerator(ModInfo arkMod, ModInfo.Revision revision)
        {
            _arkMod = arkMod;
            _revision = revision;
        }

        public DocumentGenerationResult Generate(SiteContext ctx)
        {
            return new DocumentGenerationResult
            {
                Name = "egs",
                Template = "mods/egs.cshtml",
                Model = new ModHomeModel(ctx)
                {
                    SectionName = _arkMod.Name,
                    DocumentTitle = $"{_arkMod.Name}, interactive spawning maps",
                    ModInfo = _arkMod,
                    Maps = DataManagerARK.Instance.LoadedMaps,
                },
            };
        }
    }
}