﻿using System.Collections;
using System.Numerics;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 一些数学计算的方法
/// </summary>
internal static class MathExtension
{

    /// <summary>
    /// 二进制序列转换回数值
    /// </summary>
    public static int ToInt32(this BitArray bits)
    {
        int number = 0;
        int @base = 1;
        for (int i = bits.Length - 1; i >= 0; i--)
        {
            if (bits[i])
                number += @base;
            @base <<= 1;
        }
        return number;
    }

    /// <summary>
    /// 数值转换成二进制序列
    /// </summary>
    /// <param name="number">数值</param>
    /// <param name="count">序列长度</param>
    public static BitArray ToBinary(this int number, int count)
    {
        var bits = new BitArray(count);
        for (int i = 0; i < count; i++)
            bits[i] = (number >> (count - i - 1) & 1) != 0;
        return bits;
    }

    /// <summary>
    /// 向上整除
    /// </summary>
    /// <param name="dividend">被除数</param>
    /// <param name="divisor">除数</param>
    public static int DivCeil(this int dividend, int divisor)
    {
        int quotient = dividend / divisor;
        quotient += dividend % divisor == 0 ? 0 : 1;
        return quotient;
    }

    /// <summary>
    /// 向上取平方根
    /// </summary>
    public static int SqrtCeil(this int number)
    {
        return (int)Math.Ceiling(Math.Sqrt(number));
    }

    /// <summary>
    /// x的y次方
    /// </summary>
    public static ulong Power(uint x, int y)
    {
        if (y == 0)
            return 1;
        if (y < 0)
            throw new Exception("不支持负指数");

        ulong result = x;
        for (int i = 1; i < y; i++)
        {
            result *= x;
        }
        return result;
    }

    /// <summary>
    /// 整数N的分拆
    /// </summary>
    public static long PartitionsP(int n)
    {
        var arr = new long[n];
        arr[0] = 1;
        for (int i = 1; i < arr.Length; i++)
        {
            for (int j = 1, ri = 0; ; j++)
            {
                ri += 2 * j - 1;
                if (i < ri)
                    break;
                arr[i] += (j % 2 == 0) ? -arr[i - ri] : arr[i - ri];
                ri += j;
                if (i < ri)
                    break;
                arr[i] += (j % 2 == 0) ? -arr[i - ri] : arr[i - ri];
            }
        }
        return arr[n - 1];
    }


    /// <summary>
    /// 计算x的阶乘(朴素方法)
    /// </summary>
    public static BigInteger Factorial(int x)
    {
        var result = BigInteger.One;
        for (int i = 2; i <= x; i++)
            result *= i;
        return result;
    }

}
