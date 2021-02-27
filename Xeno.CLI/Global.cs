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

using Microsoft.Extensions.Logging;

namespace Xeno.CLI
{
    internal sealed class XenoCli
    {
        internal static XenoCli Instance { get; private set; }

        internal static void TryInitialize(Program program)
            => Instance = new(program);

        internal Program Main { get; }
        internal ILogger Log { get; }
        internal bool IsVerbose { get; }
        
        private XenoCli(Program program)
        {
            Main = program;
            
            IsVerbose = Main.Verbose;
            ApplicationLogging.Initialize(IsVerbose);
            Log = ApplicationLogging.Factory.CreateLogger("Xeno.CLI");
        }
    }
}