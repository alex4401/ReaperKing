using System;

namespace Noglin.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class NoglinYamlAttribute : Attribute
    {
        public string Name { get; }

        public NoglinYamlAttribute(string name)
            => Name = name;

    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class NoglinJsonAttribute : NoglinYamlAttribute
    {
        public int Version { get; }

        public NoglinJsonAttribute(string name, int version)
            : base(name)
            => Version = version;

    }
}