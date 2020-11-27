using System;

namespace ReaperKing.Core
{
    public abstract class RkResourceResolverModule : RkModule
    {
        protected RkResourceResolverModule(Type selfType, Site site)
            : base(selfType, site)
        { }
        
        public abstract bool ResolveResource(string ns, string virtualPath, out string result);
    }
}