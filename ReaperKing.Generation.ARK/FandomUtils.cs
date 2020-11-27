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