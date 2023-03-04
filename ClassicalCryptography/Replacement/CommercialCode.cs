using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Properties;
using ClassicalCryptography.Utils;
using System.Runtime.CompilerServices;
using System.Text;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 中文电码<see href="https://github.com/Lazuplis-Mei/MorseCode.Chinese">MorseCode.Chinese</see>
/// </summary>
[Introduction("中文电码", "标准中文电码(Chinese Commercial Code)")]
public static class CommercialCode
{
    private static readonly string charDATA = Encoding.Default.GetString(GZip.Decompress(Resources.CC));

    /// <summary>
    /// 解密中文电码串(4个数字一组)
    /// </summary>
    public static string FromCodeString(string codeText)
    {
        return FromCodes(codeText.Partition(4, s => short.Parse(s)));
    }

    /// <summary>
    /// 解密中文电码
    /// </summary>
    [SkipLocalsInit]
    public static string FromCodes(params short[] codes)
    {
        Span<char> span = codes.Length <= StackLimit.MaxCharSize
            ? stackalloc char[codes.Length] : new char[codes.Length];
        for (int i = 0; i < span.Length; i++)
            span[i] = charDATA[codes[i]];
        return new(span);
    }

    /// <summary>
    /// 转换成中文电码
    /// </summary>
    public static short[] ToCodes(string text)
    {
        var codes = new short[text.Length];
        for (int i = 0; i < codes.Length; i++)
        {
            codes[i] = (short)charDATA.IndexOf(text[i]);
            if (codes[i] == -1)
                codes[i] = 0;
        }
        return codes;
    }

    /// <summary>
    /// 转换成中文电码串
    /// </summary>
    public static string ToCodesString(string text)
    {
        int len = text.Length << 2;
        Span<char> span = len <= StackLimit.MaxCharSize
            ? stackalloc char[len] : new char[len];

        for (int i = 0; i < text.Length; i++)
        {
            var code = charDATA.IndexOf(text[i]);
            if (code == -1)
                code = 0;
            code.ToString("D4").CopyTo(span[(i << 2)..]);
        }
        return new(span);
    }

    /// <summary>
    /// 加密中文摩斯密码(使用<see cref="MorseCode.ShortDigit"/>)
    /// </summary>
    public static string ToMorse(string text)
    {
        return MorseCode.ShortDigit.ToMorse(ToCodesString(text));
    }

    /// <summary>
    /// 解密中文摩斯密码(使用<see cref="MorseCode.ShortDigit"/>)
    /// </summary>
    public static string FromMorse(string morse)
    {
        return FromCodeString(MorseCode.ShortDigit.FromMorse(morse));
    }
}