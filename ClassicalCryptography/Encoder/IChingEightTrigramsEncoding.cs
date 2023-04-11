using System.Runtime.CompilerServices;
using static ClassicalCryptography.Replacement.CommonTables;
namespace ClassicalCryptography.Encoder;

/// <summary>
/// 用易经八卦代替Base64
/// </summary>
public class IChingEightTrigramsEncoding : IEncoding
{
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Encode(byte[] bytes)
    {
        return IChingEightTrigramsBase64.Encrypt(K4os.Text.BaseX.Base64.ToBase64(bytes));
    }

    /// <summary>
    /// 易经八卦编码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EncodeString(string text)
    {
        return Encode(Encoding.GetBytes(text));
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] Decode(string text)
    {
        return K4os.Text.BaseX.Base64.FromBase64(IChingEightTrigramsBase64.Decrypt(text));
    }

    /// <summary>
    /// 易经八卦解码
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DecodeString(string text)
    {
        return Encoding.GetString(Decode(text));
    }

}