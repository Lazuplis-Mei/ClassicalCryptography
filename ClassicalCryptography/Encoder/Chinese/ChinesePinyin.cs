using static ClassicalCryptography.Encoder.Chinese.ChineseHelper;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// 中文拼音
/// </summary>
public readonly partial struct ChinesePinyin
{
    /// <summary>
    /// 声母
    /// </summary>
    public readonly ChineseVowel Vowel;

    /// <summary>
    /// 韵母
    /// </summary>
    public readonly ChineseRhyme Rhyme;

    /// <summary>
    /// 声调
    /// </summary>
    public readonly ChineseToneNote ToneNote;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetRhymeString() => Rhyme switch
    {
        ChineseRhyme.IOU => "IU",
        ChineseRhyme.UEI => "UI",
        ChineseRhyme.UEN => "UN",
        ChineseRhyme.UENG => "ONG",
        _ => Rhyme.ToString(),
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetSingleRhymeString() => Rhyme switch
    {
        ChineseRhyme.IOU => "IOU",
        ChineseRhyme.UEI => "UEI",
        ChineseRhyme.UEN => "UEN",
        ChineseRhyme.UENG => "UENG",
        _ => Rhyme.ToString(),
    };

    /// <summary>
    /// 中文拼音
    /// </summary>
    public ChinesePinyin(ChineseVowel vowel, ChineseRhyme rhyme, ChineseToneNote toneNote)
    {
        GuardEx.IsDefined(vowel);
        GuardEx.IsDefined(rhyme);
        GuardEx.IsDefined(toneNote);
        Guard.IsFalse(vowel is ChineseVowel.None && rhyme is ChineseRhyme.None);

        Vowel = vowel;
        Rhyme = rhyme;
        ToneNote = toneNote;
    }

    internal ChinesePinyin(string pinyin)
    {
        int tonenote = 0;
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
        Vowel = vowel;
        Rhyme = rhyme;
        ToneNote = (ChineseToneNote)tonenote;
    }

    /// <summary>
    /// 从字符串解析拼音
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChinesePinyin Parse(string pinyin) => new(ParsePinyin(pinyin));

    /// <summary>
    /// 转换成注音符号(不包含声调)
    /// </summary>
    public string ToZhuyinWithoutToneNote()
    {
        return $"{zhuYins[Vowel.ToString()]}{zhuYins[GetRhymeString()]}";
    }

    /// <summary>
    /// 转换成注音符号(包含声调)
    /// </summary>
    public string ToZhuyin()
    {
        if (ToneNote is ChineseToneNote.None)
            return ToZhuyinWithoutToneNote();
        return $"{zhuYins[Vowel.ToString()]}{zhuYins[GetRhymeString()]}{toneNotes[(int)ToneNote - 1]}";
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

    /// <inheritdoc/>
    public override string ToString()
    {
        string? rhyme = null, vowel = null;
        if (Rhyme is ChineseRhyme.None)
            vowel = SingleVowels.Inverse[Vowel.ToString()];
        else if (Vowel is ChineseVowel.None)
            rhyme = SingleRhymes.Inverse[GetSingleRhymeString()];
        else
        {
            vowel = Vowel.ToString();
            rhyme = GetRhymeString();
        }
        return ToneNote is ChineseToneNote.None ? $"{vowel}{rhyme}" : $"{vowel}{rhyme}{(int)ToneNote}";
    }
}
