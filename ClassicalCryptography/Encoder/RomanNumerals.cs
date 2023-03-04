using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 大写罗马数字 <see href="https://github.com/qntm/big-roman"/>
/// </summary>
public static partial class RomanNumerals
{
    const char bar = '\u0305';
    static readonly string[][] banks = new[]
    {
        new[] { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"},
        new[] { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"},
        new[] { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"}
    };

    static readonly int numBanks = banks.Length;
    // Special case
    const string badThousand = "I\u0305";
    const string goodThousand = "M";

    /// <summary>
    /// Converting an Arabic digit to a Roman "digit".
    /// Roman numerals are place-value! Except, the size of a place varies and
    /// sometimes it's the empty string
    /// </summary>
    public static string ArabicToRomanDigit(int arabicDigit, int powerOfTen)
    {
        var bank = banks[powerOfTen % numBanks];
        if (arabicDigit < 0 || arabicDigit >= bank.Length)
            throw new ArgumentException("Not an Arabic digit", nameof(arabicDigit));

        var cbanks = new string[bank[arabicDigit].Length];
        for (int i = 0; i < cbanks.Length; i++)
        {
            cbanks[i] = string.Empty;
            cbanks[i] += bank[arabicDigit][i];
            for (int j = 0; j < powerOfTen / numBanks; j++)
                cbanks[i] += bar;
            cbanks[i] = cbanks[i].Replace(badThousand, goodThousand);
        }
        return string.Concat(cbanks);
    }

    /// <summary>
    /// ArabicToRoman
    /// </summary>
    public static string ArabicToRoman(string arabicNumeral)
    {
        if (string.IsNullOrEmpty(arabicNumeral))
            throw new ArgumentException("Not a non-empty string", nameof(arabicNumeral));

        var str = new StringBuilder();

        for (int i = 0; i < arabicNumeral.Length; i++)
            str.Append(ArabicToRomanDigit(arabicNumeral[i] - '0', arabicNumeral.Length - i - 1));

        return str.ToString();
    }

    /// <summary>
    /// ArabicToRoman
    /// </summary>
    public static string BigIntToRoman(BigInteger bigInt)
    {
        if (bigInt.Sign != 1)
            throw new ArgumentException("Not a positive BigInt", nameof(bigInt));

        return ArabicToRoman(bigInt.ToString());
    }

    /// <summary>
    /// ArabicToRoman
    /// </summary>
    public static string NumberToRoman(int number)
    {
        if (number <= 0)
            throw new ArgumentException("Not a positive integer", nameof(number));

        return ArabicToRoman(number.ToString());
    }

    /// <summary>
    /// RomanToArabic
    /// </summary>
    public static string RomanToArabic(string romanNumeral)
    {
        if (string.IsNullOrEmpty(romanNumeral))
            throw new ArgumentException("Not a non-empty string", nameof(romanNumeral));
        // Brute force is a completely practical approach here. Figure out the
        // magnitude of the number we're looking at, and then work backwards
        // through the expected Roman digits, consuming each as we go.
        // If the input is gibberish then we just get 0s and parsing will fail
        // later because we didn't consume the whole string

        var romanSpan = romanNumeral.AsSpan();

        string barsOnFirstLetter = FirstBars().Match(romanNumeral).Value;
        int numBarsOnFirstLetter = barsOnFirstLetter.Length;

        // Round up a tad.
        // E.g. for input "M", that's 0 bars, `maxTenPower` should be 3
        int maxTenPower = (numBarsOnFirstLetter + 1) * numBanks;

        var arabicDigits = new List<string>();

        int i = 0;

        for (int tenPower = maxTenPower; tenPower >= 0; tenPower--)
        {
            // We search in descending order of length. This averts a potential
            // parsing issue where we would parse the "I" of "II" and then be stuck
            // forever

            var possibilities = new (string arabicDigit, string romanDigit)[10];
            for (int j = 0; j < possibilities.Length; j++)
            {
                possibilities[j].arabicDigit = j.ToString();
                possibilities[j].romanDigit = ArabicToRomanDigit(j, tenPower);
            }
            Array.Sort(possibilities, (a, b) => b.romanDigit.Length - a.romanDigit.Length);

            for (int j = 0; j < possibilities.Length; j++)
            {
                var (arabicDigit, romanDigit) = possibilities[j];
                if (romanSpan[i..].StartsWith(romanDigit))
                {
                    // No leading zeroes please
                    if (arabicDigits.Count != 0 || arabicDigit != "0")
                    {
                        arabicDigits.Add(arabicDigit);
                    }
                    i += romanDigit.Length;
                    // Note that "" is a valid digit (0) and will ALWAYS be found
                    // if we don't break now
                    break;
                }
            }
        }
        if (i != romanNumeral.Length)
        {
            throw new Exception($"Could not consume a Roman digit at position {i} in: '{romanNumeral}'");
        }
        return string.Concat(arabicDigits);
    }

    /// <summary>
    /// RomanToBigInt
    /// </summary>
    public static BigInteger RomanToBigInt(string romanNumeral)
    {
        return BigInteger.Parse(RomanToArabic(romanNumeral));
    }

    /// <summary>
    /// RomanToNumber
    /// </summary>
    public static int RomanToNumber(string romanNumeral)
    {
        return int.Parse(RomanToArabic(romanNumeral));
    }

    [GeneratedRegex("^.(\\u0305*)")]
    private static partial Regex FirstBars();
}
