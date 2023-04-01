using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
/// </remarks>
[ReferenceFrom("https://github.com/tuupola/base85", ProgramingLanguage.PHP, License.MIT)]
public class TuupolaBase85Encoding
{
    /// <summary>
    /// 默认的Base85编码
    /// </summary>
    public static readonly TuupolaBase85Encoding Default = new(GlobalTables.Ascii85, false, true);

    /// <summary>
    /// Adobe ASCII85
    /// </summary>
    public static readonly TuupolaBase85Encoding Ascii85 = new(GlobalTables.Ascii85, false, true, "<~", "~>");

    /// <summary>
    /// <see href="https://rfc.zeromq.org/spec/32/">ZeroMQ (Z85)</see>
    /// </summary>
    public static readonly TuupolaBase85Encoding ZeroMQ = new(GlobalTables.Ascii85_Z85, false, false);

    /// <summary>
    /// <see href="https://tools.ietf.org/html/rfc1924">RFC1924</see>
    /// </summary>
    public static readonly TuupolaBase85Encoding RFC1924 = new(GlobalTables.Ascii85_IPv6, false, false);

    /// <summary>
    /// compress.spaces
    /// </summary>
    public readonly bool CompressSpaces;

    /// <summary>
    /// compress.zeroes
    /// </summary>
    public readonly bool CompressZeroes;

    /// <summary>
    /// prefix
    /// </summary>
    public readonly string? Prefix;

    /// <summary>
    /// suffix
    /// </summary>
    public readonly string? Suffix;

    /// <summary>
    /// characters
    /// </summary>
    public string Characters;

    private const int SPACES = 0x20202020;
    private static readonly uint[] powers = { 52200625, 614125, 7225, 85, 1 };
    private readonly Dictionary<char, int> inverseMap = new();

    /// <summary>
    /// Base85编码
    /// </summary>
    public TuupolaBase85Encoding(string characters, bool compressSpaces, bool compressZeroes, string? prefix = null, string? suffix = null)
    {
        Guard.IsEqualTo(characters.Length, 85);
        Guard.IsEqualTo(characters.ToHashSet().Count, 85);
        Characters = characters;
        for (int i = 0; i < characters.Length; i++)
            inverseMap.Add(characters[i], i);
        CompressSpaces = compressSpaces;
        CompressZeroes = compressZeroes;
        Prefix = prefix;
        Suffix = suffix;
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string Encode(byte[] bytes)
    {
        int length = bytes.Length.DivCeil(4);
        Span<uint> span = length.CanAllocInt32() ? stackalloc uint[length] : new uint[length];
        Span<byte> byteSpan = MemoryMarshal.AsBytes(span);

        span[^1] = 0;
        for (int i = 0; i < bytes.Length; i += 4)
        {
            byteSpan[i + 3] = bytes[i];
            if (i + 1 < bytes.Length)
                byteSpan[i + 2] = bytes[i + 1];
            if (i + 2 < bytes.Length)
                byteSpan[i + 1] = bytes[i + 2];
            if (i + 3 < bytes.Length)
                byteSpan[i] = bytes[i + 3];
        }

        var result = new StringBuilder();
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
                uint reminder = quotient % pow;
                quotient /= pow;
                result.Append(Characters[(int)quotient]);
                quotient = reminder;
            }
        }

        if (result[^1] == 'z')
            result.RemoveLast().Append("!!!!!");

        int padding = 0;
        int modulus = bytes.Length % 4;
        if (modulus != 0)
            padding = 4 - modulus;

        result.Remove(result.Length - padding, padding);
        return result.Append(Suffix).ToString();
    }

    /// <inheritdoc/>
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

        Guard.IsTrue(data.ToHashSet().IsSubsetOf(Characters));

        int padding = 0;
        int modulus = data.Length % 5;
        if (modulus != 0)
        {
            padding = 5 - modulus;
            data += Characters[^1].Repeat(padding);
        }

        var result = new List<byte>();
        foreach (string value in data.Partition(5))
        {
            AddConvert(result, value);
        }

        result.RemoveRange(result.Count - padding, padding);
        return result.ToArray();

        [SkipLocalsInit]
        void AddConvert(List<byte> result, string value)
        {
            int accumulator = 0;
            foreach (var character in value)
            {
                accumulator += accumulator << 2;
                accumulator += accumulator << 4;
                accumulator += inverseMap[character];
            }
            Span<byte> bytes = stackalloc byte[4];
            BitConverter.TryWriteBytes(bytes, accumulator);
            result.Add(bytes[3]);
            result.Add(bytes[2]);
            result.Add(bytes[1]);
            result.Add(bytes[0]);
        }
    }
}
