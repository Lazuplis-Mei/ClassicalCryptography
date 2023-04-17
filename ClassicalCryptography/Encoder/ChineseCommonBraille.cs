using static ClassicalCryptography.Encoder.ChineseCharacter;
using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// <a href="http://www.moe.gov.cn/jyb_sjzl/ziliao/A19/201807/W020180725666187054299.pdf">国家通用盲文方案</a>
/// </summary>
public static partial class ChineseCommonBraille
{
    private const string TONE_NOTES = "⠁⠂⠄⠆";

    private const char ULETTER_SIGN = '⠠';

    private const char LLETTER_SIGN = '⠰';

    private const char NUMBER_SIGN = '⠼';

    private static readonly Dictionary<string, char> brailles = new()
    {
        { "B", '⠃' },
        { "P", '⠏' },
        { "M", '⠍' },
        { "F", '⠋' },
        { "D", '⠙' },
        { "T", '⠞' },
        { "N", '⠝' },
        { "L", '⠇' },
        { "G", '⠛' },{ "J", '⠛' },
        { "K", '⠅' },{ "Q", '⠅' },
        { "H", '⠓' },{ "X", '⠓' },
        { "ZH", '⠌' },
        { "CH", '⠟' },
        { "SH", '⠱' },
        { "R", '⠚' },
        { "Z", '⠵' },
        { "C", '⠉' },
        { "S", '⠎' },
        { "A", '⠔' },
        { "O", '⠢' },{ "E", '⠢' },
        { "I", '⠊' },
        { "U", '⠥' },
        { "V", '⠬' },
        { "ER", '⠗' },
        { "AI", '⠪' },
        { "AO", '⠖' },
        { "EI", '⠮' },
        { "OU", '⠷' },
        { "IA", '⠫' },
        { "IAO", '⠜' },
        { "IE", '⠑' },
        { "IOU", '⠳' },{ "IU", '⠳' },
        { "UA", '⠿' },
        { "UAI", '⠽' },
        { "UEI", '⠺' },{ "UI", '⠺' },
        { "UO", '⠕' },
        { "VE", '⠾' },
        { "AN", '⠧' },
        { "ANG", '⠦' },
        { "EN", '⠴' },
        { "ENG", '⠼' },
        { "IAN", '⠩' },
        { "IANG", '⠭' },
        { "IN", '⠣' },
        { "ING", '⠡' },
        { "UAN", '⠻' },
        { "UANG", '⠶' },
        { "UEN", '⠒' },{ "UN", '⠒' },
        { "ONG", '⠲' },{ "UENG", '⠲' },
        { "VAN", '⠯' },
        { "VN", '⠸' },
        { "IONG", '⠹' },
    };

    private static readonly Dictionary<char, string> braillesInverse = new()
    {
        { '⠃', "B" },
        { '⠏', "P" },
        { '⠍', "M" },
        { '⠋', "F" },
        { '⠙', "D" },
        { '⠞', "T" },
        { '⠝', "N" },
        { '⠇', "L" },
        { '⠛', "[G/J]" },
        { '⠅', "[K/Q]" },
        { '⠓', "[H/X]" },
        { '⠌', "ZH(I)" },
        { '⠟', "CH(I)" },
        { '⠱', "SH(I)" },
        { '⠚', "R(I)" },
        { '⠵', "Z(I)" },
        { '⠉', "C(I)" },
        { '⠎', "S(I)" },
        { '⠔', "A" },
        { '⠢', "[O/E]" },
        { '⠊', "I" },
        { '⠥', "U" },
        { '⠬', "V" },
        { '⠗', "ER" },
        { '⠪', "AI" },
        { '⠖', "AO" },
        { '⠮', "EI" },
        { '⠷', "OU" },
        { '⠫', "IA" },
        { '⠜', "IAO" },
        { '⠑', "IE"},
        { '⠳', "[IU/YOU]" },
        { '⠿', "UA" },
        { '⠽', "UAI" },
        { '⠺', "[UI/WEI]" },
        { '⠕', "UO" },
        { '⠾', "VE" },
        { '⠧', "AN" },
        { '⠦', "ANG" },
        { '⠴', "EN" },
        { '⠼', "ENG" },
        { '⠩', "IAN" },
        { '⠭', "IANG" },
        { '⠣', "IN" },
        { '⠡', "ING" },
        { '⠻', "UAN" },
        { '⠶', "UANG" },
        { '⠒', "[UN/WEN]" },
        { '⠲', "[ONG/WENG]" },
        { '⠯', "VAN" },
        { '⠸', "VN" },
        { '⠹', "IONG" },
        { '⠁', "1" },
        { '⠂', "2" },
        { '⠄', "3" },
        { '⠆', "4" },
    };

    private static readonly BidirectionalDictionary<string, string> singleVowels = new()
    {
        { "ZHI", "ZH" },
        { "CHI", "CH" },
        { "SHI", "SH" },
        { "RI", "R" },
        { "ZI", "Z" },
        { "CI", "C" },
        { "SI", "S" },
    };

    private static readonly BidirectionalDictionary<string, string> singleRhymes = new()
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
        { "YUE", "IUE" },
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
    };

    private static readonly Dictionary<char, string> punctuations = new()
    {
        { '。', "⠐⠆" },
        { '，', "⠐" },
        { '、', "⠈" },
        { '；', "⠰" },
        { '？', "⠐⠄" },{ '〈', "⠐⠄" },
        { '！', "⠰⠂" },
        { '：', "⠤" },
        { '“', "⠘" },{ '”', "⠘" },
        { '‘', "⠘⠘" },{ '’', "⠘⠘" },
        { '（', "⠰⠄" },
        { '）', "⠠⠆" },
        { '【', "⠰⠆" },{ '】', "⠰⠆" },
        { '—', "⠠⠤" },
        { '…', "⠐⠐⠐" },
        { '《', "⠐⠤" },
        { '》', "⠤⠂" },
        { '〉', "⠠⠂" },
    };

    private static readonly string letters = "⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵";

    /// <summary>
    /// 指示编码时将繁体字对应为简体字
    /// </summary>
    public static bool AutoSimplify { get; set; } = false;

    /// <summary>
    /// 指示应当编码声调(轻声声调5将被忽略)
    /// </summary>
    public static bool EncodeTonenote { get; set; } = true;

    /// <summary>
    /// 指示应当编码如下的标点符号<br/>
    /// <c>。，、；？！：“”‘’（）【】—…《》〈〉</c>
    /// </summary>
    public static bool EncodePunctuation { get; set; } = true;

    /// <summary>
    /// 指示应当编码数字
    /// </summary>
    public static bool EncodeNumber { get; set; } = true;

    /// <summary>
    /// 指示应当编码英文字母
    /// </summary>
    public static bool EncodeLetters { get; set; } = true;

    /// <summary>
    /// 指示应当区分他它她
    /// </summary>
    /// <remarks>
    /// <see cref="EncodeTonenote"/>须为<see langword="true"/><br/>
    /// 来源于规则9.4
    /// </remarks>
    public static bool DistinguishThird { get; set; } = true;

    /// <summary>
    /// 使用针对声调的简写
    /// </summary>
    /// <remarks>
    /// <see cref="EncodeTonenote"/>须为<see langword="true"/><br/>
    /// 来源于规则(10.2.1~10.2.6)
    /// </remarks>
    public static bool UseAbbreviations { get; set; } = true;

    /// <summary>
    /// 指示是否在编码过程中记录拼音作为输出参数<br/>
    /// 如果该项被设置为<see langword="false"/>，则<see cref="Encode(string, out string?)"/>的参数2不起作用
    /// </summary>
    public static bool OutputPinyins { get; set; } = false;

    /// <inheritdoc cref="Encode(string, out string?)"/>
    public static string Encode(string text) => Encode(text, out _);

    /// <summary>
    /// 解码国家通用的盲文为可能的拼音
    /// </summary>
    /// <param name="brailleText">盲文文本</param>
    /// <remarks>
    /// <see cref="DistinguishThird"/><br/>
    /// <see cref="UseAbbreviations"/><br/>
    /// 必须都为<see langword="false"/>
    /// </remarks>
    public static string DecodePinyins(string brailleText)
    {
        Guard.IsFalse(DistinguishThird);
        Guard.IsFalse(UseAbbreviations);

        bool bracketsFlag = false;
        bool dQuotesFlag = false;
        bool sQuotesFlag = false;
        var result = new StringBuilder();
        IEnumerable<Match> matches = ChineseCommonBrailleRegex().Matches(brailleText);
        foreach (var match in matches)
        {
            Group group;
            if ((group = match.Groups["ULetters"]).Success)
            {
                foreach (var letter in group.ValueSpan)
                    result.Append(U_Letters[letters.IndexOf(letter)]);
                continue;
            }
            if ((group = match.Groups["LLetters"]).Success)
            {
                foreach (var letter in group.ValueSpan)
                    result.Append(L_Letters[letters.IndexOf(letter)]);
                continue;
            }
            if ((group = match.Groups["Numbers"]).Success)
            {
                foreach (var number in group.ValueSpan)
                    result.Append(VDigits[letters.IndexOf(number)]);
                continue;
            }

            if ((group = match.Groups["Others"]).Success)
            {
                var brailles = group.ValueSpan;
                for (int i = 0; i < brailles.Length; i++)
                {
                    if (brailles[i] == '⠐')
                    {
                        if (++i >= brailles.Length)
                        {
                            result.Append('，');
                            break;
                        }
                        switch (brailles[i])
                        {
                            case '⠆':
                                result.Append('。');
                                break;
                            case '⠄':
                                result.Append('？');
                                break;
                            case '⠤':
                                result.Append('《');
                                break;
                            case '⠐':
                                if (++i < brailles.Length && brailles[i] == '⠐')
                                    result.Append('…');
                                break;
                            default:
                                i--;
                                result.Append('，');
                                break;
                        }
                    }
                    else if (brailles[i] == '⠈')
                        result.Append('、');
                    else if (brailles[i] == '⠰')
                    {
                        if (++i >= brailles.Length)
                        {
                            result.Append('；');
                            break;
                        }
                        switch (brailles[i])
                        {
                            case '⠂':
                                result.Append('！');
                                break;
                            case '⠄':
                                result.Append('（');
                                break;
                            case '⠆':
                                result.Append((bracketsFlag ^= true) ? '【' : '】');
                                break;
                        }
                    }
                    else if (brailles[i] == '⠤')
                    {
                        if (++i >= brailles.Length)
                        {
                            result.Append('：');
                            break;
                        }
                        if (brailles[i] == '⠂')
                            result.Append('》');
                        else
                        {
                            i--;
                            result.Append('：');
                        }
                    }
                    else if (brailles[i] == '⠘')
                    {
                        if (++i >= brailles.Length)
                        {
                            result.Append((dQuotesFlag ^= true) ? '“' : '”');
                            break;
                        }
                        if (brailles[i] == '⠘')
                            result.Append((sQuotesFlag ^= true) ? '‘' : '’');
                        else
                        {
                            i--;
                            result.Append((dQuotesFlag ^= true) ? '“' : '”');
                        }
                    }
                    else if (brailles[i] == '⠠')
                    {
                        if (++i >= brailles.Length)
                            break;
                        switch (brailles[i])
                        {
                            case '⠆':
                                result.Append('）');
                                break;
                            case '⠤':
                                result.Append('—');
                                break;
                            case '⠂':
                                result.Append('〉');
                                break;
                        }
                    }
                    else if (braillesInverse.TryGetValue(brailles[i], out string? value))
                        result.Append(value);
                }
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// 还原部分可选的拼音
    /// </summary>
    /// <remarks>
    /// <see cref="DistinguishThird"/><br/>
    /// <see cref="UseAbbreviations"/><br/>
    /// <see cref="EncodeTonenote"/><br/>
    /// 必须都为<see langword="false"/>
    /// </remarks>
    public static string ResolvePinyins(string text)
    {
        Guard.IsFalse(DistinguishThird);
        Guard.IsFalse(UseAbbreviations);
        Guard.IsTrue(EncodeTonenote);

        var alternate = AlternateRegex();
        return PinyinResolveRegex().Replace(text, match =>
        {
            if (match.Groups["sRhyme"].Success)
                return alternate.Replace(match.Value, "${S}");
            if (match.Groups["Vowel2"].Success)
                return match.Value.Replace("(I)", string.Empty);
            if (match.Groups["sVowel"].Success)
                return match.Value.Replace("(I)", "I");
            if (match.Groups["Vowel"].Success)
            {
                var m = alternate.Match(match.Value);
                var rhyme = match.Value[(m.Index + m.Length)..];
                if (rhyme[0] == 'I' || rhyme[0] == 'V')
                    return m.Groups["S"].Value + rhyme;
                if (rhyme[0] == 'A' || rhyme[0] == 'E')
                    return m.Groups["F"].Value + rhyme;
                return match.Value;
            }
            if (match.Groups["Pinyin"].Success)
            {
                var m = alternate.Matches(match.Value);
                var rhyme = m[1].Groups["F"].Value;
                string vowel = m[0].Value;
                if (rhyme[0] == 'I' || rhyme[0] == 'V')
                    vowel = m[0].Groups["S"].Value;
                else if (rhyme[0] == 'A' || rhyme[0] == 'E')
                    vowel = m[0].Groups["F"].Value;
                return vowel + rhyme + match.Value[^1];
            }
            if (match.Groups["Pinyin2"].Success)
            {
                var m = alternate.Match(match.Value);
                var vowel = match.Value[..m.Index].Replace("(I)", "");
                var rhyme = m.Groups["F"].Value;
                return vowel + rhyme + match.Value[^1];
            }
            return string.Empty;
        });
    }

    /// <summary>
    /// 编码国家通用的盲文
    /// </summary>
    /// <remarks>
    /// 对于多音字，无法得知具体使用哪个读音，可以用括号指定<br/>
    /// 例如:<c>说(shuo1)</c><br/>
    /// 如不指定，将使用默认读音
    /// </remarks>
    /// <param name="text">中文文字</param>
    /// <param name="outPinyins">每个字使用的拼音序列</param>
    public static string Encode(string text, out string? outPinyins)
    {
        var pinyinString = OutputPinyins ? new StringBuilder() : null;
        if (AutoSimplify)
            text = ToSimplified(text);
        var result = new StringBuilder();
        IEnumerable<Match> chineseChars = ChineseCharWithPinyin().Matches(text);
        foreach (var match in chineseChars)
        {
            var group = match.Groups["Punctuations"];
            if (group.Success)
            {
                if (EncodePunctuation && punctuations.TryGetValue(group.Value[0], out string? value))
                    result.Append(value);
                continue;
            }
            group = match.Groups["Numbers"];
            if (group.Success)
            {
                if (EncodeNumber)
                {
                    result.Append(NUMBER_SIGN);
                    foreach (var number in group.Value)
                        result.Append(letters[number.VDigitsNumber()]);
                    result.Append(' ');
                }
                continue;
            }
            group = match.Groups["LLetters"];
            if (group.Success)
            {
                if (EncodeLetters)
                {
                    result.Append(LLETTER_SIGN);
                    foreach (var letter in group.Value)
                        result.Append(letters[letter.LetterIndex()]);
                    result.Append(' ');
                }
                continue;
            }
            group = match.Groups["ULetters"];
            if (group.Success)
            {
                if (EncodeLetters)
                {
                    result.Append(ULETTER_SIGN);
                    foreach (var letter in group.Value)
                        result.Append(letters[letter.LetterIndex()]);
                    result.Append(' ');
                }
                continue;
            }

            var character = match.Value[0];
            if (DistinguishThird && EncodeTonenote)
            {
                if (character == '他')
                {
                    result.Append("⠞⠔");
                    continue;
                }
                else if (character == '它')
                {
                    result.Append("⠈⠞⠔");
                    continue;
                }
            }
            var defaultPinYin = GetDefaultPinyin(character);
            int tonenote = 0;
            group = match.Groups["Pinyin"];
            string pinYin = group.Success ? ParsePinyin(group.Value) : defaultPinYin[..^1];
            if (EncodeTonenote)
            {
                group = match.Groups["Tonenote"];
                tonenote = (group.Success ? group.Value[0] : defaultPinYin[^1]).Base36Number();
            }
            if (pinyinString is not null)
            {
                pinyinString.Append(character).Append('(').Append(pinYin);
                if (EncodeTonenote)
                    pinyinString.Append(tonenote);
                pinyinString.Append(')');
            }
            result.AppendBraille(pinYin, tonenote);
        }
        outPinyins = pinyinString?.ToString();
        return result.ToString();
    }

    private static void AppendBraille(this StringBuilder builder, string pinYin, int tonenote)
    {
        if (singleVowels.ContainsKey(pinYin))
            pinYin = singleVowels[pinYin];
        if (singleRhymes.ContainsKey(pinYin))
            pinYin = singleRhymes[pinYin];
        if (EncodePunctuation && UseAbbreviations)
            ResolveTonenote(pinYin, ref tonenote);
        if (brailles.TryGetValue(pinYin, out char value))
            builder.Append(value);
        else if (pinYin[1] == 'H')
        {
            builder.Append(brailles[pinYin[..2]]);
            builder.Append(brailles[pinYin[2..]]);
        }
        else
        {
            builder.Append(brailles[pinYin[..1]]);
            builder.Append(brailles[pinYin[1..]]);
        }
        if (tonenote > 0 && tonenote <= 4)
            builder.Append(TONE_NOTES[tonenote - 1]);
    }

    private static void ResolveTonenote(string pinYin, ref int tonenote)
    {
        char firstCharacter = pinYin[0];
        if (firstCharacter == 'F' && tonenote == 1)
            tonenote = 0;
        else if ("PMTNHQCR".Contains(firstCharacter) && tonenote == 2 && pinYin != "TOU")
            tonenote = 0;
        else if ("BDLGKJXZS".Contains(firstCharacter) && tonenote == 4 && pinYin != "LE" && pinYin != "ZI")
            tonenote = 0;
        else if (singleRhymes.ContainsKey(pinYin) && tonenote == 4 && pinYin != "YI" && pinYin != "ER"
            && pinYin != "WO" && pinYin != "YE" && pinYin != "YOU" && pinYin != "E")
            tonenote = 0;
        else if ((pinYin == "YI" && tonenote == 1) || (pinYin == "ER" && tonenote == 2) ||
            (tonenote == 3 && (pinYin == "WO" || pinYin == "YE" || pinYin == "YOU")))
            tonenote = 0;
        else if (pinYin == "O")
            tonenote = 0;
    }

    [GeneratedRegex(@"(?<Punctuations>[。，、；？！：“”‘’（）【】—…《》〈〉])|(?<LLetters>[a-z]+)|(?<ULetters>[A-Z]+)|(?<Numbers>[0-9]+)|(\p{IsCJKUnifiedIdeographs}(\((?<Pinyin>[A-Za-zāáǎàōóǒòēéěèīíǐìūúǔùǖǘǚǜ]{1,6})(?<Tonenote>[1-4])?\))?)")]
    private static partial Regex ChineseCharWithPinyin();

    [GeneratedRegex("(⠠(?<ULetters>[⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵]+) )|(⠰(?<LLetters>[⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵]+) )|(⠼(?<Numbers>[⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚]+) )|(?<Others>(⠠[⠆⠤⠂])|(⠰[⠂⠄⠆])|[^⠠⠰⠼]+)")]
    private static partial Regex ChineseCommonBrailleRegex();

    [GeneratedRegex(@"((?<![A-Z\]\)])(?<sRhyme>\[[A-Z]+/[A-Z]+\])[1-4])|(?<Pinyin>(\[[A-Z]+/[A-Z]+\]){2})[1-4]|(?<Vowel>\[[A-Z]+/[A-Z]+\][A-Z]+)[1-4]|(?<sVowel>[A-Z]+\(I\))[1-4]|(?<Vowel2>[A-Z]+\(I\)[A-Z]+)[1-4]|(?<Pinyin2>[A-Z]+\(I\)\[[A-Z]+/[A-Z]+\])[1-4]")]
    private static partial Regex PinyinResolveRegex();

    [GeneratedRegex(@"\[(?<F>[A-Z]+)/(?<S>[A-Z]+)\]")]
    private static partial Regex AlternateRegex();
}
