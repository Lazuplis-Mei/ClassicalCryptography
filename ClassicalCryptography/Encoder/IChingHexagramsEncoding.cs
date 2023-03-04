using static ClassicalCryptography.Replacement.CommonTables;
namespace ClassicalCryptography.Encoder;

/// <summary>
/// Convert binary data to a string of <see href="https://github.com/qntm/hexagram-encode"/>
/// </summary>
public static class IChingHexagramsEncoding
{
    /// <summary>
    /// encode
    /// </summary>
    public static string Encode(byte[] bytes)
    {
        return IChingHexagramsBase64.Encrypt(Convert.ToBase64String(bytes));
    }

    /// <summary>
    /// decode
    /// </summary>
    public static byte[] Decode(string str)
    {
        return Convert.FromBase64String(IChingHexagramsBase64.Decrypt(str));
    }
}