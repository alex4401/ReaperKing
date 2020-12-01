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

using ReaperKing.Core;
using ReaperKing.CommonTemplates.Models;

namespace ReaperKing.CommonTemplates
{
    public class RedirectGenerator : IDocumentGenerator
    {
        public string Uri { get; init; }
        public string Name { get; init; }
        public string TargetUrl { get; init; }
        
        public DocumentGenerationResult Generate(SiteContext ctx)
        {
            return new()
            {
                Uri = Uri,
                Name = Name,
                Template = "ReaperKing.CommonTemplates/redirect.cshtml",
                Model = new RedirectModel(ctx)
                {
                    Target = TargetUrl,
                },
            };
        }
    }
}