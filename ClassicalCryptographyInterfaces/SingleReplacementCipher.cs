namespace ClassicalCryptography.Interfaces;


/// <summary>
/// 单表替换密码(对于未知的密钥如何解密，可以尝试https://www.quipqiup.com/)
/// </summary>
[Introduction("单表替换密码", "最经典朴素的密码。")]
public class SingleReplacementCipher : ICipher<string, string>
{
    /// <summary>
    /// 缺省
    /// </summary>
    public SingleReplacementCipher()
    {
        SupposedCharSet = ReflectionCharSet = "";
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
    private Dictionary<char, char> reverseDict = null!;
    /// <summary>
    /// 解密文本
    /// </summary>
    public string Decrypt(string cipherText)
    {
        reverseDict ??= BuildReverseDict();
        Span<char> result = stackalloc char[cipherText.Length];
        for (int i = 0; i < cipherText.Length; i++)
        {
            if (reverseDict.ContainsKey(cipherText[i]))
                result[i] = reverseDict[cipherText[i]];
            else
                result[i] = cipherText[i];
        }
        return result.ToString();
    }

    private Dictionary<char, char> BuildReverseDict()
    {
        reverseDict = new();
        for (int i = 0; i < ReflectionCharSet.Length; i++)
            reverseDict.Add(ReflectionCharSet[i], SupposedCharSet[i]);
        return reverseDict;
    }

    /// <summary>
    /// 加密文本
    /// </summary>
    public string Encrypt(string plainText)
    {
        dict ??= BuildDict();
        Span<char> result = stackalloc char[plainText.Length];
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
            dict.Add(SupposedCharSet[i], ReflectionCharSet[i]);
        return dict;
    }
}
