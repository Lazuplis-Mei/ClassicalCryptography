namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <see href="https://github.com/qntm/base2048">Base2048编码</see>
/// </summary>
[Introduction("Base2048编码", "https://github.com/qntm/base2048")]
[ReferenceFrom("https://github.com/qntm/base2048/blob/main/src/index.js", ProgramingLanguage.JavaScript, License.MIT)]
public class Base2048Encoding : IEncoding
{
    private static readonly BaseXXXXEncoding base2048 = new(11, new[] { Resources.Base2048PairString, "07" });

    /// <inheritdoc/>
    public static string Encode(byte[] bytes) => base2048.Encode(bytes);

    /// <inheritdoc/>
    public static byte[] Decode(string encodeText) => base2048.Decode(encodeText);
}
