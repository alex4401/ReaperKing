using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LibSassHost;
using RazorLight;

namespace SiteBuilder.Core
{
    public static class GlobalCache
    {
        public static Dictionary<object, string> Strings = new Dictionary<object, string>();

        public static bool HasString(object key)
        {
            return Strings.ContainsKey(key);
        }
    }

    public static class TemplateUtils
    {
        [Obsolete]
        public static string LinkResource(string path)
        {
            var site = Site.Instance;
            var deploymentPath = site.DeploymentPath;

            if (GlobalCache.HasString(path))
            {
                return GlobalCache.Strings[path];
            }

            var cleanPath = path;
            if (cleanPath.StartsWith("built."))
            {
                cleanPath = cleanPath.Remove(0, 6);
            }

            var extension = Path.GetExtension(cleanPath);
            cleanPath = Path.GetFileNameWithoutExtension(cleanPath);

            var inputPath = Path.Join(Environment.CurrentDirectory, "resources", path);
            var hash = HashUtils.GetHashOfFile(inputPath);
            var assetUri = $"{cleanPath}-{hash.Substring(0, 12)}{extension}";
            var uri = site.CopyResource(path, assetUri);

            GlobalCache.Strings[path] = uri;
            return uri;
        }
    }
}