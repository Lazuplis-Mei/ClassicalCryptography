using ClassicalCryptography.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// Unicode字符串
/// </summary>
public partial class UnicodeEncoding
{
    /// <summary>
    /// 转换成\u字符串
    /// </summary>
    public static string EncodeUnicode(string str, bool upperCase = false)
    {
        var patten = upperCase ? "\\u{0:X2}{1:X2}" : "\\u{0:x2}{1:x2}";
        var strbuilder = new StringBuilder();
        foreach (var c in str)
        {
            var bytes = Encoding.Unicode.GetBytes(c.ToString());//也许有更好的方法...
            strbuilder.AppendFormat(patten, bytes.Length == 2 ? bytes[1] : 0, bytes[0]);
        }
        return strbuilder.ToString();
    }

    /// <summary>
    /// 转义字符
    /// </summary>
    public static string Unescape(string str) => Regex.Unescape(str);

    /// <summary>
    /// 从\u字符串转换
    /// </summary>
    public static string DecodeUnicode(string str)
    {
        var matches = UnicodeRegex().Matches(str);
        int size = matches.Count << 1;
        Span<byte> bytes = size <= StackLimit.MaxByteSize
            ? stackalloc byte[size] : new byte[size];

        int i = 0;
        foreach (var match in matches.Cast<Match>())
        {
            var code = Convert.ToInt16(match.Value[2..], 16);
            bytes[i++] = (byte)(code & 0xff);
            bytes[i++] = (byte)(code >> 8);
        }
        return Encoding.Unicode.GetString(bytes);
    }

    [GeneratedRegex(@"\\u[0-9A-Fa-f]{2,4}")]
    private static partial Regex UnicodeRegex();
}
