using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using ReaperKing.Core;
using ReaperKing.StaticConfig.Data.ARK;

namespace ReaperKing.StaticConfig
{
    public static class FandomUtils
    {
        public static string GetMd5HashOfString(string data)
        {
            byte[] encoded = new UTF8Encoding().GetBytes(data);
            byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encoded);
            return BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();
        }

        public struct ImageInfo
        {
            public bool Cached;
            public string Bucket;
            public string Name;
            public string Version;
            public int X;
        }
        
        public static string GetImageUrl(ImageInfo info)
        {
            /*
             Reference from old code:
                function from_wiki_cdn($filename, $version, $size = -1) {
                    $sum = md5($filename);
                    if ($size > 0) {
                        $components = array(
                                            WIKI_CDN_URL,
                                            'thumb',
                                            $sum[0],
                                            substr($sum, 0, 2),
                                            $filename,
                                            $size . 'px-' . $filename . '?version=' . $version);
                    } else {
                        $components = array(
                                        WIKI_CDN_URL,
                                        $sum[0],
                                        substr($sum, 0, 2),
                                        $filename . '?version=' . $version);
                    }
                    echo join('/', $components);
                }
            */
            
            string hash = GetMd5HashOfString(info.Name);
            string result = String.Join("/", new [] {
                (info.Cached
                    ? "https://static.wikia.nocookie.net"
                    : "https://vignette.wikia.nocookie.net"), info.Bucket, "images",
                hash.Substring(0, 1), hash.Substring(0, 2),
                info.Name,
                $"revision/latest"
            });

            if (info.X > 0)
            {
                result += $"/scale-to-width-down/{info.X}";
            }

            if (!String.IsNullOrEmpty(info.Version))
            {
                result += $"?cb={info.Version}";
            }

            return result;
        }
        
        public static string GetImageUrl(MapInfo.TopomapInfo info, int size = 0)
        {
            return GetImageUrl(new ImageInfo
            {
                Bucket = "arksurvivalevolved_gamepedia",
                Cached = false,
                Name = info.Name,
                // Version = info.Version,
                X = size,
            });
        }

        public static string GetMirroredImageUri(SiteContext ctx, ImageInfo info)
        {
            // BUG: Fandom's image cache is likely to drop the entry after few days.
            //      Download the image instead of playing weird games with Thumblr.
            string origin = GetImageUrl(info);
            string ext = Path.GetExtension(info.Name);
            string name = info.Name.Substring(0, info.Name.Length - ext.Length);
            string publicKey = $"{info.Bucket}/{name}-[hash]{ext}";
            string resourceUri = $"/mirror/{publicKey}";

            string localKey = Path.Join(info.Bucket, $"{name}{info.X}{info.Version}{ext}");
            string diskPath = Path.Join("resources", localKey);
            if (!File.Exists(diskPath))
            {
                using (var client = new WebClient())
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(diskPath));
                    client.DownloadFile(origin, diskPath);
                }
            }

            return ctx.CopyVersionedResource(localKey, resourceUri);
        }

        public static string GetMirroredImageUri(SiteContext ctx, MapInfo.TopomapInfo info, int size = 0)
        {
            return GetMirroredImageUri(ctx, new ImageInfo
            {
                Bucket = "arksurvivalevolved_gamepedia",
                Cached = false,
                Name = info.Name,
                // Version = info.Version,
                X = size,
            });
        }
    }
}