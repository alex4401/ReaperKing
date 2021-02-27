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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Xeno.CLI
{
    internal static class ApplicationLogging
    {
        internal static ILoggerFactory Factory;
        
        internal static void Initialize(bool verbose)
        {
            Factory = LoggerFactory.Create(
                builder =>
                {
                    builder.AddDebug();
                    builder.AddSimpleConsole(options =>
                    {
                        options.ColorBehavior = LoggerColorBehavior.Disabled;
                        options.SingleLine = true;
                        options.IncludeScopes = false;
                    });
                    builder.SetMinimumLevel(verbose ? LogLevel.Debug : LogLevel.Information);
                });
        }
    }
}