namespace ClassicalCryptography.Encoder.Japanese;
using System.Collections.ObjectModel;
using SR = SingleReplacementCipher;
using SD = BidirectionalDictionary<char, char>;

/// <summary>
/// 半角片假名
/// </summary>
public static class JapaneseHelper
{

    #region Data

    internal static readonly SR LU_Hankana = new("ｧｨｩｪｫｬｭｮｯ", "ｱｲｳｴｵﾔﾕﾖﾂ");
    internal static readonly SR LU_Katakana = new("ァィゥェォッャュョヮヵヶ", "アイウエオツヤユヨワカケ");
    internal static readonly SR LU_Hiragana = new("ぁぃぅぇぉっゃゅょゎゕゖ", "あいうえおつやゆよわかけ");
    internal static readonly SR Han_Katakana = new("ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝﾞﾟ",
        "ヲァィゥェォャュョッーアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘ ホマミムメモヤユヨラリルレロワン゛゜");
    internal static readonly SR Hiragana_Katakana = new(
        "ぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもゃやゅゆょよらりるれろゎわゐゑをんゔゕゖゝゞ",
        "ァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶヽヾ");

    internal readonly static SD Hiragana_Handakuten = BuildDictionary("ぱぴぷぺぽ", "はひふへほ");
    internal readonly static SD Hiragana_Dakuten = BuildDictionary("がぎぐげござじずぜぞだぢづでどばびぶべぼゔ", "かきくけこさしすせそたちつてとはひふへほう");
    internal readonly static SD Katakana_Handakuten = BuildDictionary("パピプペポ", "ハヒフヘホ");
    internal readonly static SD Katakana_Dakuten = BuildDictionary("ガギグゲゴザジズゼゾダヂヅデドバビブベボヴ", "カキクケコサシスセソタチツテトハヒフヘホウ");

    /// <summary>
    /// 片假名发音
    /// </summary>
    public static readonly ReadOnlyBidirectionalDictionary<string, string> KatakanaPronunciations = new(new BidirectionalDictionary<string, string>()
    {
        { "ア", "A"  },{ "イ", "I"  },{ "ウ", "U"  },{ "エ", "E"  },{ "オ", "O"  },{ "ン", "N" },
        { "カ", "KA" },{ "キ", "KI" },{ "ク", "KU" },{ "ケ", "KE" },{ "コ", "KO" },
        { "サ", "SA" },{ "シ", "SHI"},{ "ス", "SU" },{ "セ", "SE" },{ "ソ", "SO" },
        { "タ", "TA" },{ "チ", "CHI"},{ "ツ", "TSU"},{ "テ", "TE" },{ "ト", "TO" },
        { "ナ", "NA" },{ "ニ", "NI" },{ "ヌ", "NU" },{ "ネ", "NE" },{ "ノ", "NO" },
        { "ハ", "HA" },{ "ヒ", "HI" },{ "フ", "HU" },{ "ヘ", "HE" },{ "ホ", "HO" },
        { "マ", "MA" },{ "ミ", "MI" },{ "ム", "MU" },{ "メ", "ME" },{ "モ", "MO" },
        { "ヤ", "YA" },               { "ユ", "YU" },               { "ヨ", "YO" },
        { "ラ", "RA" },{ "リ", "RI" },{ "ル", "RU" },{ "レ", "RE" },{ "ロ", "RO" },
        { "ワ", "WA" },{ "ヰ", "WI" },               { "ヱ", "WE" },{ "ヲ", "WO" },

        { "キャ", "KYA" },{ "キュ", "KYU" },{ "キョ", "KYO" },
        { "シャ", "SHA" },{ "シュ", "SHU" },{ "ショ", "SHO" },
        { "チャ", "CHA" },{ "チュ", "CHU" },{ "チョ", "CHO" },
        { "ニャ", "NYA" },{ "ニュ", "NYU" },{ "ニョ", "NYO" },
        { "ヒャ", "HYA" },{ "ヒュ", "HYU" },{ "ヒョ", "HYO" },
        { "ミャ", "MYA" },{ "ミュ", "MYU" },{ "ミョ", "MYO" },
    });

    private const char HANDAKUTEN = '゜';
    private const char DAKUTEN = '゛';
    private const char HIRAGANA_ITERATION = 'ゝ';
    private const char HIRAGANA_ITERATION_DAKUTEN = 'ゞ';
    private const char KATAKANA_ITERATION = 'ヽ';
    private const char KATAKANA_ITERATION_DAKUTEN = 'ヾ';
    private const char CHARACTER_ITERATION = '々';

    #endregion

    /// <summary>
    /// 假名是否含有浊点
    /// </summary>
    public static bool HasDakuten(char character)
    {
        return HiraganaHasDakuten(character) || KatakanaHasDakuten(character);
    }

    /// <summary>
    /// 平假名是否含有浊点
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HiraganaHasDakuten(char character)
    {
        return Hiragana_Handakuten.ContainsKey(character) || Hiragana_Dakuten.ContainsKey(character);
    }

    /// <summary>
    /// 片假名是否含有浊点
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool KatakanaHasDakuten(char character)
    {
        return Katakana_Handakuten.ContainsKey(character) || Katakana_Dakuten.ContainsKey(character);
    }

    /// <summary>
    /// 半角片假名(小)转换成半角片假名(大)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string HankanaToUpper(string text) => LU_Hankana.Encrypt(text);

    /// <summary>
    /// 片假名(小)转换成片假名(大)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string KatakanaToUpper(string text) => LU_Katakana.Encrypt(text);

    /// <summary>
    /// 平假名(小)转换成平假名(大)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string HiraganaToUpper(string text) => LU_Hiragana.Encrypt(text);

    /// <summary>
    /// 半角片假名转换成片假名
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string HankanaToKatakana(string text) => Han_Katakana.Encrypt(text);

    /// <summary>
    /// 平假名转换成片假名
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string HiraganaToKatakana(string text) => Hiragana_Katakana.Encrypt(text);

    /// <summary>
    /// 片假名转换成平假名
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string KatakanaToHiragana(string text) => Hiragana_Katakana.Decrypt(text);

    /// <summary>
    /// 将浊点和假名分离
    /// </summary>
    public static string ExpandDakuten(string text)
    {
        var result = new StringBuilder(text.Length + 16);
        foreach (var character in text)
        {
            if (Hiragana_Handakuten.TryGetValue(character, out char value))
                result.Append(value).Append(HANDAKUTEN);
            else if (Katakana_Handakuten.TryGetValue(character, out value))
                result.Append(value).Append(HANDAKUTEN);
            else if (Hiragana_Dakuten.TryGetValue(character, out value))
                result.Append(value).Append(DAKUTEN);
            else if (Katakana_Dakuten.TryGetValue(character, out value))
                result.Append(value).Append(DAKUTEN);
            else
                result.Append(character);
        }
        return result.ToString();
    }

    /// <summary>
    /// 将浊点和假名合并
    /// </summary>
    [SkipLocalsInit]
    public static string ReduceDakuten(string text)
    {
        int count = text.Length;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        int index = 0;
        for (int i = 0; i < count; i++)
        {
            var character = text[i];
            if (i == count - 1)
            {
                span[index++] = character;
                break;
            }
            switch (text[i + 1])
            {
                case HANDAKUTEN when Hiragana_Handakuten.Inverse.TryGetValue(character, out char value):
                case HANDAKUTEN when Katakana_Handakuten.Inverse.TryGetValue(character, out value):
                case DAKUTEN when Hiragana_Dakuten.Inverse.TryGetValue(character, out value):
                case DAKUTEN when Katakana_Dakuten.Inverse.TryGetValue(character, out value):
                    span[index++] = value;
                    i++;
                    break;
                default:
                    span[index++] = character;
                    break;
            }
        }
        return new(span[..index]);
    }

    /// <summary>
    /// 将叠字符号恢复成原本的字符
    /// </summary>
    [SkipLocalsInit]
    public static string RecoverIteration(string text)
    {
        int count = text.Length;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        span[0] = text[0];
        for (int i = 1; i < count; i++)
        {
            var privious = text[i - 1];
            switch (text[i])
            {
                case HIRAGANA_ITERATION when Hiragana_Katakana.IsVaildChar(privious):
                    if (Hiragana_Handakuten.TryGetValue(privious, out char value))
                        span[i] = value;
                    else if (Hiragana_Dakuten.TryGetValue(privious, out value))
                        span[i] = value;
                    else
                        span[i] = privious;
                    break;
                case HIRAGANA_ITERATION_DAKUTEN when Hiragana_Katakana.IsVaildChar(privious):
                    if (HiraganaHasDakuten(privious))
                        span[i] = privious;
                    else if (Hiragana_Dakuten.Inverse.TryGetValue(privious, out value))
                        span[i] = value;
                    break;
                case KATAKANA_ITERATION when Hiragana_Katakana.IsVaildCharInverse(privious):
                    if (Katakana_Handakuten.TryGetValue(privious, out value))
                        span[i] = value;
                    else if (Katakana_Dakuten.TryGetValue(privious, out value))
                        span[i] = value;
                    else
                        span[i] = privious;
                    break;
                case KATAKANA_ITERATION_DAKUTEN when Hiragana_Katakana.IsVaildCharInverse(privious):
                    if (KatakanaHasDakuten(privious))
                        span[i] = privious;
                    else if (Katakana_Dakuten.Inverse.TryGetValue(privious, out value))
                        span[i] = value;
                    break;
                case CHARACTER_ITERATION when privious.IsCjkUnifiedIdeographs():
                    span[i] = privious;
                    break;
                default:
                    span[i] = text[i];
                    break;
            }
        }
        return new(span);
    }

    /// <summary>
    /// 使用叠字符号代替相邻的重复字符
    /// </summary>
    [SkipLocalsInit]
    public static string ReduceIteration(string text)
    {
        int count = text.Length;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        span[0] = text[0];
        for (int i = 1; i < count; i++)
        {
            var character = text[i];
            var privious = text[i - 1];
            if (Hiragana_Handakuten.TryGetValue(character, out char value))
            {
                if (privious == value || privious == character || privious == Hiragana_Dakuten.Inverse[value])
                    span[i] = HIRAGANA_ITERATION_DAKUTEN;
                else
                    span[i] = character;
            }
            else if (Hiragana_Dakuten.TryGetValue(character, out value))
            {
                if (privious == value || privious == character || privious == Hiragana_Handakuten.Inverse[value])
                    span[i] = HIRAGANA_ITERATION_DAKUTEN;
                else
                    span[i] = character;
            }
            else if (Katakana_Handakuten.TryGetValue(character, out value))
            {
                if (privious == value || privious == character || privious == Katakana_Dakuten.Inverse[value])
                    span[i] = KATAKANA_ITERATION_DAKUTEN;
                else
                    span[i] = character;
            }
            else if (Katakana_Dakuten.TryGetValue(character, out value))
            {
                if (privious == value || privious == character || privious == Katakana_Handakuten.Inverse[value])
                    span[i] = KATAKANA_ITERATION_DAKUTEN;
                else
                    span[i] = character;
            }
            else if (Hiragana_Dakuten.Inverse.TryGetValue(character, out value))
            {
                if (privious == value || privious == character)
                    span[i] = HIRAGANA_ITERATION;
                else if (Hiragana_Handakuten.Inverse.TryGetValue(character, out value))
                    span[i] = privious == value ? HIRAGANA_ITERATION : character;
                else
                    span[i] = character;
            }
            else if (Katakana_Dakuten.Inverse.TryGetValue(character, out value))
            {
                if (privious == value || privious == character)
                    span[i] = KATAKANA_ITERATION;
                else if (Katakana_Handakuten.Inverse.TryGetValue(character, out value))
                    span[i] = privious == value ? HIRAGANA_ITERATION : character;
                else
                    span[i] = character;
            }
            else if (character.IsCjkUnifiedIdeographs() && privious == character)
                span[i] = CHARACTER_ITERATION;
            else
                span[i] = character;
        }
        return new(span);
    }
}