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
using System.Diagnostics.CodeAnalysis;
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
    public class RkFandomImageVirtualFsModule
        : RkModule, IRkResourceResolverModule
    {
        private const string UriNamespace = "fandom:";
        private const bool CachedUpstream = false;
        private static string UpstreamUrl
            => CachedUpstream
                ? "https://static.wikia.nocookie.net"
                : "https://vignette.wikia.nocookie.net";
        
        public RkFandomImageVirtualFsModule(Site site)
            : base(typeof(RkFandomImageVirtualFsModule), site)
        { }
        
        public struct ImageInfo
        {
            [NotNull] public string Bucket { get; init; }
            [NotNull] public string Name { get; init; }
            [NotNull] public int X { get; init; }

            public static ImageInfo ConstructFromUri(string virtualPath)
            {
                if (virtualPath.StartsWith(UriNamespace))
                {
                    virtualPath = virtualPath.Substring(UriNamespace.Length);
                }
                
                string[] parts = virtualPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                return new()
                {
                    Bucket = parts[0],
                    Name = parts[1],
                    X = parts.Length > 2 ? int.Parse(parts[2]) : 0,
                };
            }
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

        public bool CanAccept(string ns, string virtualPath)
        {
            return ns == UriNamespace;
        }
        
        public bool ResolveResource(string ns, string virtualPath, out string result)
        {
            result = "";
            ImageInfo info = ImageInfo.ConstructFromUri(virtualPath);
            
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