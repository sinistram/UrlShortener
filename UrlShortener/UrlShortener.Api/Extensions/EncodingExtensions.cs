using System;

namespace UrlShortener.Api.Extensions
{
    public static class EncodingExtensions
    {
        private static readonly char[] Base62Alphabet =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        public static string ToBase62(this ulong value)
        {
            const int bufferSize = 11;  // ulong.MaxValue length in base62
            var buffer = new char[bufferSize];
            var i = bufferSize;
            var targetBase = (ulong) Base62Alphabet.Length;

            do
            {
                buffer[--i] = Base62Alphabet[value % targetBase];
                value /= targetBase;
            }
            while (value > 0);

            var result = new char[bufferSize - i];
            Array.Copy(buffer, i, result, 0, bufferSize - i);

            return new string(result);
        }
    }
}