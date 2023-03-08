using ClassicalCryptography.Interfaces;
using System.Text;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <para>Base100编码，又称Emoji表情符号编码</para>
/// <para>原始的代码仓库可以在这里查看</para>
/// <seealso href="https://github.com/AdamNiederer/base100"/>
/// <para>参考的代码片段</para>
/// <see href="https://github.com/stek29/base100/blob/master/base100.py"/>
/// <para>在线的工具</para>
/// <seealso href="https://ctf.bugku.com/tool/base100"/>
/// </summary>
[Introduction("Base100编码", "将字节编码为Emoji表情符号")]
[TranslatedFrom("Python")]
public static class Base100Encoding
{

    /// <summary>
    /// 将字节编码为Emoji表情符号
    /// </summary>
    /// <param name="bytes">要编码的字节</param>
    public static string Encode(byte[] bytes)
    {
        byte[] emojiBytes = new byte[bytes.Length << 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            int j = i << 2;
            emojiBytes[j++] = 0xF0;
            emojiBytes[j++] = 0x9F;
            (int quotient, int remainder) = int.DivRem((bytes[i] + 55), 0x40);
            emojiBytes[j++] = (byte)(quotient + 0x8F);
            emojiBytes[j++] = (byte)(remainder + 0x80);
        }
        return Encoding.UTF8.GetString(emojiBytes);
    }

    /// <summary>
    /// 解码Emoji表情符号(不包含严格的检查)
    /// </summary>
    /// <param name="emojiString">Emoji表情符号</param>
    public static byte[] Decode(string emojiString)
    {
        var emojiBytes = Encoding.UTF8.GetBytes(emojiString);

        if ((emojiBytes.Length & 0B11) != 0)
            throw new ArgumentException("字符串的字节数应为4的倍数", nameof(emojiString));

        var bytes = new byte[emojiBytes.Length >> 2];

        for (int i = 0, temp = 0; i < emojiBytes.Length; i++)
        {
            if ((i & 0B11) == 2)
                temp = ((emojiBytes[i] - 0x8F) * 0x40) % 0x100;
            else if ((i & 0B11) == 3)
                bytes[i >> 2] = (byte)(emojiBytes[i] - 0xB7 + temp);
        }
        return bytes;
    }

}