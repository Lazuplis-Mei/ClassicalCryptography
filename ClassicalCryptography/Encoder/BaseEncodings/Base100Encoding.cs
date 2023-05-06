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
        int size = bytes.Length * 4;
        using var memory = size.TryAlloc();
        Span<byte> buffer = size.CanAlloc() ? stackalloc byte[size] : memory.Span;
        for (int i = 0; i < bytes.Length; i++)
        {
            int j = 4 * i;
            buffer[j++] = 0xF0;
            buffer[j++] = 0x9F;
            (int quotient, int remainder) = int.DivRem(bytes[i] + 55, 0x40);
            buffer[j++] = (byte)(quotient + 0x8F);
            buffer[j] = (byte)(remainder + 0x80);
        }
        return Encoding.UTF8.GetString(buffer);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// 该方法不包括严格的检查
    /// </remarks>
    public static byte[] Decode(string encodeText)
    {
        int count = Encoding.UTF8.GetByteCount(encodeText);
        Guard.IsEqualTo(count % 4, 0);

        using var memory = count.TryAlloc();
        Span<byte> buffer = count.CanAlloc() ? stackalloc byte[count] : memory.Span;
        Encoding.UTF8.GetBytes(encodeText, buffer);
        for (int i = 0, temp = 0; i < buffer.Length; i++)
        {
            if (i % 4 == 2)
                temp = (byte)((buffer[i] - 0x8F) << 6);
            else if (i % 4 == 3)
                buffer[i / 4] = (byte)(buffer[i] - 0xB7 + temp);
        }
        return buffer[..(count / 4)].ToArray();
    }
}
