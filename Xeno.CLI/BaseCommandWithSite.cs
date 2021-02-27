using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using RazorLight;
using Xeno.Core;

namespace Xeno.CLI
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
        protected List<string> TrustedAssemblies { get; } = new();

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

        protected void InstantiateBakeRecipe()
        {
            Log.LogInformation("Static Site Configuration object is being created");
            object instance = Activator.CreateInstance(BakeRecipeType, Config, ApplicationLogging.Factory);
            if (!(instance is Site site))
            {
                Log.LogCritical("Static Configuration instance is not valid.");
                return;
            }

            BakeRecipe = site;
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

        protected void AddTypeMetadataToRazor(Assembly assembly)
        {
            if (TrustedAssemblies.Contains(assembly.Location))
            {
                return;
            }
            
            Log.LogDebug($"Assembly trusted: {assembly.FullName}");
            AddTypeMetadataReferenceToRazor(assembly.Location);
            TrustedAssemblies.Add(assembly.Location);
            
            foreach (AssemblyName refAssemblyName in assembly.GetReferencedAssemblies())
            {
                if (refAssemblyName.Name != null)
                {
                    string assemblyName = refAssemblyName.Name;
                    
                    if (assemblyName.StartsWith("ReaperKing.")
                        || BuildConfig.AllowRazorAssemblies.Contains(assemblyName)
                    )
                    {
                        Assembly refAssembly = Assembly.Load(refAssemblyName);
                        AddTypeMetadataToRazor(refAssembly);
                    }
                }
            }
        }

        private void AddTypeMetadataReferenceToRazor(string location)
        {
            MetadataReference mRef = MetadataReference.CreateFromFile(location);
            BakeRecipe.RazorEngine.Handler.Options.AdditionalMetadataReferences.Add(mRef);
        }
    }
}