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

namespace Xeno.Core
{
    [Obsolete("Merged into PathEx.")]
    public static class PathUtils
    {
        [Obsolete("Moved to PathEx.")]
        public static string EnsureRooted(string path)
        {
            return PathEx.EnsureRooted(path);
        }
        
        [Obsolete("Moved to PathEx.")]
        public static string EnsureRooted(string path, string defaultRoot)
        {
            return PathEx.EnsureRooted(path, defaultRoot);
        }
    }
}