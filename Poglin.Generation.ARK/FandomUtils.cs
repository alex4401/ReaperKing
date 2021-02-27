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

using System.IO;

using Xeno.Core;

namespace Poglin.Generation.ARK
{
    public static class FandomUtils
    {
        public static string Mirror(SiteContext ctx, string virtualPath)
        {
            var info = RkFandomImageVirtualFsModule.ImageInfo.ConstructFromUri(virtualPath);
            
            string ext = Path.GetExtension(info.Name);
            string name = Path.GetFileNameWithoutExtension(info.Name);
            string publicKey = $"/mirror/{info.Bucket}/{name}-[hash]{ext}";
            return ctx.CopyVersionedResource(virtualPath, publicKey);
        }
    }
}