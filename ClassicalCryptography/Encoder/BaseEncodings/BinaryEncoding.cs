using CommunityToolkit.HighPerformance;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// 进制转换(2,4,8)进制
/// </summary>
/// <remarks>
/// 16进制请使用<see cref="BaseEncoding"/>中的Base16
/// </remarks>
[Introduction("进制转换", "提供(2,4,8)进制的转换")]
public class BinaryEncoding
{
    private const ulong ZEROS = 0x0030_0030_0030_0030;

    /// <summary>
    /// 转换为二进制字符串
    /// </summary>
    [SkipLocalsInit]
    public static string EncodeBinary(byte[] bytes, char seperator = ' ', bool trimStart = false)
    {
        var result = new StringBuilder(bytes.Length * 9);
        Span<char> span = stackalloc char[8];
        ulong_long zeros = new(ZEROS, ZEROS);
        ref ulong_long reference = ref Unsafe.As<char, ulong_long>(ref span.DangerousGetReference());

        foreach (var value in bytes)
        {
            var binary = Convert.ToString(value, 2);
            if (trimStart)
                result.Append(binary);
            else
            {
                reference = zeros;
                binary.CopyTo(span[^binary.Length..]);
                result.Append(span);
            }
            result.Append(seperator);
        }
        return result.RemoveLast().ToString();
    }

    /// <summary>
    /// 从二进制字符串转换
    /// </summary>
    public static byte[] DecodeBinary(string text, char seperator = ' ')
    {
        var binarys = text.Split(seperator);
        var result = new byte[binarys.Length];
        var span = result.AsSpan();
        for (int i = 0; i < binarys.Length; i++)
            span[i] = Convert.ToByte(binarys[i], 2);
        return result;
    }

    /// <summary>
    /// 转换为四进制字符串
    /// </summary>
    [SkipLocalsInit]
    public static string EncodeQuater(byte[] bytes, char seperator = ' ', bool trimStart = false)
    {
        var result = new StringBuilder(bytes.Length * 5);
        Span<char> span = stackalloc char[4];
        ref ulong reference = ref Unsafe.As<char, ulong>(ref span.DangerousGetReference());
        foreach (var value in bytes)
        {
            var binary = BaseConverter.ToBase4(value);
            if (trimStart)
                result.Append(binary);
            else
            {
                reference = ZEROS;
                binary.CopyTo(span[^binary.Length..]);
                result.Append(span);
            }
            result.Append(seperator);
        }
        return result.RemoveLast().ToString();
    }

    /// <summary>
    /// 从四进制字符串转换
    /// </summary>
    public static byte[] DecodeQuater(string text, char seperator = ' ')
    {
        var binarys = text.Split(seperator);
        var result = new byte[binarys.Length];
        var span = result.AsSpan();
        for (int i = 0; i < binarys.Length; i++)
            span[i] = BaseConverter.FromBase4(binarys[i]);
        return result;
    }

    /// <summary>
    /// 转换为八进制字符串
    /// </summary>
    [SkipLocalsInit]
    public static string EncodeOctal(byte[] bytes, char seperator = ' ', bool trimStart = false)
    {
        var result = new StringBuilder(bytes.Length * 4);
        Span<char> span = stackalloc char[4];
        ref ulong reference = ref Unsafe.As<char, ulong>(ref span.DangerousGetReference());
        foreach (var value in bytes)
        {
            var binary = Convert.ToString(value, 8);
            if (trimStart)
                result.Append(binary);
            else
            {
                reference = ZEROS;
                binary.CopyTo(span[^binary.Length..]);
                result.Append(span[1..]);
            }
            result.Append(seperator);
        }
        return result.RemoveLast().ToString();
    }

    /// <summary>
    /// 从八进制字符串转换
    /// </summary>
    public static byte[] DecodeOctal(string text, char seperator = ' ')
    {
        var binarys = text.Split(seperator);
        var result = new byte[binarys.Length];
        var span = result.AsSpan();
        for (int i = 0; i < binarys.Length; i++)
            span[i] = Convert.ToByte(binarys[i], 8);
        return result;
    }
}
