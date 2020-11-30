using ReaperKing.Anhydrate.Models;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.ARK.Models;

namespace ReaperKing.Generation.ARK
{
    public class ModHomeGenerator : ModDocument<ModHomeModel>
    {
        public ModHomeGenerator(ModInfo arkMod)
            : base(arkMod)
        { }

        public override string GetName() => "index";
        public override string GetTemplateName() => "/ARKMods/Home";
        public override FooterInfo GetFooter()
            => base.GetFooter() with {
                    Paragraphs = new[]
                    {
                        @"The topological maps come from " +
                        @"<a href=""https://ark.gamepedia.com"">the Official ARK Wiki</a>",
                    },
                };

        public override ModHomeModel GetModel() => new(Context)
        {
            SectionName = Mod.Name,
            DocumentTitle = "Spawn Maps",
            HeaderIconClass = "icon-mod",
                    
            ModInfo = Mod,
            Maps = DataManagerARK.Instance.LoadedMaps,
        };
    }
}