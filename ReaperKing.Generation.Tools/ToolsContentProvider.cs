/*!
 * This file is a part of the open-sourced engine modules for
 * https://alex4401.github.io, and those modules' repository may be found
 * at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using ReaperKing.Core;

namespace ReaperKing.Generation.Tools
{
    public class ToolsContentProvider : ISiteContentProvider
    {
        public void BuildContent(SiteContext ctx)
        {
            using (ctx.TryAddTemplateIncludeNamespace("ARKTools", "templates/Tools"))
            using (ctx.TryAddTemplateIncludeNamespace("ARKTools", "templates/tools"))
            using (ctx.TryAddTemplateDefaultIncludePath("templates/tools"))
            {
                ctx.BuildPage(new LegacyCreatureStats());
                ctx.BuildPage(new CreatureStats());
                ctx.BuildPage(new ColorTable());
                ctx.BuildPage(new VexApplication());
            }
        }
    }
}