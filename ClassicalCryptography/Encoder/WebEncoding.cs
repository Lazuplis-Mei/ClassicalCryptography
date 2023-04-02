using System.Net;
using System.Runtime.CompilerServices;
using System.Web;


namespace ClassicalCryptography.Encoder;

/// <summary>
/// 网络相关的编码
/// </summary>
public static class WebEncoding
{
    /// <summary>
    /// 转换为Url编码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string UrlEncode(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return HttpUtility.UrlEncode(input, encoding);
    }

    /// <summary>
    /// 从Url编码转换
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string UrlDecode(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return HttpUtility.UrlDecode(input, encoding);
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
        var number = new BigInteger(Ipv6Encode(ipv6), true, true);
        return BaseEncoding.NumberToBase(number, GlobalTables.Ascii85_IPv6);
    }

    /// <summary>
    /// Base85解码ipv6地址
    /// </summary>
    public static string Base85Ipv6Decode(string base85)
    {
        var number = BaseEncoding.NumberFromBase(base85, GlobalTables.Ascii85_IPv6);
        return Ipv6Decode(number.ToByteArray(true, true));
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
