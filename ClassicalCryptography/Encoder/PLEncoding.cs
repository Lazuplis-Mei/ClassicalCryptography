using ClassicalCryptography.Encoder.PLEncodings;
using System.Globalization;
using static ClassicalCryptography.Encoder.PLEncodings.Constants;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 几个编程语言相关的编码
/// </summary>
public static partial class PLEncoding
{
    [GeneratedRegex(@"\\x[0-9a-f]{2}")]
    private static partial Regex PYHexRegex();


    /// <summary>
    /// 转换为python bytes的格式
    /// </summary>
    public static string ToPythonBytes(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var result = new StringBuilder(bytes.Length << 2);
        foreach (var code in bytes)
        {
            result.Append(@"\x").Append(code.ToString("x2"));
        }
        return result.ToString();
    }

    /// <summary>
    /// 从python bytes转换
    /// </summary>
    public static string FromPythonBytes(string input)
    {
        var matches = PYHexRegex().Matches(input);
        Span<byte> bytes = matches.Count.CanAllocate()
            ? stackalloc byte[matches.Count] : new byte[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            bytes[i] = Convert.ToByte(matches[i].Value[2..], 16);
        }
        return Encoding.UTF8.GetString(bytes);
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
        static void AppendCharacter(int character, StringBuilder strBuilder)
        {
            if (character > 255)
                return;
            var code = ppCodes[character].RandomItem();
            for (var i = 0; i < code.Length; i += 2)
            {
                strBuilder.Append(ppWords[Convert.ToInt32(code.Substring(i, 2), 16)]);
                strBuilder.Append(' ');
            }
        }

        var result = new StringBuilder();
        result.AppendLine("#!/usr/bin/perl -w");
        AppendCharacter(Random.Shared.Next(1, 10), result);
        for (var i = 0; i < code.Length; i++)
        {
            result.Append("and print chr ");
            AppendCharacter(code[i], result);
        }
        result.AppendLine();
        return result.ToString();
    }

    /// <summary>
    /// 编码JavaScript代码为颜文字
    /// </summary>
    /// <param name="jsCode">js代码</param>
    [ReferenceFrom("https://github.com/ezeeo/ctf-tools/blob/095808a84d34e7ebfa6bcbe063275da24563f092/Library/jj_and_aa/aaencode.js", ProgramingLanguage.JavaScript)]
    public static string AAEncode(string jsCode)
    {
        var result = new StringBuilder();
        result.Append(Properties.Resources.AAEncodingString);

        for (int i = 0; i < jsCode.Length; i++)
        {
            int character = jsCode[i];
            result.Append("(ﾟДﾟ)[ﾟεﾟ]+");
            if (character <= 0x7F)
            {
                var octStrng = Convert.ToString(character, 8);
                for (int j = 0; j < octStrng.Length; j++)
                {
                    result.Append(aaCodes[octStrng[j].Base36Number()]).Append("+ ");
                }
            }
            else
            {
                var hexString = character.ToString("x4");
                result.Append("(oﾟｰﾟo)+ ");
                for (int j = 0; j < hexString.Length; j++)
                {
                    result.Append(aaCodes[hexString[j].Base36Number()]).Append("+ ");
                }
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

        return string.Format(Properties.Resources.JJEncodngString, varName, result);
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
