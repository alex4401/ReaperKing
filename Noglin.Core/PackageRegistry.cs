using System.Collections.Generic;

namespace Noglin.Core
{
    public class PackageRegistry
    {
        private struct PackageInfo
        {
            public string FilePath { get; init; }
            public object Value { get; init; }
        }
        
        private IPackageLoader Loader { get; init; }
        private List<PackageInfo> Packages { get; init; } = new();

        public PackageRegistry(IPackageLoader loader)
            => Loader = loader;

        public void AddType<T>()
            => Loader.AddType<T>();

        public object LoadAnonymousPackage(string filepath)
        {
            object obj = Loader.LoadFile(filepath);
            Packages.Add(new()
            {
                FilePath = filepath,
                Value = obj,
            });

            return obj;
        }
        
        public T LoadPackage<T>(string filepath)
            where T : class
        {
            T obj = Loader.LoadFile<T>(filepath);
            Packages.Add(new()
            {
                FilePath = filepath,
                Value = obj,
            });

            return obj;
        }

        public IEnumerable<T> Find<T>()
        {
            foreach (PackageInfo info in Packages)
            {
                if (info.Value is T obj)
                {
                    yield return obj;
                }
            }
        }
    }
}