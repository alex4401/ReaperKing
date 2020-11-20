using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        /**
         * Returns a path of a resource packed with the assemblies,
         * outside of project content directory.
         * AssemblyRoot takes priority over the builder executable's
         * path.
         */
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
        
        /**
         * Copies a file from the project content directory into
         * deployment folder. Returns the public URI of the resource.
         *
         * Fires off a signal to Resource Processor Modules.
         */
        public string CopyFileToLocation(string inputFile, string uri)
        {
            string filePath = Path.Join(ContentRoot, inputFile);
            
            foreach (var module in GetModuleInstances<RkResourceProcessorModule>())
            {
                module.ProcessResource(inputFile, ref filePath, ref uri);
            }
            
            // Combine into final path and public URI
            var diskPath = Path.Join(DeploymentPath, uri);
            var publicUri = Path.Join(ProjectConfig.Paths.Root, uri);
            
            // Ensure the directory exists.
            Directory.CreateDirectory(Path.GetDirectoryName(diskPath));
            
            // Copy file to the path if it does not exist.
            if (!File.Exists(diskPath))
            {
                Log.LogInformation($"Copying an asset: {filePath}");
                File.Copy(filePath, diskPath);
            }
            return publicUri;
        }

        /**
         * Copies a file from the project resources directory into
         * the public asset folder.
         */
        public string CopyResource(string inputFile, string uri)
        {
            return CopyFileToLocation(Path.Join("resources", inputFile), 
                                     Path.Join(ProjectConfig.Paths.Resources, uri));
        }

        /**
         * Copies a resource, and substitutes a [hash] placeholder
         * with first 12 characters of a SHA256 hash.
         */
        public string CopyVersionedResource(string inputFile, string uri)
        {
            var inputPath = Path.Join(ContentRoot, "resources", inputFile);
            var hash = HashUtils.GetHashOfFile(inputPath);
            var assetUri = uri.Replace("[hash]", hash.Substring(0, 12));
            return CopyResource(inputFile, assetUri);
        }
    }
}