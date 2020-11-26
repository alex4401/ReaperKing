namespace ReaperKing.Core
{
    public abstract class RkDocumentProcessorModule : RkModule
    {
        protected RkDocumentProcessorModule(Site site)
            : base(site)
        { }
        
        public abstract void PostProcessDocument(string uri, ref IntermediateGenerationResult result);
    }
}