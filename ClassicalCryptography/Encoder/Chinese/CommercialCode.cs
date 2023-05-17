using ClassicalCryptography.Replacement;
using ClassicalCryptography.Utils;

namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// <see href="https://github.com/Lazuplis-Mei/MorseCode.Chinese">中文电码</see>
/// </summary>
/// <remarks>
/// <see href="https://www.qqxiuzi.cn/bianma/dianbao.html">《标准电码本（修订本）》</see><br/>
/// 不兼容的两个字：螀(5831)镵(7016)，除此以外添加了1907个字，总体覆盖范围为：<br/>
/// [\u00B7\u00D7\u00F7]<br/>
/// [\u03A9]<br/>
/// [\u0410-\u042F]<br/>
/// [\u2014\u2018\u2019\u201C\u201D\u2026]<br/>
/// [\u2160-\u2169]<br/>
/// [\u3001\u3002\u300A\u300B\u3037]<br/>
/// [\u3105-\u3129]<br/>
/// [\u32C0-\u32CB]<br/>
/// [\u3358-\u3370\u33E0-\u33FE]<br/>
/// [\u4E00-\u9FFF](非全部)<br/>
/// [\uFF01\uFF08\uFF09\uFF0B-\uFF0D\uFF0F-\uFF1B\uFF0D\uFF0F\uFF21-\uFF3A]
/// </remarks>
[Introduction("中文电码", "标准中文电码(Chinese Commercial Code)")]
public static class CommercialCode
{
    private const char CHINESE_SPACE = '　';
    private static readonly Dictionary<char, ushort> commercialCodeData = new(9196);
    private static readonly string commercialCodeString;

    static CommercialCode()
    {
        commercialCodeString = Encoding.UTF8.GetString(GZip.Decompress(Resources.CommercialCode));
        commercialCodeData.Add(CHINESE_SPACE, 9998);
        for (ushort i = 0; i < commercialCodeString.Length; i++)
            if (commercialCodeString[i] is not CHINESE_SPACE)
                commercialCodeData.Add(commercialCodeString[i], i);
    }

    /// <summary>
    /// 解密中文电码串(4个数字一组)
    /// </summary>
    public static string FromCodeString(string codeText)
    {
        return new(codeText.Partition(4, s => commercialCodeString[s.ReadDigits4()]));
    }

    /// <summary>
    /// 解密中文电码
    /// </summary>
    [SkipLocalsInit]
    public static string FromCodes(params ushort[] codes)
    {
        int count = codes.Length;
        using var memory = count.TryAllocString();
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : memory.Span;
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
            if (commercialCodeData.TryGetValue(text[i], out ushort code))
                codes[i] = code;
        return codes;
    }

    /// <summary>
    /// 转换成中文电码串
    /// </summary>
    [SkipLocalsInit]
    public static string ToCodesString(string text)
    {
        int count = text.Length * 4;
        using var memory = count.TryAllocString();
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : memory.Span;
        for (int i = 0; i < count; i++)
        {
            int code = 0;
            if (commercialCodeData.TryGetValue(text[i / 4], out ushort value))
                code = value;
            span.WriteDigits4(code, ref i);
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
