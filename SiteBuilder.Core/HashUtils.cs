using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SiteBuilder.Core
{
    public static class HashUtils
    {
        public static string GetHashOfFile(string inputPath)
        {
            var data = File.ReadAllText(inputPath);
            var hash = GetSha256HashOfString(data);
            return hash;
        }

        public static string GetSha256HashOfString(string data)
        {
            byte[] encoded = new UTF8Encoding().GetBytes(data);
            byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("SHA256")).ComputeHash(encoded);
            return BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();
        }
    }
}