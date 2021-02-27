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

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace Xeno.CLI
{
    [Command("xeno")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(BakeCommand)
    )]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class Program
    {
        static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [Option("--verbose")]
        public bool Verbose { get; }
        
        public void OnExecute(CommandLineApplication app)
            => app.ShowHelp();
        
        internal static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }
}