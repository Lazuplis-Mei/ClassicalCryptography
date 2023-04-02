using System.Numerics;

namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 密钥
/// </summary>
/// <typeparam name="T">密钥类型</typeparam>
public interface IKey<T>
{
    /// <summary>
    /// 密钥值
    /// </summary>
    T KeyValue { get; }

    /// <summary>
    /// 是否可逆
    /// </summary>
    bool CanInverse { get; }

    /// <summary>
    /// 逆向密钥(如果存在)
    /// </summary>
    IKey<T>? InversedKey { get; }

    /// <summary>
    /// 从文本格式创建密钥
    /// </summary>
    /// <param name="strKey">文本类型的密钥</param>
    static abstract IKey<T> FromString(string strKey);

    /// <summary>
    /// 产生随机密钥
    /// </summary>
    /// <param name="textLength">加密内容的长度</param>
    static abstract IKey<T> GenerateKey(int textLength);

    /// <summary>
    /// 获得密钥的空间
    /// </summary>
    /// <param name="textLength">加密内容的长度</param>
    static abstract BigInteger GetKeySpace(int textLength);

    /// <summary>
    /// 返回密钥的文本形式
    /// </summary>
    string GetString();
}
