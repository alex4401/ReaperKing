/*!
 * This file is a part of Xeno, and the project's repository may be found at https://github.com/alex4401/rk.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * http://www.gnu.org/licenses/.
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Xeno.Core
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
    }
}