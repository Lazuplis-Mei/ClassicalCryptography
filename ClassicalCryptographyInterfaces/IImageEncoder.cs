using System.Drawing;

namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 图像编码器
/// </summary>
public interface IImageEncoder<T>
{
    /// <summary>
    /// 图形相关的
    /// </summary>
    static CipherType Type => CipherType.Image;

    /// <summary>
    /// 编码图像
    /// </summary>
    static abstract Bitmap Encode(T plain);
}
