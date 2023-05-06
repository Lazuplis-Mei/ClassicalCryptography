using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// Base85编码
/// </summary>
/// <remarks>
/// <list type="bullet">
///     <item>
///         <term>参考资料</term>
///         <description>
///             <see href="https://en.wikipedia.org/wiki/Ascii85">wikipedia/Ascii85</see>
///         </description>
///     </item>
///     <item>
///         <term>在线工具</term>
///         <description>
///             <see href="http://www.hiencode.com/base85.html">hiencode/base85</see>
///         </description>
///     </item>
/// </list>
/// 也可以使用<seealso cref="SimpleBase.Base85"/>代替
/// </remarks>
[ReferenceFrom("https://github.com/tuupola/base85", ProgramingLanguage.PHP, License.MIT)]
public class TuupolaBase85Encoding
{
    #region Base85

    /// <summary>
    /// 默认的Base85编码
    /// </summary>
    public static readonly TuupolaBase85Encoding Default = new(Ascii85, false, true);

    /// <summary>
    /// Adobe ASCII85
    /// </summary>
    public static readonly TuupolaBase85Encoding AdobeAscii85 = new(Ascii85, false, true, "<~", "~>");

    /// <summary>
    /// <see href="https://rfc.zeromq.org/spec/32/">ZeroMQ (Z85)</see>
    /// </summary>
    public static readonly TuupolaBase85Encoding ZeroMQ = new(Ascii85_Z85, false, false);

    /// <summary>
    /// <see href="https://tools.ietf.org/html/rfc1924">RFC1924</see>
    /// </summary>
    public static readonly TuupolaBase85Encoding RFC1924 = new(Ascii85_IPv6, false, false);

    #endregion

    /// <summary>
    /// 压缩连续的空格(0x20)
    /// </summary>
    public readonly bool CompressSpaces;

    /// <summary>
    /// 压缩连续的0
    /// </summary>
    public readonly bool CompressZeroes;

    /// <summary>
    /// 前缀
    /// </summary>
    public readonly string? Prefix;

    /// <summary>
    /// 后缀
    /// </summary>
    public readonly string? Suffix;

    /// <summary>
    /// 字符列表
    /// </summary>
    public string Characters;

    private const int SPACES = 0x20202020;
    private static readonly uint[] powers = { 85 * 85 * 85 * 85, 85 * 85 * 85, 85 * 85, 85, 1 };
    private readonly Dictionary<char, uint> inverseMap = new();

    /// <summary>
    /// Base85编码
    /// </summary>
    public TuupolaBase85Encoding(string characters, bool compressSpaces, bool compressZeroes, string? prefix = null, string? suffix = null)
    {
        Guard.IsEqualTo(characters.Length, 85);
        Guard.IsEqualTo(characters.ToHashSet().Count, 85);
        Characters = characters;
        for (int i = 0; i < characters.Length; i++)
            inverseMap.Add(characters[i], (uint)i);
        CompressSpaces = compressSpaces;
        CompressZeroes = compressZeroes;
        Prefix = prefix;
        Suffix = suffix;
    }

    /// <inheritdoc cref="IEncoding.Encode"/>
    [SkipLocalsInit]
    public string Encode(byte[] bytes)
    {
        int length = bytes.Length.DivCeil(4);
        using var memory = length.CanAllocInt32() ? SpanOwner<uint>.Empty : SpanOwner<uint>.Allocate(length);
        Span<uint> span = length.CanAllocInt32() ? stackalloc uint[length] : memory.Span;
        Span<byte> byteSpan = bytes.AsSpan();

        for (int i = 0; i < length - 1; i++)
        {
            span[i] = BinaryPrimitives.ReadUInt32BigEndian(byteSpan);
            byteSpan = byteSpan[4..];
        }
        SpanExtension.WriteBigEndian(ref span[^1], byteSpan);

        length = (span.Length + 1) * 5;
        if (Prefix is not null)
            length += Prefix.Length;
        if (Suffix is not null)
            length += Suffix.Length;
        var result = new StringBuilder(length);

        result.Append(Prefix);
        foreach (var value in span)
        {
            if (CompressSpaces && value is SPACES)
            {
                result.Append('y');
                continue;
            }
            if (CompressZeroes && value is 0)
            {
                result.Append('z');
                continue;
            }

            uint quotient = value;
            foreach (var pow in powers)
            {
                (quotient, uint reminder) = uint.DivRem(quotient, pow);
                result.Append(Characters[(int)quotient]);
                quotient = reminder;
            }
        }

        if (result[^1] == 'z')
            result.RemoveLast().Append("!!!!!");

        result.RemoveLast(byteSpan.Length.DivPadding(4));
        return result.Append(Suffix).ToString();
    }

    /// <inheritdoc cref="IEncoding.Decode"/>
    public byte[] Decode(string data)
    {
        if (Prefix is not null && data.StartsWith(Prefix))
            data = data[Prefix.Length..];
        if (Suffix is not null && data.EndsWith(Suffix))
            data = data[..^Suffix.Length];
        if (CompressZeroes)
            data = data.Replace("z", "!!!!!");
        if (CompressSpaces)
            data = data.Replace("y", "+<VdL");
        IsVaild(data);

        int padding = data.Length.DivPadding(5);
        data += Characters[^1].Repeat(padding);
        var bytes = new byte[data.Length / 5 * 4 - padding];
        data.ForEachPartition(5, AddConvert);
        return bytes;

        void AddConvert(ReadOnlySpan<char> span, int index)
        {
            uint value = 0;
            foreach (var character in span)
            {
                value += value << 2;
                value += value << 4;
                value += inverseMap[character];
            }
            bytes.AsSpan(4 * index).WriteBigEndian(value);
        }
    }

    [Conditional("DEBUG")]
    private void IsVaild(string data) => GuardEx.IsSubsetOf(data.ToHashSet(), Characters);
}
