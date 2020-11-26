namespace ReaperKing.Core
{
    public abstract class RkResourceProcessorModule : RkModule
    {
        protected RkResourceProcessorModule(Site site)
            : base(site)
        { }
        
        public abstract void ProcessResource(string filePath, ref string diskPath, ref string uri);
    }
}