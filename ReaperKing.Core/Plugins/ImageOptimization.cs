using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core.Plugins
{
    public class RkImageOptimizationModule : RkResourceProcessorModule
    {
        public bool UseOxipng { get; } = true;
        public string OxipngBinaryPath { get; } = "/usr/bin/oxipng";
        public int PngCompressionLevel { get; } = 3;

        public bool UseMozJpeg { get; } = true;
        public string JpegRecompressBinaryPath { get; } = "/usr/bin/jpeg-recompress";
        public int JpegMinCompressionLevel { get; } = 60;
        
        public string CacheDirectory { get; }

        public string[] AllowedFileExtensions { get; } = {
            "jpg", "jpeg", "png",
        };

        public RkImageOptimizationModule(Site site) : base(site)
        {
            CacheDirectory = Path.Join(Site.ContentRoot, "resources", "_cache");
            Directory.CreateDirectory(CacheDirectory);
        }

        private bool _invokeOxipng(string source, string target)
        {
            if (!UseOxipng)
            {
                return false;
            }
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = OxipngBinaryPath,
                    Arguments = $"--strip safe -o {PngCompressionLevel} \"{source}\" --out \"{target}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }

        private bool _invokeJpegRecompress(string source, string target)
        {
            if (!UseMozJpeg)
            {
                return false;
            }
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = JpegRecompressBinaryPath,
                    Arguments = $"-s -a -q high -m smallfry -n {JpegMinCompressionLevel} -l 10 \"{source}\" \"{target}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
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
            
            string cacheKey = HashUtils.GetSha256HashOfString(resourceKey) + "." + extension;
            string optimizedPath = Path.Join(CacheDirectory, cacheKey);

            if (!File.Exists(optimizedPath))
            {
                Log.LogInformation($"Compressing image: \"{resourceKey}\"");

                bool success = false;
                switch (extension)
                {
                    case "png":
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