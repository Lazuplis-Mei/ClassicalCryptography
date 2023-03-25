using System.Runtime.CompilerServices;
using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 进制转换
/// </summary>
internal static class BaseConverter
{
    /// <summary>
    /// 转换成36进制
    /// </summary>
    public static string ToBase36(ulong number)
    {
        var stack = new Stack<char>();
        while (number != 0)
        {
            stack.Push(Base36[(int)(number % 36)]);
            number /= 36;
        }
        return new(stack.ToArray());
    }

    /// <summary>
    /// 转换成10000进制
    /// </summary>
    public static ushort[] ToBase10000(ulong number)
    {
        var stack = new Stack<ushort>();
        while (number != 0)
        {
            stack.Push((ushort)(number % 10000));
            number /= 10000;
        }
        return stack.ToArray();
    }

    /// <summary>
    /// 从36进制转换
    /// </summary>
    public static ulong FromBase36(ReadOnlySpan<char> span)
    {
        ulong result = 0;
        ulong @base = 1;
        for (int i = span.Length - 1; i >= 0; i--)
        {
            int n = span[i].Base36Number();
            checked
            {
                result += (ulong)n * @base;
                @base = (@base << 5) + (@base << 2);
            }
        }
        return result;
    }


    /// <summary>
    /// 转换成5进制
    /// </summary>
    public static byte[] ToBase5(ulong number)
    {
        var stack = new Stack<byte>();
        while (number != 0)
        {
            stack.Push((byte)(number % 5));
            number /= 5;
        }
        return stack.ToArray();
    }

    /// <summary>
    /// 转换成4进制
    /// </summary>
    [SkipLocalsInit]
    public static string ToBase4(int number)
    {
        Span<char> span = stackalloc char[4];
        int i = 3;
        while (number != 0)
        {
            span[i--] = (char)('0' + (number & 0B11));
            number >>= 2;
        }
        return new(span[(i + 1)..]);
    }

    /// <summary>
    /// 从5进制转换
    /// </summary>
    public static ulong FromBase5(byte[] array)
    {
        ulong result = 0;
        ulong @base = 1;
        for (int i = array.Length - 1; i >= 0; i--)
        {
            checked
            {
                result += array[i] * @base;
                @base += @base << 2;
            }
        }
        return result;
    }

    /// <summary>
    /// 从4进制转换
    /// </summary>
    public static byte FromBase4(string text)
    {
        byte result = 0;
        int @base = 1;
        for (int i = text.Length - 1; i >= 0; i--)
        {
            checked
            {
                result += (byte)((text[i] - '0') * @base);
                @base <<= 2;
            }
        }
        return result;
    }

}
