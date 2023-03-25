namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <a href="https://github.com/qntm/base32768">Base32768编码</a>
/// </summary>
[Introduction("Base32768编码", "https://github.com/qntm/base32768")]
[ReferenceFrom("https://github.com/qntm/base32768/blob/main/src/index.js", ProgramingLanguage.JavaScript, License.MIT)]
public class Base32768Encoding : IEncoding
{
    private const int BITS_PER_CHAR = 15;
    private const int BITS_PER_BYTE = 8;
    private static readonly string[] pairStrings =
    {
        string.Empty,
        "ƀƟɀʟ"
    };

    private static BaseXXXXEncoding? base32768;


    /// <inheritdoc/>
    public static string Encode(byte[] bytes)
    {
        if (pairStrings[0] == string.Empty)
            pairStrings[0] = Properties.Resources.Base32768PairString;
        base32768 ??= new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);
        return base32768.Encode(bytes);
    }


    /// <inheritdoc/>
    public static byte[] Decode(string encodeText)
    {
        if (pairStrings[0] == string.Empty)
            pairStrings[0] = Properties.Resources.Base32768PairString;
        base32768 ??= new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);
        return base32768.Decode(encodeText);
    }
}
