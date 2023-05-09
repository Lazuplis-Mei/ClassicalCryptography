namespace ClassicalCryptography.Encoder.Chinese;

/// <summary>
/// <see href="https://zh.wikipedia.org/zh-cn/%E5%9B%9B%E8%A7%92%E5%8F%B7%E7%A0%81">四角号码</see>
/// </summary>
/// <remarks>
/// 在线工具:<see href="https://www.qqxiuzi.cn/bianma/sijiaohaoma/">四角号码</see>
/// </remarks>
[Introduction("四角号码", "检索汉字的一种方法，依据汉字四个角的笔形编制的数字号码")]
public class FourCornerCode
{
    private static readonly Dictionary<int, string> fourCornerCodeData;

    static FourCornerCode()
    {
        fourCornerCodeData = new(19912);
        using var reader = new StreamReader(GZip.DecompressToStream(Resources.FourCorner));
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!;
            fourCornerCodeData.Add(Convert.ToInt32($"1{line[..5]}"), line[5..]);
        }
    }

    /// <summary>
    /// 解密四角号码串(5个数字一组)
    /// </summary>
    /// <remarks>
    /// 四角号码有很多重码，默认使用第一个寻找到的(这通常不会给你期望的结果)。<br/>
    /// 可以使用<see cref="FromCode(string)"/>查询号码对应所有可能的汉字
    /// </remarks>
    public static string FromCodeString(string codeText)
    {
        return new(codeText.Partition(5, s => fourCornerCodeData[100000 + int.Parse(s)][0]));
    }

    /// <summary>
    /// 获得一个四角号码对应的所有可能的汉字
    /// </summary>
    public static string FromCode(string code)
    {
        return fourCornerCodeData[100000 + Convert.ToInt32(code)];
    }

    /// <summary>
    /// 转换成四角号码串
    /// </summary>
    [SkipLocalsInit]
    public static string ToCodesString(string text)
    {
        int length = text.Length * 5;
        using var memory = length.TryAllocString();
        Span<char> span = length.CanAllocString() ? stackalloc char[length] : memory.Span;
        var currentSpan = span;
        foreach (char character in text)
        {
            var code = fourCornerCodeData.FirstOrDefault(pair => pair.Value.Contains(character)).Key;
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
