using ClassicalCryptography.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// <para>盲文编码，用盲文符号编码字节</para>
/// <para>参考代码</para>
/// <see href="https://github.com/qntm/braille-encode"/>
/// </summary>
[Introduction("盲文编码", "用盲文符号编码字节")]
[TranslatedFrom("JavaScript")]
public class BrailleEncoding
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// <see href="http://www.unicode.org/charts/PDF/U2800.pdf"/>
    /// </summary>
    public const int FIRST_BRAILLE = '\u2800';

    private static readonly string characters = Properties.Resources.BrailleEncodingString;

    private static readonly Dictionary<char, byte> decodeMap = new();
    static BrailleEncoding()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            decodeMap.Add(characters[i], (byte)i);
        }
    }

    /// <summary>
    /// 用盲文符号编码字节
    /// </summary>
    [SkipLocalsInit]
    public static string EncodeBytes(byte[] bytes)
    {
        Span<char> span = bytes.Length <= StackLimit.MaxCharSize
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
        Span<char> span = bytes.Length <= StackLimit.MaxCharSize
            ? stackalloc char[bytes.Length] : new char[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
            span[i] = (char)(FIRST_BRAILLE + bytes[i]);
        return new(span);
    }

    /// <summary>
    /// 用盲文符号编码字符串
    /// </summary>
    public static string Encode(string text)
    {
        return EncodeBytes(Encoding.GetBytes(text));
    }

    /// <summary>
    /// 解码盲文符号
    /// </summary>
    [SkipLocalsInit]
    public static byte[] DecodeBytes(string brailles)
    {
        var bytes = new byte[brailles.Length];
        for (int i = 0; i < brailles.Length; i++)
            bytes[i] = decodeMap[brailles[i]];
        return bytes;
    }

    /// <summary>
    /// 用Unicode顺序解码盲文符号
    /// </summary>
    [SkipLocalsInit]
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
    public static string Decode(string text)
    {
        return Encoding.GetString(DecodeBytes(text));
    }
}