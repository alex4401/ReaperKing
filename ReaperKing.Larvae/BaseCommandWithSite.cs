using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using ReaperKing.Core;

namespace ReaperKing.Larvae
{
    internal abstract class BaseCommandWithSite : BaseCommandWithProject
    {
        [Argument(order: 0)]
        [Required]
        public string RecipeAssemblyName { get; private set; }

        [Option(LongName = "assembly-path")]
        public string RecipeAssemblyPath { get; private set; } = "";
        
        public Assembly BakeRecipeAssembly { get; private set; }
        public Type BakeRecipeType { get; private set; }
        public Site BakeRecipe { get; private set; }

        protected void LoadRecipeAssembly()
        {
            if (RecipeAssemblyName.Contains("/"))
            {
                Log.LogDebug("Splitting recipe assembly name into full path info");
                RecipeAssemblyPath = Path.GetDirectoryName(RecipeAssemblyName);
                RecipeAssemblyName = Path.GetFileName(RecipeAssemblyName);
            }
            
            if (!String.IsNullOrEmpty(RecipeAssemblyPath))
            {
                Log.LogDebug("Inserting custom path assembly resolver");
                UnsafeAssemblyLoad.AllowAssembliesFromUserPath(RecipeAssemblyPath);
            }

            AssemblyName name = new(RecipeAssemblyName);
            BakeRecipeAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(name);
            BakeRecipeType = GetSiteClassFromAssembly(BakeRecipeAssembly);
            
            Log.LogInformation($"Found a build recipe in the assembly: {BakeRecipeType.FullName}");
            Config.SchemaManager.ImportFromAssembly(BakeRecipeAssembly);
        }
        
        /**
         * Finds a class with the SiteAttribute in an assembly.
         * Only one is permitted per assembly.
         */
        private static Type GetSiteClassFromAssembly(Assembly siteAssembly)
        {
            foreach (Type type in siteAssembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(SiteRecipeAttribute), true).Length > 0)
                {
                    return type;
                }
            }

            return null;
        }
    }
}