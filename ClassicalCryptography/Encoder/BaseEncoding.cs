using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// base编码
/// </summary>
public static class BaseEncoding
{
    /// <summary>
    /// 转换为Base64编码
    /// </summary>
    public static string ToBase64(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 从Base64编码转换
    /// </summary>
    public static string FromBase64(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = Convert.FromBase64String(input);
        return encoding.GetString(bytes);
    }

    /// <summary>
    /// 转换为Base32编码
    /// </summary>
    public static string ToBase32(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        return Base32Encoding.ToString(bytes);
    }

    /// <summary>
    /// 从Base32编码转换
    /// </summary>
    public static string FromBase32(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = Base32Encoding.ToBytes(input);
        return encoding.GetString(bytes);
    }

    /// <summary>
    /// 转换为Hex
    /// </summary>
    public static string ToBase16(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        return Convert.ToHexString(bytes);
    }

    /// <summary>
    /// 从Hex编码转换
    /// </summary>
    public static string FromBase16(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = Convert.FromHexString(input);
        return encoding.GetString(bytes);
    }

}
