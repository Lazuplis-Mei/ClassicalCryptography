using ClassicalCryptography.Interfaces;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 随机数
/// </summary>
internal static class RandomHelper
{
    /// <summary>
    /// 随机的字节值
    /// </summary>
    /// <param name="maxValue"></param>
    /// <returns></returns>
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
        Span<byte> buffer = stackalloc byte[byteCount];
        byte firstBit = (byte)(maxValue >> ((byteCount - 1) << 3));
        buffer[0] = RandomByte(firstBit);
        Random.Shared.NextBytes(buffer[1..]);
        return new BigInteger(buffer, true, true);
    }

    /// <summary>
    /// 随机的列表项目
    /// </summary>
    public static T RandomItem<T>(this List<T> list)
    {
        return list[Random.Shared.Next(list.Count)];
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
