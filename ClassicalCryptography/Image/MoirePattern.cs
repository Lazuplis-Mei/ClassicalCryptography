using ClassicalCryptography.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Image;


/// <summary>
/// 摩尔纹密码
/// </summary>
[Introduction("摩尔纹", "通过密集的条形图案隐藏信息")]
[SupportedOSPlatform("windows")]
public static class MoirePattern
{
    /// <summary>
    /// 图形密码
    /// </summary>
    public static CipherType Type => CipherType.Image;

    /// <summary>
    /// 前景颜色
    /// </summary>
    public static Brush Foreground { get; set; } = Brushes.Black;
    /// <summary>
    /// 背景颜色
    /// </summary>
    public static Brush Background { get; set; } = Brushes.White;
    /// <summary>
    /// 图片保存的格式
    /// </summary>
    public static ImageFormat ImgFormat { get; set; } = ImageFormat.Png;

    /// <summary>
    /// 使用的字体
    /// </summary>
    public static Font Font { get; set; } = new Font("Cascadia Mono", 32);

    private const int patternWidth = 16;

    /// <summary>
    /// 绘制单次模糊边缘的文字
    /// </summary>
    public static Bitmap DrawText(string text)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 填充单次模糊边缘的文字
    /// </summary>
    public static Bitmap FillPatten(Bitmap patten)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 在条纹缝隙中绘制多个文字
    /// </summary>
    public static Bitmap DrawTexts(params string[] texts)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 偏移填充多次条纹中的汉字
    /// </summary>
    public static Bitmap[] FillPattens(Bitmap patten)
    {
        throw new NotImplementedException();
    }

}
