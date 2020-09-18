using System;
using System.IO;

namespace SiteBuilder.Core
{
    public abstract partial class Site
    {
        public string CopyFileToLocation(string inputFile, string uri)
        {
            inputFile = Path.Join(Environment.CurrentDirectory, inputFile);
            var diskPath = Path.Join(DeploymentPath, uri);
            var publicUri = Path.Join(ProjectConfig.Paths.Root, uri);
            
            Directory.CreateDirectory(Path.GetDirectoryName(diskPath));
            
            if (!File.Exists(diskPath))
            {
                File.Copy(inputFile, diskPath);
            }

            return publicUri;
        }

        public string CopyResource(string inputFile, string uri)
        {
            return CopyFileToLocation(Path.Join("resources", inputFile), 
                                  Path.Join(ProjectConfig.Paths.Resources, uri));
        }

        public string CopyVersionedResource(string inputFile, string uri)
        {
            var inputPath = Path.Join(Environment.CurrentDirectory, "resources", inputFile);
            var hash = HashUtils.GetHashOfFile(inputPath);
            var assetUri = uri.Replace("[hash]", hash.Substring(0, 12));
            return CopyResource(inputFile, assetUri);
        }
    }
}