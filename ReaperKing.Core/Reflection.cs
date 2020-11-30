using System;

namespace ReaperKing.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SiteRecipeAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class RkConfigurableAttribute : Attribute
    {
        public string Namespace { get; }
        public Type[] Properties { get; }
        
        public RkConfigurableAttribute(string ns, Type[] properties)
            => (Namespace, Properties) = (ns, properties);
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RkProjectPropertyAttribute : Attribute
    {
        public string Name { get; }

        public RkProjectPropertyAttribute(string name)
            => (Name) = name;
    }

}