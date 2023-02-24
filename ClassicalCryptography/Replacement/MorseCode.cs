using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode.Internal;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 摩斯密码  https://github.com/Lazuplis-Mei/MorseCode.Chinese
/// </summary>
public class MorseCode
{

    private const char S = '.';
    private const char L = '-';
    private const string standardLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string[] standardCodes =
    {
            ".-",
            "-...",
            "-.-.",
            "-..",
            ".",
            "..-.",
            "--.",
            "....",
            "..",
            ".---",
            "-.-",
            ".-..",
            "--",
            "-.",
            "---",
            ".--.",
            "--.-",
            ".-.",
            "...",
            "-",
            "..-",
            "...-",
            ".--",
            "-..-",
            "-.--",
            "--..",
        };
    /// <summary>
    /// 标准摩斯密码
    /// </summary>
    public static readonly MorseCode Standred = new(standardLetters, standardCodes);

    private const string extendedLetters = $"!\"#$'()*,-./0123456789:;=?@{standardLetters}[]_";
    private static readonly string[] extendedCodes =
    {
            "-.-.--",
            ".-..-.",
            "..--",
            "...-..-",
            ".----.",
            "-.--.",
            "-.--.-",
            "----",
            "--..--",
            "-....-",
            ".-.-.-",
            "-..-.",
            "-----",
            ".----",
            "..---",
            "...--",
            "....-",
            ".....",
            "-....",
            "--...",
            "---..",
            "----.",
            "---...",
            "-.-.-.",
            "-...-",
            "..--..",
            ".--.-.",
                ".-",
                "-...",
                "-.-.",
                "-..",
                ".",
                "..-.",
                "--.",
                "....",
                "..",
                ".---",
                "-.-",
                ".-..",
                "--",
                "-.",
                "---",
                ".--.",
                "--.-",
                ".-.",
                "...",
                "-",
                "..-",
                "...-",
                ".--",
                "-..-",
                "-.--",
                "--..",
            "-.-..",
            ".---.",
            "..--.-",
        };
    /// <summary>
    /// 扩展的摩斯密码
    /// </summary>
    public static readonly MorseCode Extended = new(extendedLetters, extendedCodes);

    private const string shortDigits = "0123456789";
    private static readonly string[] shortDigitsCodes =
    {
            ".-",
            "..-",
            "...--",
            "....-",
            ".....",
            "-....",
            "--...",
            "-..",
            "-.",
            "-",
        };
    /// <summary>
    /// 数字短码
    /// </summary>
    public static readonly MorseCode ShortDigit = new(shortDigits, shortDigitsCodes);


    private readonly string letters;
    private readonly string lowerLetters;
    private readonly string[] codes;

    private MorseCode(string letters, string[] codes)
    {
        this.letters = letters;
        this.lowerLetters = letters.ToLower();
        this.codes = codes;
    }

    private int GetLetterIndex(char letter)
    {
        int index = letters.IndexOf(letter);
        if (index == -1)
        {
            index = lowerLetters.IndexOf(letter);
            if (index == -1)
                throw new ArgumentOutOfRangeException(nameof(letter));
        }
        return index;
    }

    /// <summary>
    /// 字符可以编码
    /// </summary>
    public bool IsVaildLetter(char letter)
    {
        return letters.Contains(letter) || lowerLetters.Contains(letter);
    }

    private int GetCodeIndex(string code)
    {
        int index = Array.IndexOf(codes, code);
        if (index == -1)
            throw new ArgumentOutOfRangeException(nameof(code));
        return index;
    }

    /// <summary>
    /// 存在的摩斯密码
    /// </summary>
    public bool IsVaildCode(string code)
    {
        return Array.IndexOf(codes, code) != -1;
    }

    /// <summary>
    /// 解密
    /// </summary>
    public string FromMorse(string morse, char separator = '/')
    {
        var strBuilder = new StringBuilder();
        foreach (var code in morse.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
            strBuilder.Append(letters[GetCodeIndex(code)]);
        }
        return strBuilder.ToString();
    }

    /// <summary>
    /// 加密
    /// </summary>
    public string ToMorse(string text, char separator = '/')
    {
        var result = new string[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            result[i] = codes[GetLetterIndex(text[i])];
        }
        return string.Join(separator, result);
    }

}
