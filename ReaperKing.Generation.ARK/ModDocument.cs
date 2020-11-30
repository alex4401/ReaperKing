using System;
using System.IO;
using ReaperKing.Anhydrate;
using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.ARK.Models;

namespace ReaperKing.Generation.ARK
{
    public abstract class ModDocument<T> : AnhydrateDocument<T>
        where T : AnhydrateModel
    {
        protected ModInfo Mod { get; }

        public ModDocument(ModInfo arkMod)
        {
            Mod = arkMod;
        }

        public override NavigationItem[] GetNavigation()
        {
            return new[]
            {
                new NavigationItem("Spawn Maps", Context.GetRootUri()),
                new NavigationItem("Workshop", $"https://steamcommunity.com/sharedfiles/filedetails/?id={Mod.SteamId}"),
                new NavigationItem("Epic.INI", $"{Context.GetRootUri()}/egs.html",
                                   Context.IsConstantDefined(StaticSwitchesArk.EpicIni)),
            };
        }
        
        public override string GetOpenGraphsType() => "website";

        public override string GetOpenGraphsImage()
        {
            if (String.IsNullOrEmpty(Mod.OgImageResource))
            {
                return "";
            }

            string fileExtension = Path.GetExtension(Mod.OgImageResource);
            string fileName = Path.GetFileNameWithoutExtension(Mod.OgImageResource);
            
            return Context.CopyVersionedResource(Mod.OgImageResource,
                                                 $"/og_images/{fileName}-[hash]{fileExtension}");
        }

        public override FooterInfo GetFooter()
            => new() 
            {
                CopyrightMessage = @"2020 <a href=""https://github.com/alex4401"">alex4401</a>",
            };
    }
}