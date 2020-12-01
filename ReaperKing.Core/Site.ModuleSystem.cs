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
    public abstract partial class Site
    {
        private readonly List<RkModule> _modules = new();

        protected void AddModule<T>(T module)
            where T : RkModule
        {
            _modules.Add(module);
        }
        
        public T GetModuleInstance<T>()
            where T : RkModule
        {
            foreach (RkModule module in _modules)
            {
                if (module is T rkModule)
                {
                    return rkModule;
                }
            }

            return null;
        }
        
        public IEnumerable<T> GetModuleInstances<T>()
            where T : RkModule
        {
            foreach (RkModule module in _modules)
            {
                if (module is T rkModule)
                {
                    yield return rkModule;
                }
            }
        }
    }
}