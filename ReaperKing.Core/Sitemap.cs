using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using ReaperKing.Core.Plugins;

namespace ReaperKing.Core
{
    public class SitemapGenerator : IPageGenerator
    {
        private RkDocumentCollectionModule _collectionModule;
        
        public SitemapGenerator(RkDocumentCollectionModule collectionModule)
        {
            _collectionModule = collectionModule;
        }
        
        /**
         * Generates a sitemap from metadata collected by the
         * Document Collection Module.
         */
        public PageGenerationResult Generate(SiteContext ctx)
        {
            // Create a temporary in-memory stream and an XML
            // writer with enabled indentation.
            using var memStream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(memStream, new XmlWriterSettings
            {
                Indent = true,
            });
            
            // Write the UrlSet element as per the schema.
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (var page in _collectionModule.Collected)
            {
                // Skip the page if it's set to be skipped
                // in sitemaps in the metadata.
                if (page.Meta.SkipInSitemap)
                {
                    continue;
                }
                
                // Write a Url element with a Loc sub-element.
                // Inner text of the Loc element has a URI
                // of the target page.
                writer.WriteStartElement("url");
                writer.WriteStartElement("loc");
                writer.WriteString(page.Uri);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            // End the document.
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return new PageGenerationResult
            {
                Extension = "xml",
                Name = "sitemap",
                Text = Encoding.UTF8.GetString(memStream.ToArray()),
            };
        }
    }
}