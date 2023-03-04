using System;
using System.Text;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// 使用Emoji表情符号编码
/// </summary>
public static class Base100Encoding
{
    /// <summary>
    /// EncodeBase100
    /// </summary>
    public static string Encode(byte[] bytes)
    {
        byte[] strBytes = new byte[4 * bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            strBytes[4 * i] = 0xF0;
            strBytes[4 * i + 1] = 0x9F;
            strBytes[4 * i + 2] = (byte)((bytes[i] + 55) / 0x40 + 0x8F);
            strBytes[4 * i + 3] = (byte)((bytes[i] + 55) % 0x40 + 0x80);
        }
        return Encoding.UTF8.GetString(strBytes);
    }

    /// <summary>
    /// DecodeBase100
    /// </summary>
    public static byte[] Decode(string emojis)
    {
        var data = Encoding.UTF8.GetBytes(emojis);
        if (data.Length % 4 != 0)
            throw new Exception("Length of string should be divisible by 4");
        var bytes = new byte[data.Length / 4];
        for (int i = 0, t = 0; i < data.Length; i++)
        {
            if (i % 4 == 2)
                t = (data[i] - 0x8F) * 0x40 % 0x100;
            else if (i % 4 == 3)
                bytes[i / 4] = (byte)((data[i] - 0x80 + t - 55) & 0xFF);
        }
        return bytes;
    }

}