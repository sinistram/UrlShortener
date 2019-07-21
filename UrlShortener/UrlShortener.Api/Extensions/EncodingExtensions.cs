using System;
using System.Linq;

namespace UrlShortener.Api.Extensions
{
    public static class EncodingExtensions
    {
        private static readonly char[] Base62Alphabet =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        private const string UlongMaxInBase62 = "LygHa16AHYF";

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

        public static ulong FromBase62(this string value)
        {
            if (value.Any(c => !Base62Alphabet.Contains(c)))
            {
                throw new ArgumentException("Given wrong base62 string", nameof(value));
            }

            if (string.CompareOrdinal(value, UlongMaxInBase62) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Given base62 is more than ulong.Max");
            }

            var targetBase = (ulong)Base62Alphabet.Length;
            var result = (ulong) 0;
            for (var i = 0; i < value.Length; i++)
            {
                result += (ulong) Math.Pow(targetBase, value.Length - i - 1) *
                          (ulong) Array.IndexOf(Base62Alphabet, value[i]);
            }

            return result;
        }
    }
}