using CommunityToolkit.HighPerformance;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Hashing;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Image;

/// <summary>
/// 对图片的指定区域进行加密
/// </summary>
[SupportedOSPlatform("windows")]
public static class NotMosaic
{
    /// <summary>
    /// 禁止矩形之间的重叠
    /// </summary>
    public static bool NoOverlap { get; set; } = false;

    /// <summary>
    /// 掩码
    /// </summary>
    public static int Mask { get; set; } = 0B11000000_11000000_11000000;

    /// <summary>
    /// 加密图形指定区域
    /// </summary>
    public static unsafe Bitmap Encrypt(Bitmap bitmap, Rectangle[] rects, out EncryptRegions encryptRegions, string? password = null)
    {
        if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            bitmap.MakeTransparent();
        var regions = rects.Where(item => item.X >= 0 && item.Y >= 0 &&
            item.Right <= bitmap.Width && item.Bottom <= bitmap.Height).ToArray();
        encryptRegions = new EncryptRegions(regions, !string.IsNullOrEmpty(password));
        if (NoOverlap && encryptRegions.HasOverlap)
            throw new ArgumentException("矩形区域有重叠", nameof(rects));
        return EncryptInternal(bitmap, encryptRegions, password);
    }

    /// <summary>
    /// 解密图像
    /// </summary>
    public static unsafe Bitmap Decrypt(Bitmap bitmap, EncryptRegions encryptRegions, string? password = null)
    {
        Guard.IsTrue(bitmap.PixelFormat == PixelFormat.Format32bppArgb);
        if (encryptRegions.IncludePassword)
            Guard.IsNotNullOrEmpty(password);
        return EncryptInternal(bitmap, encryptRegions, password);
    }

    private static unsafe Bitmap EncryptInternal(Bitmap bitmap, EncryptRegions encryptRegions, string? password)
    {
        var regionBytes = encryptRegions.ToBytes();
        Random random;
        if (encryptRegions.IncludePassword)
            random = new(BitConverter.ToInt32(Crc32.Hash(Encoding.UTF8.GetBytes(password!))));
        else
            random = new(BitConverter.ToInt32(Crc32.Hash(regionBytes)));
        
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var dataSpan = new Span2D<int>(data.Scan0.ToPointer(), bitmap.Height, bitmap.Width, 0);
        foreach (var region in encryptRegions.Regions)
        {
            var span2D = dataSpan.Slice(region.Y, region.X, region.Height, region.Width);
            for (int x = 0; x < span2D.Width; x++)
                for (int y = 0; y < span2D.Height; y++)
                    span2D[y, x] ^= random.Next(0xFFFFFF) & Mask;
        }
        bitmap.UnlockBits(data);
        return bitmap;
    }
}
