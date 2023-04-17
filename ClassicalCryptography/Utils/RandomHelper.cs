using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 随机数
/// </summary>
internal static class RandomHelper
{
    /// <summary>
    /// 随机的真值
    /// </summary>
    public static bool TrueOrFalse => Random.Shared.NextDouble() <= 0.5;

    /// <summary>
    /// 随机的4进制数
    /// </summary>
    public static byte TwoBits => (byte)(Random.Shared.Next() & 0B11);

    /// <summary>
    /// 随机的字节值
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte RandomByte(byte maxValue)
    {
        return (byte)Random.Shared.Next(maxValue);
    }

    /// <summary>
    /// 随机的字节值
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte RandomByte(byte minValue, byte maxValue)
    {
        return (byte)Random.Shared.Next(minValue, maxValue);
    }

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
            permutation[i] = list.PopRandomItem();
        return permutation;
    }

    /// <summary>
    /// 打乱数组
    /// </summary>
    public static void Shuffle<T>(this T[] array)
    {
        for (int i = array.Length - 1; i > 1; i--)
        {
            int j = Random.Shared.Next(i);
            if (i == j) continue;
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
        Guard.IsInRange(count, 0, max);

        var list = new List<int>(count);
        for (int temp, i = 0; i < count; i++)
        {
            do
                temp = Random.Shared.Next(max);
            while (list.Contains(temp));
            list.Add(temp);
        }
        return list;
    }

    /// <summary>
    /// 获得随机的4进制数组
    /// </summary>
    /// <param name="count">数组长度</param>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BigInteger RandomBigInt(BigInteger maxValue)
    {
        GuardEx.IsPositive(maxValue);
        if (maxValue <= long.MaxValue)
            return Random.Shared.NextInt64((long)maxValue);
        Span<byte> bytes = maxValue.ToByteArray(true, true);
        bytes[0] = RandomByte(bytes[0]);
        Random.Shared.NextBytes(bytes[1..]);
        return new BigInteger(bytes, true, true);
    }

    /// <summary>
    /// 产生范围内的随机数
    /// </summary>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BigInteger RandomBigInt(BigInteger minValue, BigInteger maxValue)
    {
        GuardEx.IsPositive(minValue);
        GuardEx.IsPositive(maxValue);
        Guard.IsGreaterThan(maxValue, minValue);

        if (maxValue <= long.MaxValue)
            return Random.Shared.NextInt64((long)minValue, (long)maxValue);

        Span<byte> minBytes = minValue.ToByteArray(true, true);
        Span<byte> maxBytes = maxValue.ToByteArray(true, true);
        var bytes = minBytes;
        
        int j = 0;
        while (minBytes[j] == maxBytes[j])
            j++;
        minBytes = minBytes[j..];
        maxBytes = maxBytes[j..];

        var minByte = minBytes[0];
        minBytes[0] = RandomByte(minByte, maxBytes[0]);
        if (minBytes[0] != minByte)
            Random.Shared.NextBytes(minBytes[1..]);
        else
        {
            minBytes = minBytes[1..];
            for (int i = 0; i < minBytes.Length; i++)
            {
                var value = RandomByte(minBytes[i], byte.MaxValue);
                if (value != minBytes[i])
                {
                    minBytes[i++] = value;
                    Random.Shared.NextBytes(minBytes[i..]);
                    break;
                }
            }
        }
        return new BigInteger(bytes, true, true);
    }

    /// <summary>
    /// 随机的列表项目
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T RandomItem<T>(this List<T> list)
    {
        Guard.HasSizeGreaterThan(list, 0);
        if (list.Count == 1)
            return list[0];
        return list[Random.Shared.Next(list.Count)];
    }

    /// <summary>
    /// 随机的数组项目
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T RandomItem<T>(this T[] array)
    {
        Guard.HasSizeGreaterThan(array, 0);
        if (array.Length == 1)
            return array[0];
        return array[Random.Shared.Next(array.Length)];
    }

    /// <summary>
    /// 返回并移除随机的列表项目
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T PopRandomItem<T>(this List<T> list)
    {
        Guard.HasSizeGreaterThan(list, 0);
        if (list.Count == 1)
        {
            var value = list[0];
            list.Clear();
            return value;
        }
        int t = Random.Shared.Next(list.Count);
        var item = list[t];
        list.RemoveAt(t);
        return item;
    }
}
