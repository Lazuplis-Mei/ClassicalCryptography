using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// Base32编码，使用字母表为ABCDEFGHIJKLMNOPQRSTUVWXYZ234567
/// </summary>
/// <remarks>
/// 在线工具:<a href="https://www.qqxiuzi.cn/bianma/base.php">Base32</a>
/// </remarks>
[ReferenceFrom("https://stackoverflow.com/questions/641361/base32-decoding#answer-7135008")]
[Introduction("Base32编码", "使用字母表为ABCDEFGHIJKLMNOPQRSTUVWXYZ234567")]
public class Base32Encoding : IEncoding
{
    /// <inheritdoc/>
    public static byte[] Decode(string encodeText)
    {
        Guard.IsNotNullOrEmpty(encodeText);

        var base32String = encodeText.TrimEnd('=');
        int byteCount = base32String.Length;
        byteCount += byteCount << 2;
        byteCount >>= 3;
        var bytes = new byte[byteCount];

        byte currentByte = 0, bitsRemaining = 8;
        int index = 0;

        foreach (char character in base32String)
        {
            int characterValue = CharToValue(character);
            int mask;
            if (bitsRemaining > 5)
            {
                mask = characterValue << bitsRemaining - 5;
                currentByte = (byte)(currentByte | mask);
                bitsRemaining -= 5;
            }
            else
            {
                mask = characterValue >> 5 - bitsRemaining;
                currentByte = (byte)(currentByte | mask);
                bytes[index++] = currentByte;
                currentByte = (byte)(characterValue << 3 + bitsRemaining);
                bitsRemaining += 3;
            }
        }

        if (index != byteCount)
            bytes[index] = currentByte;

        return bytes;
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static string Encode(byte[] bytes)
    {
        Guard.IsNotNull(bytes);
        Guard.HasSizeGreaterThan(bytes, 0);

        int length = bytes.Length.DivCeil(5) << 3;
        Span<char> span = length.CanAllocateString() ? stackalloc char[length] : new char[length];

        byte nextCharacterCode = 0, bitsRemaining = 5;
        int index = 0;

        foreach (byte b in bytes)
        {
            nextCharacterCode = (byte)(nextCharacterCode | b >> 8 - bitsRemaining);
            span[index++] = ValueToChar(nextCharacterCode);

            if (bitsRemaining < 4)
            {
                nextCharacterCode = (byte)(b >> 3 - bitsRemaining & 31);
                span[index++] = ValueToChar(nextCharacterCode);
                bitsRemaining += 5;
            }

            bitsRemaining -= 3;
            nextCharacterCode = (byte)(b << bitsRemaining & 31);
        }

        if (index != length)
        {
            span[index++] = ValueToChar(nextCharacterCode);
            while (index != length)
                span[index++] = '=';
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
    private static char ValueToChar(byte byteCode)
    {
        return GlobalTables.Base32_RFC3548[byteCode];
    }
}
