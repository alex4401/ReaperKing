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

namespace Xeno.Anhydrate.Models
{
    public record NavigationItem
    {
        public string Label { get; }
        public string Destination { get; }
        public bool IsEnabled { get; }
        public bool IsCurrentRoute { get; }

        public NavigationItem(string label, string destination,
                              bool enabled = true, bool current = false)
            => (Label, Destination, IsEnabled, IsCurrentRoute) = (label, destination, enabled, current);
    }
}