using CommunityToolkit.HighPerformance;
using StackBlur.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Image;

/// <summary>
/// <see href="https://en.wikipedia.org/wiki/Stereogram">裸眼3D图</see>
/// </summary>
[Introduction("裸眼3D图", "使用深度图和纹理创建裸眼3D图")]
[ReferenceFrom("https://github.com/mexomagno/stereogramaxo", ProgramingLanguage.Python, License.GPL2_0)]
[SupportedOSPlatform("windows")]
public static class Stereogram
{
    /// <summary>
    /// 默认使用深度图时的模糊参数
    /// </summary>
    public const int DEFAULT_DEPTHMAP_BLUR = 2;

    /// <summary>
    /// 图像最大长宽
    /// </summary>
    public const int MAX_DIMENSION = 1500;

    /// <summary>
    /// 纹理模板数量
    /// </summary>
    public const int PATTERN_FRACTION = 8;

    /// <summary>
    /// 扩大采样系数
    /// </summary>
    public const double OVERSAMPLE = 1.8;

    /// <summary>
    /// 位移比率
    /// </summary>
    public const double SHIFT_RATIO = 0.3;

    /// <summary>
    /// 查看使用的双眼聚焦方法
    /// </summary>
    public enum EyeMode
    {
        /// <summary>
        /// 平行眼
        /// </summary>
        WallEye = 1,

        /// <summary>
        /// 交叉眼
        /// </summary>
        CrossEye = -1
    }

    /// <summary>
    /// 模糊参数(0~100)
    /// </summary>
    public static int BlurRadius { get; set; }

    /// <summary>
    /// 指定使用平行眼还是交叉/斗鸡眼
    /// </summary>
    public static EyeMode SightEyeMode { get; set; } = EyeMode.WallEye;

    /// <summary>
    /// 使用深度(0~1)
    /// </summary>
    public static double ScaleDepth { get; set; }

    /// <summary>
    /// 使用模板纹理创建裸眼3D图
    /// </summary>
    /// <param name="depthmap">深度信息图</param>
    /// <param name="pattern">纹理模板</param>
    public static Bitmap MakeWithPattern(Bitmap depthmap, Bitmap pattern)
    {
        depthmap.Ensure32bppArgb();

        ResizeBitmap(ref depthmap);
        ResizeBitmap(ref pattern);

        if (BlurRadius > 0 && BlurRadius <= 100)
            depthmap.StackBlur(BlurRadius);
        else
            depthmap.StackBlur(DEFAULT_DEPTHMAP_BLUR);

        if (ScaleDepth > 0)
            depthmap.RedistributeGrays(ScaleDepth);

        int patternWidth = depthmap.Width / PATTERN_FRACTION;
        int patternHeight = pattern.Height * patternWidth / pattern.Width;

        using var canvas = new Bitmap(depthmap.Width + patternWidth, depthmap.Height);
        using var patternStrip = new Bitmap(patternWidth, depthmap.Height);
        using var patternUnit = new Bitmap(pattern, patternWidth, patternHeight);

        patternStrip.FillStrip(patternUnit);

        using var newDepthmap = depthmap.WithScale(OVERSAMPLE);
        using var newCanvas = canvas.WithScale(OVERSAMPLE);
        using var newPatternStrip = patternStrip.WithScale(OVERSAMPLE);

        patternWidth = newPatternStrip.Width;
        var centerX = newDepthmap.Width >> 1;

        using var graphics = Graphics.FromImage(newCanvas);
        graphics.DrawImage(newPatternStrip, centerX, 0);
        if (SightEyeMode == EyeMode.CrossEye)
            graphics.DrawImage(newPatternStrip, centerX - patternWidth, 0);

        ShiftPixels(newCanvas, patternWidth, centerX, newDepthmap, 1);
        ShiftPixels(newCanvas, patternWidth, centerX + patternWidth, newDepthmap, -1);

        return newCanvas.WithScale(1 / OVERSAMPLE);
    }

    private static void ShiftPixels(Bitmap canvas, int patternWidth, int startX, Bitmap depthmap, int direction)
    {
        var depthData = depthmap.LockBits();
        var depthDataSpan = depthData.AsSpan2D<int>();
        var canvasData = canvas.LockBits();
        var canvasDataSpan = canvasData.AsSpan2D<int>();
        double factor = patternWidth * SHIFT_RATIO * (int)SightEyeMode;
        for (factor /= 255; startX >= 0 && startX < depthmap.Width; startX += direction * patternWidth)
        {
            for (int y = 0; y < depthmap.Height; y++)
            {
                var end = Math.Max(0, Math.Min(depthmap.Width - 1, startX + direction * patternWidth));
                for (int x = startX; direction > 0 ? x <= end : x >= end; x += direction)
                {
                    var gray = GraphicsExtension.GetGrayscale(depthDataSpan[y, x]);
                    var dx = (int)(gray * factor) * direction;
                    if (direction == 1)
                        canvasDataSpan[y, x + patternWidth] = canvasDataSpan[y, dx + x];
                    if (direction == -1)
                        canvasDataSpan[y, x] = canvasDataSpan[y, x + patternWidth + dx];
                }
            }
        }
        canvas.UnlockBits(canvasData);
        depthmap.UnlockBits(depthData);
    }

    private static void FillStrip(this Bitmap patternStrip, Bitmap patternUnit)
    {
        using var graphics = Graphics.FromImage(patternStrip);
        for (int y = 0; y < patternStrip.Height; y += patternUnit.Height)
            graphics.DrawImage(patternUnit, 0, y);
    }

    private static void ResizeBitmap(ref Bitmap image)
    {
        float max = Math.Max(image.Width, image.Height);
        if (max > MAX_DIMENSION)
        {
            var factor = MAX_DIMENSION / max;
            var size = new SizeF(image.Width, image.Height);
            image = new Bitmap(image, (size * factor).ToSize());
        }
    }
}
