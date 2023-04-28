using static ClassicalCryptography.Encoder.Chinese.ChineseHelper;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// 中文拼音
/// </summary>
public readonly record struct ChinesePinyin(ChineseVowel Vowel, ChineseRhyme Rhyme, ChineseToneNote ToneNote)
{
    private static readonly BidirectionalDictionary<string, string> zhuYins = new()
    {
        { "None", "" },
        { "B", "ㄅ" },
        { "P", "ㄆ" },
        { "M", "ㄇ" },
        { "F", "ㄈ" },
        { "D", "ㄉ" },
        { "T", "ㄊ" },
        { "N", "ㄋ" },
        { "L", "ㄌ" },
        { "G", "ㄍ" },
        { "K", "ㄎ" },
        { "H", "ㄏ" },
        { "J", "ㄐ" },
        { "Q", "ㄑ" },
        { "X", "ㄒ" },
        { "ZH", "ㄓ" },
        { "CH", "ㄔ" },
        { "SH", "ㄕ" },
        { "R", "ㄖ" },
        { "Z", "ㄗ" },
        { "C", "ㄘ" },
        { "S", "ㄙ" },

        { "A", "ㄚ" },
        { "O", "ㄛ" },
        { "E", "ㄜ" },
        { "I", "ㄧ" },
        { "U", "ㄨ" },
        { "V", "ㄩ" },
        { "ER", "ㄦ" },
        { "AI", "ㄞ" },
        { "AO", "ㄠ" },
        { "EI", "ㄟ" },
        { "OU", "ㄡ" },
        { "IA", "ㄧㄚ" },
        { "IAO", "ㄧㄠ" },
        { "IE", "ㄧㄝ" },
        { ChineseRhyme.IOU.ToString(), "ㄧㄡ" },
        { "UA", "ㄨㄚ" },
        { "UAI", "ㄨㄞ" },
        { ChineseRhyme.UEI.ToString(), "ㄨㄟ" },
        { "UO", "ㄨㄛ" },
        { "VE", "ㄩㄝ" },
        { "AN", "ㄢ" },
        { "ANG", "ㄤ" },
        { "EN", "ㄣ" },
        { "ENG", "ㄥ" },
        { "IAN", "ㄧㄢ" },
        { "IANG", "ㄧㄤ" },
        { "IN", "ㄧㄣ" },
        { "ING", "ㄧㄥ" },
        { "UAN", "ㄨㄢ" },
        { "UANG", "ㄨㄤ" },
        { ChineseRhyme.UEN.ToString(), "ㄨㄣ" },
        { ChineseRhyme.ONG.ToString(), "ㄨㄥ" },
        { "VAN", "ㄩㄢ" },
        { "VN", "ㄩㄣ" },
        { "IONG", "ㄩㄥ" },
    };

    /// <summary>
    /// 从字符串解析拼音
    /// </summary>
    public static ChinesePinyin Parse(string pinyin)
    {
        int tonenote = 0;
        pinyin = ParsePinyin(pinyin);
        if (char.IsAsciiDigit(pinyin[^1]))
        {
            tonenote = pinyin[^1].Base36Number();
            pinyin = pinyin[..^1];
        }
        var vowel = ChineseVowel.None;
        var rhyme = ChineseRhyme.None;
        if (SingleVowels.TryGetValue(pinyin, out string py))
            vowel = Enum.Parse<ChineseVowel>(py);
        else if (SingleRhymes.TryGetValue(pinyin, out py))
            rhyme = Enum.Parse<ChineseRhyme>(py);
        else if (pinyin[1] == 'H')
        {
            vowel = Enum.Parse<ChineseVowel>(pinyin[..2]);
            rhyme = Enum.Parse<ChineseRhyme>(pinyin[2..]);
        }
        else
        {
            vowel = Enum.Parse<ChineseVowel>(pinyin[..1]);
            rhyme = Enum.Parse<ChineseRhyme>(pinyin[1..]);
        }
        return new(vowel, rhyme, (ChineseToneNote)tonenote);
    }

    /// <summary>
    /// 转换成注音符号(不包含声调)
    /// </summary>
    public string ToZhuyin() => $"{zhuYins[Vowel.ToString()]}{zhuYins[Rhyme.ToString()]}";

    /// <summary>
    /// 注音符号
    /// </summary>
    public static ChinesePinyin FromZhuyin(string zhuyin)
    {
        var vowel = ChineseVowel.None;
        var rhyme = ChineseRhyme.None;
        if (zhuYins.Inverse.TryGetValue(zhuyin, out string pinYin))
        {
            if (SingleVowels.Inverse.ContainsKey(pinYin))
                vowel = Enum.Parse<ChineseVowel>(pinYin);
            else if (SingleRhymes.Inverse.ContainsKey(pinYin))
                rhyme = Enum.Parse<ChineseRhyme>(pinYin);
        }
        else
        {
            vowel = Enum.Parse<ChineseVowel>(zhuYins.Inverse[zhuyin[..1]]);
            rhyme = Enum.Parse<ChineseRhyme>(zhuYins.Inverse[zhuyin[1..]]);
        }
        return new(vowel, rhyme, ChineseToneNote.None);
    }

}

#pragma warning disable CS1591

/// <summary>
/// 中文拼音的声母
/// </summary>
public enum ChineseVowel
{
    None,
    B,
    P,
    M,
    F,
    D,
    T,
    N,
    L,
    G,
    K,
    H,
    J,
    Q,
    X,
    ZH,
    CH,
    SH,
    R,
    Z,
    C,
    S,
}

/// <summary>
/// 中文拼音的韵母
/// </summary>
public enum ChineseRhyme
{
    None,
    A,
    O,
    E,
    I,
    U,
    V,
    ER,
    AI,
    AO,
    EI,
    OU,
    IA,
    IAO,
    IE,
    IOU,
    IU = IOU,
    UA,
    UAI,
    UEI,
    UI = UEI,
    UO,
    VE,
    AN,
    ANG,
    EN,
    ENG,
    IAN,
    IANG,
    IN,
    ING,
    UAN,
    UANG,
    UEN,
    UN = UEN,
    ONG,
    UENG = ONG,
    VAN,
    VN,
    IONG,
}

#pragma warning restore CS1591

/// <summary>
/// 中文拼音的声调
/// </summary>
public enum ChineseToneNote
{
    /// <summary>
    /// 没有声调
    /// </summary>
    None,

    /// <summary>
    /// 阴平
    /// </summary>
    LevelTone,

    /// <summary>
    /// 阳平
    /// </summary>
    RisingTone,

    /// <summary>
    /// 上声
    /// </summary>
    FallingRisingTone,

    /// <summary>
    /// 去声
    /// </summary>
    FallingTone,

    /// <summary>
    /// 轻声
    /// </summary>
    LightTone,
}
