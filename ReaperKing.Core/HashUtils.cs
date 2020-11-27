using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ReaperKing.Core
{
    public static class HashUtils
    {
        public static string GetHashOfStringMd5(string data)
        {
            byte[] encoded = new UTF8Encoding().GetBytes(data);
            byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encoded);
            return BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();
        }

        public static string GetHashOfStringSha256(string data)
        {
            byte[] encoded = new UTF8Encoding().GetBytes(data);
            byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("SHA256")).ComputeHash(encoded);
            return BitConverter.ToString(hash)
                .Replace("-", "")
                .ToLower();
        }
        
        public static string GetHashOfFileSha256(string inputPath)
        {
            var data = File.ReadAllText(inputPath);
            var hash = GetHashOfStringSha256(data);
            return hash;
        }
        
        public static string GetHashOfFile(string inputPath)
            => GetHashOfFileSha256(inputPath);

        [Obsolete("Renamed to GetHashOfStringSha256.")]
        public static string GetSha256HashOfString(string data)
            => GetHashOfStringSha256(data);
    }
}