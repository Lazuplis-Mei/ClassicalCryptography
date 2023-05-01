using CommunityToolkit.HighPerformance;
using System.Buffers;
using System.Text.Unicode;
using System.Xml;

namespace ClassicalCryptography.Utils;

internal delegate TResult ReadOnlySpanFunc<T, out TResult>(ReadOnlySpan<T> span);

internal static class StringExtension
{

    public static bool IsCjkUnifiedIdeographs(this char character)
    {
        var unicodeRange = UnicodeRanges.CjkUnifiedIdeographs;
        char first = (char)unicodeRange.FirstCodePoint;
        char end = (char)(unicodeRange.FirstCodePoint + unicodeRange.Length);
        return char.IsBetween(character, first, end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ToUpperAscii(this char character)
    {
        if (character is >= 'a' and <= 'z')
            return (char)(character & 0B11011111);
        return character;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ToLowerAscii(this char character)
    {
        if (character is >= 'A' and <= 'Z')
            return (char)(character | 0B00100000);
        return character;
    }

    /// <summary>
    /// 字符串中的小写英文字母转换成大写英文字母
    /// </summary>
    [SkipLocalsInit]
    public static string ToUpperAscii(this string text)
    {
        int size = text.Length;
        Span<char> span = size.CanAllocString() ? stackalloc char[size] : new char[size];
        for (int i = 0; i < size; i++)
            span[i] = text[i].ToUpperAscii();
        return new(span);
    }

    /// <summary>
    /// 字符串中的大写英文字母转换成小写英文字母
    /// </summary>
    [SkipLocalsInit]
    public static string ToLowerAscii(this string text)
    {
        int size = text.Length;
        Span<char> span = size.CanAllocString() ? stackalloc char[size] : new char[size];
        for (int i = 0; i < size; i++)
            span[i] = text[i].ToLowerAscii();
        return new(span);
    }

    /// <summary>
    /// 添加一个xml元素，拥有指定的名字和内容
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteElement(this XmlWriter writer, string name, string text)
    {
        writer.WriteStartElement(name);
        writer.WriteString(text);
        writer.WriteEndElement();
    }

    /// <summary>
    /// 添加一个xml元素，内容是<see cref="BigInteger"/>的Base64形式
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteElement(this XmlWriter writer, string name, BigInteger number)
    {
        writer.WriteElement(name, number.ToBase64());
    }

    /// <summary>
    /// 移除最后一个字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder RemoveLast(this StringBuilder stringBuilder)
    {
        if (stringBuilder.Length > 0)
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        return stringBuilder;
    }

    /// <summary>
    /// 移除最后n个字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder RemoveLast(this StringBuilder stringBuilder, int count)
    {
        if (stringBuilder.Length >= count)
            stringBuilder.Remove(stringBuilder.Length - count, count);
        return stringBuilder;
    }

    /// <summary>
    /// 查找字符列表中的子序列
    /// </summary>
    /// <param name="list">字符列表</param>
    /// <param name="subsequence">要查找的子序列</param>
    /// <param name="positions">子序列中字符的位置</param>
    /// <returns>是否存在指定的子序列</returns>
    public static bool ContainsSubsequence(this List<char> list, string subsequence, out int[] positions)
    {
        positions = new int[subsequence.Length];
        positions[^1] = -1;
        if (list.Count < subsequence.Length)
            return false;
        for (int i = 0, j = 0; i < list.Count && j < positions.Length; i++)
        {
            if (list[i] == subsequence[j])
                positions[j++] = i;
        }
        return positions[^1] >= 0;
    }

    /// <summary>
    /// 英文字母转换成对应的数字A对应1
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LetterNumber(this char character) => character switch
    {
        >= 'A' and <= 'Z' => character - 'A' + 1,
        >= 'a' and <= 'z' => character - 'a' + 1,
        _ => throw new ArgumentOutOfRangeException(nameof(character)),
    };

    /// <summary>
    /// 英文字母转换成对应的数字A对应0
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LetterIndex(this char character) => character switch
    {
        >= 'A' and <= 'Z' => character - 'A',
        >= 'a' and <= 'z' => character - 'a',
        _ => throw new ArgumentOutOfRangeException(nameof(character)),
    };

    /// <summary>
    /// 小写36进制字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Base36Number(this char character) => character switch
    {
        >= '0' and <= '9' => character - '0',
        >= 'a' and <= 'z' => character - 'a' + 10,
        >= 'A' and <= 'Z' => character - 'A' + 10,
        _ => throw new ArgumentOutOfRangeException(nameof(character)),
    };

    /// <summary>
    /// <see cref="VChar64"/>字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int VChar64Number(this char character) => character switch
    {
        >= '0' and <= '9' => character - '0',
        >= 'A' and <= 'Z' => character - 'A' + 10,
        >= 'a' and <= 'z' => character - 'a' + 10 + 26,
        '+' => 62,
        '/' => 63,
        _ => throw new ArgumentOutOfRangeException(nameof(character)),
    };

    /// <summary>
    /// <see cref="DigitsOneFirst"/>字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DigitsOneFirstNumber(this char character) => character switch
    {
        '0' => 9,
        >= '1' and <= '9' => character - '1',
        _ => throw new ArgumentOutOfRangeException(nameof(character)),
    };

    /// <summary>
    /// 分解字符串为不重复的大写字母字符集合
    /// </summary>
    public static HashSet<char> Decompose(this string text)
    {
        var set = new HashSet<char>();
        foreach (var c in text)
        {
            if (c is >= 'a' and <= 'z')
                set.Add((char)('A' + c - 'a'));
            else
                set.Add(c);
        }
        return set;
    }

    /// <summary>
    /// 找到字符出现的所有位置
    /// </summary>
    /// <param name="text"></param>
    /// <param name="character"></param>
    public static List<int> FindAll(this string text, char character)
    {
        var result = new List<int>(2);
        for (int i = 0; i < text.Length; i++)
            if (text[i] == character)
                result.Add(i);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPrintable(this char character) => character is >= ' ' and not (char)0x7F;

    /// <summary>
    /// 按长度分割字符串
    /// </summary>
    /// <param name="text"></param>
    /// <param name="length">多达n个字符</param>
    /// <remarks>
    /// 最后一个字符串长度可能不足<paramref name="length"/>
    /// </remarks>
    public static string[] PartitionUpTo(this string text, int length)
    {
        var result = new string[text.Length.DivCeil(length)];
        int count = 0;
        for (int i = 0; i < result.Length - 1; i++, count += length)
            result[i] = text.Substring(count, length);
        result[^1] = text[count..];
        return result;
    }

    public static T[] Partition<T>(this string text, int length, ReadOnlySpanFunc<char, T> func)
    {
        var result = new T[text.Length / length];
        for (int i = 0; i < result.Length; i++)
            result[i] = func(text.AsSpan(i * length, length));
        return result;
    }

    public static void ForEachPartition(this string text, int length, ReadOnlySpanAction<char, int> action)
    {
        var count = text.Length / length;
        for (int i = 0; i < count; i++)
            action(text.AsSpan(i * length, length), i);
    }

    /// <summary>
    /// 以重复的字符生成字符串
    /// </summary>
    /// <param name="character">重复的字符</param>
    /// <param name="count">重复次数</param>
    [SkipLocalsInit]
    public static string Repeat(this char character, int count)
    {
        if (count <= 0)
            return string.Empty;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        span.Fill(character);
        return new(span);
    }

    /// <summary>
    /// 将数值转换成base64编码
    /// </summary>
    /// <param name="number">要转换的数</param>
    /// <returns>base64编码</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBase64(this BigInteger number)
    {
        return K4os.Text.BaseX.Base64.ToBase64(number.ToByteArray(true, true));
    }

}
