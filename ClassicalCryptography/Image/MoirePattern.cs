using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.Versioning;
using System.Runtime.CompilerServices;

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
    public static Color Foreground { get; set; } = Color.Black;

    /// <summary>
    /// 前景颜色2
    /// </summary>
    public static Color Foreground2 { get; set; } = Color.FromArgb(255, 20, 20, 20);
    /// <summary>
    /// 背景颜色
    /// </summary>
    public static Color Background { get; set; } = Color.White;

    /// <summary>
    /// 使用的字体
    /// </summary>
    public static Font Font { get; set; } = new Font("微软雅黑", 24);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetCenterPoint(ref this RectangleF rect, Bitmap bitmap)
    {
        var centerX = (bitmap.Width - rect.Width) / 2;
        var centerY = (bitmap.Height - rect.Height) / 2;
        rect.Location = new PointF(centerX, centerY);
    }

    /// <summary>
    /// 绘制单个字符串
    /// </summary>
    /// <param name="text">要绘制的文字</param>
    /// <param name="imageWidth">图像宽度</param>
    /// <param name="imageHeight">图像高度</param>
    /// <param name="pattenType">条纹模式</param>
    /// <param name="embedded">是否使用嵌入式的条纹</param>
    /// <returns>带有条纹的图形</returns>
    public unsafe static Bitmap DrawText(string text, int imageWidth, int imageHeight, Func<int, int, bool> pattenType, bool embedded = false)
    {
        var bitmap = new Bitmap(imageWidth, imageHeight);
        using var graphics = Graphics.FromImage(bitmap);
        var textSize = graphics.MeasureString(text, Font);
        var rectF = new RectangleF(Point.Empty, textSize);
        rectF.SetCenterPoint(bitmap);
        graphics.DrawString(text, Font, Brushes.Black, rectF);


        var background = Background.ToArgb();
        var foreground = Foreground.ToArgb();
        var foreground2 = Foreground2.ToArgb();

        var rect = new Rectangle(0, 0, imageWidth, imageHeight);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var dataSpan = new Span<int>((void*)data.Scan0, imageWidth * imageHeight);

        for (int x = 0; x < imageWidth; x++)
        {
            for (int y = 0; y < imageHeight; y++)
            {
                if (pattenType(x, y))
                {
                    if (embedded)
                    {
                        if (dataSpan[x + y * imageWidth] == 0)
                            dataSpan[x + y * imageWidth] = foreground2;
                        continue;
                    }
                    if (dataSpan[x + y * imageWidth] == 0)
                        dataSpan[x + y * imageWidth] = foreground;
                    else
                        dataSpan[x + y * imageWidth] = background;
                }
                else
                {
                    if (embedded)
                    {
                        dataSpan[x + y * imageWidth] = background;
                        continue;
                    }
                    if (dataSpan[x + y * imageWidth] == 0)
                        dataSpan[x + y * imageWidth] = background;
                    else
                        dataSpan[x + y * imageWidth] = foreground;
                }
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }

    /// <summary>
    /// 给单个字符串填充条纹
    /// </summary>
    /// <param name="bitmap">要填充的图像</param>
    /// <param name="pattenType">条纹类型</param>
    /// <param name="embedded">是否是嵌入式</param>
    /// <param name="removePatten">是否移除条纹(仅对非嵌入式有效)</param>
    public unsafe static Bitmap FillPatten(Bitmap bitmap, Func<int, int, bool> pattenType, bool embedded = false, bool removePatten = false)
    {
        var background = Background.ToArgb();
        var foreground = Foreground.ToArgb();
        int imageWidth = bitmap.Width;
        int imageHeight = bitmap.Height;

        var rect = new Rectangle(0, 0, imageWidth, imageHeight);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var dataSpan = new Span<int>((void*)data.Scan0, imageWidth * imageHeight);

        for (int x = 0; x < imageWidth; x++)
        {
            for (int y = 0; y < imageHeight; y++)
            {
                if (embedded)
                {
                    if (dataSpan[x + y * imageWidth] != foreground)
                        dataSpan[x + y * imageWidth] = background;
                }
                else if (pattenType(x, y))
                {
                    if (dataSpan[x + y * imageWidth] != foreground)
                        dataSpan[x + y * imageWidth] = foreground;
                    else if (removePatten)
                        dataSpan[x + y * imageWidth] = background;
                }
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }

    /// <summary>
    /// 在条纹缝隙中绘制多个文字
    /// </summary>
    public unsafe static Bitmap DrawTexts(string[] texts, int imageWidth, int imageHeight)
    {
        Guard.HasSizeGreaterThan(texts, 1);

        var bitmap = new Bitmap(imageWidth, imageHeight);
        using var graphics = Graphics.FromImage(bitmap);

        var brush = new SolidBrush(Foreground);
        var textSize = graphics.MeasureString(texts[0], Font);
        var rectF = new RectangleF(Point.Empty, textSize);
        rectF.SetCenterPoint(bitmap);
        graphics.Clear(Background);
        graphics.DrawString(texts[0], Font, brush, rectF);

        var rect = new Rectangle(0, 0, imageWidth, imageHeight);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var dataSpan = new Span<int>((void*)data.Scan0, imageWidth * imageHeight);

        using var bufferBitmap = new Bitmap(imageWidth, imageHeight);
        using var bufferGraphics = Graphics.FromImage(bufferBitmap);

        for (int i = 1; i < texts.Length; i++)
        {
            textSize = bufferGraphics.MeasureString(texts[i], Font);
            rectF = new RectangleF(Point.Empty, textSize);
            rectF.SetCenterPoint(bufferBitmap);
            bufferGraphics.Clear(Background);
            bufferGraphics.DrawString(texts[i], Font, brush, rectF);

            var bufferData = bufferBitmap.LockBits(rect, ImageLockMode.ReadWrite, bufferBitmap.PixelFormat);
            var bufferDataSpan = new Span<int>((void*)bufferData.Scan0, imageWidth * imageHeight);

            for (int x = 0; x < imageWidth; x++)
                for (int y = 0; y < imageHeight; y++)
                    if (x % texts.Length == i)
                        dataSpan[x + y * imageWidth] = bufferDataSpan[x + y * imageWidth];

            bufferBitmap.UnlockBits(bufferData);
        }
        bitmap.UnlockBits(data);
        return bitmap;
    }

    /// <summary>
    /// 偏移填充多个条纹中的汉字
    /// </summary>
    /// <param name="bitmap">原始图像</param>
    /// <param name="count">偏移次数</param>
    public static Bitmap[] FillPattens(Bitmap bitmap, int count)
    {
        var result = new Bitmap[count];
        for (int i = 0; i < count; i++)
            result[i] = FillPatten(new Bitmap(bitmap), (x, _) => (x - i + count) % count > 0);
        return result;
    }

    /// <summary>
    /// 偏移填充多个条纹中的汉字并保存
    /// </summary>
    /// <param name="bitmap">原始图像</param>
    /// <param name="count">偏移次数</param>
    /// <param name="path">保存到的文件夹</param>
    public static void FillAndSavePattens(Bitmap bitmap, int count, string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        var hashCode = bitmap.GetHashCode();
        var bitmaps = FillPattens(bitmap, count);
        for (int i = 0; i < bitmaps.Length; i++)
            bitmaps[i].Save(Path.Combine(path, $"{hashCode}_{i}.png"));

    }

}
