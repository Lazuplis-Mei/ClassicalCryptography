using ClassicalCryptography.Replacement;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// <see href="https://github.com/Lazuplis-Mei/MorseCode.Chinese">中文电码</see>
/// </summary>
[Introduction("中文电码", "标准中文电码(Chinese Commercial Code)")]
public class CommercialCode
{
    private static readonly Dictionary<char, ushort> commercialCodeData = new();
    private static readonly string commercialCodeString;

    static CommercialCode()
    {
        commercialCodeString = Encoding.UTF8.GetString(GZip.Decompress(Resources.CommercialCode));
        for (ushort i = 0; i < commercialCodeString.Length; i++)
        {
            if (commercialCodeString[i] != commercialCodeString[0])
                commercialCodeData.Add(commercialCodeString[i], i);
        }
    }

    /// <summary>
    /// 解密中文电码串(4个数字一组)
    /// </summary>
    public static string FromCodeString(string codeText)
    {
        return new(codeText.Partition(4, s =>
        {
            int value = s[0].Base36Number() * 1000;
            value += s[1].Base36Number() * 100;
            value += s[2].Base36Number() * 10;
            value += s[3].Base36Number();
            return commercialCodeString[value];
        }));
    }

    /// <summary>
    /// 解密中文电码
    /// </summary>
    [SkipLocalsInit]
    public static string FromCodes(params ushort[] codes)
    {
        int count = codes.Length;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        for (int i = 0; i < count; i++)
            span[i] = commercialCodeString[codes[i]];
        return new(span);
    }

    /// <summary>
    /// 转换成中文电码
    /// </summary>
    public static ushort[] ToCodes(string text)
    {
        var codes = new ushort[text.Length];
        for (int i = 0; i < codes.Length; i++)
        {
            if (commercialCodeData.TryGetValue(text[i], out ushort code))
                codes[i] = code;
        }
        return codes;
    }

    /// <summary>
    /// 转换成中文电码串
    /// </summary>
    [SkipLocalsInit]
    public static string ToCodesString(string text)
    {
        int count = text.Length * 4;
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : new char[count];
        for (int i = 0; i < count; i++)
        {
            int code = 0;
            if (commercialCodeData.TryGetValue(text[i / 4], out ushort value))
                code = value;
            span[i++] = Digits[code / 1000];
            span[i++] = Digits[code / 100 % 10];
            span[i++] = Digits[code / 10 % 10];
            span[i] = Digits[code % 10];
        }
        return new(span);
    }

    /// <summary>
    /// 加密中文摩斯密码(使用<see cref="MorseCode.ShortDigit"/>)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToMorse(string text)
    {
        return MorseCode.ShortDigit.ToMorse(ToCodesString(text));
    }

    /// <summary>
    /// 解密中文摩斯密码(使用<see cref="MorseCode.ShortDigit"/>)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FromMorse(string morse)
    {
        return FromCodeString(MorseCode.ShortDigit.FromMorse(morse));
    }
}
