using static ClassicalCryptography.Encoder.Chinese.ChineseCommonBraille.Helper;
using static ClassicalCryptography.Encoder.Chinese.ChineseHelper;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// <see href="http://www.moe.gov.cn/jyb_sjzl/ziliao/A19/201807/W020180725666187054299.pdf">国家通用盲文方案</see>
/// </summary>
/// <remarks>
/// 程序中可接受的中文汉字范围为[〇\u4E00-\u9FA5\uE81A-\uE863]<br/>
/// </remarks>
public partial class ChineseCommonBraille
{
    /// <summary>
    /// 默认配置(除<see cref="Settings.CheckPinyin"/>，<see cref="Settings.AutoSimplify"/>和<see cref="Settings.OutputPinyins"/>都为<see langword="true"/>)
    /// </summary>
    public static readonly ChineseCommonBraille Default = new(Settings.FromCode(126));

    /// <summary>
    /// 输出拼音配置(除<see cref="Settings.CheckPinyin"/>，<see cref="Settings.AutoSimplify"/>都为<see langword="true"/>)
    /// </summary>
    public static readonly ChineseCommonBraille OutputPinyin = new(Settings.FromCode(127));

    /// <summary>
    /// 可解码配置(除<see cref="Settings.CheckPinyin"/>，<see cref="Settings.AutoSimplify"/>,<see cref="Settings.DistinguishThird"/>和<see cref="Settings.UseAbbreviations"/>都为<see langword="true"/>)
    /// </summary>
    public static readonly ChineseCommonBraille Decodeable = new(Settings.FromCode(121));

    /// <summary>
    /// 默认配置(除<see cref="Settings.AutoSimplify"/>和<see cref="Settings.OutputPinyins"/>都为<see langword="true"/>)
    /// </summary>
    public static readonly ChineseCommonBraille CheckPinyin = new(Settings.FromCode(382));

    internal Settings settings;

    /// <summary>
    /// 使用指定的选项编码盲文
    /// </summary>
    public ChineseCommonBraille(Settings settings) => this.settings = settings;

    /// <inheritdoc cref="Encode(string, out string?)"/>
    public string Encode(string text) => Encode(text, out _);

    /// <summary>
    /// 编码国家通用的盲文
    /// </summary>
    /// <remarks>
    /// 对于多音字，无法得知具体使用哪个读音(必须包括声调)，可以用括号指定<br/>
    /// 例如说(SHUO1)，说(shuo1)或者说(shuō)<br/>
    /// 如不指定，将使用默认读音<br/>
    /// <seealso cref="GetDefaultPinyin"/>
    /// </remarks>
    /// <param name="text">中文文字</param>
    /// <param name="outPinyins">每个字使用的拼音序列</param>
    public string Encode(string text, out string? outPinyins)
    {
        if (settings.AutoSimplify)
            text = ToSimplified(text);
        IList<Match> chineseChars = ChineseCharWithPinyin().Matches(text);
        var result = new StringBuilder(chineseChars.Count * 2);
        StringBuilder? pinyinString = null;
        HashSet<char>? hashset = null;
        if (settings.OutputPinyins)
        {
            pinyinString = new StringBuilder(chineseChars.Count * 4);
            hashset = new HashSet<char>();
        }
        foreach (var match in chineseChars)
        {
            Group group;
            if ((group = match.Groups["Punctuations"]).Success)
            {
                if (settings.EncodePunctuation && punctuations.TryGetValue(group.ValueSpan[0], out var value))
                    result.Append(value);
                continue;
            }
            if ((group = match.Groups["Numbers"]).Success)
            {
                if (settings.EncodeNumber)
                {
                    result.Append(NUMBER_SIGN);
                    foreach (var number in group.ValueSpan)
                        result.Append(letters[number.DigitsOneFirstNumber()]);
                    result.Append(' ');
                }
                continue;
            }
            if ((group = match.Groups["LLetters"]).Success)
            {
                if (settings.EncodeLetters)
                {
                    result.Append(LLETTER_SIGN);
                    foreach (var letter in group.ValueSpan)
                        result.Append(letters[letter.LetterIndex()]);
                    result.Append(' ');
                }
                continue;
            }
            if ((group = match.Groups["ULetters"]).Success)
            {
                if (settings.EncodeLetters)
                {
                    result.Append(ULETTER_SIGN);
                    foreach (var letter in group.ValueSpan)
                        result.Append(letters[letter.LetterIndex()]);
                    result.Append(' ');
                }
                continue;
            }

            var character = match.ValueSpan[0];
            if (settings.DistinguishThird && settings.EncodeTonenote)
            {
                switch (character)
                {
                    case '他':
                        result.Append("⠞⠔");
                        continue;
                    case '它':
                        result.Append("⠈⠞⠔");
                        continue;
                    case '她':
                        result.Append("⠞⠔⠁");
                        continue;
                }
            }

            string pinYin;
            int tonenote = 0;
            if ((group = match.Groups["Pinyin"]).Success)
            {
                pinYin = ParsePinyin(group.Value);
                if (char.IsAsciiDigit(pinYin[^1]))
                {
                    if (settings.EncodeTonenote)
                    {
                        if (settings.CheckPinyin && !IsPinyinOf(pinYin, character))
                            throw new ArgumentException($"`{pinYin}`不是字符`{character}`的读音", nameof(text));
                        tonenote = pinYin[^1].Base36Number();
                    }
                    pinYin = pinYin[..^1];
                }
                else if (settings.EncodeTonenote)
                    throw new ArgumentException($"字符`{character}`没有指定读音的声调", nameof(text));
            }
            else
            {
                pinYin = GetDefaultPinyin(character);
                if (settings.EncodeTonenote)
                    tonenote = pinYin[^1].Base36Number();
                pinYin = pinYin[..^1];
            }

            if (settings.OutputPinyins && hashset!.Add(character))
            {
                pinyinString!.Append(character).Append('(').Append(pinYin);
                if (settings.EncodeTonenote)
                    pinyinString.Append(tonenote);
                pinyinString.Append(')');
            }
            AppendBraille(result, pinYin, tonenote);
        }
        outPinyins = pinyinString?.ToString();
        return result.ToString();
    }

    /// <summary>
    /// 解码国家通用的盲文为可能的拼音或字母，数字和符号
    /// </summary>
    /// <param name="brailleText">盲文文本</param>
    /// <remarks>
    /// <see cref="Settings.CanDecodePinyin"/>需为<see langword="true"/><br/>
    /// 由于`？`和`〈`的盲文相同，解码时无法正确识别，都会解码为`？`<br/>
    /// 对于`“”‘’【】`成对的符号，则分别执行左右左右的循环，需自行校对<br/>
    /// `⠁⠂⠄⠆`所代表的声调会被解码为数字1234<br/>
    /// 单独的`⠠`或者其他不能解码的符号或情况将被忽略
    /// </remarks>
    public string DecodePinyins(string brailleText)
    {
        Guard.IsTrue(settings.CanDecodePinyin);

        bool bracketsFlag = false;
        bool dQuotesFlag = false;
        bool sQuotesFlag = false;
        IList<Match> matches = ChineseCommonBrailleRegex().Matches(brailleText);
        var result = new StringBuilder(matches.Count * 4);
        foreach (var match in matches)
        {
            Group group;
            if ((group = match.Groups["Others"]).Success)
            {
                var brailles = group.ValueSpan;
                for (int i = 0; i < brailles.Length; i++)
                {
                    switch (brailles[i])
                    {
                        case '⠈':
                            result.Append('、');
                            break;
                        case '⠐':
                            AppendCase1(result, brailles, ref i);
                            break;
                        case '⠰':
                            AppendCase2(result, brailles, ref i, ref bracketsFlag);
                            break;
                        case '⠤':
                            AppendCase3(result, brailles, ref i);
                            break;
                        case '⠘':
                            AppendCase4(result, brailles, ref i, ref dQuotesFlag, ref sQuotesFlag);
                            break;
                        case '⠠':
                            AppendCase5(result, brailles, ref i);
                            break;
                        default:
                            if (braillesInverse.TryGetValue(brailles[i], out var value))
                                result.Append(value);
                            break;
                    }
                }
                continue;
            }

            string table;
            if ((group = match.Groups["ULetters"]).Success)
                table = U_Letters;
            else if ((group = match.Groups["LLetters"]).Success)
                table = L_Letters;
            else if ((group = match.Groups["Numbers"]).Success)
                table = DigitsOneFirst;
            else continue;

            foreach (var number in group.ValueSpan)
                result.Append(table[letters.IndexOf(number)]);
        }
        return result.ToString();
    }

    /// <summary>
    /// 还原部分可选的拼音，例如[UI/WEI]和(I)
    /// </summary>
    /// <remarks>
    /// <see cref="Settings.CanResolvePinyin"/>需为<see langword="true"/><br/>
    /// 由于轻声声调在盲文中事实上会被忽略，所以还原会受到干扰
    /// </remarks>
    public string ResolvePinyins(string text)
    {
        Guard.IsTrue(settings.CanResolvePinyin);
        return PinyinResolveRegex().Replace(text, ResolvePinyinInternal);
    }

    private void AppendBraille(StringBuilder builder, string pinYin, int tonenote)
    {
        if (SingleVowels.TryGetValue(pinYin, out string py))
            pinYin = py;
        if (SingleRhymes.TryGetValue(pinYin, out py))
            pinYin = py;
        if (settings.UseAbbreviations && settings.EncodePunctuation)
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
            builder.Append(toneNotes[tonenote - 1]);
    }
}
