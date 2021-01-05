using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Noglin.Core;

namespace Noglin.Ark
{
    public static class PackageRegistryExtension
    {
        public const string Core = "";

        public static IEnumerable<T> FindByModId<T>(this PackageRegistry registry, ulong modId)
            where T : JsonPackageSchema
        {
            foreach (T package in FindByModId<T>(registry, modId.ToString()))
            {
                yield return package;
            }
        }

        public static IEnumerable<T> FindByModId<T>(this PackageRegistry registry, string modId)
            where T : JsonPackageSchema
        {
            foreach (T package in registry.Find<T>())
            {
                if (package.Mod.Id == modId)
                {
                    yield return package;
                }
            }
        }
    }
}