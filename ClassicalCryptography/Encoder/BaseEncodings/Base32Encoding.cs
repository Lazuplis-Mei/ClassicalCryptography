using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// <para>Base32编码，使用字母表为ABCDEFGHIJKLMNOPQRSTUVWXYZ234567</para>
/// <para>代码参考</para>
/// <see href="https://stackoverflow.com/questions/641361/base32-decoding#answer-7135008"/>
/// <para>在线工具</para>
/// <see href="https://www.qqxiuzi.cn/bianma/base.php"/>
/// </summary>
public static class Base32Encoding
{

    /// <summary>
    /// 解码Base32
    /// </summary>
    /// <param name="base32String">Base32字符串</param>
    public static byte[] Decode(string base32String)
    {
        if (string.IsNullOrEmpty(base32String))
            throw new ArgumentNullException(nameof(base32String));

        var base32Span = base32String;
        base32Span = base32Span.TrimEnd('=');
        int byteCount = (base32Span.Length * 5) >> 3;
        var bytes = new byte[byteCount];

        byte currentByte = 0, bitsRemaining = 8;
        int arrayIndex = 0;

        foreach (char character in base32Span)
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
                bytes[arrayIndex++] = currentByte;
                currentByte = (byte)(characterValue << 3 + bitsRemaining);
                bitsRemaining += 3;
            }
        }

        if (arrayIndex != byteCount)
            bytes[arrayIndex] = currentByte;

        return bytes;
    }

    /// <summary>
    /// 编码Base32
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>base32字符串</returns>
    [SkipLocalsInit]
    public static string Encode(byte[] bytes)
    {
        if (bytes is null || bytes.Length == 0)
            throw new ArgumentNullException(nameof(bytes));

        int spanCount = (int)Math.Ceiling(bytes.Length / 5.0) * 8;
        Span<char> span = spanCount <= StackLimit.MaxCharSize
            ? stackalloc char[spanCount] : new char[spanCount];

        byte nextCharacterCode = 0, bitsRemaining = 5;
        int arrayIndex = 0;

        foreach (byte b in bytes)
        {
            nextCharacterCode = (byte)(nextCharacterCode | b >> 8 - bitsRemaining);
            span[arrayIndex++] = ValueToChar(nextCharacterCode);

            if (bitsRemaining < 4)
            {
                nextCharacterCode = (byte)(b >> 3 - bitsRemaining & 31);
                span[arrayIndex++] = ValueToChar(nextCharacterCode);
                bitsRemaining += 5;
            }

            bitsRemaining -= 3;
            nextCharacterCode = (byte)(b << bitsRemaining & 31);
        }

        if (arrayIndex != spanCount)
        {
            span[arrayIndex++] = ValueToChar(nextCharacterCode);
            while (arrayIndex != spanCount)
                span[arrayIndex++] = '=';
        }

        return new(span);
    }

    private static int CharToValue(char character)
    {
        if (Debugger.IsAttached)
        {
            int value = GlobalTables.Base32_RFC3548.IndexOf(character);
            if (value == -1)
                throw new ArgumentException("非Base32字符", nameof(character));
            return value;
        }
        else
        {
            int value = character;

            return value switch
            {
                >= 'A' and <= 'Z' => value - 'A',
                >= '2' and <= '7' => value - '2' + 26,
                >= 'a' and <= 'z' => value - 'a',
                _ => throw new ArgumentException("非Base32字符", nameof(character))
            };
        }
    }

    private static char ValueToChar(byte byteCode)
    {
        return GlobalTables.Base32_RFC3548[byteCode];
    }

}