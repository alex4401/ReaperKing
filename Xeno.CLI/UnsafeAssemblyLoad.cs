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
using System.Data;
using System.IO;
using System.Reflection;

namespace Xeno.CLI
{
    public static class UnsafeAssemblyLoad
    {
        private static string _customPath;
        
        public struct AssemblyLoadMarker : IDisposable
        {
            public void Dispose()
            {
                AppDomain.CurrentDomain.AssemblyResolve -= LoadAssemblyInCustomSearchPath;
                _customPath = null;
            }
        }
        
        public static AssemblyLoadMarker AllowAssembliesFromUserPath(string path)
        {
            if (!String.IsNullOrEmpty(_customPath))
            {
                throw new ConstraintException("Multiple custom assembly load paths are not allowed.");
            }

            _customPath = path;
            AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblyInCustomSearchPath;
            return new AssemblyLoadMarker();
        }

        /**
         * Finds an assembly in the directory of the site
         * assembly, if the runtime fails to locate one on
         * its own.
         *
         * Not exactly safe, but given that the Static Config
         * assembly is already loaded and this is a fallback,
         * there's not much to lose.
         */
        private static Assembly LoadAssemblyInCustomSearchPath(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            
            if (args != null && !string.IsNullOrEmpty(args.Name))
            {
                var assemblyName = args.Name.Split(new string[] { "," }, StringSplitOptions.None)[0];
                var assemblyPath = Path.Combine(_customPath, $"{assemblyName}.dll");
                if (File.Exists(assemblyPath))
                {
                    result = Assembly.LoadFrom(assemblyPath);
                }
            }

            return result;
        }
    }
}