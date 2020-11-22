using System.Collections.Generic;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public struct DocumentMetadata
    {
        public PageGenerationResult Meta { get; set; }
        public string Uri { get; set; }
    }
    
    public class RkDocumentCollectionModule : RkDocumentProcessorModule
    {
        public List<DocumentMetadata> Collected { get; } = new List<DocumentMetadata>();
        
        public RkDocumentCollectionModule(Site site) : base(site)
        { }

        public override void PostProcessDocument(string uri, ref IntermediateGenerationResult result)
        {
            Collected.Add(new DocumentMetadata
            {
                Meta = result.Meta,
                Uri = result.Uri,
            });
        }
    }
}