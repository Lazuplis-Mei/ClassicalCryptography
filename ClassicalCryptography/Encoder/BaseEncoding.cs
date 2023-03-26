using ClassicalCryptography.Encoder.BaseEncodings;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 包含多种Base编码和解码方案
/// </summary>
public static partial class BaseEncoding
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 转换为Base64编码
    /// </summary>
    public static string ToBase64(string input)
    {
        return Convert.ToBase64String(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 转换为Base64URL编码
    /// </summary>
    public static string ToBase64URL(string input)
    {
        var str = Convert.ToBase64String(Encoding.GetBytes(input));
        return str.Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    /// <summary>
    /// 从Base64编码转换
    /// </summary>
    public static string FromBase64(string input)
    {
        return Encoding.GetString(Convert.FromBase64String(input));
    }

    /// <summary>
    /// 从Base64URL编码转换
    /// </summary>
    public static string FromBase64URL(string input)
    {
        input = input.Replace('_', '/').Replace('-', '+');
        switch (input.Length % 4)
        {
            case 2:
                input += "==";
                break;
            case 3:
                input += "=";
                break;
        }
        return Encoding.GetString(Convert.FromBase64String(input));
    }

    /// <summary>
    /// 转换为Base32编码
    /// </summary>
    public static string ToBase32(string input)
    {
        return Base32Encoding.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base32编码转换
    /// </summary>
    public static string FromBase32(string input)
    {
        return Encoding.GetString(Base32Encoding.Decode(input));
    }

    /// <summary>
    /// 转换为Base32768编码
    /// </summary>
    public static string ToBase32768(string input)
    {
        return Base32768Encoding.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base32768编码转换
    /// </summary>
    public static string FromBase32768(string input)
    {
        return Encoding.GetString(Base32768Encoding.Decode(input));
    }

    /// <summary>
    /// 转换为Base65536编码
    /// </summary>
    public static string ToBase65536(string input)
    {
        return Base65536Encoding.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base65536编码转换
    /// </summary>
    public static string FromBase65536(string input)
    {
        return Encoding.GetString(Base65536Encoding.Decode(input));
    }

    /// <summary>
    /// 转换为Base2048编码
    /// </summary>
    public static string ToBase2048(string input)
    {
        return Base2048Encoding.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base2048编码转换
    /// </summary>
    public static string FromBase2048(string input)
    {
        return Encoding.GetString(Base2048Encoding.Decode(input));
    }

    /// <summary>
    /// 转换为16进制编码
    /// </summary>
    public static string ToBase16(string input)
    {
        return Convert.ToHexString(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 编码最基本形式的QuotedPrintable
    /// </summary>
    public static string ToQuotedPrintable(string input)
    {
        var result = new StringBuilder();
        foreach (var character in input)
        {
            if (char.IsAscii(character))
            {
                if (character == '=')
                    result.Append("=3D");
                else
                    result.Append(character);
            }
            else
            {
                var bytes = Encoding.GetBytes(character.ToString());
                foreach (var value in bytes)
                {
                    result.Append('=');
                    result.Append(GlobalTables.UHexString[value >> 4]);
                    result.Append(GlobalTables.UHexString[value & 0xF]);
                }
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// 从16进制编码转换
    /// </summary>
    public static string FromBase16(string input)
    {
        return Encoding.GetString(Convert.FromHexString(input));
    }

    /// <summary>
    /// 解码最基本形式的QuotedPrintable
    /// </summary>
    public static string FromQuotedPrintable(string input)
    {
        IEnumerable<Match> matches = QuotedPrintableRegex().Matches(input);
        var result = new StringBuilder();
        foreach (var match in matches)
        {
            var group = match.Groups["Hex"];
            if (group.Success)
            {
                int length = group.Value.Length / 3;
                var bytes = new byte[length];
                for (int i = 0; i < group.Value.Length; i += 3)
                {
                    bytes[i / 3] = (byte)(group.Value[i + 1].Base36Number() << 4);
                    bytes[i / 3] += (byte)group.Value[i + 2].Base36Number();
                }
                result.Append(Encoding.GetString(bytes));
                continue;
            }
            group = match.Groups["Other"];
            if (group.Success)
                result.Append(group.Value);
        }
        return result.ToString();
    }

    /// <summary>
    /// 转换为Emoji表情符号
    /// </summary>
    public static string ToBase100(string input)
    {
        return Base100Encoding.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Emoji表情符号转换
    /// </summary>
    public static string FromBase100(string input)
    {
        return Encoding.GetString(Base100Encoding.Decode(input));
    }

    /// <summary>
    /// 转换为Base85
    /// </summary>
    public static string ToBase85(string input)
    {
        return Base85Encoding.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base85转换
    /// </summary>
    public static string FromBase85(string input)
    {
        return Encoding.GetString(Base85Encoding.Decode(input));
    }

    /// <summary>
    /// 使用指定表(如<see cref="Utils.GlobalTables.Base36"/>等)编码
    /// </summary>
    public static string ToBase(string input, string table)
    {
        var bytes = Encoding.GetBytes(input);
        var number = new BigInteger(bytes, true, true);

        var stack = new Stack<char>();
        while (!number.IsZero)
        {
            stack.Push(table[(int)(number % table.Length)]);
            number /= table.Length;
        }
        return new(stack.ToArray());
    }

    /// <summary>
    /// 使用指定表(如<see cref="Utils.GlobalTables.Base36"/>等)编码
    /// </summary>
    public static string FromBase(string input, string table)
    {
        var number = BigInteger.Zero;
        int baseNumber = table.Length;
        for (int i = 0; i < input.Length; i++)
        {
            var poweredNumber = BigInteger.Pow(baseNumber, input.Length - i - 1);
            number += table.IndexOf(input[i]) * poweredNumber;
        }
        var bytes = number.ToByteArray(true, true);
        return Encoding.GetString(bytes);
    }

    [GeneratedRegex("(?<Hex>(=[0-9a-fA-F]{2})+)|(?<Other>[^=]+)")]
    private static partial Regex QuotedPrintableRegex();

}
