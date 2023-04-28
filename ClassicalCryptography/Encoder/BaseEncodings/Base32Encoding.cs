namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// Base32编码(RFC4648)
/// </summary>
/// <remarks>
/// 使用字母表为ABCDEFGHIJKLMNOPQRSTUVWXYZ234567编码字节数据
/// <list type="bullet">
///     <item>
///         <term>参考资料</term>
///         <description>
///             <see href="https://en.wikipedia.org/wiki/Base32">wikipedia/Base32</see>
///         </description>
///     </item>
///     <item>
///         <term>在线工具</term>
///         <description>
///             <see href="https://www.qqxiuzi.cn/bianma/base.php">qqxiuzi/base</see>
///         </description>
///     </item>
/// </list>
/// </remarks>
[ReferenceFrom("https://stackoverflow.com/questions/641361/base32-decoding#answer-7135008")]
[Introduction("Base32编码", "使用字母表为ABCDEFGHIJKLMNOPQRSTUVWXYZ234567")]
public class Base32Encoding : IEncoding
{
    /// <inheritdoc/>
    public static byte[] Decode(string encodeText)
    {
        Guard.IsNotNullOrEmpty(encodeText);

        var base32 = encodeText.AsSpan().TrimEnd('=');
        var bytes = new byte[(base32.Length * 5 / 8)];
        var span = bytes.AsSpan();

        byte currentByte = 0, bitsRemaining = 8;
        int index = 0;

        foreach (var character in base32)
        {
            int mask, value = CharToValue(character);
            if (bitsRemaining > 5)
            {
                mask = value << (bitsRemaining -= 5);
                currentByte = (byte)(currentByte | mask);
            }
            else
            {
                mask = value >> (5 - bitsRemaining);
                span[index++] = (byte)(currentByte | mask);
                currentByte = (byte)(value << (bitsRemaining += 3));
            }
        }

        if (index != base32.Length * 5 / 8)
            span[index] = currentByte;

        return bytes;
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static string Encode(byte[] bytes)
    {
        Guard.IsNotNull(bytes);
        Guard.HasSizeGreaterThan(bytes, 0);
        
        int length = bytes.Length.DivCeil(5) * 8;
        Span<char> span = length.CanAllocString() ? stackalloc char[length] : new char[length];

        byte nextCode = 0, bitsRemaining = 5;
        int index = 0;

        foreach (var value in bytes)
        {
            nextCode = (byte)(nextCode | value >> (8 - bitsRemaining));
            span[index++] = ValueToChar(nextCode);

            if (bitsRemaining < 4)
            {
                nextCode = (byte)((value >> (3 - bitsRemaining)) & 0B11111);
                span[index++] = ValueToChar(nextCode);
                bitsRemaining += 5;
            }

            bitsRemaining -= 3;
            nextCode = (byte)((value << bitsRemaining) & 0B11111);
        }

        if (index != length)
        {
            span[index++] = ValueToChar(nextCode);
            span[index..].Fill('=');
        }

        return new(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CharToValue(char character) => character switch
    {
        >= 'A' and <= 'Z' => character - 'A',
        >= 'a' and <= 'z' => character - 'a',
        >= '2' and <= '7' => character - '2' + 26,
        _ => throw new ArgumentException("非Base32字符", nameof(character))
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char ValueToChar(byte byteCode) => Base32_RFC4648[byteCode];
}
