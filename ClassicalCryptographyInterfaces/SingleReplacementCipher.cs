using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Interfaces;


/// <summary>
/// 单表替换密码
/// </summary>
[Introduction("单表替换密码", "最经典朴素的密码，通过将明文内容替换成其他内容实现。")]
public class SingleReplacementCipher : ICipher<string, string>
{
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

    private Dictionary<char, char> dict = null!;
    private Dictionary<char, char> refDict = null!;

    /// <summary>
    /// 解密文本
    /// </summary>
    [SkipLocalsInit]
    public string Decrypt(string cipherText)
    {
        Span<char> result = cipherText.Length <= StackLimit.MaxCharSize 
            ? stackalloc char[cipherText.Length] : new char[cipherText.Length];
        
        refDict ??= BuildReflectionDict();
        for (int i = 0; i < cipherText.Length; i++)
        {
            if (refDict.ContainsKey(cipherText[i]))
                result[i] = refDict[cipherText[i]];
            else
                result[i] = cipherText[i];
        }
        return result.ToString();
    }

    private Dictionary<char, char> BuildReflectionDict()
    {
        refDict = new();
        for (int i = 0; i < ReflectionCharSet.Length; i++)
            refDict[ReflectionCharSet[i]] = SupposedCharSet[i];

        return refDict;
    }

    /// <summary>
    /// 加密文本
    /// </summary>
    [SkipLocalsInit]
    public string Encrypt(string plainText)
    {
        Span<char> result = plainText.Length <= StackLimit.MaxCharSize
            ? stackalloc char[plainText.Length] : new char[plainText.Length];

        dict ??= BuildDict();
        for (int i = 0; i < plainText.Length; i++)
        {
            if (dict.ContainsKey(plainText[i]))
                result[i] = dict[plainText[i]];
            else
                result[i] = plainText[i];
        }
        return result.ToString();
    }

    private Dictionary<char,char> BuildDict()
    {
        dict = new();
        for (int i = 0; i < SupposedCharSet.Length; i++)
            dict[SupposedCharSet[i]] = ReflectionCharSet[i];
        return dict;
    }
}
