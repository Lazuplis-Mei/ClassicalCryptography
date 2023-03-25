using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// <a href="http://www.moe.gov.cn/jyb_sjzl/ziliao/A19/201807/W020180725666187054299.pdf">国家通用盲文方案</a>
/// </summary>
public static partial class ChineseCommonBraille
{

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
        { "IOU", '⠳' },
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
        { "UEN", '⠒' },
        { "ONG", '⠲' },{ "UENG", '⠲' },
        { "VAN", '⠯' },
        { "VN", '⠸' },
        { "IONG", '⠹' },
    };

    private static readonly Dictionary<string, string> special = new()
    {
        { "ZHI", "ZH" },
        { "CHI", "CH" },
        { "SHI", "SH" },
        { "RI", "R" },
        { "ZI", "Z" },
        { "CI", "C" },
        { "SI", "S" },
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
        { '？', "⠐⠄" },
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
        { '〈', "⠐⠄" },
        { '〉', "⠠⠂" },
    };

    private static readonly string letters = "⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵";
    private const string Tonenotes = "⠁⠂⠄⠆";

    private const char ULETTER_SIGN = '⠠';
    private const char LLETTER_SIGN = '⠰';
    private const char NUMBER_SIGN = '⠼';

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
    /// 指示应当区分他它她(<see cref="EncodePunctuation"/>必须为<see langword="true"/>)
    /// </summary>
    public static bool DistinguishThird { get; set; } = true;

    /// <summary>
    /// 指示是否在编码过程中记录拼音作为输出参数<br/>
    /// 如果该项被设置为<see langword="false"/>，则<see cref="Encode(string, out string?)"/>的参数2不起作用
    /// </summary>
    public static bool OutputPinyins { get; set; } = false;

    /// <inheritdoc cref="Encode(string, out string?)"/>
    public static string Encode(string text)
    {
        return Encode(text, out _);
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
        outPinyins = null;
        StringBuilder? pinyins = null;
        if (OutputPinyins)
            pinyins = new StringBuilder();
        if (AutoSimplify)
            text = ChineseConverter.Convert(text, ChineseConversionDirection.TraditionalToSimplified);
        var result = new StringBuilder();
        IEnumerable<Match> chineseChars = ChineseCharWithPinyin().Matches(text);
        foreach (var match in chineseChars)
        {
            var group = match.Groups["Punctuations"];
            if (group.Success)
            {
                if (EncodePunctuation)
                {
                    if (punctuations.TryGetValue(group.Value[0], out string? value))
                        result.Append(value);
                }
                continue;
            }
            group = match.Groups["Numbers"];
            if (group.Success)
            {
                if (EncodeNumber)
                {
                    result.Append(NUMBER_SIGN);
                    foreach (var number in group.Value)
                    {
                        result.Append(letters[number.Base36Number()]);
                    }
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
                    {
                        result.Append(letters[letter - 'a']);
                    }
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
                    {
                        result.Append(letters[letter - 'A']);
                    }
                }
                continue;
            }

            var chineseChar = new ChineseChar(match.Value[0]);
            var defaultPinYin = chineseChar.Pinyins[0];
            string pinYin;
            int tonenote = 0;

            group = match.Groups["Pinyin"];
            if (group.Success)
                pinYin = group.Value.ToUpper();
            else
                pinYin = defaultPinYin[..^1];

            if (EncodeTonenote)
            {
                if (DistinguishThird)
                {
                    if (chineseChar.ChineseCharacter == '他')
                    {
                        result.Append("⠞⠔");
                        continue;
                    }
                    else if (chineseChar.ChineseCharacter == '它')
                    {
                        result.Append("⠈⠞⠔");
                        continue;
                    }
                }
                group = match.Groups["Tonenote"];
                if (group.Success)
                    tonenote = group.Value[0].Base36Number();
                else
                    tonenote = defaultPinYin[^1].Base36Number();
            }
            if (OutputPinyins)
            {
                pinyins!.Append(chineseChar.ChineseCharacter);
                pinyins!.Append('(').Append(pinYin);
                if (EncodeTonenote)
                    pinyins!.Append(tonenote);
                pinyins!.Append(')');
            }
            GetBrailleCode(pinYin, tonenote, result);
        }
        if (OutputPinyins)
            outPinyins = pinyins!.ToString();
        return result.ToString();
    }

    private static void GetBrailleCode(string pinYin, int tonenote, StringBuilder stringBuilder)
    {
        if (special.ContainsKey(pinYin))
            pinYin = special[pinYin];

        if (brailles.TryGetValue(pinYin, out char value))
            stringBuilder.Append(value);
        else
        {
            if (pinYin[1] == 'H')
            {
                stringBuilder.Append(brailles[pinYin[..2]]);
                stringBuilder.Append(brailles[pinYin[2..]]);
            }
            else
            {
                stringBuilder.Append(brailles[pinYin[0].ToString()]);
                stringBuilder.Append(brailles[pinYin[1..]]);
            }
        }
        if (tonenote > 0 && tonenote <= 4)
            stringBuilder.Append(Tonenotes[tonenote - 1]);
    }

    [GeneratedRegex(@"(?<Punctuations>[。，、；？！：“”‘’（）【】—…《》〈〉])|(?<LLetters>[a-z]+)|(?<ULetters>[A-Z]+)|(?<Numbers>[0-9]+)|(\p{IsCJKUnifiedIdeographs}(\((?<Pinyin>[A-Za-z]{1,6})(?<Tonenote>[1-4])?\))?)")]
    private static partial Regex ChineseCharWithPinyin();
}
