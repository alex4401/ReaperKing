using System;

namespace Noglin.Core
{
    [AttributeUsage(AttributeTargets.Struct, Inherited = false)]
    public class PackageTypeAttribute : Attribute
    {
        public string PublicTypeName { get; }
        public string Tag { get; }

        public PackageTypeAttribute(string publicTypeName, string tag)
        {
            PublicTypeName = publicTypeName;
            Tag = tag;
        }
    }
}