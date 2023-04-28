namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <see href="https://github.com/qntm/base32768">Base32768编码</see>
/// </summary>
[Introduction("Base32768编码", "https://github.com/qntm/base32768")]
[ReferenceFrom("https://github.com/qntm/base32768/blob/main/src/index.js", ProgramingLanguage.JavaScript, License.MIT)]
public class Base32768Encoding : IEncoding
{
    private static readonly BaseXXXXEncoding base32768 = new(15, new[] { Resources.Base32768PairString, "ƀƟɀʟ" });
    
    /// <inheritdoc/>
    public static string Encode(byte[] bytes) => base32768.Encode(bytes);

    /// <inheritdoc/>
    public static byte[] Decode(string encodeText) => base32768.Decode(encodeText);
}
