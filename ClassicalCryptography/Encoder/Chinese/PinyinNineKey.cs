﻿namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// 汉字的拼音九键输入法
/// </summary>
/// <remarks>
/// 可接受的中文汉字范围为[〇\u4E00-\u9FA5\uE81A-\uE863]
/// </remarks>
public partial class PinyinNineKey
{
    private readonly BidirectionalDictionary<char, (char Key, int Position)> keyboards;

    /// <summary>
    /// 是否要包括拼音的位置
    /// </summary>
    public bool IncludePosition { get; set; } = true;

    /// <summary>
    /// 汉字的拼音九键输入法
    /// </summary>
    public PinyinNineKey(BidirectionalDictionary<char, (char Key, int Position)> keyboards)
    {
        this.keyboards = keyboards;
    }

    /// <summary>
    /// 将英文字母转换为九键输入
    /// </summary>
    public string LettersToCodes(string text)
    {
        var count = text.Length * 2;
        using var memory = count.TryAllocString();
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : memory.Span;
        for (int i = 0; i < count; i += 2)
        {
            var (Key, Position) = keyboards[text[i / 2].ToUpperAscii()];
            span[i] = Key;
            span[i + 1] = Digits[Position];
        }
        return new(span);
    }

    /// <summary>
    /// 汉字转换成拼音九键输入
    /// </summary>
    /// <remarks>
    /// 对于多音字，无法得知具体使用哪个读音(不能包括声调)，可以用括号指定<br/>
    /// 例如说(SHUO)<br/>
    /// 如不指定，将使用默认读音<br/>
    /// <seealso cref="ChineseHelper.GetDefaultPinyin"/>
    /// </remarks>
    /// <param name="text">汉字(拼音)组成的文本</param>
    public string ToCodes(string text)
    {
        var result = new StringBuilder();
        IEnumerable<Match> chineseChars = ChineseCharWithPinyin().Matches(text);
        foreach (var match in chineseChars)
        {
            var defaultPinYin = ChineseHelper.GetDefaultPinyin(match.ValueSpan[0]);
            var group = match.Groups["Pinyin"];
            var pinYin = group.Success ? group.Value.ToUpperAscii() : defaultPinYin[..^1];
            foreach (var character in pinYin)
            {
                var (Key, Position) = keyboards[character];
                result.Append(Key);
                if (IncludePosition)
                    result.Append(Digits[Position]);
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// 包含位置的九键转换成拼音或字母
    /// </summary>
    [SkipLocalsInit]
    public string FromCodes(string text)
    {
        Guard.IsTrue(IncludePosition);
        var count = text.Length / 2;
        using var memory = count.TryAllocString();
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : memory.Span;
        for (int i = 0; i < count; i++)
        {
            int j = i * 2;
            var value = (text[j], text[j + 1].Base36Number());
            span[i] = keyboards.Inverse[value];
        }
        return new(span);
    }

    [GeneratedRegex(@"[〇\u4E00-\u9FA5\uE81A-\uE863](\((?<Pinyin>[A-Za-z]{1,6})\))?")]
    private static partial Regex ChineseCharWithPinyin();

}
