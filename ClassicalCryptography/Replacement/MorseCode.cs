namespace ClassicalCryptography.Replacement;

/// <summary>
/// 摩斯密码
/// </summary>
[Introduction("摩斯密码", "一种用信号时长和断续表示内容的代码")]
public partial class MorseCode
{
    private const char SEPARATOR = '/';
    private const char WORD_SEPARATOR = ' ';
    private readonly BidirectionalDictionary<char, string> morseData;

    internal MorseCode(BidirectionalDictionary<char, string> morseData)
    {
        this.morseData = morseData;
    }

    /// <summary>
    /// 字符是否可以编码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsVaildLetter(char letter)
    {
        return morseData.ContainsKey(letter.ToUpperAscii());
    }

    /// <summary>
    /// 是否是合法的摩斯密码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsVaildCode(string code)
    {
        return morseData.Inverse.ContainsKey(code);
    }

    /// <summary>
    /// 解密字符时的特别处理
    /// </summary>
    protected virtual char ConvertInverse(char character) => character;

    /// <summary>
    /// 加密字符时的特别处理
    /// </summary>
    protected virtual char Convert(char character)
    {
        if (character is '*' && morseData.ContainsKey('X'))
            character = 'X';
        return character;
    }

    /// <summary>
    /// 解密摩斯密码
    /// </summary>
    public string FromMorse(string morse)
    {
        var result = new StringBuilder(morse.Length / 4);
        foreach (string word in morse.Split(WORD_SEPARATOR))
        {
            foreach (string code in word.Split(SEPARATOR))
            {
                if (morseData.Inverse.TryGetValue(code, out char character))
                    result.Append(ConvertInverse(character));
            }
            result.Append(WORD_SEPARATOR);
        }
        return result.RemoveLast().ToString();
    }

    /// <summary>
    /// 加密为摩斯密码
    /// </summary>
    public string ToMorse(string text)
    {
        var result = new StringBuilder(text.Length * 4);
        for (int i = 0; i < text.Length; i++)
        {
            char character = Convert(text[i]);
            if (character == WORD_SEPARATOR)
                result.Append(WORD_SEPARATOR);
            else if (morseData.TryGetValue(character.ToUpperAscii(), out string code))
                result.Append(code).Append(SEPARATOR);
        }
        return result.RemoveLast().ToString();
    }
}
