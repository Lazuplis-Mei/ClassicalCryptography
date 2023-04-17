using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 中文汉字的拼音，繁体简体转换，和笔画数的获取
/// </summary>
/// <remarks>
/// <seealso cref="ChineseChar"/>
/// </remarks>
public static partial class ChineseCharacter
{
    /// <summary>
    /// 获取汉字的默认拼音
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDefaultPinyin(char character)
    {
        return new ChineseChar(character).Pinyins[0];
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
    public static bool IsValid(string pinyin)
    {
        return ChineseChar.IsValidPinyin(pinyin);
    }

    /// <summary>
    /// 获取汉字的拼音
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] GetPinyins(char character)
    {
        var chineseCharacter = new ChineseChar(character);
        return chineseCharacter.Pinyins.Take(chineseCharacter.PinyinCount).ToArray();
    }

    /// <summary>
    /// 将形如'shuǐ'的拼音转换成'SHUI3'
    /// </summary>
    /// <param name="pinyin"></param>
    /// <returns></returns>
    public static string ParsePinyin(string pinyin)
    {
        int tonenote = -1;
        var keys = new[] { "A", "O", "E", "I", "U", "V" };
        var result = PinyinRegex().Replace(pinyin, match =>
        {
            Guard.IsEqualTo(tonenote, -1);
            tonenote = match.Value[0] switch
            {
                'ā' or 'ō' or 'ē' or 'ī' or 'ū' or 'ǖ' or '1' => 1,
                'á' or 'ó' or 'é' or 'í' or 'ú' or 'ǘ' or '2' => 2,
                'ǎ' or 'ǒ' or 'ě' or 'ǐ' or 'ǔ' or 'ǚ' or '3' => 3,
                'à' or 'ò' or 'è' or 'ì' or 'ù' or 'ǜ' or '4' => 4,
                _ => 5,
            };
            if (match.Groups["N"].Success)
                return string.Empty;
            return keys.First(key => match.Groups[key].Success);
        });
        return tonenote == -1 ? result.ToUpper() : result.ToUpper() + tonenote;
    }

    [GeneratedRegex("(?<A>[āáǎà])|(?<O>[ōóǒò])|(?<E>[ēéěè])|(?<I>[īíǐì])|(?<U>[ūúǔù])|(?<V>[ǖǘǚǜ])|(?<N>[12345])")]
    private static partial Regex PinyinRegex();
}