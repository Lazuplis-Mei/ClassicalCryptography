using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassicalCryptography.Utils;

namespace ClassicalCryptographyTest
{
    static class RandomString
    {
        public static string Generate(int textLength)
        {
            Span<char> chars = stackalloc char[textLength];
            for (int i = 0; i < textLength; i++)
                chars[i] = Globals.VBase64[Random.Shared.Next(64)];
            return new string(chars);
        }

        public static string GenerateUppers(int textLength)
        {
            Span<char> chars = stackalloc char[textLength];
            for (int i = 0; i < textLength; i++)
                chars[i] = Globals.ULetters[Random.Shared.Next(26)];
            return new string(chars);
        }
    }
}
