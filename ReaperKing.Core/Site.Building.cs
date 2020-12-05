/*!
 * This file is a part of Reaper King, and the project's repository may be
 * found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
 * the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using System;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        /**
         * Creates a SiteContext backed by this instance.
         */
        protected SiteContext GetContext(string uri = null)
        {
            return new()
            {
                Site = this,
                PathPrefix = uri,
            };
        }
        
        /**
         * Creates a temporary context to a IDocumentGenerator and writes
         * the result to disk.
         */
        public void EmitDocument(IDocumentGenerator generator, string uri = null)
        {
            var context = GetContext(uri);
            var result = generator.Generate(context);
            SavePage(result, uri);
        }
        
        /**
         * Creates and passes a context to a ISiteContentProvider.
         */
        public void EmitDocumentsFrom(ISiteContentProvider provider, string uri = null)
        {
            var context = GetContext(uri);
            provider.BuildContent(context);
        }
        
        [Obsolete("Replaced with EmitDocument(...). This alias will be removed at later date.")]
        public void BuildPage(IDocumentGenerator generator, string uri = null)
        {
            EmitDocument(generator, uri);
        }

        [Obsolete("Replaced with EmitDocumentsFrom(...). This alias will be removed at later date.")]
        public void BuildWithProvider(ISiteContentProvider provider, string uri = null)
        {
            EmitDocumentsFrom(provider, uri);
        }
    }
}