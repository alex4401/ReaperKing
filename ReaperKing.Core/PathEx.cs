using System.IO;

namespace ReaperKing.Core
{
    public static class PathEx
    {
        public static string Prefix(string a, string b)
        {
            return Path.Combine(a, b.TrimStart('/'));
        }
    }
}