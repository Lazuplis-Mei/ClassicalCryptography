using ClassicalCryptography.Interfaces;

namespace ClassicalCryptography.Transposition;


/// <summary>
/// 反转密码
/// </summary>
[Introduction("反转密码", "明文倒序读出。")]
public class ReverseCipher : TranspositionCipher
{
    /// <summary>
    /// 转换顺序(并没有实际使用)
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    protected override ushort[] Transpose(ushort[] indexes)
    {
        Array.Reverse(indexes);
        return indexes;
    }

    /// <summary>
    /// 加密指定的文本
    /// </summary>
    /// <param name="plainText">明文文本</param>
    public override string Encrypt(string plainText)
    {
        char[] chars = plainText.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    /// <summary>
    /// 解密指定的文本
    /// </summary>
    /// <param name="cipherText">密文文本</param>
    public override string Decrypt(string cipherText)
    {
        return Encrypt(cipherText);
    }
}
