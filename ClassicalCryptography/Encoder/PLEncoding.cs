using ClassicalCryptography.Utils;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static ClassicalCryptography.Encoder.PLEncodings.Constants;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 几个编程语言相关的编码
/// </summary>
public static partial class PLEncoding
{
    [GeneratedRegex("\\\\x[0-9a-f]")]
    private static partial Regex PYHexRegex();


    /// <summary>
    /// 转换为python bytes
    /// </summary>
    public static string ToPYBytes(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        var str = new StringBuilder(bytes.Length * 4);
        foreach (var b in bytes)
        {
            str.Append(@"\x");
            str.Append(b.ToString("x2"));
        }
        return str.ToString();
    }

    /// <summary>
    /// 从python bytes转换
    /// </summary>
    public static string FromPYBytes(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var matches = PYHexRegex().Matches(input);
        var bytes = new byte[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            bytes[i++] = Convert.ToByte(match.Value[2..], 16);
        }
        return encoding.GetString(bytes);
    }

    /// <summary>
    /// To Punycode
    /// </summary>
    public static string ToPunycode(string input)
    {
        var mapping = new IdnMapping();
        return mapping.GetAscii(input);
    }

    /// <summary>
    /// From Punycode
    /// </summary>
    public static string FromPunycode(string input)
    {
        var mapping = new IdnMapping();
        return mapping.GetUnicode(input);
    }


    /// <summary>
    /// a subset of Perl who restricts source code to have only Perl keywords
    /// </summary>
    public static string PPEncode(string code)
    {
        static void character(int c, StringBuilder strBuilder)
        {
            if (c > 255)
                return;
            var p = ppcodes[c].RandomItem();
            for (var i = 0; i < p.Length; i += 2)
            {
                strBuilder.Append(ppwords[Convert.ToInt32(p.Substring(i, 2), 16)]);
                strBuilder.Append(' ');
            }
        }
        var result = new StringBuilder();
        result.AppendLine("#!/usr/bin/perl -w");
        character(Random.Shared.Next(1, 10), result);
        for (var i = 0; i < code.Length; i++)
        {
            result.Append("and print chr ");
            character(code[i], result);
        }
        result.AppendLine();
        return result.ToString();
    }

    /// <summary>
    /// Encode any JavaScript program to Japanese style emoticons
    /// </summary>
    public static string AAEncode(string jscode)
    {
        var result = new StringBuilder();
        result.Append(aacode);

        for (int i = 0; i < jscode.Length; i++)
        {
            int c = jscode[i];
            result.Append("(ﾟДﾟ)[ﾟεﾟ]+");
            if (c <= 127)
            {
                var m = Convert.ToString(c, 8);
                for (int k = 0; k < m.Length; k++)
                {
                    result.Append(aacodes[m[k] - '0']);
                    result.Append("+ ");
                }
            }
            else
            {
                var m = c.ToString("x4");
                result.Append("(oﾟｰﾟo)+ ");
                for (int k = 0; k < m.Length; k++)
                {
                    result.Append(aacodes["0123456789abcdef".IndexOf(m[k])]);
                    result.Append("+ ");
                }
            }
        }
        result.Append("(ﾟДﾟ)[ﾟoﾟ]) (ﾟΘﾟ)) ('_');");
        return result.ToString();
    }

    /// <summary>
    /// Encode any JavaScript program using only symbols
    /// </summary>
    public static string JJEncode(string jscode, string gv = "DeJS")
    {
        var result = new StringBuilder();
        var str = new StringBuilder();
        for (var i = 0; i < jscode.Length; i++)
        {
            int c = jscode[i];
            switch (c)
            {
                case 0x22:
                case 0x5c:
                    str.Append($@"\\\{jscode[i]}");
                    break;
                case >= 0x21 and <= 0x2f:
                case >= 0x3A and <= 0x40:
                case >= 0x5b and <= 0x60:
                case >= 0x7b and <= 0x7f:
                    //}else if( (0x20 <= n && n <= 0x2f) || (0x3A <= n == 0x40) || ( 0x5b <= n && n <= 0x60 ) || ( 0x7b <= n && n <= 0x7f ) ){
                    str.Append(jscode[i]);
                    break;
                case >= 0x30 and <= 0x39:
                case >= 0x61 and <= 0x66:
                    if (str.Length > 0)
                        result.Append($@"""{str}""+");
                    result.Append($"{gv}.{jjcodes[c < 0x40 ? c - 0x30 : c - 0x57]}+");
                    str.Clear();
                    break;
                case 0x6c:
                    if (str.Length > 0)
                        result.Append($@"""{str}""+");
                    result.Append($@"(![]+"""")[{gv}._$_]+");
                    str.Clear();
                    break;
                case 0x6f:
                    if (str.Length > 0)
                        result.Append($@"""{str}""+");
                    result.Append($"{gv}._$+");
                    str.Clear();
                    break;
                case 0x74:
                    if (str.Length > 0)
                        result.Append($@"""{str}""+");
                    result.Append($"{gv}.__+");
                    str.Clear();
                    break;
                case 0x75:
                    if (str.Length > 0)
                        result.Append($@"""{str}""+");
                    result.Append($"{gv}._+");
                    str.Clear();
                    break;
                case < 128:
                {
                    if (str.Length > 0)
                        result.Append($@"""{str}");
                    else
                        result.Append('"');
                    result.Append(@"\\""+");

                    var m = Convert.ToString(c, 8);
                    for (int k = 0; k < m.Length; k++)
                    {
                        result.Append($"{gv}.{jjcodes[m[k] - '0']}+");
                    }
                    str.Clear();
                    break;
                }

                default:
                {
                    if (str.Length > 0)
                        result.Append($"\"{str}");
                    else
                        result.Append('"');
                    result.Append($@"\\""+{gv}._+");
                    var m = c.ToString("x4");
                    for (int k = 0; k < m.Length; k++)
                    {
                        result.Append(jjcodes["0123456789abcdef".IndexOf(m[k])]);
                        result.Append('+');
                    }

                    str.Clear();
                    break;
                }
            }
        }

        if (str.Length > 0)
            result.Append($@"""{str}""+");

        return string.Format(jjcode, gv, result);
    }

    /// <summary>
    /// JotherEncode
    /// </summary>
    public static string JotherEncode(string jsocde)
    {
        return "";
    }


    /// <summary>
    /// jsfuck
    /// </summary>
    public static string JSFuckEncode(string jsocde)
    {
        return "";
    }


    /// <summary>
    /// BrainfuckEncode
    /// </summary>
    public static string BrainfuckEncode(string text)
    {
        return "";
    }

    /// <summary>
    /// BrainfuckDecode
    /// </summary>
    public static string BrainfuckDecode(string bfcode)
    {
        return "";
    }


}
