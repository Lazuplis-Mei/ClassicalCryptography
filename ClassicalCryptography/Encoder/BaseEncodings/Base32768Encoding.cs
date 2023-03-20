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
        Properties.Resources.Base32768PairString,
        "ƀƟɀʟ"
    };

    private static readonly BaseXXXXEncoding base32768 = new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);


    /// <inheritdoc/>
    public static string Encode(byte[] bytes) => base32768.Encode(bytes);


    /// <inheritdoc/>
    public static byte[] Decode(string encodeText) => base32768.Decode(encodeText);

}
