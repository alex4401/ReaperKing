using System;

namespace Noglin.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class NoglinSchemaAttribute : Attribute
    {
        public string Name { get; }

        public NoglinSchemaAttribute(string name)
            => Name = name;

    }
}