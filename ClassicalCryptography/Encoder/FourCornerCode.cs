using ClassicalCryptography.Properties;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// <see href="https://zh.wikipedia.org/zh-cn/%E5%9B%9B%E8%A7%92%E5%8F%B7%E7%A0%81">四角号码</see>
/// </summary>
/// <remarks>
/// 在线工具:<see href="https://www.qqxiuzi.cn/bianma/sijiaohaoma/">四角号码</see>
/// </remarks>
[Introduction("四角号码", "检索汉字的一种方法，依据汉字四个角的笔形编制的数字号码")]
public class FourCornerCode
{
    private static readonly Dictionary<int, string> fourCornerCodeData = new();
    private static bool dataLoaded = false;

    private static void LoadData()
    {
        using var reader = new StreamReader(GZip.DecompressToStream(Resources.FourCorner));
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!;
            fourCornerCodeData.Add(Convert.ToInt32($"1{line[..5]}"), line[5..]);
        }
        dataLoaded = true;
    }

    /// <summary>
    /// 解密四角号码串(5个数字一组)
    /// </summary>
    /// <remarks>
    /// 四角号码有很多重码，默认使用第一个寻找到的。<br/>
    /// 可以使用<see cref="FromCode(string)"/>查询号码对应所有可能的汉字
    /// </remarks>
    public static string FromCodeString(string codeText)
    {
        return FromCodes(codeText.Partition(5, s => int.Parse(s)));
    }

    /// <summary>
    /// 解密四角号码
    /// </summary>
    /// <remarks>
    /// 四角号码有很多重码，默认使用第一个寻找到的。<br/>
    /// 可以使用<see cref="FromCode(string)"/>查询号码对应所有可能的汉字
    /// </remarks>
    [SkipLocalsInit]
    private static string FromCodes(params int[] codes)
    {
        if (!dataLoaded) LoadData();
        Span<char> span = codes.Length.CanAllocString()
            ? stackalloc char[codes.Length] : new char[codes.Length];
        for (int i = 0; i < span.Length; i++)
            span[i] = fourCornerCodeData[100000 + codes[i]][0];
        return new(span);
    }

    /// <summary>
    /// 获得一个四角号码对应的所有可能的汉字
    /// </summary>
    public static string FromCode(string code)
    {
        if (!dataLoaded) LoadData();
        return fourCornerCodeData[100000 + Convert.ToInt32(code)];
    }

    /// <summary>
    /// 转换成四角号码串
    /// </summary>
    /// <remarks>
    /// 使用99999代替没有找到的字符
    /// </remarks>
    [SkipLocalsInit]
    public static string ToCodesString(string text)
    {
        if (!dataLoaded) LoadData();
        int len = text.Length * 5;
        Span<char> span = len.CanAllocString()
            ? stackalloc char[len] : new char[len];
        var currentSpan = span;
        for (int i = 0; i < text.Length; i++)
        {
            var code = fourCornerCodeData.FirstOrDefault(pair => pair.Value.Contains(text[i])).Key;
            if (code == 0)
                continue;
            else
                code -= 100000;
            code.ToString("D5").CopyTo(currentSpan);
            currentSpan = currentSpan[5..];
        }
        return new(span[..^currentSpan.Length]);
    }
}
