/*!
 * This file is a part of Reaper King, and the project's repository may be
 * found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
 * the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
        /**
         * Resolves a resource that might be located in a virtual
         * asset namespace to a local key.
         */
        public string ResolveResourceVirtualPath(string inputPath)
        {
            if (inputPath[0] == '/')
            {
                return inputPath;
            }

            string[] parts = inputPath.Split('/', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts[0].Last() == ':')
            {
                foreach (var module in GetModuleInstances<RkResourceResolverModule>())
                {
                    if (!module.CanAccept(parts[0], parts[1]))
                    {
                        continue;
                    }
                    
                    if (module.ResolveResource(parts[0], parts[1], out var result))
                    {
                        return result;
                    }
                }

                throw new ArgumentException($"Resource key could not be resolved: {inputPath}");
            }

            return inputPath;
        }
        
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
            var publicUri = Path.Join(WebConfig.Root, uri);
            
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
            inputFile = ResolveResourceVirtualPath(inputFile);
            return CopyFileToLocation(Path.Join("resources", inputFile), 
                                     Path.Join(WebConfig.Resources, uri));
        }

        /**
         * Copies a resource, and substitutes a [hash] placeholder
         * with first 12 characters of a SHA256 hash.
         */
        public string CopyVersionedResource(string inputFile, string uri)
        {
            inputFile = ResolveResourceVirtualPath(inputFile);
            
            var inputPath = Path.Join(ContentRoot, "resources", inputFile);
            var hash = HashUtils.GetHashOfFile(inputPath);
            var assetUri = uri.Replace("[hash]", hash.Substring(0, 12));
            return CopyResource(inputFile, assetUri);
        }
    }
}