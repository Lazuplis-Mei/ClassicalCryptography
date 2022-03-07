namespace ClassicalCryptography.Interfaces;

/*
 private readonly bool saveKey;
    private readonly Dictionary<IKey<ushort[][]>, ushort[,]> keys;

    public CycleTranspose(bool saveOrder = false)
    {
        saveKey = saveOrder;
        keys = null!;
        if (saveOrder) keys = new();
    }
 
 */
/// <summary>
/// 密码接口
/// </summary>
/// <typeparam name="TP">明文类型</typeparam>
/// <typeparam name="TC">密文类型</typeparam>
/// <typeparam name="TKey">密钥类型(如果指定为bool将代表不需要密钥)</typeparam>
public interface ICipher<TP, TC, TKey>
{
    /// <summary>
    /// 密码类型
    /// </summary>
    CipherType Type { get; }
    /// <summary>
    /// 是否存储密钥
    /// </summary>
    bool StoreKey { get; set; }

    /// <summary>
    /// 加密指定的内容
    /// </summary>
    /// <param name="plainText">明文:要加密的内容</param>
    /// <param name="key">密钥</param>
    TC Encrypt(TP plainText, IKey<TKey> key);
    /// <summary>
    /// 解密指定的内容
    /// </summary>
    /// <param name="cipherText">密文:要解密的内容</param>
    /// <param name="key">密钥</param>
    TP Decrypt(TC cipherText, IKey<TKey> key);
}


/// <summary>
/// 密码接口
/// </summary>
/// <typeparam name="TP">明文类型</typeparam>
/// <typeparam name="TC">密文类型</typeparam>
public interface ICipher<TP, TC>
{
    /// <summary>
    /// 密码类型
    /// </summary>
    CipherType Type { get; }

    /// <summary>
    /// 加密指定的内容
    /// </summary>
    /// <param name="plainText">明文:要加密的内容</param>
    TC Encrypt(TP plainText);
    /// <summary>
    /// 解密指定的内容
    /// </summary>
    /// <param name="cipherText">密文:要解密的内容</param>
    TP Decrypt(TC cipherText);
}