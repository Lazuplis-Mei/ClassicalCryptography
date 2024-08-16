using System.Drawing;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Image;

/// <summary>
/// 幻影坦克
/// </summary>
[Introduction("幻影坦克", "利用调色和混合让图片在黑白两色的背景下显示不同的内容")]
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
        foreGround.Ensure32bppArgb();
        backGround.Ensure32bppArgb();

        var data = bitmap.LockBits();
        var dataSpan = data.AsSpan();
        var fdata = foreGround.LockBits();
        var fdataSpan = fdata.AsSpan();
        var bdata = backGround.LockBits();
        var bdataSpan = bdata.AsSpan();

        for (int i = 0; i < dataSpan.Length; i++)
        {
            double fColor = Color.FromArgb(fdataSpan[i]).GetGrayscale() * FOREGROUND_WEIGHT;
            double bColor = Color.FromArgb(bdataSpan[i]).GetGrayscale() * BACKGROUNG_WEIGHT;
            byte alpha = (byte)Math.Min(255, (int)(255 + bColor - fColor));
            if (alpha == 0)
            {
                dataSpan[i] = 0;
                continue;
            }
            byte mean = (byte)Math.Min(255, bColor * 255.0 / alpha);
            dataSpan[i] = BitsHelper.CombineInt32(alpha, mean, mean, mean);
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
        foreGround.Ensure32bppArgb();
        backGround.Ensure32bppArgb();

        var data = bitmap.LockBits();
        var dataSpan = data.AsSpan();
        var fdata = foreGround.LockBits();
        var fdataSpan = fdata.AsSpan();
        var bdata = backGround.LockBits();
        var bdataSpan = bdata.AsSpan();

        for (int i = 0; i < dataSpan.Length; i++)
        {
            var fColor = Color.FromArgb(fdataSpan[i]).ApplyWeight(FOREGROUND_WEIGHT);
            var bColor = Color.FromArgb(bdataSpan[i]).ApplyWeight(FOREGROUND_WEIGHT);
            byte alpha = fColor.LABDistanceToAlpha(bColor);
            if (alpha == 0)
            {
                dataSpan[i] = 0;
                continue;
            }
            double weight = 255 * BACKGROUNG_WEIGHT / alpha;
            dataSpan[i] = bColor.WithAlpha(alpha).ApplyWeight(weight).ToArgb();
        }

        bitmap.UnlockBits(data);
        foreGround.Dispose();
        backGround.Dispose();
        return bitmap;
    }

    /// <summary>
    /// 将两张图片作为前景和背景组合，背景为彩图
    /// </summary>
    public static unsafe Bitmap Mirage(Bitmap foreGround, Bitmap backGround, bool grayscale = false)
    {
        var maxWidth = Math.Max(foreGround.Width, backGround.Width);
        var maxHeight = Math.Max(foreGround.Height, backGround.Height);
        var bitmap = new Bitmap(maxWidth, maxHeight);
        foreGround = new Bitmap(foreGround, bitmap.Size);
        backGround = new Bitmap(backGround, bitmap.Size);
        foreGround.Ensure32bppArgb();
        backGround.Ensure32bppArgb();

        var data = bitmap.LockBits();
        var dataSpan = data.AsSpan();
        var fdata = foreGround.LockBits();
        var fdataSpan = fdata.AsSpan();
        var bdata = backGround.LockBits();
        var bdataSpan = bdata.AsSpan();

        for (int i = 0; i < dataSpan.Length; i++)
        {
            int R, G, B;
            var fColor = Color.FromArgb(fdataSpan[i]);
            var bColor = Color.FromArgb(bdataSpan[i]);
            if (grayscale)
            {
                var g = fColor.GetGrayscale();
                fColor = Color.FromArgb(fColor.A, g, g, g);
                g = bColor.GetGrayscale();
                bColor = Color.FromArgb(bColor.A, g, g, g);
            }

            R = fColor.R + (255 - fColor.R) * 128 / 255;
            G = fColor.G + (255 - fColor.G) * 128 / 255;
            B = fColor.B + (255 - fColor.B) * 128 / 255;
            var avg1 = (R + G + B) / 3;

            R = bColor.R + bColor.R * (-128) / 255;
            G = bColor.G + bColor.G * (-128) / 255;
            B = bColor.B + bColor.B * (-128) / 255;
            var avg2 = (R + G + B) / 3;
            double alpha = avg2 - avg1 + 255;
            if (alpha == 0)
                alpha = 0.0001;
            R = (int)Math.Min(255, R * 255 / alpha);
            G = (int)Math.Min(255, G * 255 / alpha);
            B = (int)Math.Min(255, B * 255 / alpha);

            dataSpan[i] = BitsHelper.CombineInt32((byte)alpha, (byte)R, (byte)G, (byte)B);
        }

        bitmap.UnlockBits(data);
        foreGround.Dispose();
        backGround.Dispose();
        return bitmap;
    }

}
