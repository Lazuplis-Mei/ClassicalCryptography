using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// Base100编码
/// </summary>
/// <remarks>
/// 使用Emoji表情符号编码字节数据
/// <list type="bullet">
///     <item>
///         <term>原始仓库</term>
///         <description>
///             <see href="https://github.com/AdamNiederer/base100">github/AdamNiederer/base100</see>
///         </description>
///     </item>
///     <item>
///         <term>在线工具</term>
///         <description>
///             <see href="https://ctf.bugku.com/tool/base100">bugku/base100</see>
///         </description>
///     </item>
/// </list>
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
        Span<byte> emojiBytes = length.CanAlloc() ? stackalloc byte[length] : new byte[length];
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
        for (int i = 0, temp = 0; i < emojiBytes.Length; i++)
        {
            if ((i & 0B11) == 2)
                temp = (byte)((emojiBytes[i] - 0x8F) << 6);
            else if ((i & 0B11) == 3)
                bytes[i >> 2] = (byte)(emojiBytes[i] - 0xB7 + temp);
        }
        return bytes;
    }
}
