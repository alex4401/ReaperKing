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

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Xeno.CLI
{
    internal abstract class BaseCommand
    {
        protected Program Parent { get; }
        protected CommandLineApplication App { get; private set; }
        protected ILogger Log { get; private set; }

        public int OnExecute(CommandLineApplication app)
        {
            XenoCli.TryInitialize(Parent);
            Log = XenoCli.Instance.Log;
            App = app;
            
            Log.LogInformation($"Xeno.CLI, v{Program.GetVersion()}");
            BeforeExecution();
            return Execute();
        }

        public virtual void BeforeExecution()
        { }

        public abstract int Execute();
    }

}