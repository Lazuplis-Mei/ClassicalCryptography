using static ClassicalCryptography.Encoder.Chinese.ChineseHelper;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// 中文拼音
/// </summary>
public readonly partial record struct ChinesePinyin(ChineseVowel Vowel, ChineseRhyme Rhyme, ChineseToneNote ToneNote)
{
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
    public string ToZhuyinWithoutToneNote() => $"{zhuYins[Vowel.ToString()]}{zhuYins[Rhyme.ToString()]}";

    /// <summary>
    /// 转换成注音符号(包含声调)
    /// </summary>
    public string ToZhuyin()
    {
        if (ToneNote is ChineseToneNote.None)
            return ToZhuyinWithoutToneNote();
        return $"{zhuYins[Vowel.ToString()]}{zhuYins[Rhyme.ToString()]}{toneNotes[(int)ToneNote - 1]}";
    }

    /// <summary>
    /// 注音符号
    /// </summary>
    public static ChinesePinyin FromZhuyin(string zhuyin)
    {
        var vowel = ChineseVowel.None;
        var rhyme = ChineseRhyme.None;
        var toneNote = (ChineseToneNote)(toneNotes.IndexOf(zhuyin[^1]) + 1);
        if (toneNote != ChineseToneNote.None)
            zhuyin = zhuyin[..^1];
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
        return new(vowel, rhyme, toneNote);
    }

}
