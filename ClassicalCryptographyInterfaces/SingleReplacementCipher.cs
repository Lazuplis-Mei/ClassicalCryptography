using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 单表替换密码
/// </summary>
[Introduction("单表替换密码", "最经典朴素的密码，通过将明文内容替换成其他内容实现。")]
public class SingleReplacementCipher : ICipher<string, string>
{
    private readonly BidirectionalDictionary<char, char> map = new();

    /// <summary>
    /// 单表替换密码
    /// </summary>
    public SingleReplacementCipher()
    {
        SupposedCharSet = ReflectionCharSet = string.Empty;
    }

    /// <summary>
    /// 初始化单表
    /// </summary>
    public SingleReplacementCipher(string supposedCharSet, string reflectionCharSet)
    {
        SupposedCharSet = supposedCharSet;
        ReflectionCharSet = reflectionCharSet;
        BuildMap();
    }

    /// <summary>
    /// 密码类型(替换密码)
    /// </summary>
    public CipherType Type => CipherType.Substitution;

    /// <summary>
    /// 支持的字符集
    /// </summary>
    public virtual string SupposedCharSet { get; }

    /// <summary>
    /// 映射的字符集
    /// </summary>
    public virtual string ReflectionCharSet { get; }

    /// <summary>
    /// 解密文本
    /// </summary>
    [SkipLocalsInit]
    public string Decrypt(string cipherText)
    {
        int length = cipherText.Length;
        Span<char> result = length.CanAllocString() ? stackalloc char[length] : new char[length];

        for (int i = 0; i < length; i++)
        {
            if (map.Inverse.ContainsKey(cipherText[i]))
                result[i] = map.Inverse[cipherText[i]];
            else
                result[i] = cipherText[i];
        }
        return result.ToString();
    }

    /// <summary>
    /// 加密文本
    /// </summary>
    [SkipLocalsInit]
    public string Encrypt(string plainText)
    {
        int length = plainText.Length;
        Span<char> result = length.CanAllocString() ? stackalloc char[length] : new char[length];

        for (int i = 0; i < length; i++)
        {
            if (map.ContainsKey(plainText[i]))
                result[i] = map[plainText[i]];
            else
                result[i] = plainText[i];
        }
        return result.ToString();
    }

    /// <summary>
    /// 创建映射
    /// </summary>
    protected void BuildMap()
    {
        for (int i = 0; i < SupposedCharSet.Length; i++)
            map[SupposedCharSet[i]] = ReflectionCharSet[i];
    }
}
