using System.Diagnostics;

namespace ClassicalCryptography.Undefined;


/// <summary>
/// <see href="https://www.youtube.com/watch?v=9JN5f7_3YmQ">帕斯卡谜题密码</see>
/// </summary>
[Introduction("帕斯卡谜题密码", "https://www.youtube.com/watch?v=9JN5f7_3YmQ")]
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
    /// 编码字节
    /// </summary>
    public static byte[] Encode(string text)
    {
        return Transform(Encoding.GetBytes(text));
    }

    /// <summary>
    /// 解码字节
    /// </summary>
    public static string Decode(byte[] bytes)
    {
        return Encoding.GetString(Transform(bytes));
    }

}
