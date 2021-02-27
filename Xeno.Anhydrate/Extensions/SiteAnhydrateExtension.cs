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

using System;

using Xeno.Core;

namespace Xeno.Anhydrate.Extensions
{
    public static class SiteAnhydrateExtension
    {
        private const string Namespace = "Xeno.Anhydrate";
        private const string AlternateNamespace = "ReaperKing.Anhydrate";
        private const string RealDirectory = "Xeno.Anhydrate";
        
        public static void EnableAnhydrateTemplates(this Site site)
        {
            string selfDir = site.GetInternalResourcePath(RealDirectory);

            AnhydrateConfiguration config = site.ProjectConfig.Get<AnhydrateConfiguration>();
            if (!String.IsNullOrEmpty(config.IncludePath))
            {
                selfDir = config.IncludePath;
            }
            
            site.AddTemplateIncludeNamespace(Namespace, selfDir);
            site.AddTemplateIncludeNamespace(AlternateNamespace, selfDir);
        }
    }
}