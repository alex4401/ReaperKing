using System;

namespace Noglin.Core
{
    public interface IPackageLoader
    {
        public void AddType(Type type);
        public void AddType<T>();
        public object LoadFile(string path);
        public T LoadFile<T>(string path)
            where T : class;
    }
}