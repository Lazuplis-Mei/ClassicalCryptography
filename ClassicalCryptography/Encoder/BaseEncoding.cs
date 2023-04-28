using ClassicalCryptography.Encoder.BaseEncodings;
using CommunityToolkit.HighPerformance.Buffers;

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
        return K4os.Text.BaseX.Base64.ToBase64(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base64编码转换
    /// </summary>
    public static string FromBase64(string input)
    {
        return Encoding.GetString(K4os.Text.BaseX.Base64.FromBase64(input));
    }

    /// <summary>
    /// 转换为Base64URL编码
    /// </summary>
    /// <remarks>
    /// 适用于URL和文件名的Base64字符表:<see href="https://www.rfc-editor.org/rfc/rfc4648.html#page-7">rfc4648/page-7</see>
    /// </remarks>
    public static string ToBase64URL(string input)
    {
        return K4os.Text.BaseX.Base64.Url.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base64URL编码转换
    /// </summary>
    /// <remarks>
    /// 适用于URL和文件名的Base64字符表:<see href="https://www.rfc-editor.org/rfc/rfc4648.html#page-7">rfc4648/page-7</see>
    /// </remarks>
    public static string FromBase64URL(string input)
    {
        return Encoding.GetString(K4os.Text.BaseX.Base64.Url.Decode(input));
    }

    /// <summary>
    /// 转换为Base32编码(RFC4648)
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="Base32Encoding"/><br/>
    /// 更多变体请参考<seealso cref="SimpleBase.Base32"/>
    /// </remarks>
    public static string ToBase32(string input)
    {
        return Base32Encoding.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base32编码转换(RFC4648)
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="Base32Encoding"/><br/>
    /// 更多变体请参考<see cref="SimpleBase.Base32"/>
    /// </remarks>
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
    /// 转换为16进制编码(大写)
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="Convert.ToHexString(byte[])"/><br/>
    /// 更多变体请参考<seealso cref="SimpleBase.Base16"/><br/>
    /// 硬件加速请使用<see cref="K4os.Text.BaseX.Base16"/>
    /// </remarks>
    public static string ToBase16(string input)
    {
        return Convert.ToHexString(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从16进制编码转换(大写)
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="Convert.FromHexString(string)"/><br/>
    /// 更多变体请参考<seealso cref="SimpleBase.Base16"/><br/>
    /// 硬件加速请使用<see cref="K4os.Text.BaseX.Base16"/>
    /// </remarks>
    public static string FromBase16(string input)
    {
        return Encoding.GetString(Convert.FromHexString(input));
    }

    /// <summary>
    /// 编码最基本形式的QuotedPrintable
    /// </summary>
    [SkipLocalsInit]
    public static string ToQuotedPrintable(string input)
    {
        var result = new StringBuilder(input.Length + 16);
        Span<char> oneCharacter = stackalloc char[1];
        Span<byte> span = stackalloc byte[4];
        foreach (var character in input)
        {
            if (char.IsAscii(character))
            {
                if (character is not '=' and not < '!' and not > '~')
                {
                    result.Append(character);
                    continue;
                }
                ushort value = character;
                result.Append('=');
                result.Append(HexUpper[value >> 4]);
                result.Append(HexUpper[value & 0xF]);
                continue;
            }
            oneCharacter[0] = character;
            var count = Encoding.GetBytes(oneCharacter, span);
            foreach (var value in span[..count])
            {
                result.Append('=');
                result.Append(HexUpper[value >> 4]);
                result.Append(HexUpper[value & 0xF]);
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// 解码最基本形式的QuotedPrintable
    /// </summary>
    public static string FromQuotedPrintable(string input)
    {
        IEnumerable<Match> matches = QuotedPrintableRegex().Matches(input);
        var result = new StringBuilder(input.Length / 2);
        foreach (var match in matches)
        {
            Group group;
            if ((group = match.Groups["Hex"]).Success)
            {
                int length = group.Value.Length / 3;
                using var memory = MemoryOwner<byte>.Allocate(length);
                var bytes = memory.Span;
                for (int i = 0; i < group.Value.Length; i++)
                {
                    int high = group.Value[++i].Base36Number();
                    int low = group.Value[++i].Base36Number();
                    bytes[i / 3] = MathEx.MakeByte((byte)high, (byte)low);
                }
                result.Append(Encoding.GetString(bytes));
                continue;
            }
            result.Append(match.Value);
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
    /// 转换为Base85(Ascii85)
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="K4os.Text.BaseX.Base85"/><br/>
    /// 更多变体请参考<seealso cref="SimpleBase.Base85"/>和<seealso cref="TuupolaBase85Encoding"/>
    /// </remarks>
    public static string ToBase85(string input)
    {
        return K4os.Text.BaseX.Base85.ToBase85(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base85转换(Ascii85)
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="K4os.Text.BaseX.Base85"/><br/>
    /// 更多变体请参考<seealso cref="SimpleBase.Base85"/>和<seealso cref="TuupolaBase85Encoding"/>
    /// </remarks>
    public static string FromBase85(string input)
    {
        return Encoding.GetString(K4os.Text.BaseX.Base85.FromBase85(input));
    }

    /// <summary>
    /// 转换为Base58
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="SimpleBase.Base58.Bitcoin"/><br/>
    /// 更多变体请参考<seealso cref="SimpleBase.Base58"/>
    /// </remarks>
    public static string ToBase58(string input)
    {
        return SimpleBase.Base58.Bitcoin.Encode(Encoding.GetBytes(input));
    }

    /// <summary>
    /// 从Base58转换
    /// </summary>
    /// <remarks>
    /// 默认使用<see cref="SimpleBase.Base58.Bitcoin"/><br/>
    /// 更多变体请参考<seealso cref="SimpleBase.Base58"/>
    /// </remarks>
    public static string FromBase58(string input)
    {
        return Encoding.GetString(SimpleBase.Base58.Bitcoin.Decode(input));
    }

    /// <summary>
    /// 使用指定表(如<see cref="Base36"/>等)编码
    /// </summary>
    public static string ToBase(string input, string table)
    {
        var bytes = Encoding.GetBytes(input);
        var number = new BigInteger(bytes, true, true);
        return NumberToBase(number, table);
    }

    /// <summary>
    /// 使用指定表编码整数
    /// </summary>
    public static string NumberToBase(BigInteger number, string table)
    {
        var stack = new Stack<char>();
        while (!number.IsZero)
        {
            stack.Push(table[(int)(number % table.Length)]);
            number /= table.Length;
        }
        return new(stack.ToArray());
    }

    /// <summary>
    /// 使用指定表(如<see cref="Base36"/>等)编码
    /// </summary>
    public static string FromBase(string input, string table)
    {
        BigInteger number = NumberFromBase(input, table);
        var bytes = number.ToByteArray(true, true);
        return Encoding.GetString(bytes);
    }

    /// <summary>
    /// 使用指定表解码整数
    /// </summary>
    public static BigInteger NumberFromBase(string input, string table)
    {
        var number = BigInteger.Zero;
        int baseNumber = table.Length;
        for (int i = 0; i < input.Length; i++)
        {
            var poweredNumber = BigInteger.Pow(baseNumber, input.Length - i - 1);
            number += table.IndexOf(input[i]) * poweredNumber;
        }
        return number;
    }

    [GeneratedRegex("(?<Hex>(=[0-9a-fA-F]{2})+)|[^=]+")]
    private static partial Regex QuotedPrintableRegex();
}
