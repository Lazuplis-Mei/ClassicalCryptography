using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Undefined;

/// <summary>
/// 帕斯卡谜题密码
/// </summary>
/// <remarks>
/// 请参考: <see href="https://www.youtube.com/watch?v=9JN5f7_3YmQ">youtube/9JN5f7_3YmQ</see><br/>
/// 对3进制数组进行某种特定的转换
/// </remarks>
[Introduction("帕斯卡谜题密码", "对3进制数组进行某种特定的转换")]
public static class PascalianPuzzleCipher
{
    private const int BYTE_2X = 0x200;
    private const long INT32_2X = 0x200000000;

    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 加密/解密数据，使用3进制
    /// </summary>
    public static byte[] Transform(Span<byte> bytes)
    {
        Guard.HasSizeLessThanOrEqualTo(bytes, 0x1000);
        var array = BaseConverter.ToBase3(bytes);

        for (int j = 0; j < array.Count - 1; j++)
            for (int i = array.Count - 1; i > j; i--)
                array[i] = (6 - (array[i] + array[i - 1])) % 3;

        return BaseConverter.FromBase3(array);
    }

    /// <summary>
    /// 加密/解密数据
    /// </summary>
    public static void TransformBytes(Span<byte> bytes)
    {
        Guard.HasSizeLessThanOrEqualTo(bytes, 0xA000);
        for (int j = 0; j < bytes.Length - 1; j++)
            for (int i = bytes.Length - 1; i > j; i--)
                bytes[i] = (byte)(BYTE_2X - (bytes[i] + bytes[i - 1]));
    }

    /// <summary>
    /// 加密数据一轮
    /// </summary>
    public static void TransformBytesOnce(Span<byte> bytes)
    {
        for (int i = bytes.Length - 1; i > 0; i--)
            bytes[i] = (byte)(BYTE_2X - (bytes[i] + bytes[i - 1]));
    }

    /// <summary>
    /// 解密数据一轮
    /// </summary>
    public static void TransformBytesOnceInverse(Span<byte> bytes)
    {
        for (int i = 1; i < bytes.Length; i++)
            bytes[i] = (byte)(BYTE_2X - (bytes[i] + bytes[i - 1]));
    }

    /// <summary>
    /// 加密/解密数据
    /// </summary>
    public static void TransformInts(Span<uint> ints)
    {
        Guard.HasSizeLessThanOrEqualTo(ints, 0xA000);
        for (int j = 0; j < ints.Length - 1; j++)
            for (int i = ints.Length - 1; i > j; i--)
                ints[i] = (uint)(INT32_2X - (ints[i] + ints[i - 1]));
    }

    /// <summary>
    /// 加密数据一轮
    /// </summary>
    public static void TransformIntsOnce(Span<uint> ints)
    {
        for (int i = ints.Length - 1; i > 0; i--)
            ints[i] = (uint)(INT32_2X - (ints[i] + ints[i - 1]));
    }

    /// <summary>
    /// 解密数据一轮
    /// </summary>
    public static void TransformIntsOnceInverse(Span<uint> ints)
    {
        for (int i = 1; i < ints.Length; i++)
            ints[i] = (uint)(INT32_2X - (ints[i] + ints[i - 1]));
    }

    /// <summary>
    /// 编码字符串为字节数组
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] EncodeStrng(string text)
    {
        var bytes = Encoding.GetBytes(text);
        TransformBytes(bytes);
        return bytes;
    }

    /// <summary>
    /// 解码字节数组为字符串
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DecodeBytes(byte[] bytes)
    {
        TransformBytes(bytes);
        return Encoding.GetString(bytes);
    }
}
