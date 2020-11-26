using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.ARK.Models;

namespace ReaperKing.Generation.ARK
{
    public abstract class ModContentGenerator : IPageGenerator
    {
        protected ModInfo Mod { get; }

        public ModContentGenerator(ModInfo arkMod)
        {
            Mod = arkMod;
        }

        public virtual NavigationItem[] GetNavigation(SiteContext ctx)
        {
            return new[]
            {
                new NavigationItem("Spawn Maps", ctx.GetRootUri()),
                new NavigationItem("Workshop", $"https://steamcommunity.com/sharedfiles/filedetails/?id={Mod.SteamId}"),
                new NavigationItem("Epic.INI", $"{ctx.GetRootUri()}/egs.html", ctx.IsConstantDefined(StaticSwitchesArk.EpicIni)),
            };
        }

        public abstract PageGenerationResult Generate(SiteContext ctx);
    }
}