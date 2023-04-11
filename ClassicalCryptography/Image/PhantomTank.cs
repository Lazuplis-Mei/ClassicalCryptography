using ClassicalCryptography.Utils;
using System.Drawing;
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

}
