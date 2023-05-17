using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 进制转换
/// </summary>
internal static class BaseConverter
{

    /// <summary>
    /// 转换成10000进制
    /// </summary>
    [SkipLocalsInit]
    public static ushort[] ToBase10000(ulong_long number)
    {
        if (number == 0)
            return Array.Empty<ushort>();
        //[Log10000(ulong.MaxValue)] = 5
        int index = 5;
        Span<ushort> span = stackalloc ushort[index--];
        while (number != 0)
        {
            span[index--] = (ushort)(number % 10000);
            number /= 10000;
        }
        return span[(index + 1)..].ToArray();
    }

    /// <summary>
    /// 转换成36进制
    /// </summary>
    [SkipLocalsInit]
    public static string ToBase36(ulong_long number)
    {
        if (number == 0)
            return "0";

        //[Log36(ulong_long.MaxValue)] = 25
        int index = 25;
        Span<char> span = stackalloc char[index--];
        while (number != 0)
        {
            span[index--] = Base36[(int)(number % 36)];
            number /= 36;
        }
        return new(span[(index + 1)..]);
    }

    /// <summary>
    /// 从36进制转换
    /// </summary>
    public static ulong_long FromBase36(ReadOnlySpan<char> span)
    {
        ulong_long result = 0;
        for (int i = 0; i < span.Length; i++)
        {
            int n = span[i].Base36Number();
            checked
            {
                result *= 36;
                result += (ulong)n;
            }
        }
        return result;
    }

    /// <summary>
    /// 转换成5进制
    /// </summary>
    public static byte[] ToBase5(ulong number)
    {
        if (number == 0)
            return Array.Empty<byte>();

        //[Log5(ulong.MaxValue)] = 28
        int index = 28;
        Span<byte> span = stackalloc byte[index--];
        while (number != 0)
        {
            span[index--] = ((byte)(number % 5));
            number /= 5;
        }
        return (span[(index + 1)..]).ToArray();
    }

    /// <summary>
    /// 从5进制转换
    /// </summary>
    public static ulong FromBase5(byte[] array)
    {
        ulong result = 0;
        for (int i = 0; i < array.Length; i++)
        {
            checked
            {
                result *= 5;
                result += array[i];
            }
        }
        return result;
    }

    /// <summary>
    /// 转换成4进制
    /// </summary>
    [SkipLocalsInit]
    public static string ToBase4(byte number)
    {
        int index = 4;
        Span<char> span = stackalloc char[index--];
        while (number != 0)
        {
            span[index--] = Digits[number & 0B11];
            number >>= 2;
        }
        return new(span[(index + 1)..]);
    }

    /// <summary>
    /// 从4进制转换
    /// </summary>
    public static byte FromBase4(string text)
    {
        byte result = 0;
        for (int i = 0; i < text.Length; i++)
        {
            checked
            {
                result <<= 2;
                result += (byte)text[i].Base36Number();
            }
        }
        return result;
    }

    /// <summary>
    /// 转换成3进制(虽然存储在4进制数组中)
    /// </summary>
    public static QuaterArray ToBase3(Span<byte> bytes)
    {
        if (bytes.Length == 0)
            return new QuaterArray(0);
        Guard.IsNotEqualTo<byte>(bytes[0], 0);

        var number = new BigInteger(bytes, true, true);
        int index = (int)Math.Ceiling(BigInteger.Log(number, 3));
        var array = new QuaterArray(index--);
        while (!number.IsZero)
        {
            array[index--] = (int)(number % 3);
            number /= 3;
        }
        return array;
    }

    /// <summary>
    /// 从3进制转换
    /// </summary>
    public static byte[] FromBase3(QuaterArray array)
    {
        if (array.Count == 0)
            return Array.Empty<byte>();

        var number = BigInteger.Zero;
        for (int i = 0; i < array.Count; i++)
        {
            number *= 3;
            int value = array[i];
            if (value == 1 || value == 2)
                number += array[i];
        }
        return number.ToByteArray(true, true);
    }
}
