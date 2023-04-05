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
    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 加密/解密数据
    /// </summary>
    public static byte[] Transform(byte[] bytes)
    {
        var array = BaseConverter.ToBase3(bytes);

        for (int j = 0; j < array.Count - 1; j++)
            for (int i = array.Count - 1; i > j; i--)
                array[i] = (6 - (array[i] + array[i - 1])) % 3;

        return BaseConverter.FromBase3(array);
    }

    /// <summary>
    /// 编码字符串为字节数组
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] EncodeStrng(string text) => Transform(Encoding.GetBytes(text));

    /// <summary>
    /// 解码字节数组为字符串
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DecodeBytes(byte[] bytes) => Encoding.GetString(Transform(bytes));
}
