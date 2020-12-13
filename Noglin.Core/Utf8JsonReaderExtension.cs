using System;
using System.IO;
using System.Text.Json;

namespace Noglin.Core
{
    public static class Utf8JsonReaderExtension
    {
        public static void SkipNull(this ref Utf8JsonReader reader)
        {
            while (reader.TokenType == JsonTokenType.None)
            {
                reader.Read();
            }
        }
        
        public static void AssertIs(this ref Utf8JsonReader reader, JsonTokenType tokenType)
        {
            if (reader.TokenType != tokenType)
            {
                throw new InvalidDataException($"Expected \"{tokenType}\", found \"{reader.TokenType}\".");
            }
        }
        
        public static void Expect(this ref Utf8JsonReader reader, JsonTokenType tokenType)
        {
            reader.Read();
            AssertIs(ref reader, tokenType);
        }
    }
}