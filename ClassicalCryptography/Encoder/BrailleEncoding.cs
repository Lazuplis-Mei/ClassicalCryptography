using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 盲文编码，用盲文符号编码字节
/// </summary>
[Introduction("盲文编码", "用盲文符号编码字节")]
[ReferenceFrom("https://github.com/qntm/braille-encode", ProgramingLanguage.JavaScript, License.MIT)]
public class BrailleEncoding : IEncoding
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// <a href="http://www.unicode.org/charts/PDF/U2800.pdf">U2800</a>
    /// </summary>
    public const int FIRST_BRAILLE = '\u2800';

    private static readonly string characters = Properties.Resources.BrailleEncodingString;

    private static Dictionary<char, byte>? decodeMap;

    [MemberNotNull(nameof(decodeMap))]
    private static void BuildMap()
    {
        decodeMap = new();
        for (int i = 0; i < characters.Length; i++)
        {
            decodeMap.Add(characters[i], (byte)i);
        }
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static string Encode(byte[] bytes)
    {
        Span<char> span = bytes.Length.CanAllocString()
            ? stackalloc char[bytes.Length] : new char[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
            span[i] = characters[bytes[i]];
        return new(span);
    }

    /// <summary>
    /// 用Unicode顺序的盲文符号编码字节
    /// </summary>
    [SkipLocalsInit]
    public static string EncodeWithUnicodeOrder(byte[] bytes)
    {
        Span<char> span = bytes.Length.CanAllocString()
            ? stackalloc char[bytes.Length] : new char[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
            span[i] = (char)(FIRST_BRAILLE + bytes[i]);
        return new(span);
    }

    /// <summary>
    /// 用盲文符号编码字符串
    /// </summary>
    public static string EncodeString(string text)
    {
        return Encode(Encoding.GetBytes(text));
    }

    /// <inheritdoc/>
    public static byte[] Decode(string brailles)
    {
        if (decodeMap is null)
            BuildMap();

        var bytes = new byte[brailles.Length];
        for (int i = 0; i < brailles.Length; i++)
            bytes[i] = decodeMap[brailles[i]];
        return bytes;
    }

    /// <summary>
    /// 用Unicode顺序解码盲文符号
    /// </summary>
    public static byte[] DecodeWithUnicodeOrder(string brailles)
    {
        var bytes = new byte[brailles.Length];
        for (int i = 0; i < brailles.Length; i++)
            bytes[i] = (byte)(brailles[i] - FIRST_BRAILLE);
        return bytes;
    }

    /// <summary>
    /// 解码盲文符号字符串
    /// </summary>
    public static string DecodeString(string text)
    {
        return Encoding.GetString(Decode(text));
    }
}