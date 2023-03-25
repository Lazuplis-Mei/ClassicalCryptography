using Microsoft.International.Converters.PinYinConverter;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 汉字的拼音九键输入法
/// </summary>
/// <remarks>
/// ABC DEF GHI JKL MNO PQRS TUV WXYZ
/// </remarks>
public static partial class PinyinNineKey
{
    private static readonly BidirectionalDictionary<char, byte> keyboards = new()
    {
        { 'A', 21 }, { 'B', 22 }, { 'C', 23 },
        { 'D', 31 }, { 'E', 32 }, { 'F', 33 },
        { 'G', 41 }, { 'H', 42 }, { 'I', 43 },
        { 'J', 51 }, { 'K', 52 }, { 'L', 53 },
        { 'M', 61 }, { 'N', 62 }, { 'O', 63 },
        { 'P', 71 }, { 'Q', 72 }, { 'R', 73 }, { 'S', 74 },
        { 'T', 81 }, { 'U', 82 }, { 'V', 83 },
        { 'W', 91 }, { 'X', 92 }, { 'Y', 93 }, { 'Z', 94 }
    };

    /// <summary>
    /// 将英文字母转换为九键输入
    /// </summary>
    public static string LettersToCodes(string text)
    {
        var count = text.Length << 1;
        Span<char> span = count.CanAllocateString() ? stackalloc char[count] : new char[count];
        for (int i = 0; i < count; i += 2)
        {
            var code = keyboards[char.ToUpper(text[i >> 1])];
            span[i] = (char)('0' + (code / 10));
            span[i + 1] = (char)('0' + (code % 10));
        }
        return new(span);
    }

    /// <summary>
    /// 汉字转换成拼音九键输入
    /// </summary>
    /// <remarks>
    /// 对于多音字，无法得知具体使用哪个读音，可以用括号指定<br/>
    /// 例如:<c>说(shuo)</c><br/>
    /// 如不指定，将使用默认读音
    /// </remarks>
    /// <param name="text">汉字(拼音)组成的文本</param>
    /// <param name="includePosition">是否要包括拼音的位置</param>
    public static string ToCodes(string text, bool includePosition = true)
    {
        var result = new StringBuilder();
        IEnumerable<Match> chineseChars = ChineseCharWithPinyin().Matches(text);
        foreach (var match in chineseChars)
        {
            var chineseChar = new ChineseChar(match.Value[0]);
            var defaultPinYin = chineseChar.Pinyins[0];
            string pinYin;
            var group = match.Groups["Pinyin"];
            if (group.Success)
                pinYin = group.Value.ToUpper();
            else
                pinYin = defaultPinYin[..^1];
            foreach (var character in pinYin)
            {
                if (!includePosition)
                    result.Append(keyboards[character] / 10);
                else
                    result.Append(keyboards[character]);
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
        var count = text.Length >> 1;
        Span<char> span = count.CanAllocateString() ? stackalloc char[count] : new char[count];
        for (int i = 0; i < count; i++)
        {
            span[i] = keyboards.Inverse[Convert.ToByte(text.Substring(i << 1, 2))];
        }
        return new(span);
    }

    [GeneratedRegex(@"\p{IsCJKUnifiedIdeographs}(\((?<Pinyin>[A-Za-z]{1,6})\))?")]
    private static partial Regex ChineseCharWithPinyin();
}
