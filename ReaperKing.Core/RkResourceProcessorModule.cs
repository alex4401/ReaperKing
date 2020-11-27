using System;

namespace ReaperKing.Core
{
    public abstract class RkResourceProcessorModule : RkModule
    {
        protected RkResourceProcessorModule(Type selfType, Site site)
            : base(selfType, site)
        { }
        
        public abstract void ProcessResource(string filePath, ref string diskPath, ref string uri);
    }
}