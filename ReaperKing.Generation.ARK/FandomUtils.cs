/*!
 * This file is a part of the open-sourced engine modules for
 * https://alex4401.github.io, and those modules' repository may be found
 * at https://github.com/alex4401/ReaperKing.
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

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK
{
    public static class FandomUtils
    {
        public static string GetMirroredImageUri(SiteContext ctx, MapInfo.TopomapInfo info, int size = 0)
        {
            return GetVirtualUri(ctx, "arksurvivalevolved_gamepedia", info.Name, size);
        }

        public static string GetVirtualUri(SiteContext ctx, string bucket, string filename, int size)
        {
            string ext = Path.GetExtension(filename);
            string name = Path.GetFileNameWithoutExtension(filename);
            string publicKey = $"{bucket}/{name}-[hash]{ext}";
            string resourceUri = $"/mirror/{publicKey}";
            string virtualPath = $"fandom://{bucket}/{filename}";
            if (size != 0)
            {
                virtualPath += $"/{size}";
            }
            
            return ctx.CopyVersionedResource(virtualPath, resourceUri);
        }
    }
}