using System.Text;

namespace ClassicalCryptography.Utils;

/// <summary>
/// 
/// </summary>
internal static class StringExtension
{
    /// <summary>
    /// 分解字符串为不重复的大写字母字符集合
    /// </summary>
    public static HashSet<char> Decompose(this string str)
    {
        var set = new HashSet<char>();
        foreach (var c in str)
        {
            if (c is >= 'a' and <= 'z')
                set.Add((char)('A' + c - 'a'));
            else
                set.Add(c);
        }
        return set;
    }

    /// <summary>
    /// 找到字符出现的所有位置
    /// </summary>
    /// <param name="str"></param>
    /// <param name="c"></param>
    public static List<int> FindAll(this string str, char c)
    {
        var result = new List<int>();
        /*
        int si = str.IndexOf(c);
        while (si != -1)
        {
            result.Add(si);
            si = str.IndexOf(c, si + 1);
        }
        */
        for (int i = 0; i < str.Length; i++)
            if (str[i] == c)
                result.Add(i);
        return result;
    }

    /// <summary>
    /// 找到字符出现的所有位置
    /// </summary>
    /// <param name="str"></param>
    /// <param name="c"></param>
    public static List<int> FindAll(this char[] str, char c)
    {
        var result = new List<int>();
        /*
        int si = str.IndexOf(c);
        while (si != -1)
        {
            result.Add(si);
            si = str.IndexOf(c, si + 1);
        }
        */
        for (int i = 0; i < str.Length; i++)
            if (str[i] == c)
                result.Add(i);
        return result;
    }

    /// <summary>
    /// 默认编码
    /// </summary>
    public static readonly Encoding DefaultEncoding = Encoding.UTF8;

    /// <summary>
    /// 中文按UTF-8转换成Base64字符串
    /// </summary>
    /// <param name="text">中文字符串</param>
    public static string ToBase64(this string text)
    {
        var bytes = DefaultEncoding.GetBytes(text);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64字符串按UTF-8转换成中文
    /// </summary>
    /// <param name="base64">中文字符串</param>
    public static string FromBase64(this string base64)
    {
        var bytes = Convert.FromBase64String(base64);
        return DefaultEncoding.GetString(bytes);
    }

    public static bool IsPrintable(this char c) => c is >= ' ' and not (char)0x7F;
}
