namespace ClassicalCryptography.Transposition;

/// <summary>
/// 反转密码
/// </summary>
[Introduction("反转密码", "明文倒序读出。")]
public class ReverseCipher : TranspositionCipher
{
    private static TranspositionCipher? cipher;

    /// <summary>
    /// <see cref="ReverseCipher"/>的实例
    /// </summary>
    public static TranspositionCipher Cipher => cipher ??= new ReverseCipher();

    /// <inheritdoc/>
    public override string Encrypt(string plainText)
    {
        var charArray = plainText.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    /// <inheritdoc/>
    public override string Decrypt(string cipherText) => Encrypt(cipherText);

    /// <inheritdoc/>
    protected override ushort[] Transpose(ushort[] indexes)
    {
        Array.Reverse(indexes);
        return indexes;
    }
}
