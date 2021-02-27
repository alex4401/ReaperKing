/*!
 * This file is a part of the Poglin project, whose repository may be found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General
 * Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Affero General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using System.IO;

using Xeno.Core;
using Xeno.CommonTemplates.Extensions;

namespace Poglin.Generation.Tools
{
    public class ToolsRedirectsProvider : IDocumentProvider
    {
        public void BuildContent(SiteContext ctx)
        {
            string root = ctx.Site.WebConfig.Root;
            
            ctx.RedirectPage("color-table", Path.Join(root, "ark/tools/color-table.html"));
            ctx.RedirectPage("creature-stats", Path.Join(root, "ark/tools/creature-stats.html"));
            ctx.RedirectPage("legacy", "stats", Path.Join(root, "ark/tools/legacy/stats.html"));
        }
    }
}