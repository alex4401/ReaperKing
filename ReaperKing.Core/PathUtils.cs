using System;
using System.IO;

namespace ReaperKing.Core
{
    public static class PathUtils
    {
        public static string EnsureRooted(string path)
        {
            return EnsureRooted(path, Environment.CurrentDirectory);
        }
        
        public static string EnsureRooted(string path, string defaultRoot)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            return Path.Join(defaultRoot, path);
        }
    }
}