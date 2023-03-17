using ClassicalCryptography.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// Unicode字符串
/// </summary>
public partial class UnicodeEncoding
{
    /// <summary>
    /// 转换成\u字符串
    /// </summary>
    [SkipLocalsInit]
    public static string Encode(string str)
    {
        int size = str.Length * 6;
        Span<char> span = size <= StackLimit.MaxCharSize
            ? stackalloc char[size] : new char[size];
        var orignalSpan = span;
        for (int i = 0; i < str.Length; i++)
        {
            @$"\u{(int)str[i]:x4}".CopyTo(span);
            span = span[6..];
        }
        return new string(orignalSpan);
    }

    /// <summary>
    /// 转义字符
    /// </summary>
    public static string Unescape(string str) => Regex.Unescape(str);

    /// <summary>
    /// 从\u字符串转换
    /// </summary>
    [SkipLocalsInit]
    public static string Decode(string str)
    {
        var matches = UnicodeRegex().Matches(str);
        Span<char> span = matches.Count <= StackLimit.MaxCharSize
            ? stackalloc char[matches.Count] : new char[matches.Count];

        for (int i = 0; i < matches.Count; i++)
        {
            var code = Convert.ToInt16(matches[i].Value[2..], 16);
            span[i] = (char)code;
        }
        return new string(span);
    }

    [GeneratedRegex(@"\\u[0-9A-Fa-f]{1,4}")]
    private static partial Regex UnicodeRegex();
}
