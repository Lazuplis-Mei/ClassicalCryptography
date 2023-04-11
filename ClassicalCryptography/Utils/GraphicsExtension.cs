using CommunityToolkit.HighPerformance;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Utils;

[SupportedOSPlatform("windows")]
internal static class GraphicsExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Translate(this Graphics graphics, int xOffset)
    {
        graphics.TranslateTransform(xOffset, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TranslateBreakLine(this Graphics graphics, int yOffset)
    {
        graphics.TranslateTransform(-graphics.Transform.OffsetX, yOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitmapData LockBits(this Bitmap bitmap)
    {
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        return bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span2D<T> AsSpan2D<T>(this BitmapData bitmapData)
    {
        return new Span2D<T>(bitmapData.Scan0.ToPointer(), bitmapData.Height, bitmapData.Width, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<int> AsSpan(this BitmapData bitmapData)
    {
        return new Span<int>(bitmapData.Scan0.ToPointer(), bitmapData.Height * bitmapData.Width);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetCenterPoint(this Bitmap bitmap, ref RectangleF rect)
    {
        var centerX = (bitmap.Width - rect.Width) / 2;
        var centerY = (bitmap.Height - rect.Height) / 2;
        rect.Location = new PointF(centerX, centerY);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color ApplyWeight(this Color color, double weight)
    {
        Guard.IsGreaterThanOrEqualTo(weight, 0);
        int r = (int)Math.Min(255, (color.R * weight));
        int g = (int)Math.Min(255, (color.G * weight));
        int b = (int)Math.Min(255, (color.B * weight));
        return Color.FromArgb(color.A, r, g, b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color WithAlpha(this Color color, byte alpha)
    {
        return Color.FromArgb(alpha, color.R, color.G, color.B);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetGrayscale(this Color color)
    {
        return (byte)(color.GetBrightness() * 255);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte LABDistanceToAlpha(this Color color1, Color color2)
    {
        int dr = color2.R - color1.R;
        int dg = color2.G - color1.G;
        int db = color2.B - color1.B;
        double r = (color1.R + color2.R) / 256.0;
        double a = 8 + (255 + (dr - db)) / 256.0;
        double b = 4 * dr + 8 * dg + 6 * db + r * (dr - db) + (dr * dr - db * db) / 512.0;
        int alpha = Math.Min(255, (int)(255 + b / (2 * a)));
        return (byte)Math.Max(0, alpha);
    }
}
