using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Image;

/// <summary>
/// 幻影坦克
/// </summary>
[SupportedOSPlatform("windows")]
public static class PhantomTank
{
    /// <summary>
    /// 前景权重
    /// </summary>
    public const double FOREGROUND_WEIGHT = 1.0;
    /// <summary>
    /// 背景权重
    /// </summary>
    public const double BACKGROUNG_WEIGHT = 0.6;

    /// <summary>
    /// 将两张图片作为前景和背景组合
    /// </summary>
    public static unsafe Bitmap Combine(Bitmap foreGround, Bitmap backGround)
    {
        var maxWidth = Math.Max(foreGround.Width, backGround.Width);
        var maxHeight = Math.Max(foreGround.Height, backGround.Height);
        var bitmap = new Bitmap(maxWidth, maxHeight);
        foreGround = new Bitmap(foreGround, bitmap.Size);
        backGround = new Bitmap(backGround, bitmap.Size);
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var dataSpan = new Span<int>(data.Scan0.ToPointer(), bitmap.Width * bitmap.Height);
        var fdata = foreGround.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var fdataSpan = new Span<int>(fdata.Scan0.ToPointer(), bitmap.Width * bitmap.Height);
        var bdata = backGround.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var bdataSpan = new Span<int>(bdata.Scan0.ToPointer(), bitmap.Width * bitmap.Height);

        for (int i = 0; i < dataSpan.Length; i++)
        {
            double fColor = 255 * Color.FromArgb(fdataSpan[i]).GetBrightness() * FOREGROUND_WEIGHT;
            double bColor = 255 * Color.FromArgb(bdataSpan[i]).GetBrightness() * BACKGROUNG_WEIGHT;
            var alpha = Math.Min(255, (int)(255 - fColor + bColor));
            int mean = (int)Math.Min(255, bColor / alpha * 255);
            dataSpan[i] = Color.FromArgb(alpha, mean, mean, mean).ToArgb();
        }

        bitmap.UnlockBits(data);
        foreGround.Dispose();
        backGround.Dispose();
        return bitmap;
    }

    /// <summary>
    /// 将两张图片作为前景和背景组合，背景为彩图
    /// </summary>
    public static unsafe Bitmap CombineColorful(Bitmap foreGround, Bitmap backGround)
    {
        var maxWidth = Math.Max(foreGround.Width, backGround.Width);
        var maxHeight = Math.Max(foreGround.Height, backGround.Height);
        var bitmap = new Bitmap(maxWidth, maxHeight);
        foreGround = new Bitmap(foreGround, bitmap.Size);
        backGround = new Bitmap(backGround, bitmap.Size);
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var dataSpan = new Span<int>(data.Scan0.ToPointer(), bitmap.Width * bitmap.Height);
        var fdata = foreGround.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var fdataSpan = new Span<int>(fdata.Scan0.ToPointer(), bitmap.Width * bitmap.Height);
        var bdata = backGround.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var bdataSpan = new Span<int>(bdata.Scan0.ToPointer(), bitmap.Width * bitmap.Height);

        for (int i = 0; i < dataSpan.Length; i++)
        {
            var fColor = Color.FromArgb(fdataSpan[i]);
            var bColor = Color.FromArgb(bdataSpan[i]);
            int r = (int)(fColor.R * FOREGROUND_WEIGHT);
            int g = (int)(fColor.G * FOREGROUND_WEIGHT);
            int b = (int)(fColor.B * FOREGROUND_WEIGHT);
            fColor = Color.FromArgb(fColor.A, r, g, b);
            r = (int)(bColor.R * FOREGROUND_WEIGHT);
            g = (int)(bColor.G * FOREGROUND_WEIGHT);
            b = (int)(bColor.B * FOREGROUND_WEIGHT);
            bColor = Color.FromArgb(bColor.A, r, g, b);
            int dr = bColor.R - fColor.R;
            int dg = bColor.G - fColor.G;
            int db = bColor.B - fColor.B;
            double coe_a = 8 + 255 / 256.0 + (dr - db) / 256.0;
            double coe_b = 4 * dr + 8 * dg + 6 * db +
                (dr - db) * (bColor.R + fColor.R) / 256.0 +
                (dr * dr - db * db) / 512.0;
            int alpha = (int)(255 + coe_b / (2 * coe_a));
            if (alpha <= 0)
                alpha = r = g = b = 0;
            else
            {
                alpha = Math.Min(255, alpha);
                r = (int)Math.Min(255, 255 * bColor.R * BACKGROUNG_WEIGHT / alpha);
                g = (int)Math.Min(255, 255 * bColor.G * BACKGROUNG_WEIGHT / alpha);
                b = (int)Math.Min(255, 255 * bColor.B * BACKGROUNG_WEIGHT / alpha);
            }
            dataSpan[i] = Color.FromArgb(alpha, r, g, b).ToArgb();
        }

        bitmap.UnlockBits(data);
        foreGround.Dispose();
        backGround.Dispose();
        return bitmap;
    }
}
