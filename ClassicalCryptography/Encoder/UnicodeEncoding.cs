namespace ClassicalCryptography.Encoder;

/// <summary>
/// Unicode字符串(使用C#默认的字符串编码转换)
/// </summary>
public partial class UnicodeEncoding
{
    /// <summary>
    /// 转换成\u字符串
    /// </summary>
    /// <remarks>
    /// 所有字符都会被转换为\u0000这样的形式
    /// </remarks>
    [SkipLocalsInit]
    public static string Encode(string str)
    {
        int size = str.Length * 6;
        using var memory = size.TryAllocString();
        Span<char> span = size.CanAllocString() ? stackalloc char[size] : memory.Span;
        var orignalSpan = span;
        foreach (int character in str)
        {
            @$"\u{character:x4}".CopyTo(span);
            span = span[6..];
        }
        return new string(orignalSpan);
    }

    /// <summary>
    /// 转义字符
    /// </summary>
    public static string Escape(string str) => Regex.Escape(str);

    /// <summary>
    /// 转义字符
    /// </summary>
    public static string Unescape(string str) => Regex.Unescape(str);

    /// <summary>
    /// 从\u字符串转换
    /// </summary>
    /// <remarks>
    /// 非\u0000形式的字符都会被忽略
    /// </remarks>
    [SkipLocalsInit]
    public static string Decode(string str)
    {
        var matches = UnicodeRegex().Matches(str);
        int count = matches.Count;
        using var memory = count.TryAllocString();
        Span<char> span = count.CanAllocString() ? stackalloc char[count] : memory.Span;

        for (int i = 0; i < matches.Count; i++)
        {
            var code = Convert.ToInt16(matches[i].Value[2..], 16);
            span[i] = (char)code;
        }
        return new string(span);
    }

    [GeneratedRegex(@"\\u[0-9A-Fa-f]{1,4}")]
    private static partial Regex UnicodeRegex();
}
