using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        public string GetInternalResourcePath(string path)
        {
            // Try AssemblyRoot
            string workingPath = Path.Join(AssemblyRoot, path);
            if (Directory.Exists(workingPath) || File.Exists(workingPath))
            {
                return workingPath;
            }
            
            // Try builder's root
            workingPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, path);
            return workingPath;
        }
        
        public string CopyFileToLocation(string inputFile, string uri)
        {
            inputFile = Path.Join(ContentRoot, inputFile);
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
            var inputPath = Path.Join(ContentRoot, "resources", inputFile);
            var hash = HashUtils.GetHashOfFile(inputPath);
            var assetUri = uri.Replace("[hash]", hash.Substring(0, 12));
            return CopyResource(inputFile, assetUri);
        }
    }
}