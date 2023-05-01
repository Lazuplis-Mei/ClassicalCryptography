using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("ClassicalCryptography")]
namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 单表替换密码
/// </summary>
[Introduction("单表替换密码", "最经典朴素的密码，通过将明文内容替换成其他内容实现。")]
public class SingleReplacementCipher : ICipher<string, string>
{
    internal readonly BidirectionalDictionary<char, char> map = new();

    /// <summary>
    /// 单表替换密码
    /// </summary>
    public SingleReplacementCipher()
    {
        SupposedCharSet = ReflectionCharSet = string.Empty;
        IgnoreCase = false;
    }

    /// <summary>
    /// 初始化单表
    /// </summary>
    public SingleReplacementCipher(string supposedCharSet, string reflectionCharSet, bool ignoreCase = false)
    {
        SupposedCharSet = supposedCharSet;
        ReflectionCharSet = reflectionCharSet;
        IgnoreCase = ignoreCase;
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
    /// 是否忽略大小写
    /// </summary>
    public virtual bool IgnoreCase { get; }

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
            var character = cipherText[i];
            bool lower = char.IsAsciiLetterLower(character);
            if (IgnoreCase && lower)
                character = (char)(character & 0B11011111);
            character = map.Inverse.TryGetValue(character, out char value) ? value : character;
            if (IgnoreCase && lower && char.IsAsciiLetterUpper(character))
                character = (char)(character | 0B00100000);
            result[i] = character;
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
            var character = plainText[i];
            bool lower = char.IsAsciiLetterLower(character);
            if (IgnoreCase && lower)
                character = (char)(character & 0B11011111);
            character = map.TryGetValue(character, out char value) ? value : character;
            if (IgnoreCase && lower && char.IsAsciiLetterUpper(character))
                character = (char)(character | 0B00100000);
            result[i] = character;
        }
        return result.ToString();
    }

    /// <summary>
    /// 是否是可加密的字符
    /// </summary>
    public bool IsVaildChar(char character)
    {
        bool lower = char.IsAsciiLetterLower(character);
        if (IgnoreCase && lower)
            character = (char)(character & 0B11011111);
        return map.ContainsKey(character);
    }

    /// <summary>
    /// 是否是可解密的字符
    /// </summary>
    public bool IsVaildCharInverse(char character)
    {
        bool lower = char.IsAsciiLetterLower(character);
        if (IgnoreCase && lower)
            character = (char)(character & 0B11011111);
        return map.Inverse.ContainsKey(character);
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
