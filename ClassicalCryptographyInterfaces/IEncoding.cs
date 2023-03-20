namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 编码接口
/// </summary>
public interface IEncoding
{
    /// <summary>
    /// 编码字节数组为字符串
    /// </summary>
    static abstract string Encode(byte[] bytes);

    /// <summary>
    /// 字符串解码为字节数组
    /// </summary>
    static abstract byte[] Decode(string encodeText);
}
