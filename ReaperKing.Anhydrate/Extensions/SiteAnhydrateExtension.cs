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
using ReaperKing.Core;

namespace ReaperKing.Anhydrate.Extensions
{
    public static class SiteAnhydrateExtension
    {
        private const string Namespace = "ReaperKing.Anhydrate";
        private const string RealDirectory = "ReaperKing.Anhydrate";
        
        public static void EnableAnhydrateTemplates(this Site site)
        {
            string selfDir = site.GetInternalResourcePath(RealDirectory);
            // TODO: Make this more generic.
            var defines = site.ProjectConfig.Build.Define;
            for (int index = defines.Count - 1; index >= 0; index--)
            {
                string define = defines[index];
                if (define.StartsWith("ANHYDRATE_PATH"))
                {
                    string part2 = define.Split('=', 2)[1];
                    if (!String.IsNullOrEmpty(part2))
                    {
                        selfDir = part2;
                    }

                    break;
                }
            }
            
            site.AddTemplateIncludeNamespace(Namespace, selfDir);
        }
    }
}