using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace ClassicalCryptography.Encoder;

/// <summary>
/// 网络相关的编码
/// </summary>
public class WebEncoding
{
    /// <summary>
    /// 转换为Url编码
    /// </summary>
    public static string UrlEncode(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return HttpUtility.UrlEncode(input, encoding);
    }

    /// <summary>
    /// 从Url编码转换
    /// </summary>
    public static string UrlDecode(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return HttpUtility.UrlDecode(input, encoding);
    }

    /// <summary>
    /// 转换为Html编码
    /// </summary>
    public static string HtmlEncode(string input)
    {
        return HttpUtility.HtmlEncode(input);
    }

    /// <summary>
    /// 从Html编码转换
    /// </summary>
    public static string HtmlDecode(string input)
    {
        return HttpUtility.HtmlDecode(input);
    }
}
