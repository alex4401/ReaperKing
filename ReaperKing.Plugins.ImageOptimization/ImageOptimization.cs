using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;

namespace ReaperKing.Plugins
{
    public partial class RkImageOptimizationModule : RkResourceProcessorModule
    {
        public const int CacheVersion = 2;
        public string CacheDirectory { get; }

        public string[] AllowedFileExtensions { get; } = {
            "jpg", "jpeg", "png",
        };

        public RkImageOptimizationModule(Site site)
            : base(typeof(RkImageOptimizationModule), site)
        {
            CacheDirectory = Path.Join(Site.ContentRoot, "resources", "_cache");
            Directory.CreateDirectory(CacheDirectory);
        }
        
        public override void ProcessResource(string filePath, ref string diskPath, ref string uri)
        {
            // Check if the file is located within the resources directory
            if (!filePath.StartsWith("resources/"))
            {
                return;
            }
            
            // Split the file path
            string resourceKey = filePath.Substring("resources/".Length);
            string fileName = Path.GetFileName(filePath);
            string extension = Path.GetExtension(filePath)?.Substring(1);
            string fileDir = Path.GetDirectoryName(filePath);
            
            // Check if the file extension is allowed
            if (!AllowedFileExtensions.Contains(extension))
            {
                return;
            }
            
            string cacheKey = HashUtils.GetHashOfStringSha256(resourceKey + CacheVersion) + "." + extension;
            string optimizedPath = Path.Join(CacheDirectory, cacheKey);

            if (!File.Exists(optimizedPath))
            {
                Log.LogInformation($"Compressing image: \"{resourceKey}\"");

                bool success = false;
                switch (extension)
                {
                    case "png":
                        bool usesTransparency = _hasAlphaChannel(filePath);
                        if (!usesTransparency)
                        {
                            cacheKey = HashUtils.GetHashOfStringSha256(resourceKey + CacheVersion) + ".jpg";
                            optimizedPath = Path.Join(CacheDirectory, cacheKey);

                            if (_invokeConvert(filePath, optimizedPath))
                            {
                                uri = uri.Substring(0, uri.Length - Path.GetExtension(uri).Length) + ".jpg";
                                diskPath = optimizedPath;

                                ProcessResource(Path.Join("resources/_cache", cacheKey), ref diskPath, ref uri);
                                return;
                            }

                            Log.LogWarning($"Failed to convert non-transparent PNG \"{resourceKey}\" to a JPEG file. Falling back to Oxipng.");
                        }
                        
                        success = _invokeOxipng(filePath, optimizedPath);
                        break;
                    
                    case "jpeg":
                    case "jpg":
                        success = _invokeJpegRecompress(filePath, optimizedPath);
                        break;

                    default:
                        Log.LogWarning("A file extension has been included for image optimization, but no optimizer is implemented.");
                        break;
                }

                if (!success)
                {
                    Log.LogError($"Failed to optimize \"{resourceKey}\": see above for more information. Original resource will be used.");
                    return;
                }
            }

            diskPath = optimizedPath;
        }
    }
}