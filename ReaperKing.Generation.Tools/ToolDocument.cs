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

using ReaperKing.Anhydrate;
using ReaperKing.Anhydrate.Models;

namespace ReaperKing.Generation.Tools
{
    public abstract class ToolDocument<T> : AnhydrateDocument<T>
        where T : AnhydrateModel
    {
        public override NavigationItem[] GetNavigation()
        {
            return new[]
            {
                new NavigationItem("Creature Stats", $"{Context.GetRootUri()}/creature-stats.html"),
                new NavigationItem("Colors", $"{Context.GetRootUri()}/color-table.html"),
                new NavigationItem("Vex", $"{Context.GetRootUri()}/vex.html"),
            };
        }

        public override FooterInfo GetFooter()
            => new() 
            {
                CopyrightMessage = @"2020 <a href=""https://github.com/alex4401"">alex4401</a>",
                Paragraphs = new[] {
                    "This tool is made for private use and, unless listed below, " +
                    "you may not be allowed to use it.",
                    
                    "• ARK: Survival Evolved administration team @ ark.gamepedia.com",
                    "• Primal Fear team",
                    "• Ark Eternal wiki team @ ark.gamepedia.com",
                },
            };
    }
}