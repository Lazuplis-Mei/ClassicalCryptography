﻿using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 随机数
/// </summary>
internal static class RandomHelper
{
    /// <summary>
    /// 随机的字节值
    /// </summary>
    public static byte RandomByte(byte maxValue)
    {
        return (byte)Random.Shared.Next(maxValue);
    }
    /// <summary>
    /// 随机的真值
    /// </summary>
    public static bool TrueOrFalse => Random.Shared.NextDouble() <= 0.5;
    /// <summary>
    /// 随机的4进制数
    /// </summary>
    public static byte TwoBits => (byte)(Random.Shared.Next() & 0B11);

    /// <summary>
    /// 获得随机的排列
    /// </summary>
    /// <param name="count">排列长度</param>
    public static ushort[] RandomPermutation(int count)
    {
        var permutation = new ushort[count];
        permutation.FillOrder();
        var list = permutation.ToList();
        for (int i = 0; i < count; i++)
        {
            int t = Random.Shared.Next(list.Count);
            permutation[i] = list[t];
            list.RemoveAt(t);
        }
        return permutation;
    }

    /// <summary>
    /// 打乱数组
    /// </summary>
    public static void Shuffle(this int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Shared.Next(i + 1);
            if (i != j)
                (array[j], array[i]) = (array[i], array[j]);
        }
    }

    /// <summary>
    /// 获得随机且不重复的自然数
    /// </summary>
    /// <param name="max">最大值</param>
    /// <param name="count">数量</param>
    public static List<int> RandomSample(int max, int count)
    {
        if (count > max || count <= 0)
            throw new ArgumentException("不正确的数量", nameof(count));
        var list = new List<int>(count);
        for (int i = 0; i < count; i++)
        {
            int temp;
            do
                temp = Random.Shared.Next(max);
            while (list.Contains(temp));
            list.Add(temp);
        }
        return list;
    }

    /// <summary>
    /// 获得随机的排列
    /// </summary>
    /// <param name="count">排列长度</param>
    public static QuaterArray RandomQuaterArray(int count)
    {
        var array = new QuaterArray(count);
        for (int i = 0; i < count; i++)
            array[i] = TwoBits;
        return array;
    }


    /// <summary>
    /// 产生不大于<paramref name="maxValue"/>的随机数
    /// </summary>
    /// <param name="maxValue">最大值</param>
    [SkipLocalsInit]
    public static BigInteger RandomBigInt(BigInteger maxValue)
    {
        if (maxValue < int.MaxValue)
            return Random.Shared.Next((int)maxValue);
        int byteCount = maxValue.GetByteCount(true);
        Span<byte> buffer = byteCount.CanAllocate()
            ? stackalloc byte[byteCount] : new byte[byteCount];
        byte firstBit = (byte)(maxValue >> ((byteCount - 1) << 3));
        buffer[0] = RandomByte(firstBit);
        Random.Shared.NextBytes(buffer[1..]);
        return new BigInteger(buffer, true, true);
    }

    /// <summary>
    ///  生成一个[min,max]范围的<see cref="BigInteger"/>
    /// <para>see  </para>
    /// </summary>
    /// <param name="min">min value</param>
    /// <param name="max">max value</param>
    [ReferenceFrom("https://github.com/mikolajtr/dotnetprime/blob/master/MillerRabin/Helpers/PrimeGeneratorHelpers.cs#L8")]
    public static BigInteger RandomBigInteger(BigInteger min, BigInteger max)
    {
        byte[] bytes = max.ToByteArray();

        Random.Shared.NextBytes(bytes);
        bytes[^1] &= 0x7F;
        var value = new BigInteger(bytes);
        return (value % (max - min + 1)) + min;
    }

    /// <summary>
    /// 随机的列表项目
    /// </summary>
    public static T RandomItem<T>(this List<T> list)
    {
        Guard.HasSizeGreaterThan(list, 0);
        if (list.Count == 1)
            return list[0];
        return list[Random.Shared.Next(list.Count)];
    }

    /// <summary>
    /// 随机的列表项目
    /// </summary>
    public static T RandomItem<T>(this T[] array)
    {
        return array[Random.Shared.Next(array.Length)];
    }

    /// <summary>
    /// 返回并移除随机的列表项目
    /// </summary>
    public static T PopRandomItem<T>(this List<T> list)
    {
        var item = list.RandomItem();
        list.Remove(item);
        return item;
    }
}
