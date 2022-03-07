using ClassicalCryptography.Utils;
using System.Collections;

namespace ClassicalCryptography.Transposition;

/// <summary>
/// 4进制数组
/// </summary>
public class QuaterArray : IEnumerable<int>
{
    private readonly byte[] array;
    /// <summary>
    /// 元素个数
    /// </summary>
    public int Count { get; }
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
    /// 设置和获取元素
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            byte val = array[index >> 2];
            return (index & 0B11) switch
            {
                0B00 => (val >> 6),
                0B01 => ((val >> 4) & 0B11),
                0B10 => ((val >> 2) & 0B11),
                //0B11
                _ => (val & 0B11),
            };
        }
        set
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            value &= 0B11;
            switch (index & 0B11)
            {
                case 0B00:
                    array[index >> 2] &= 0B00111111;
                    array[index >> 2] |= (byte)(value << 6);
                    break;
                case 0B01:
                    array[index >> 2] &= 0B11001111;
                    array[index >> 2] |= (byte)(value << 4);
                    break;
                case 0B10:
                    array[index >> 2] &= 0B11110011;
                    array[index >> 2] |= (byte)(value << 2);
                    break;
                default://3
                    array[index >> 2] &= 0B11111100;
                    array[index >> 2] |= (byte)(value);
                    break;
            }
        }
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
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}