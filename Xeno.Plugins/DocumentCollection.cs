/*!
 * This file is a part of Xeno, and the project's repository may be found at https://github.com/alex4401/rk.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * http://www.gnu.org/licenses/.
 */

using System.Collections.Generic;

using Xeno.Core;
using Xeno.Core.Configuration;

namespace Xeno.Plugins
{
    public struct DocumentMetadata
    {
        public DocumentGenerationResult Meta { get; init; }
        public string Uri { get; init; }
    }
    
    public class RkDocumentCollectionModule
        : IRkDocumentProcessorModule
    {
        public List<DocumentMetadata> Collected { get; } = new();
        
        public RkDocumentCollectionModule(Site site)
        { }

        public void AcceptConfiguration(ProjectConfigurationManager config)
        { }

        public void PostProcessDocument(string uri, ref IntermediateGenerationResult result)
        {
            Collected.Add(new DocumentMetadata
            {
                Meta = result.Meta,
                Uri = result.Uri,
            });
        }
    }
}