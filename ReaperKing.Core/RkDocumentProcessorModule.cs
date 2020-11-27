using System;

namespace ReaperKing.Core
{
    public abstract class RkDocumentProcessorModule : RkModule
    {
        protected RkDocumentProcessorModule(Type selfType, Site site)
            : base(selfType, site)
        { }
        
        public abstract void PostProcessDocument(string uri, ref IntermediateGenerationResult result);
    }
}