namespace ClassicalCryptography.Utils;

using System;
using static ClassicalCryptography.Utils.Globals;

/// <summary>
/// 进制转换
/// </summary>
public static class BaseConverter
{
    /// <summary>
    /// 转换成36进制
    /// </summary>
    public static string ToBase36(ulong n)
    {
        var stack = new Stack<char>();
        while (n != 0)
        {
            stack.Push(Base36[(int)(n % 36)]);
            n /= 36;
        }
        return new(stack.ToArray());
    }

    /// <summary>
    /// 从36进制转换
    /// </summary>
    public static ulong FromBase36(string str) => FromBase36(str.AsSpan());

    /// <summary>
    /// 从36进制转换
    /// </summary>
    public static ulong FromBase36(ReadOnlySpan<char> span)
    {
        ulong result = 0;
        ulong b = 1;
        for (int i = span.Length - 1; i >= 0; i--)
        {
            int pos = Base36.IndexOf(span[i]);
            checked
            {
                result += (ulong)pos * b;
                b = (b << 5) + (b << 2);//base *= 36;
            }
        }
        return result;
    }


    /// <summary>
    /// 转换成5进制
    /// </summary>
    public static byte[] ToBase5(ulong n)
    {
        var stack = new Stack<byte>();
        while (n != 0)
        {
            stack.Push((byte)(n % 5));
            n /= 5;
        }
        return stack.ToArray();
    }

    /// <summary>
    /// 从5进制转换
    /// </summary>
    public static ulong FromBase5(byte[] arr)
    {
        ulong result = 0;
        ulong b = 1;
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            checked
            {
                result += arr[i] * b;
                b += b << 2;//base *= 5;
            }
        }
        return result;
    }
}
