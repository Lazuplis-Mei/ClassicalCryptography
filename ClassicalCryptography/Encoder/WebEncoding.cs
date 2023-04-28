using System.Net;
using System.Web;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 网络相关的编码
/// </summary>
public static class WebEncoding
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 转换为Url编码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string UrlEncode(string input)
    {
        return HttpUtility.UrlEncode(input, Encoding);
    }

    /// <summary>
    /// 从Url编码转换
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string UrlDecode(string input)
    {
        return HttpUtility.UrlDecode(input, Encoding);
    }

    /// <summary>
    /// 转换为Html编码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string HtmlEncode(string input)
    {
        return HttpUtility.HtmlEncode(input);
    }

    /// <summary>
    /// 从Html编码转换
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string HtmlDecode(string input)
    {
        return HttpUtility.HtmlDecode(input);
    }

    /// <summary>
    /// ipv6地址编码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] Ipv6Encode(string ipv6)
    {
        var ip = IPAddress.Parse(ipv6);
        return ip.GetAddressBytes();
    }

    /// <summary>
    /// Base85编码ipv6地址
    /// </summary>
    public static string Base85Ipv6Encode(string ipv6)
    {
        return SimpleBase.Base85.Rfc1924.EncodeIpv6(IPAddress.Parse(ipv6));
    }

    /// <summary>
    /// Base85解码ipv6地址
    /// </summary>
    public static string Base85Ipv6Decode(string base85)
    {
        return SimpleBase.Base85.Rfc1924.DecodeIpv6(base85).ToString();
    }

    /// <summary>
    /// ipv6地址编码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Ipv6Decode(byte[] bytes)
    {
        var ip = new IPAddress(bytes);
        return ip.ToString();
    }
}
