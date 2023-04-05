﻿using System.Collections;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 4进制数组
/// </summary>
public class QuaterArray : IEnumerable<int>
{
    private readonly byte[] array;

    /// <summary>
    /// 4进制数组
    /// </summary>
    public QuaterArray(int length)
    {
        Count = length;
        array = new byte[length.DivCeil(4)];
    }

    /// <summary>
    /// 4进制数组
    /// </summary>
    private QuaterArray(int length, byte[] arr)
    {
        Count = length;
        array = arr;
    }

    /// <summary>
    /// 元素个数
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// 设置和获取元素
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public int this[int index]
    {
        get
        {
            Guard.IsInRange(index, 0, Count);
            byte value = array[index >> 2];
            return (index & 0B11) switch
            {
                0B00 => (value >> 6),
                0B01 => ((value >> 4) & 0B11),
                0B10 => ((value >> 2) & 0B11),
                _ => (value & 0B11),
            };
        }
        set
        {
            Guard.IsInRange(index, 0, Count);
            value &= 0B11;
            int i = index >> 2;
            switch (index & 0B11)
            {
                case 0B00:
                    array[i] &= 0B00111111;
                    array[i] |= (byte)(value << 6);
                    break;
                case 0B01:
                    array[i] &= 0B11001111;
                    array[i] |= (byte)(value << 4);
                    break;
                case 0B10:
                    array[i] &= 0B11110011;
                    array[i] |= (byte)(value << 2);
                    break;
                default:
                    array[i] &= 0B11111100;
                    array[i] |= (byte)(value);
                    break;
            }
        }
    }

    /// <summary>
    /// 从字符串创建
    /// </summary>
    /// <param name="str"></param>
    public static QuaterArray FromString(string str)
    {
        int si = str.IndexOf(':');
        int count = int.Parse(str[..si++]);
        byte[] arr = Convert.FromBase64String(str[si..]);
        return new QuaterArray(count, arr);
    }

    /// <summary>
    /// 迭代器
    /// </summary>
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return this[i];
    }

    /// <summary>
    /// 字符串形式(Count:Base64)
    /// </summary>
    public override string ToString()
    {
        return $"{Count}:{Convert.ToBase64String(array)}";
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
