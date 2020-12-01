using System.Collections.Generic;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public struct DocumentMetadata
    {
        public DocumentGenerationResult Meta { get; init; }
        public string Uri { get; init; }
    }
    
    public class RkDocumentCollectionModule : RkDocumentProcessorModule
    {
        public List<DocumentMetadata> Collected { get; } = new();
        
        public RkDocumentCollectionModule(Site site)
            : base(typeof(RkDocumentCollectionModule), site)
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