using CommunityToolkit.HighPerformance.Buffers;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// BubbleBabble编码
/// </summary>
/// <remarks>
/// <list type="bullet">
///     <item>
///         <term>参考代码</term>
///         <description>
///             <see href="https://github.com/bohwaz/bubblebabble/blob/master/bubble_babble.php">github/bohwaz/bubblebabble</see>
///         </description>
///     </item>
///     <item>
///         <term>参考资料</term>
///         <description>
///             <see href="http://bohwaz.net/archives/web/Bubble_Babble.html">bohwaz/Bubble_Babble</see>
///         </description>
///     </item>
///     <item>
///         <term>参考资料</term>
///         <description>
///             <see href="http://wiki.yak.net/589/Bubble_Babble_Encoding.txt">wiki.yak.net/Bubble_Babble_Encoding</see>
///         </description>
///     </item>
///     <item>
///         <term>在线工具</term>
///         <description>
///             <see href="http://www.hiencode.com/bubble.html">hiencode/bubble</see>
///         </description>
///     </item>
/// </list>
/// </remarks>
[ReferenceFrom("https://github.com/bohwaz/bubblebabble/blob/master/bubble_babble.php", ProgramingLanguage.PHP)]
public partial class BubbleBabble : IEncoding
{
    private static readonly string vowels = "aeiouy";//аиоуыэ
    private static readonly string consonants = "bcdfghklmnprstvzx";//бгджзклмнпрстфхцч

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
        if (string.IsNullOrEmpty(text))
            return "xexax";
        return Encode(Encoding.GetBytes(text));
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static string Encode(byte[] bytes)
    {
        int count = bytes.Length / 2 * 6 + 5;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];

        int index = 0;
        span[index++] = 'x';
        int checksum = 1;
        for (int i = 0; ; i++)
        {
            if (i == bytes.Length)
            {
                (int quotient, int remainder) = int.DivRem(checksum, 6);
                span[index++] = vowels[remainder];
                span[index++] = 'x';
                span[index++] = vowels[quotient];
                break;
            }

            int byte1 = bytes[i];
            span[index++] = vowels[((byte1 >> 6) + checksum) % 6];
            span[index++] = consonants[(byte1 >> 2) & 0B1111];
            span[index++] = vowels[((byte1 & 0B11) + (checksum / 6)) % 6];

            if (++i == bytes.Length)
                break;

            int byte2 = bytes[i];
            span[index++] = consonants[byte2 >> 4];
            span[index++] = '-';
            span[index++] = consonants[byte2 & 0B1111];

            checksum = (checksum * 5 + byte1 * 8 - byte1 + byte2) % 36;
        }
        span[index++] = 'x';
        return new(span);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// 注意：虽然校验和会在异常中指定可能出现错误的位置<br/>
    /// 但位置不具备真正准确的参考意义
    /// </remarks>
    public static byte[] Decode(string text)
    {
        Guard.IsTrue(CheckString(text));

        string[] pairs = text[1..^1].PartitionUpTo(6);
        using var memory = MemoryOwner<byte>.Allocate(pairs.Length * 2 - 1);
        var span = memory.Span;
        int index = 0;

        int checksum = 1;
        for (int i = 0; i < pairs.Length; i++)
        {
            int position = i * 6;
            var (T0, T1, T2, T3, T5) = DecodeTuple(pairs[i]);
            if (i < pairs.Length - 1)
            {
                var byte1 = MakeByte(T0, T1, T2, position, checksum);
                var byte2 = MathEx.MakeByte(T3, T5);
                span[index++] = byte1;
                span[index++] = byte2;
                checksum = (checksum * 5 + byte1 * 8 - byte1 + byte2) % 36;
                continue;
            }
            if (T1 == 16)
            {
                if (T0 != checksum % 6)
                    throw new ArgumentException($"字符串在`{position}`处错误", nameof(text));
                if (T2 != checksum / 6)
                    throw new ArgumentException($"字符串在`{position + 2}`处错误", nameof(text));
                continue;
            }
            span[index++] = MakeByte(T0, T1, T2, position, checksum);
        }
        return span[..index].ToArray();
    }

    /// <summary>
    /// 解码BubbleBabble
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DecodeString(string text)
    {
        if (text == "xexax")
            return string.Empty;
        return Encoding.GetString(Decode(text));
    }

    /// <summary>
    /// 检查它是否可能是BubbleBabble字符串
    /// </summary>
    public static bool CheckString(string text)
    {
        if (text[0] != 'x' || text[^1] != 'x')
            return false;

        if (text.Length % 6 != 5)
            return false;

        return BubbleBabbleRegex().IsMatch(text);
    }

    private static (byte T0, byte T1, byte T2, byte T3, byte T5) DecodeTuple(string pair)
    {
        (byte T0, byte T1, byte T2, byte T3, byte T5) tuple = default;
        tuple.T0 = (byte)vowels.IndexOf(pair[0]);
        Guard.IsNotEqualTo(tuple.T0, (byte)255);
        tuple.T1 = (byte)consonants.IndexOf(pair[1]);
        Guard.IsNotEqualTo(tuple.T1, (byte)255);
        tuple.T2 = (byte)vowels.IndexOf(pair[2]);
        Guard.IsNotEqualTo(tuple.T2, (byte)255);
        if (pair.Length > 3)
        {
            tuple.T3 = (byte)consonants.IndexOf(pair[3]);
            Guard.IsNotEqualTo(tuple.T3, (byte)255);
            tuple.T5 = (byte)consonants.IndexOf(pair[5]);
            Guard.IsNotEqualTo(tuple.T5, (byte)255);
        }
        return tuple;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte MakeByte(byte high, byte mid, byte low, int offset, int checksum)
    {
        (int quotient, int remainder) = int.DivRem(checksum, 6);
        high = (byte)((high - remainder + 6) % 6);
        if (high >= 4)
            throw new Exception($"字符串在`{offset}`处错误");
        low = (byte)((low - quotient % 6 + 6) % 6);
        if (low >= 4)
            throw new Exception($"字符串在`{offset + 2}`处错误");
        return (byte)(high << 6 | mid << 2 | low);
    }

    [GeneratedRegex(@"^([a-z-[jqw]]{5})([+\-_=][a-z-[jqw]]{5})*$")]
    private static partial Regex BubbleBabbleRegex();
}
