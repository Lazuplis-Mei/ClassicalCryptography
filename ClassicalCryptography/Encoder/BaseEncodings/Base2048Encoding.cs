using ClassicalCryptography.Interfaces;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <para>Base2048编码</para>
/// <see href="https://github.com/qntm/base2048"/>
/// </summary>
[Introduction("Base2048编码", "https://github.com/qntm/base2048")]
[TranslatedFrom("JavaScript")]
public static class Base2048Encoding
{
    const int BITS_PER_CHAR = 11;
    const int BITS_PER_BYTE = 8;

    private static readonly string[] pairStrings =
    {
        Properties.Resources.Base2048PairString,
        "07"
    };

    private static readonly BaseXXXXEncoding base2048 = new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);

    /// <summary>
    /// encode Base2048
    /// </summary>
    public static string Encode(byte[] uint8Array) => base2048.Encode(uint8Array);

    /// <summary>
    /// decode Base2048
    /// </summary>
    public static byte[] Decode(string str) => base2048.Decode(str);
}