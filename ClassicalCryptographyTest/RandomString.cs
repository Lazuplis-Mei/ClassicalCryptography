using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;

namespace ClassicalCryptographyTest
{
    static class RandomString
    {
        public static string Generate(int textLength)
        {
            Span<char> chars = textLength <= StackLimit.MAX_CHAR_SIZE
                ? stackalloc char[textLength] : new char[textLength];
            for (int i = 0; i < textLength; i++)
                chars[i] = GlobalTables.VChar64[Random.Shared.Next(64)];
            return new string(chars);
        }

        public static string GenerateUppers(int textLength)
        {
            Span<char> chars = textLength <= StackLimit.MAX_CHAR_SIZE
                ? stackalloc char[textLength] : new char[textLength];
            for (int i = 0; i < textLength; i++)
                chars[i] = GlobalTables.U_Letters[Random.Shared.Next(26)];
            return new string(chars);
        }
    }
}
