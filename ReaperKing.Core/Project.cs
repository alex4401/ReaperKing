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

using System.Collections.Generic;

namespace ReaperKing.Core
{
    public struct Project
    {
        public string Inherits;
        public string ContentDirectory;
        public string AssemblyDirectory;
        
        public PathConfig Paths;
        public ResourceConfig Resources;
        public BuildConfig Build;

        public struct PathConfig
        {
            public string Root;
            public string Deployment;
            public string Resources;
            public string Sitemap;
        }

        public struct ResourceConfig
        {
            public string[] CopyNonVersioned;
        }

        public struct BuildConfig
        {
            public List<string> Define;
            public bool MinifyHtml;
            public List<string> RunBefore;
            
            public string[] AddRunBeforeCmds;
            public string[] AddDefines;
        }
    }
}