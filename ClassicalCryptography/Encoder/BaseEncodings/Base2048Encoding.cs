namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <a href="https://github.com/qntm/base2048">Base2048编码</a>
/// </summary>
[Introduction("Base2048编码", "https://github.com/qntm/base2048")]
[ReferenceFrom("https://github.com/qntm/base2048/blob/main/src/index.js", ProgramingLanguage.JavaScript, License.MIT)]
public class Base2048Encoding : IEncoding
{
    const int BITS_PER_CHAR = 11;
    const int BITS_PER_BYTE = 8;

    private static readonly string[] pairStrings =
    {
        Properties.Resources.Base2048PairString,
        "07"
    };

    private static readonly BaseXXXXEncoding base2048 = new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);

    /// <inheritdoc/>
    public static string Encode(byte[] bytes) => base2048.Encode(bytes);

    /// <inheritdoc/>
    public static byte[] Decode(string encodeText) => base2048.Decode(encodeText);
}