using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using ReaperKing.Core;

namespace ReaperKing.Generation.ARK
{
    /**
     * This module resolves resources and downloads them from Fandom's
     * Thumblr service.
     *
     * The "local" path has to follow the following scheme:
     *  fandom://wiki_id/filename.extension
     *  fandom://wiki_id/filename.extension/width
     */
    public class RkFandomImageVirtualFsModule : RkResourceResolverModule
    {
        private const bool CachedUpstream = false;
        private static string UpstreamUrl
            => CachedUpstream
                ? "https://static.wikia.nocookie.net"
                : "https://vignette.wikia.nocookie.net";
        
        public RkFandomImageVirtualFsModule(Site site)
            : base(typeof(RkFandomImageVirtualFsModule), site)
        { }
        
        private struct ImageInfo
        {
            public string Bucket { get; init; }
            public string Name { get; init; }
            public int X { get; init; }
        }
        
        private string GetImageUrl(ImageInfo info)
        {
            string hash = HashUtils.GetHashOfStringMd5(info.Name);
            string result = String.Join(
                                        "/", 
                                        UpstreamUrl,
                                        info.Bucket,
                                        "images", 
                                        hash.Substring(0, 1),
                                        hash.Substring(0, 2), 
                                        info.Name, 
                                        "revision/latest");

            if (info.X > 0)
            {
                result += $"/scale-to-width-down/{info.X}";
            }

            return result + "?format=original";
        }
        
        public override bool ResolveResource(string ns, string virtualPath, out string result)
        {
            result = "";
            if (ns != "fandom:")
            {
                return false;
            }

            string[] parts = virtualPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            ImageInfo info = new()
            {
                Bucket = parts[0],
                Name = parts[1],
                X = parts.Length > 2 ? int.Parse(parts[2]) : 0,
            };
            
            string origin = GetImageUrl(info);
            string ext = Path.GetExtension(info.Name);
            string name = info.Name.Substring(0, info.Name.Length - ext.Length);
            
            string localKey = Path.Join("_cache", info.Bucket, $"{name}-{info.X}px{ext}");
            string diskPath = Path.Join(Site.ContentRoot, "resources", localKey);
            
            if (!File.Exists(diskPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(diskPath));
                
                using (var client = new WebClient())
                {
                    Log.LogInformation($"Downloading {name} ({info.X}px) from Fandom");
                    client.DownloadFile(origin, diskPath);
                }
            }

            result = localKey;
            return true;
        }
    }
}