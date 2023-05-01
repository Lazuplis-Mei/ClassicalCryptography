using ClassicalCryptography.Encoder.Japanese;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 用于日语的摩斯密码
/// </summary>
public class WabunCode : MorseCode
{
    internal WabunCode(BidirectionalDictionary<char, string> morseData) : base(morseData)
    {
    }

    /// <summary>
    /// 自动处理片假名(小)
    /// </summary>
    protected override char Convert(char character)
    {
        if (JapaneseHelper.Han_Katakana.map.TryGetValue(character, out char value))
            character = value;
        if (JapaneseHelper.LU_Katakana.map.TryGetValue(character, out value))
            character = value;
        return character;
    }
}