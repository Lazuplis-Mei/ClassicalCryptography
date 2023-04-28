using Microsoft.International.Converters.PinYinConverter;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// 汉字的拼音九键输入法
/// </summary>
/// <remarks>
/// ABC DEF GHI JKL MNO PQRS TUV WXYZ
/// </remarks>
public partial class PinyinNineKey
{
    private static readonly BidirectionalDictionary<char, byte> keyboards = new()
    {
        { 'A', 21 }, { 'B', 22 }, { 'C', 23 },              //2
        { 'D', 31 }, { 'E', 32 }, { 'F', 33 },              //3
        { 'G', 41 }, { 'H', 42 }, { 'I', 43 },              //4
        { 'J', 51 }, { 'K', 52 }, { 'L', 53 },              //5
        { 'M', 61 }, { 'N', 62 }, { 'O', 63 },              //6
        { 'P', 71 }, { 'Q', 72 }, { 'R', 73 }, { 'S', 74 }, //7
        { 'T', 81 }, { 'U', 82 }, { 'V', 83 },              //8
        { 'W', 91 }, { 'X', 92 }, { 'Y', 93 }, { 'Z', 94 }  //9
    };

    /// <summary>
    /// 将英文字母转换为九键输入
    /// </summary>
    public static string LettersToCodes(string text)
    {
        var count = text.Length * 2;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        for (int i = 0; i < count; i += 2)
        {
            var code = keyboards[text[i / 2].ToUpperAscii()];
            span[i] = Digits[code / 10];
            span[i + 1] = Digits[code % 10];
        }
        return new(span);
    }

    /// <summary>
    /// 汉字转换成拼音九键输入
    /// </summary>
    /// <remarks>
    /// 对于多音字，无法得知具体使用哪个读音，可以用括号指定<br/>
    /// 例如:<c>说(shuo)</c><br/>
    /// 如不指定，将使用默认读音，不包含声调
    /// </remarks>
    /// <param name="text">汉字(拼音)组成的文本</param>
    /// <param name="includePosition">是否要包括拼音的位置</param>
    public static string ToCodes(string text, bool includePosition = true)
    {
        var result = new StringBuilder();
        IEnumerable<Match> chineseChars = ChineseCharWithPinyin().Matches(text);
        foreach (var match in chineseChars)
        {
            var defaultPinYin = ChineseHelper.GetDefaultPinyin(match.Value[0]);
            string pinYin;
            var group = match.Groups["Pinyin"];
            pinYin = group.Success ? group.Value.ToUpperAscii() : defaultPinYin[..^1];
            foreach (var character in pinYin)
            {
                if (includePosition)
                    result.Append(keyboards[character]);
                else
                    result.Append(keyboards[character] / 10);
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// 包含位置的九键转换成拼音或字母
    /// </summary>
    [SkipLocalsInit]
    public static string FromCodes(string text)
    {
        var count = text.Length / 2;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        for (int i = 0; i < count; i++)
        {
            int j = i * 2;
            int value = text[j++].Base36Number() * 10 + text[j].Base36Number();
            span[i] = keyboards.Inverse[(byte)value];
        }
        return new(span);
    }

    [GeneratedRegex(@"\p{IsCJKUnifiedIdeographs}(\((?<Pinyin>[A-Za-z]{1,6})\))?")]
    private static partial Regex ChineseCharWithPinyin();

}
