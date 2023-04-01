namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <see href="https://github.com/qntm/base2048">Base2048编码</see>
/// </summary>
[Introduction("Base2048编码", "https://github.com/qntm/base2048")]
[ReferenceFrom("https://github.com/qntm/base2048/blob/main/src/index.js", ProgramingLanguage.JavaScript, License.MIT)]
public class Base2048Encoding : IEncoding
{
    private const int BITS_PER_CHAR = 11;
    private const int BITS_PER_BYTE = 8;

    private static readonly string[] pairStrings =
    {
        string.Empty,
        "07"
    };

    private static BaseXXXXEncoding? base2048;

    /// <inheritdoc/>
    public static string Encode(byte[] bytes)
    {
        if (pairStrings[0] == string.Empty)
            pairStrings[0] = Properties.Resources.Base2048PairString;
        base2048 ??= new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);
        return base2048.Encode(bytes);
    }

    /// <inheritdoc/>
    public static byte[] Decode(string encodeText)
    {
        if (pairStrings[0] == string.Empty)
            pairStrings[0] = Properties.Resources.Base2048PairString;
        base2048 ??= new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);
        return base2048.Decode(encodeText);
    }
}
