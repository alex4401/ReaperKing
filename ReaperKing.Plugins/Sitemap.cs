/*!
 * This file is a part of Reaper King, and the project's repository may be found at
 * https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using System.IO;
using System.Text;
using System.Xml;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public class SitemapGenerator : IDocumentGenerator
    {
        private readonly RkDocumentCollectionModule _collectionModule;
        
        public SitemapGenerator(RkDocumentCollectionModule collectionModule)
            => _collectionModule = collectionModule;

        /**
         * Generates a sitemap from metadata collected by the Document Collection Module.
         */
        public DocumentGenerationResult Generate(SiteContext ctx)
        {
            // Create a temporary in-memory stream and an XML writer with enabled indentation.
            using var memStream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(memStream, new()
            {
                Indent = true,
                Encoding = Encoding.UTF8,
            });
            
            // Write the UrlSet element as per the schema.
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (var page in _collectionModule.Collected)
            {
                // Skip the page if it's set to be skipped in sitemaps in the metadata.
                if (page.Meta.SkipInSitemap)
                {
                    continue;
                }
                
                // Write a Url element with a Loc sub-element. Inner text of the Loc element has a URI of the target
                // page.
                writer.WriteStartElement("url");
                writer.WriteStartElement("loc");
                writer.WriteString(Path.Join(ctx.Site.WebConfig.ExternalAddress, page.Uri));
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            // End the document.
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return new()
            {
                Extension = "xml",
                Name = "sitemap",
                Text = Encoding.UTF8.GetString(memStream.ToArray()),
            };
        }
    }
}