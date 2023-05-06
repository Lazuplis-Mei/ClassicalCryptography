using ClassicalCryptography.Encoder.PLEncodings;
using CommunityToolkit.HighPerformance.Buffers;
using System.Globalization;
using static ClassicalCryptography.Encoder.PLEncodings.Constants;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 几个编程语言相关的编码
/// </summary>
public static partial class PLEncoding
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    [GeneratedRegex(@"(?<Hex>(\\x[0-9a-f]{2})+)|(\\\\|[^\\])+")]
    private static partial Regex PYHexRegex();

    /// <summary>
    /// 转换为python bytes的格式
    /// </summary>
    /// <remarks>
    /// 值得注意的是，此方法将转换所有字符，不保留可见的ascii字符。<br/>
    /// 且.Net中的Unicode并不等同于python中的utf-16，而是utf-16le，因为python中的utf-16带有BOM
    /// </remarks>
    public static string ToPythonBytes(string input)
    {
        int count = Encoding.GetByteCount(input);
        using var memory = count.TryAlloc();
        Span<byte> buffer = count.CanAlloc() ? stackalloc byte[count] : memory.Span;
        Encoding.GetBytes(input, buffer);
        int size = count * 4;
        using var memory2 = size.TryAllocString();
        Span<char> buffer2 = size.CanAllocString() ? stackalloc char[size] : memory2.Span;
        for (int i = 0; i < count; i++)
        {
            int j = i * 4;
            buffer2[j++] = '\\';
            buffer2[j++] = 'x';
            buffer2[j++] = HexLower[buffer[i] >> 4];
            buffer2[j] = HexLower[buffer[i] & 0xF];
        }
        return new(buffer2);
    }

    /// <summary>
    /// 从python bytes转换
    /// </summary>
    /// <remarks>
    /// 关于编码，请参考<seealso cref="ToPythonBytes"/>
    /// </remarks>
    public static string FromPythonBytes(string input)
    {
        IEnumerable<Match> matches = PYHexRegex().Matches(input);
        var result = new StringBuilder(input.Length / 3);
        foreach (var match in matches)
        {
            Group group;
            if ((group = match.Groups["Hex"]).Success)
            {
                ReadOnlySpan<char> span = group.ValueSpan;
                int length = span.Length / 4;
                using var memory = SpanOwner<byte>.Allocate(length);
                var bytes = memory.Span;
                for (int i = 1; i < span.Length; i++)
                {
                    int high = span[++i].Base36Number();
                    int low = span[++i].Base36Number();
                    bytes[i++ / 4] = MathEx.MakeByte((byte)high, (byte)low);
                }
                result.Append(Encoding.GetString(bytes));
                continue;
            }
            result.Append(match.ValueSpan);
        }
        return result.ToString();
    }

    private static readonly IdnMapping idnMapping = new();

    /// <summary>
    /// 转换成Punycode编码
    /// </summary>
    public static string ToPunycode(string input) => idnMapping.GetAscii(input);

    /// <summary>
    /// 从Punycode编码转换
    /// </summary>
    public static string FromPunycode(string input) => idnMapping.GetUnicode(input);

    /// <summary>
    /// Perl的一个子集，限制了源代码只能有Perl的关键字
    /// </summary>
    [ReferenceFrom("https://github.com/ezeeo/ctf-tools/blob/095808a84d34e7ebfa6bcbe063275da24563f092/Library/ppencode/ppencode.js", ProgramingLanguage.JavaScript)]
    public static string PPEncode(string code)
    {
        Span<byte> bytes = Encoding.GetBytes(code);
        var result = new StringBuilder(512);
        result.AppendLine("#!/usr/bin/perl -w");
        result.AppendCharacter(RandomHelper.NextByte(1, 10));
        foreach (var value in bytes)
        {
            result.Append("and print chr ");
            result.AppendCharacter(value);
        }
        result.AppendLine();
        return result.ToString();
    }

    private static void AppendCharacter(this StringBuilder strBuilder, byte index)
    {
        var code = ppCodes[index, Random.Shared.Next(3)];
        for (var i = 0; i < code.Length; i++)
        {
            int high = code[i++].Base36Number();
            int low = code[i].Base36Number();
            strBuilder.Append(ppWords[MathEx.MakeByte((byte)high, (byte)low)]);
            strBuilder.Append(' ');
        }
    }

    /// <summary>
    /// 编码JavaScript代码为颜文字
    /// </summary>
    /// <param name="jsCode">js代码</param>
    [ReferenceFrom("https://github.com/ezeeo/ctf-tools/blob/095808a84d34e7ebfa6bcbe063275da24563f092/Library/jj_and_aa/aaencode.js", ProgramingLanguage.JavaScript)]
    public static string AAEncode(string jsCode)
    {
        var result = new StringBuilder();
        result.Append(Resources.AAEncodingString);

        for (int i = 0; i < jsCode.Length; i++)
        {
            result.Append("(ﾟДﾟ)[ﾟεﾟ]+");
            var character = jsCode[i];
            if (char.IsAscii(character))
            {
                var octStrng = Convert.ToString(character, 8);
                for (int j = 0; j < octStrng.Length; j++)
                    result.Append(aaCodes[octStrng[j].Base36Number()]).Append("+ ");
            }
            else
            {
                var hexString = ((int)character).ToString("x4");
                result.Append("(oﾟｰﾟo)+ ");
                for (int j = 0; j < hexString.Length; j++)
                    result.Append(aaCodes[hexString[j].Base36Number()]).Append("+ ");
            }
        }
        result.Append("(ﾟДﾟ)[ﾟoﾟ]) (ﾟΘﾟ)) ('_');");
        return result.ToString();
    }

    /// <summary>
    /// 编码JavaScript代码
    /// </summary>
    /// <param name="jsCode">js代码</param>
    /// <param name="varName">变量名</param>
    [ReferenceFrom("https://github.com/ezeeo/ctf-tools/blob/095808a84d34e7ebfa6bcbe063275da24563f092/Library/jj_and_aa/jjencode.js", ProgramingLanguage.JavaScript)]
    public static string JJEncode(string jsCode, string varName = "DeJS")
    {
        var result = new StringBuilder();
        var tempString = new StringBuilder();
        for (var i = 0; i < jsCode.Length; i++)
        {
            int character = jsCode[i];
            switch (character)
            {
                case 0x22:
                case 0x5C:
                    tempString.Append($@"\\\{jsCode[i]}");
                    break;
                case >= 0x21 and <= 0x2F:
                case >= 0x3A and <= 0x40:
                case >= 0x5B and <= 0x60:
                case >= 0x7B and <= 0x7F:
                    tempString.Append(jsCode[i]);
                    break;
                case >= 0x30 and <= 0x39:
                case >= 0x61 and <= 0x66:
                    if (tempString.Length > 0)
                        result.Append($@"""{tempString}""+");
                    result.Append($"{varName}.{jjCodes[character < 0x40
                        ? character - 0x30 : character - 0x57]}+");
                    tempString.Clear();
                    break;
                case 0x6C:
                    if (tempString.Length > 0)
                        result.Append($@"""{tempString}""+");
                    result.Append($@"(![]+"""")[{varName}._$_]+");
                    tempString.Clear();
                    break;
                case 0x6F:
                    if (tempString.Length > 0)
                        result.Append($@"""{tempString}""+");
                    result.Append($"{varName}._$+");
                    tempString.Clear();
                    break;
                case 0x74:
                    if (tempString.Length > 0)
                        result.Append($@"""{tempString}""+");
                    result.Append($"{varName}.__+");
                    tempString.Clear();
                    break;
                case 0x75:
                    if (tempString.Length > 0)
                        result.Append($@"""{tempString}""+");
                    result.Append($"{varName}._+");
                    tempString.Clear();
                    break;
                case < 0x80:
                {
                    result.Append('"');
                    if (tempString.Length > 0)
                        result.Append(tempString);
                    result.Append(@"\\""+");

                    var octString = Convert.ToString(character, 8);
                    for (int j = 0; j < octString.Length; j++)
                        result.Append($"{varName}.{jjCodes[octString[j].Base36Number()]}+");
                    tempString.Clear();
                    break;
                }

                default:
                {
                    result.Append('"');
                    if (tempString.Length > 0)
                        result.Append(tempString);
                    result.Append($@"\\""+{varName}._+");
                    var hexString = character.ToString("x4");
                    for (int j = 0; j < hexString.Length; j++)
                    {
                        result.Append(jjCodes[hexString[j].Base36Number()]);
                        result.Append('+');
                    }

                    tempString.Clear();
                    break;
                }
            }
        }

        if (tempString.Length > 0)
            result.Append($@"""{tempString}""+");
        return string.Format(Resources.JJEncodngString, varName, result);
    }

    /// <summary>
    /// 使用Jother编码JavaScript代码
    /// </summary>
    /// <param name="jscode">js代码</param>
    public static string JotherEncode(string jscode) => Jother.ToScript(jscode);

    /// <summary>
    /// 使用Jother编码JavaScript字符串
    /// </summary>
    /// <param name="text">字符串</param>
    public static string JotherEncodeString(string text) => Jother.ToString(text);

    /// <summary>
    /// 编码为brainfuck代码
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    public static string BrainfuckEncode(string text) => Brainfuck.GenerateCode(text);

    /// <summary>
    /// 解释Brainfuck代码
    /// </summary>
    public static string BrainfuckDecode(string bfcode) => Brainfuck.Interpret(bfcode);
}
