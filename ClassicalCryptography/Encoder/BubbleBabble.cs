using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// <a href="http://bohwaz.net/archives/web/Bubble_Babble.html">Bubble Babble</a>
/// </summary>
[ReferenceFrom("https://github.com/bohwaz/bubblebabble/blob/master/bubble_babble.php", ProgramingLanguage.PHP)]
public partial class BubbleBabble : IEncoding
{
    static readonly string vowels = "aeiouy";

    static readonly string consonants = "bcdfghklmnprstvzx";

    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 编码BubbleBabble
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EncodeString(string text)
    {
        return Encode(Encoding.GetBytes(text));
    }

    /// <inheritdoc/>
    public static string Encode(byte[] bytes)
    {
        var result = new StringBuilder();
        result.Append('x');
        int checksum = 1;
        for (int i = 0; ; i++)
        {
            if (i >= bytes.Length)
            {
                result.Append(vowels[checksum % 6]).Append(consonants[16]).Append(vowels[checksum / 6]);
                break;
            }

            int byte1 = bytes[i];
            result.Append(vowels[((byte1 >> 6 & 3) + checksum) % 6]);
            result.Append(consonants[byte1 >> 2 & 15]);
            result.Append(vowels[((byte1 & 3) + checksum / 6) % 6]);

            if (++i >= bytes.Length)
                break;

            int byte2 = bytes[i];
            result.Append(consonants[byte2 >> 4 & 15]).Append('-');
            result.Append(consonants[byte2 & 15]);

            checksum = (checksum * 5 + byte1 * 7 + byte2) % 36;
        }

        result.Append('x');
        return result.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static byte Decode2WayByte(int a1, int a2, int offset)
    {
        if (a1 > 16)
            throw new ArgumentException($"字符串在 {offset} 处错误", nameof(offset));
        if (a2 > 16)
            throw new ArgumentException($"字符串在 {offset + 2} 处错误", nameof(offset));
        return (byte)(a1 << 4 | a2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static byte Decode3WayByte(int a1, int a2, int a3, int offset, int c)
    {
        var high2 = (a1 - c % 6 + 6) % 6;
        if (high2 >= 4)
            throw new ArgumentException($"字符串在 {offset} 处错误", nameof(offset));
        if (a2 > 16)
            throw new ArgumentException($"字符串在 {offset + 1} 处错误", nameof(offset));
        var mid4 = a2;
        var low2 = (a3 - c / 6 % 6 + 6) % 6;
        if (low2 >= 4)
            throw new ArgumentException($"字符串在 {offset + 2} 处错误", nameof(offset));
        return (byte)(high2 << 6 | mid4 << 2 | low2);
    }

    [SkipLocalsInit]
    static int[] DecodeTuple(string src, int pos)
    {
        Span<int> tuple = stackalloc int[6];
        tuple[0] = vowels.IndexOf(src[0]);
        tuple[1] = consonants.IndexOf(src[1]);
        tuple[2] = vowels.IndexOf(src[2]);

        if (src.Length > 3)
        {
            tuple[3] = consonants.IndexOf(src[3]);
            tuple[4] = '-';
            tuple[5] = consonants.IndexOf(src[5]);
            return tuple.ToArray();
        }
        return tuple[..3].ToArray();
    }

    /// <inheritdoc/>
    public static byte[] Decode(string text)
    {
        int checksum = 1;

        if (text[0] != 'x')
            throw new ArgumentException("字符串必须以'x'开头", nameof(text));
        if (text[^1] != 'x')
            throw new ArgumentException("字符串必须以'x'结尾", nameof(text));
        if (text.Length != 5 && text.Length % 6 != 5)
            throw new ArgumentException("字符串长度不正确", nameof(text));

        string[] texts = text[1..^1].Partition(6);
        var lastTuple = texts.Length - 1;
        var byteList = new List<byte>();

        for (int k = 0; k < texts.Length; k++)
        {
            var pos = k * 6;
            var tuple = DecodeTuple(texts[k], pos);

            if (k == lastTuple)
            {
                if (tuple[1] == 16)
                {
                    if (tuple[0] != checksum % 6)
                        throw new Exception($"Corrupt string at offset {pos} (checksum)");
                    if (tuple[2] != checksum / 6)
                        throw new Exception($"Corrupt string at offset {pos + 2} (checksum)");
                }
                else
                {
                    byteList.Add(Decode3WayByte(tuple[0], tuple[1], tuple[2], pos, checksum));
                }
            }
            else
            {
                var byte1 = Decode3WayByte(tuple[0], tuple[1], tuple[2], pos, checksum);
                var byte2 = Decode2WayByte(tuple[3], tuple[5], pos);
                byteList.Add(byte1);
                byteList.Add(byte2);
                checksum = (checksum * 5 + byte1 * 7 + byte2) % 36;
            }
        }
        return byteList.ToArray();
    }

    /// <summary>
    /// 解码BubbleBabble
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DecodeString(string text)
    {
        return Encoding.GetString(Decode(text));
    }

    /// <summary>
    /// 检查它是否可能是BubbleBabble字符串
    /// </summary>
    public static bool CheckString(string text)
    {
        if (text[0] != 'x' || text[^1] != 'x')
            return false;

        if (text.Length != 5 && text.Length % 6 != 5)
            return false;

        return BubbleBabbleRegex().IsMatch(text);
    }

    [GeneratedRegex("^[bcdfghklmnprstvzxaeiouy]{5}(-[bcdfghklmnprstvzxaeiouy]{5})*$")]
    private static partial Regex BubbleBabbleRegex();
}
