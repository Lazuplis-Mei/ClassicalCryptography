namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 换位密码(一维)
/// </summary>
/// <typeparam name="T">密钥类型</typeparam>
[Introduction("换位密码(一维)", "明文字符不变，仅打乱字符位置。")]
public abstract class TranspositionCipher<T> : ICipher<string, string, T>
{
    /// <summary>
    /// 密码类型(换位密码)
    /// </summary>
    public CipherType Type => CipherType.Transposition;
    /// <summary>
    /// 是否存储密钥
    /// </summary>
    public bool StoreKey { get; set; }
    /// <summary>
    /// 是否自动填充初始顺序
    /// </summary>
    protected bool FillOrder = true;

    private Dictionary<IKey<T>, ushort[]>? keys;

    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    /// <param name="key">密钥</param>
    protected abstract ushort[] Transpose(ushort[] indexes, IKey<T> key);

    /// <summary>
    /// 补足长度
    /// </summary>
    /// <param name="length">文本长度</param>
    protected virtual int PadLength(int length) => length;

    /// <summary>
    /// 加密指定的文本
    /// </summary>
    /// <param name="plainText">明文文本</param>
    /// <param name="key">密钥</param>
    public virtual string Encrypt(string plainText, IKey<T> key)
    {
        ushort[] order = GetOrder(PadLength(plainText.Length), key);
        return order.AssembleText(plainText);
    }

    private ushort[] GetOrder(int textLength, IKey<T> key)
    {
        ushort[] order;
        if (StoreKey)
        {
            if (keys == null) keys = new();
            if (!keys.ContainsKey(key))
            {
                order = new ushort[textLength];
                if (FillOrder)
                    order.FillOrder();
                order = Transpose(order, key);
                keys.Add(key, order);
            }
            else order = keys[key];
        }
        else
        {
            order = new ushort[textLength];
            if (FillOrder)
                order.FillOrder();
            order = Transpose(order, key);
        }

        return order;
    }

    /// <summary>
    /// 解密指定的文本
    /// </summary>
    /// <param name="cipherText">密文文本</param>
    /// <param name="key">密钥</param>
    public virtual string Decrypt(string cipherText, IKey<T> key)
    {
        ushort[] order = GetOrder(PadLength(cipherText.Length), key);
        return order.AssembleTextInverse(cipherText);
    }
}


/// <summary>
/// 换位密码(一维)
/// </summary>
[Introduction("换位密码(一维)", "明文字符不变，仅打乱字符位置。")]
public abstract class TranspositionCipher : ICipher<string, string>
{
    /// <summary>
    /// 密码类型(换位密码)
    /// </summary>
    public CipherType Type => CipherType.Transposition;
    /// <summary>
    /// 是否自动填充初始顺序
    /// </summary>
    protected bool FillOrder = true;
    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    protected abstract ushort[] Transpose(ushort[] indexes);
    /// <summary>
    /// 补足长度
    /// </summary>
    /// <param name="length">文本长度</param>
    protected virtual int PadLength(int length) => length;
    /// <summary>
    /// 加密指定的文本
    /// </summary>
    /// <param name="plainText">明文文本</param>
    public virtual string Encrypt(string plainText)
    {
        ushort[] order = new ushort[PadLength(plainText.Length)];
        if (FillOrder)
            order.FillOrder();
        return Transpose(order).AssembleText(plainText);
    }
    /// <summary>
    /// 解密指定的文本
    /// </summary>
    /// <param name="cipherText">密文文本</param>
    public virtual string Decrypt(string cipherText)
    {
        ushort[] order = new ushort[PadLength(cipherText.Length)];
        if (FillOrder)
            order.FillOrder();
        return Transpose(order).AssembleTextInverse(cipherText);
    }
}
