[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ClassicalCryptographyTest")]

namespace ClassicalCryptography.Encoder.Chinese;

public static class ChineseHelperEx
{
    internal static ulong_long FromCodeString(ReadOnlySpan<char> text)
    {
        var result = ulong_long.One;
        for (int i = 0; i < text.Length; i++)
        {
            checked
            {
                result *= 5;
                result += (ulong)(text[i] - '1');
            }
        }
        return result;
    }

    internal static string ToCodeString(ulong_long codeValue)
    {
        int index = 56;
        Span<char> span = stackalloc char[index--];
        var value = ulong_long.Zero;
        while (codeValue > 1)
        {
            (codeValue, value) = ulong_long.DivRem(codeValue, 5);
            span[index--] = (char)('1' + value);
        }
        return new(span[(index + 1)..]);
    }
}