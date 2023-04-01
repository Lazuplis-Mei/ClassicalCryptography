﻿using CommunityToolkit.HighPerformance;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;

namespace ClassicalCryptography.Utils;

internal static class StringExtension
{

    /// <summary>
    /// 字符串转换成可修改的内存
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> AsWriteableSpan(this string text)
    {
        return MemoryMarshal.CreateSpan(ref text.DangerousGetReference(), text.Length);
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
    /// 查找字符列表中的子序列
    /// </summary>
    public static bool ContainsSubString(this List<char> list, string subString, out int[] positions)
    {
        positions = new int[subString.Length];
        positions[^1] = -1;
        if (list.Count < subString.Length)
            return false;
        for (int i = 0, j = 0; i < list.Count && j < positions.Length; i++)
        {
            if (list[i] == subString[j])
                positions[j++] = i;
        }
        return positions[^1] >= 0;
    }

    /// <summary>
    /// 英文字母转换成对应的数字
    /// </summary>
    public static int LetterNumber(this char character)
    {
        if (character is >= 'A' and <= 'Z')
            return character - 'A' + 1;
        else if (character is >= 'a' and <= 'z')
            return character - 'a' + 1;
        throw new ArgumentOutOfRangeException(nameof(character));
    }

    /// <summary>
    /// 小写36进制字符
    /// </summary>
    public static int Base36Number(this char character)
    {
        if (character is >= '0' and <= '9')
            return character - '0';
        else if (character is >= 'a' and <= 'z')
            return character - 'a' + 10;
        else if (character is >= 'A' and <= 'Z')
            return character - 'A' + 10;
        throw new ArgumentOutOfRangeException(nameof(character));
    }

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
        var result = new List<int>();
        for (int i = 0; i < text.Length; i++)
            if (text[i] == character)
                result.Add(i);
        return result;
    }

    /// <summary>
    /// 中文按UTF-8转换成Base64字符串
    /// </summary>
    /// <param name="text">中文字符串</param>
    /// <param name="encoding">默认编码UTF8</param>
    public static string ToBase64(this string text, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var bytes = encoding.GetBytes(text);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64字符串按UTF-8转换成中文
    /// </summary>
    /// <param name="base64">中文字符串</param>
    /// <param name="encoding">默认编码UTF8</param>
    public static string FromBase64(this string base64, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var bytes = Convert.FromBase64String(base64);
        return encoding.GetString(bytes);
    }

    public static bool IsPrintable(this char c) => c is >= ' ' and not (char)0x7F;

    public static string[] Partition(this string self, int length)
    {
        var result = new string[self.Length.DivCeil(length)];
        for (int i = 0; i < result.Length - 1; i++)
            result[i] = self.Substring(i * length, length);
        result[^1] = self[((result.Length - 1) * length)..];
        return result;
    }

    public static T[] Partition<T>(this string self, int length, Func<string, T> converter)
    {
        var result = new T[self.Length / length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = converter(self.Substring(i * length, length));
        }
        return result;
    }

    /// <summary>
    /// 以重复的字符生成字符串
    /// </summary>
    /// <param name="character">重复的字符</param>
    /// <param name="count">重复次数</param>
    [SkipLocalsInit]
    public static string Repeat(this char character, int count)
    {
        Span<char> span = count.CanAllocString()
            ? stackalloc char[count] : new char[count];
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
        return Convert.ToBase64String(number.ToByteArray(true, true));
    }

}
