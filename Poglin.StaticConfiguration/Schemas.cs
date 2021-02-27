/*!
 * This file is a part of the Poglin project, whose repository may be found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General
 * Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Affero General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using ReaperKing.Anhydrate;
using Xeno.Core.Configuration;

using Poglin.Generation.ARK;
using ReaperKing.Plugins;

namespace Poglin.StaticConfiguration
{
    [RkSchema("ReaperKing/v2+poglin1")]
    public sealed class SchemaV2Poglin1 : Xeno.Core.Schemas.V2
    {
        public override PropertySetDescriptor[] PropertySets
            => new PropertySetDescriptor[]
            {
                new("modules", new PropertyDescriptor[]
                {
                    new("anhydrate", typeof(AnhydrateConfiguration)),
                    new("uglify", typeof(RkUglifyConfiguration)),
                }),
                
                new("site", new PropertyDescriptor[]
                {
                    new("ark", typeof(BuildConfigurationArk)),
                }),
            };
    }
}