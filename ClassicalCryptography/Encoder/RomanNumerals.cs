namespace ClassicalCryptography.Encoder;

/// <summary>
/// 大写罗马数字
/// </summary>
[ReferenceFrom("https://github.com/qntm/big-roman/blob/main/src/index.js", ProgramingLanguage.JavaScript, License.MIT)]
public static class RomanNumerals
{
    const char BAR_HAT = '\u0305';
    static readonly string[][] romanNumers = new[]
    {
        new[] { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"},
        new[] { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"},
        new[] { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"}
    };

    const string BAD_THOUSAND = "I\u0305";
    const string GOOD_THOUSAND = "M";

    /// <summary>
    /// 将阿拉伯数字转换成罗马数字
    /// </summary>
    /// <param name="arabicDigit">个位数</param>
    /// <param name="powerOfTen">指数</param>
    private static string ArabicToRoman(int arabicDigit, int powerOfTen)
    {
        var number = romanNumers[powerOfTen % 3][arabicDigit];
        var result = new StringBuilder();
        for (int i = 0; i < number.Length; i++)
        {
            result.Append(number[i]);
            for (int j = 0; j < powerOfTen / 3; j++)
                result.Append(BAR_HAT);
        }
        result.Replace(BAD_THOUSAND, GOOD_THOUSAND);
        return result.ToString();
    }

    /// <summary>
    /// 将阿拉伯数字转换成罗马数字
    /// </summary>
    public static string ArabicToRoman(ulong arabicNumeral)
    {
        var result = new StringBuilder();
        var arabicString = arabicNumeral.ToString();
        int length = arabicString.Length;
        for (int i = 0; i < length; i++)
            result.Append(ArabicToRoman(arabicString[i].Base36Number(), length - i - 1));

        return result.ToString();
    }

    /// <summary>
    /// 将罗马数字转换成阿拉伯数字
    /// </summary>
    public static ulong RomanToArabic(string romanNumeral)
    {
        Guard.IsNotNullOrEmpty(romanNumeral);

        var romanSpan = romanNumeral.AsSpan();

        int numBarsOnFirstLetter = 1;
        for (int j = 1; romanSpan[j] == BAR_HAT && j < romanSpan.Length; j++)
            numBarsOnFirstLetter++;
        int maxTenPower = (numBarsOnFirstLetter + 1) * 3;

        var arabicDigits = new StringBuilder();
        int index = 0;
        for (int tenPower = maxTenPower; tenPower >= 0; tenPower--)
        {
            var possibilities = new (string arabicDigit, string romanDigit)[10];
            for (int j = 0; j < possibilities.Length; j++)
                possibilities[j] = (j.ToString(), ArabicToRoman(j, tenPower));

            Array.Sort(possibilities, (a, b) => b.romanDigit.Length - a.romanDigit.Length);

            for (int j = 0; j < possibilities.Length; j++)
            {
                var (arabicDigit, romanDigit) = possibilities[j];
                if (romanSpan[index..].StartsWith(romanDigit))
                {
                    if (arabicDigits.Length != 0 || arabicDigit != "0")
                        arabicDigits.Append(arabicDigit);
                    index += romanDigit.Length;
                    break;
                }
            }
        }
        if (index != romanSpan.Length)
            throw new ArgumentException($"无法转换剩余的数字，位置:{index}", nameof(romanNumeral));

        return ulong.Parse(arabicDigits.ToString());
    }
}
