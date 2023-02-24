using ClassicalCryptography.Properties;
using ClassicalCryptography.Utils;
using System.Text;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 中文电码 https://github.com/Lazuplis-Mei/MorseCode.Chinese
/// </summary>
public static class CommercialCode
{
    private static readonly string DATA = Encoding.Default.GetString(GZip.Decompress(Resources.CC));

    /// <summary>
    /// 解密中文电码
    /// </summary>
    public static string FromCodeString(string codeText)
    {
        return FromCodes(codeText.Partition(4, s => short.Parse(s)));
    }

    /// <summary>
    /// 解密中文电码
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    public static string FromCodes(params short[] codes)
    {
        var strBuilder = new StringBuilder();
        foreach (var code in codes)
        {
            strBuilder.Append(DATA[code]);
        }
        return strBuilder.ToString();
    }

    /// <summary>
    /// 中文电码
    /// </summary>
    public static short[] ToCodes(string text)
    {
        var codes = new short[text.Length];
        for (int i = 0; i < codes.Length; i++)
        {
            codes[i] = (short)DATA.IndexOf(text[i]);
            if (codes[i] == -1)
                codes[i] = 0;
        }
        return codes;
    }

    /// <summary>
    /// 中文电码
    /// </summary>
    public static string ToCodesString(string text)
    {
        var str = new StringBuilder(text.Length * 4);
        for (int i = 0; i < text.Length; i++)
        {
            var code = DATA.IndexOf(text[i]);
            if (code == -1)
                code = 0;
            str.Append(code.ToString("D4"));
        }
        return str.ToString();
    }

    /// <summary>
    /// 加密中文摩斯密码
    /// </summary>
    public static string ToMorse(string text)
    {
        return MorseCode.ShortDigit.ToMorse(ToCodesString(text));
    }

    /// <summary>
    /// 解密中文摩斯密码
    /// </summary>
    public static string FromMorse(string morse)
    {
        return FromCodeString(MorseCode.ShortDigit.FromMorse(morse));
    }
}