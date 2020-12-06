using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;

namespace ReaperKing.Builder
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed partial class Program
    {
        private void AllowAssembliesFromUserPath()
        {
            // Insert a custom assembly resolver if Assembly Path is given.
            if (!String.IsNullOrEmpty(AssemblyPath))
            {
                AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblyInCustomSearchPath;
            }
        }

        private (Assembly, Type) GetSiteBuildRecipeType()
        {
            Assembly siteAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(SiteAssemblyName));
            Type siteClassType = GetSiteClassFromAssembly(siteAssembly);
            Log.LogInformation($"Found a build recipe in the assembly: {siteClassType.FullName}");
            return (siteAssembly, siteClassType);
        }

        private void AddTypeMetadataToRazor(Assembly assembly)
        {
            var metadataReferences = SiteObject.RazorEngine.Handler.Options.AdditionalMetadataReferences;
            metadataReferences.Add(MetadataReference.CreateFromFile(assembly.Location));
            
            foreach (var otherAssembly in assembly.GetReferencedAssemblies())
            {
                if (otherAssembly.Name != null && otherAssembly.Name.StartsWith("ReaperKing"))
                {
                    var otherAssemblyPath = Assembly.Load(otherAssembly).Location;
                    metadataReferences.Add(MetadataReference.CreateFromFile(otherAssemblyPath));
                }
            }
        }

        /**
         * Finds an assembly in the directory of the site
         * assembly, if the runtime fails to locate one on
         * its own.
         *
         * Not exactly safe, but given that the Static Config
         * assembly is already loaded and this is a fallback,
         * there's not much to lose.
         */
        private Assembly LoadAssemblyInCustomSearchPath(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            
            if (args != null && !string.IsNullOrEmpty(args.Name))
            {
                var assemblyName = args.Name.Split(new string[] { "," }, StringSplitOptions.None)[0];
                var assemblyPath = Path.Combine(AssemblyPath, $"{assemblyName}.dll");
                if (File.Exists(assemblyPath))
                {
                    result = Assembly.LoadFrom(assemblyPath);
                }
            }

            return result;
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