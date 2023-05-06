using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System.Collections.ObjectModel;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// 中文汉字的拼音与繁体简体转换
/// </summary>
/// <remarks>
/// <seealso cref="ChineseChar"/>
/// </remarks>
public static partial class ChineseHelper
{
    /// <summary>
    /// 单声母拼音映射
    /// </summary>
    public static readonly ReadOnlyBidirectionalDictionary<string, string> SingleVowels = new(new BidirectionalDictionary<string, string>()
    {
        { "ZHI", "ZH" },
        { "CHI", "CH" },
        { "SHI", "SH" },
        { "RI", "R" },
        { "ZI", "Z" },
        { "CI", "C" },
        { "SI", "S" },
    });

    /// <summary>
    /// 单韵母拼音映射
    /// </summary>
    public static readonly ReadOnlyBidirectionalDictionary<string, string> SingleRhymes = new(new BidirectionalDictionary<string, string>()
    {
        { "A", "A" },
        { "O", "O" },
        { "E", "E" },
        { "ER", "ER" },
        { "AI", "AI" },
        { "AO", "AO" },
        { "EI", "EI" },
        { "OU", "OU" },
        { "AN", "AN" },
        { "ANG", "ANG" },
        { "EN", "EN" },

        { "YI", "I" },
        { "WU", "U" },
        { "YU", "V" },
        { "YA", "IA" },
        { "YAO", "IAO" },
        { "YE", "IE" },
        { "YOU", "IOU" },
        { "WA", "UA" },
        { "WAI", "UAI" },
        { "WEI", "UEI" },
        { "WO", "UO" },
        { "YUE", "VE" },
        { "YAN", "IAN" },
        { "YANG", "IANG" },
        { "YIN", "IN" },
        { "YING", "ING" },
        { "WAN", "UAN" },
        { "WANG", "UANG" },
        { "WEN", "UEN" },
        { "WENG", "UENG" },
        { "YUAN", "VAN" },
        { "YUN", "VN" },
        { "YONG", "IONG" },
    });

    /// <summary>
    /// 获取汉字的默认拼音
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDefaultPinyin(char character)
    {
        return new ChineseChar(character).Pinyins[0];
    }

    /// <summary>
    /// 获得每一个汉字的拼音
    /// </summary>
    /// <remarks>
    /// 对于多音字，无法得知具体使用哪个读音，可以用括号指定<br/>
    /// 例如:<c>说(shuo)</c><br/>
    /// 如不指定，将使用默认读音，不包含声调
    /// </remarks>
    public static ChinesePinyin[] GetPinyinFromText(string text)
    {
        IList<Match> chineseChars = ChineseCharWithPinyin().Matches(text);
        var result = new ChinesePinyin[chineseChars.Count];
        for (int i = 0; i < chineseChars.Count; i++)
        {
            var match = chineseChars[i];
            var character = match.ValueSpan[0];
            var group = match.Groups["Pinyin"];
            string pinYin = group.Success ? ParsePinyin(group.Value) : GetDefaultPinyin(character);
            result[i] = ChinesePinyin.Parse(pinYin);
        }
        return result;
    }

    /// <summary>
    /// 简体转繁体
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTraditional(string text)
    {
        return ChineseConverter.Convert(text, ChineseConversionDirection.SimplifiedToTraditional);
    }

    /// <summary>
    /// 繁体转简体
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToSimplified(string text)
    {
        return ChineseConverter.Convert(text, ChineseConversionDirection.TraditionalToSimplified);
    }

    /// <summary>
    /// 是否是有效的汉字
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid(char character)
    {
        return ChineseChar.IsValidChar(character);
    }

    /// <summary>
    /// 是否是有效的拼音
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidPinyin(string pinyin)
    {
        return ChineseChar.IsValidPinyin(pinyin.ToUpperAscii());
    }

    /// <summary>
    /// 获取汉字的拼音
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] GetAllPinyins(char character)
    {
        var chineseCharacter = new ChineseChar(character);
        return chineseCharacter.Pinyins.Take(chineseCharacter.PinyinCount).ToArray();
    }

    private static readonly string[] pinyinKeys = new[] { "A", "O", "E", "I", "U", "V" };

    /// <summary>
    /// 将形如`shuǐ`的拼音转换成`SHUI3`
    /// </summary>
    public static string ParsePinyin(string pinyin)
    {
        int tonenote = -1;
        var result = PinyinRegex().Replace(pinyin, match =>
        {
            Guard.IsEqualTo(tonenote, -1);
            tonenote = match.ValueSpan[0] switch
            {
                'ā' or 'ō' or 'ē' or 'ī' or 'ū' or 'ǖ' or '1' => 1,
                'á' or 'ó' or 'é' or 'í' or 'ú' or 'ǘ' or '2' => 2,
                'ǎ' or 'ǒ' or 'ě' or 'ǐ' or 'ǔ' or 'ǚ' or '3' => 3,
                'à' or 'ò' or 'è' or 'ì' or 'ù' or 'ǜ' or '4' => 4,
                _ => 5,
            };
            if (match.Groups["Tonenote"].Success)
                return string.Empty;
            return pinyinKeys.First(key => match.Groups[key].Success);
        });
        return tonenote == -1 ? result.ToUpperAscii() : result.ToUpperAscii() + tonenote;
    }

    [GeneratedRegex("(?<A>[āáǎà])|(?<O>[ōóǒò])|(?<E>[ēéěè])|(?<I>[īíǐì])|(?<U>[ūúǔù])|(?<V>[ǖǘǚǜ])|(?<Tonenote>[1-5])")]
    private static partial Regex PinyinRegex();

    [GeneratedRegex(@"\p{IsCJKUnifiedIdeographs}(\((?<Pinyin>[A-Za-zāáǎàōóǒòēéěèīíǐìūúǔùǖǘǚǜ]{1,6}[1-5]?)\))?")]
    private static partial Regex ChineseCharWithPinyin();
}