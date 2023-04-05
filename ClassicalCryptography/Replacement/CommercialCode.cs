using ClassicalCryptography.Properties;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// <see href="https://github.com/Lazuplis-Mei/MorseCode.Chinese">中文电码</see>
/// </summary>
[Introduction("中文电码", "标准中文电码(Chinese Commercial Code)")]
public class CommercialCode
{
    private static readonly Dictionary<char, short> commercialCodeData = new();
    private static string commercialCodeString = string.Empty;
    private static bool dataLoaded = false;

    private static void LoadData()
    {
        commercialCodeString = Encoding.UTF8.GetString(GZip.Decompress(Resources.CommercialCode));
        for (short i = 0; i < commercialCodeString.Length; i++)
        {
            if (commercialCodeString[i] != commercialCodeString[0])
                commercialCodeData.Add(commercialCodeString[i], i);
        }
        dataLoaded = true;
    }

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
        if (!dataLoaded) LoadData();
        Span<char> span = codes.Length.CanAllocString()
            ? stackalloc char[codes.Length] : new char[codes.Length];
        for (int i = 0; i < span.Length; i++)
            span[i] = commercialCodeString[codes[i]];
        return new(span);
    }

    /// <summary>
    /// 转换成中文电码
    /// </summary>
    public static short[] ToCodes(string text)
    {
        if (!dataLoaded) LoadData();
        var codes = new short[text.Length];
        for (int i = 0; i < codes.Length; i++)
        {
            if (commercialCodeData.ContainsKey(text[i]))
                codes[i] = commercialCodeData[text[i]];
        }
        return codes;
    }

    /// <summary>
    /// 转换成中文电码串
    /// </summary>
    [SkipLocalsInit]
    public static string ToCodesString(string text)
    {
        if (!dataLoaded) LoadData();
        int len = text.Length << 2;
        Span<char> span = len.CanAllocString()
            ? stackalloc char[len] : new char[len];

        for (int i = 0; i < text.Length; i++)
        {
            var code = 0;
            if (commercialCodeData.TryGetValue(text[i], out short value))
                code = value;
            code.ToString("D4").CopyTo(span[(i << 2)..]);
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