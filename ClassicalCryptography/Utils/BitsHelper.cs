using System.Collections;

namespace ClassicalCryptography.Utils;

internal static class BitsHelper
{

    /// <summary>
    /// 二进制序列转换为数值
    /// </summary>
    public static int ToInt32(this BitArray bits)
    {
        int number = 0;
        foreach (var value in bits.EnumeratorUnBox())
        {
            number <<= 1;
            if (value)
                number++;
        }
        return number;
    }

    /// <summary>
    /// 二进制序列转换为数值
    /// </summary>
    public static int ToInt32(this ReadOnlySpan<bool> bits)
    {
        int number = 0;
        foreach (var value in bits)
        {
            number <<= 1;
            if (value)
                number++;
        }
        return number;
    }

    /// <summary>
    /// 数值转换成<see cref="BitArray"/>
    /// </summary>
    /// <param name="number">数值</param>
    /// <param name="count">长度</param>
    public static BitArray ToBitArray(this int number, int count)
    {
        var bits = new BitArray(count);
        //如果可以直接修改m-array就好了
        for (int i = 0; i < count; i++)
            bits[i] = (number >> (count - i - 1) & 1) != 0;
        return bits;
    }

    /// <summary>
    /// 数值转换成<see cref="bool"/>序列
    /// </summary>
    public static void ToBitArray(this int number, Span<bool> bits)
    {
        for (int i = 0; i < bits.Length; i++)
            bits[i] = (number >> (bits.Length - i - 1) & 1) != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static int CombineInt32(byte b1, byte b2, byte b3, byte b4)
    {
        int number;
        byte* pointer = (byte*)&number;
        pointer[3] = b1;
        pointer[2] = b2;
        pointer[1] = b3;
        pointer[0] = b4;
        return number;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static uint CombineUInt32(byte b1, byte b2, byte b3, byte b4)
    {
        uint number;
        byte* pointer = (byte*)&number;
        pointer[3] = b1;
        pointer[2] = b2;
        pointer[1] = b3;
        pointer[0] = b4;
        return number;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void DecomposeInt32(int number, out byte b1, out byte b2, out byte b3, out byte b4)
    {
        byte* pointer = (byte*)&number;
        b1 = pointer[3];
        b2 = pointer[2];
        b3 = pointer[1];
        b4 = pointer[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void DecomposeUInt32(uint number, out byte b1, out byte b2, out byte b3, out byte b4)
    {
        byte* pointer = (byte*)&number;
        b1 = pointer[3];
        b2 = pointer[2];
        b3 = pointer[1];
        b4 = pointer[0];
    }

}
