using System.Text;
using static ClassicalCryptography.Replacement.CommonTables;
namespace ClassicalCryptography.Encoder;

/// <summary>
/// 用易经八卦代替Base64
/// </summary>
public static class IChingEightTrigramsEncoding
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;


    /// <summary>
    /// 易经八卦编码
    /// </summary>
    public static string EncodeBytes(byte[] bytes)
    {
        return IChingEightTrigramsBase64.Encrypt(Convert.ToBase64String(bytes));
    }

    /// <summary>
    /// 易经八卦编码
    /// </summary>
    public static string Encode(string text)
    {
        return EncodeBytes(Encoding.GetBytes(text));
    }

    /// <summary>
    /// 易经八卦解码
    /// </summary>
    public static byte[] DecodeBytes(string text)
    {
        return Convert.FromBase64String(IChingEightTrigramsBase64.Decrypt(text));
    }

    /// <summary>
    /// 易经八卦解码
    /// </summary>
    public static string Decode(string text)
    {
        return Encoding.GetString(DecodeBytes(text));
    }
}