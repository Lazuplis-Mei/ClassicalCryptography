using ClassicalCryptography.Interfaces;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <para>Base32768编码</para>
/// <see href="https://github.com/qntm/base32768"/>
/// </summary>
[Introduction("Base32768编码", "https://github.com/qntm/base32768")]
[TranslatedFrom("JavaScript")]
public static class Base32768Encoding
{
    private const int BITS_PER_CHAR = 15;
    private const int BITS_PER_BYTE = 8;
    private static readonly string[] pairStrings =
    {
        Properties.Resources.Base32768PairString,
        "ƀƟɀʟ"
    };

    private static readonly BaseXXXXEncoding base32768 = new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);


    /// <summary>
    /// encode Base32768
    /// </summary>
    public static string Encode(byte[] uint8Array) => base32768.Encode(uint8Array);


    /// <summary>
    /// decode Base32768
    /// </summary>
    public static byte[] Decode(string str) => base32768.Decode(str);

}
