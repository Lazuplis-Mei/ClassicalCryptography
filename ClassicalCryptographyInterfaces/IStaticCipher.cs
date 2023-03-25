namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 密码接口
/// </summary>
/// <typeparam name="TP">明文类型</typeparam>
/// <typeparam name="TC">密文类型</typeparam>
public interface IStaticCipher<TP, TC>
{
    /// <summary>
    /// 密码类型
    /// </summary>
    static abstract CipherType Type { get; }

    /// <summary>
    /// 加密指定的内容
    /// </summary>
    /// <param name="plainText">明文:要加密的内容</param>
    static abstract TC Encrypt(TP plainText);

    /// <summary>
    /// 解密指定的内容
    /// </summary>
    /// <param name="cipherText">密文:要解密的内容</param>
    static abstract TP Decrypt(TC cipherText);
}