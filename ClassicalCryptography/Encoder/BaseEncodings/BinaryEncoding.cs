namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// 进制转换(2,4,8)进制
/// </summary>
[Introduction("进制转换", "提供(2,4,8)进制的转换")]
public class BinaryEncoding
{
    /// <summary>
    /// 转换为二进制字符串
    /// </summary>
    public static string EncodeBinary(byte[] bytes, char seperator = ' ', bool trimStart = false)
    {
        var result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            var binary = Convert.ToString(bytes[i], 2);
            result.Append(trimStart ? binary : binary.PadLeft(8, '0'));
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
        for (int i = 0; i < binarys.Length; i++)
            result[i] = Convert.ToByte(binarys[i], 2);
        return result;
    }

    /// <summary>
    /// 转换为四进制字符串
    /// </summary>
    public static string EncodeQuater(byte[] bytes, char seperator = ' ', bool trimStart = false)
    {
        var result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            var binary = BaseConverter.ToBase4(bytes[i]);
            result.Append(trimStart ? binary : binary.PadLeft(4, '0'));
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
        for (int i = 0; i < binarys.Length; i++)
            result[i] = BaseConverter.FromBase4(binarys[i]);
        return result;
    }

    /// <summary>
    /// 转换为八进制字符串
    /// </summary>
    public static string EncodeOctal(byte[] bytes, char seperator = ' ', bool trimStart = false)
    {
        var result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            var binary = Convert.ToString(bytes[i], 8);
            result.Append(trimStart ? binary : binary.PadLeft(3, '0'));
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
        for (int i = 0; i < binarys.Length; i++)
            result[i] = Convert.ToByte(binarys[i], 8);
        return result;
    }
}
