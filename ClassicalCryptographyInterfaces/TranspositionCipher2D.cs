namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 换位密码(二维)
/// </summary>
/// <typeparam name="T">密钥类型</typeparam>
[Introduction("换位密码(二维)", "类型同一维，用二维的形式编码顺序。")]
public abstract class TranspositionCipher2D<T> : ICipher<string, string, T>
{
    /// <summary>
    /// 是否按列进行加密/解密
    /// </summary>
    public bool ByColumn { get; set; }
    /// <summary>
    /// 是否自动填充初始顺序
    /// </summary>
    protected bool FillOrder = true;

    /// <summary>
    /// 密码类型(换位密码)
    /// </summary>
    public CipherType Type => CipherType.Transposition;
    /// <summary>
    /// 是否存储密钥
    /// </summary>
    public bool StoreKey { get; set; }
    private Dictionary<IKey<T>, ushort[,]>? keys;
    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    /// <param name="key">密钥</param>
    protected abstract (int Width, int Height) Partition(int textLength, IKey<T> key);

    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    /// <param name="key">密钥</param>
    protected abstract ushort[,] Transpose(ushort[,] indexes, IKey<T> key);
    /// <summary>
    /// 加密指定的文本
    /// </summary>
    /// <param name="plainText">明文文本</param>
    /// <param name="key">密钥</param>
    public virtual string Encrypt(string plainText, IKey<T> key)
    {
        ushort[,] order = GetOrder(plainText.Length, key);
        if (ByColumn)
            return order.AssembleTextByColumn(plainText);
        return order.AssembleTextByRow(plainText);
    }

    private ushort[,] GetOrder(int textLength, IKey<T> key)
    {
        (int width, int height) = Partition(textLength, key);
        ushort[,] order;
        if (StoreKey)
        {
            if (keys == null)
                keys = new();
            if (!keys.ContainsKey(key))
            {
                order = new ushort[width, height];
                if (FillOrder)
                    order.FillOrderByRow();
                order = Transpose(order, key);
                keys.Add(key, order);
            }
            return keys[key];
        }
        else
        {
            order = new ushort[width, height];
            if (FillOrder)
                order.FillOrderByRow();
            return Transpose(order, key);
        }
    }

    /// <summary>
    /// 多重加密
    /// </summary>
    /// <param name="plainText">明文文本</param>
    /// <param name="key">密钥</param>
    /// <param name="n">加密次数</param>
    public string MultiEncrypt(string plainText, IKey<T> key, int n)
    {
        ushort[,] order = GetOrder(plainText.Length, key);
        order = order.MultiTranspose(n, ByColumn);
        if (ByColumn)
            return order.AssembleTextByColumn(plainText);
        return order.AssembleTextByRow(plainText);
    }

    /// <summary>
    /// 多重解密
    /// </summary>
    /// <param name="cipherText">密文文本</param>
    /// <param name="key">密钥</param>
    /// <param name="n">解密次数</param>
    public string MultiDecrypt(string cipherText, IKey<T> key, int n)
    {
        ushort[,] order = GetOrder(cipherText.Length, key);
        order = order.MultiTranspose(n, ByColumn);
        if (ByColumn)
            return order.AssembleTextByColumnInverse(cipherText);
        return order.AssembleTextByRowInverse(cipherText);
    }

    /// <summary>
    /// 解密指定的文本
    /// </summary>
    /// <param name="cipherText">密文文本</param>
    /// <param name="key">密钥</param>
    public virtual string Decrypt(string cipherText, IKey<T> key)
    {
        ushort[,] order = GetOrder(cipherText.Length, key);
        if (ByColumn)
            return order.AssembleTextByColumnInverse(cipherText);
        return order.AssembleTextByRowInverse(cipherText);
    }

}


/// <summary>
/// 换位密码(二维)
/// </summary>
[Introduction("换位密码(二维)", "类型同一维，用二维的形式编码顺序。")]
public abstract class TranspositionCipher2D : ICipher<string, string>
{
    /// <summary>
    /// 是否按列进行加密/解密
    /// </summary>
    public bool ByColumn { get; set; }
    /// <summary>
    /// 是否自动填充初始顺序
    /// </summary>
    protected bool FillOrder = true;
    /// <summary>
    /// 密码类型(换位密码)
    /// </summary>
    public CipherType Type => CipherType.Transposition;

    /// <summary>
    /// 划分二维顺序矩阵
    /// </summary>
    /// <param name="textLength">原文长度</param>
    protected abstract (int Width, int Height) Partition(int textLength);

    /// <summary>
    /// 转换顺序
    /// </summary>
    /// <param name="indexes">正常顺序</param>
    protected abstract ushort[,] Transpose(ushort[,] indexes);
    /// <summary>
    /// 加密指定的文本
    /// </summary>
    /// <param name="plainText">明文文本</param>
    public virtual string Encrypt(string plainText)
    {
        (int width, int height) = Partition(plainText.Length);
        ushort[,] order = new ushort[width, height];
        if (FillOrder)
            order.FillOrderByRow();
        if (ByColumn)
            return Transpose(order).AssembleTextByColumn(plainText);
        return Transpose(order).AssembleTextByRow(plainText);
    }

    /// <summary>
    /// 多重加密
    /// </summary>
    /// <param name="plainText">明文文本</param>
    /// <param name="n">加密次数</param>
    public string MultiEncrypt(string plainText, int n)
    {
        (int width, int height) = Partition(plainText.Length);
        ushort[,] order = new ushort[width, height];
        if (FillOrder)
            order.FillOrderByRow();
        order = Transpose(order).MultiTranspose(n, ByColumn);
        if (ByColumn)
            return order.AssembleTextByColumn(plainText);
        return order.AssembleTextByRow(plainText);
    }

    /// <summary>
    /// 多重解密
    /// </summary>
    /// <param name="cipherText">密文文本</param>
    /// <param name="n">解密次数</param>
    public string MultiDecrypt(string cipherText, int n)
    {
        (int width, int height) = Partition(cipherText.Length);
        ushort[,] order = new ushort[width, height];
        if (FillOrder)
            order.FillOrderByRow();
        order = Transpose(order).MultiTranspose(n, ByColumn);
        if (ByColumn)
            return order.AssembleTextByColumnInverse(cipherText);
        return order.AssembleTextByRowInverse(cipherText);
    }

    /// <summary>
    /// 解密指定的文本
    /// </summary>
    /// <param name="cipherText">密文文本</param>
    public virtual string Decrypt(string cipherText)
    {
        (int width, int height) = Partition(cipherText.Length);
        ushort[,] order = new ushort[width, height];
        if (FillOrder)
            order.FillOrderByRow();
        if (ByColumn)
            return Transpose(order).AssembleTextByColumnInverse(cipherText);
        return Transpose(order).AssembleTextByRowInverse(cipherText);
    }

}