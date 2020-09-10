using System;
using System.Security.Cryptography;
using System.Text;
using ProjectReaperKing.Data.ARK;

namespace ProjectReaperKing
{
    public static class GamepediaUtils
    {
        public static string GetMd5HashOfString(string data)
        {
            byte[] encoded = new UTF8Encoding().GetBytes(data);
            byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encoded);
            return BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();
        }
        
        public static string GetImageUrl(string filename)
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
            
            string hash = GetMd5HashOfString(filename);
            string[] uriParts = 
            {
                "https://static.wikia.nocookie.net/arksurvivalevolved_gamepedia/images",
                hash.Substring(0, 1),
                hash.Substring(0, 2),
                filename,
                $"revision/latest"
            };
            return String.Join("/", uriParts);
        }
        
        public static string GetImageUrl(string filename, string version)
        {
            string uri = GetImageUrl(filename);
            // BUG: Fandom's image cache is likely to drop the entry after few days.
            //      Prefer non-versioned files or use Thumblr (Vignette replacement).
            return uri; //+ $"?cb={version}";
        }
        
        public static string GetImageUrl(string filename, string version, int size)
        {
            string uri = GetImageUrl(filename);
            // BUG: Fandom's image cache is likely to drop the entry after few days.
            //      Prefer non-versioned files or use Thumblr (Vignette replacement).
            return uri + $"/scale-to-width-down/{size}"; //?cb={version}";
        }
        
        public static string GetImageUrl(MapInfo.TopomapInfo info)
        {
            return GetImageUrl(info.Name, info.Version);
        }
        
        public static string GetImageUrl(MapInfo.TopomapInfo info, int size)
        {
            return GetImageUrl(info.Name, info.Version, size);
        }
    }
}