using ClassicalCryptography.Encoder.BaseEncodings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// base编码
/// </summary>
public static partial class BaseEncoding
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
    /// 转换为Base32768编码
    /// </summary>
    public static string ToBase32768(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        return Base32768Encoding.Encode(bytes);
    }

    /// <summary>
    /// 从Base32768编码转换
    /// </summary>
    public static string FromBase32768(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = Base32768Encoding.Decode(input);
        return encoding.GetString(bytes);
    }


    /// <summary>
    /// 转换为Base65536编码
    /// </summary>
    public static string ToBase65536(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        return Base65536Encoding.Encode(bytes);
    }


    /// <summary>
    /// 从Base65536编码转换
    /// </summary>
    public static string FromBase65536(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = Base65536Encoding.Decode(input);
        return encoding.GetString(bytes);
    }


    /// <summary>
    /// 转换为Base2048编码
    /// </summary>
    public static string ToBase2048(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        return Base2048Encoding.Encode(bytes);
    }


    /// <summary>
    /// 从Base2048编码转换
    /// </summary>
    public static string FromBase2048(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = Base2048Encoding.Decode(input);
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


    /// <summary>
    /// 转换为Emoji表情符号
    /// </summary>
    public static string ToBase100(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        return Base100Encoding.Encode(bytes);
    }

    /// <summary>
    /// 从Emoji表情符号转换
    /// </summary>
    public static string FromBase100(string input, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = Base100Encoding.Decode(input);
        return encoding.GetString(bytes);
    }

    /// <summary>
    /// 使用指定表(<see cref="Utils.GlobalTables.Base36"/>等)编码
    /// </summary>
    public static string ToBase(string input, string table, Encoding? encoding = null)
    {
        encoding ??= Encoding.Default;
        var bytes = encoding.GetBytes(input);
        var number = new BigInteger(bytes, true, true);

        var stack = new Stack<char>();
        while (!number.IsZero)
        {
            stack.Push(table[(int)(number % table.Length)]);
            number /= table.Length;
        }
        return new(stack.ToArray());
    }

}
