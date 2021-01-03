/*!
 * This file is a part of Reaper King, and the project's repository may be found at
 * https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using System;

namespace ReaperKing.Core
{
    [Obsolete("Switch to a pure RkModule and implement IRkResourceProcessorModule.")]
    public abstract class RkResourceProcessorModule
        : RkModule, IRkResourceProcessorModule
    {
        protected RkResourceProcessorModule(Type selfType, Site site)
            : base(selfType, site)
        { }
        
        public abstract void ProcessResource(string filePath, ref string diskPath, ref string uri);
    }
}