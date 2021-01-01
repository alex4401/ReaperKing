/*!
 * This file is a part of the Poglin project, whose repository may be
 * found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using Noglin.Ark.Schemas;
using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;

namespace Poglin.Generation.ARK.Models
{
    public record AnhydrateModModel : AnhydrateModel
    {
        public ModSpecificationSchema ModInfo { get; init; }
        public BuildConfigurationArk Configuration { get; init; }
        
        public AnhydrateModModel(SiteContext ctx)
            : base(ctx)
        { }
    }
}