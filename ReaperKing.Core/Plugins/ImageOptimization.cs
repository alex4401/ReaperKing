using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ReaperKing.Core.Plugins
{
    public class RkImageOptimizationModule : RkResourceProcessorModule
    {
        public bool UseOxipng { get; } = true;
        public string OxipngBinaryPath { get; } = "/usr/bin/oxipng";
        public int PngCompressionLevel { get; } = 3;
        
        public bool UseMozJpeg { get; }
        public string MozJpegBinaryPath { get; }
        
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
                    Arguments = $"-o {PngCompressionLevel} \"{source}\" --out \"{target}\"",
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
            
            Console.WriteLine(filePath);
            // Check if the file extension is allowed
            if (!AllowedFileExtensions.Contains(extension))
            {
                return;
            }
            
            string cacheKey = HashUtils.GetSha256HashOfString(resourceKey) + "." + extension;
            string optimizedPath = Path.Join(CacheDirectory, cacheKey);

            if (!File.Exists(optimizedPath))
            {
                bool success = false;
                switch (extension)
                {
                    case "png":
                        success = _invokeOxipng(filePath, optimizedPath);
                        break;
                
                    default:
                        // log: unknown extension
                        break;
                }

                if (!success)
                {
                    // log: failure
                    return;
                }
            }

            diskPath = optimizedPath;
        }
    }
}