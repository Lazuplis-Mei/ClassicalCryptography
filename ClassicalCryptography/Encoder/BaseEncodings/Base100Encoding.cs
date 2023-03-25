using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// Base100编码，又称Emoji表情符号编码<br/>
/// </summary>
/// <remarks>
/// <a href="https://github.com/AdamNiederer/base100">原始的代码实现</a><br/>
/// 在线工具:<a href="https://ctf.bugku.com/tool/base100">Base100</a>
/// </remarks>
[Introduction("Base100编码", "将字节编码为Emoji表情符号")]
[ReferenceFrom("https://github.com/stek29/base100/blob/master/base100.py", ProgramingLanguage.Python, License.Unlicense)]
public class Base100Encoding : IEncoding
{
    /// <inheritdoc/>
    [SkipLocalsInit]
    public static string Encode(byte[] bytes)
    {
        int length = bytes.Length << 2;
        Span<byte> emojiBytes = length.CanAllocate() ? stackalloc byte[length] : new byte[length];
        for (int i = 0; i < bytes.Length; i++)
        {
            int j = i << 2;
            emojiBytes[j++] = 0xF0;
            emojiBytes[j++] = 0x9F;
            (int quotient, int remainder) = int.DivRem(bytes[i] + 55, 0x40);
            emojiBytes[j++] = (byte)(quotient + 0x8F);
            emojiBytes[j] = (byte)(remainder + 0x80);
        }
        return Encoding.UTF8.GetString(emojiBytes);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// 该方法不包括严格的检查
    /// </remarks>
    public static byte[] Decode(string encodeText)
    {
        var emojiBytes = Encoding.UTF8.GetBytes(encodeText);
        Guard.IsEqualTo(emojiBytes.Length & 0B11, 0);

        var bytes = new byte[emojiBytes.Length >> 2];
        int temp = 0;
        for (int i = 0; i < emojiBytes.Length; i++)
        {
            if ((i & 0B11) == 2)
                temp = (byte)((emojiBytes[i] - 0x8F) << 6);
            else if ((i & 0B11) == 3)
                bytes[i >> 2] = (byte)(emojiBytes[i] - 0xB7 + temp);
        }
        return bytes;
    }
}
