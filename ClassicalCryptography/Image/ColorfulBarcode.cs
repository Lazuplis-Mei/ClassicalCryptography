using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace ClassicalCryptography.Image;

/// <summary>
/// 彩色二维码
/// </summary>
[Introduction("彩色二维码", "以颜色通道分别存储多个二维码")]
[SupportedOSPlatform("windows")]
public static class ColorfulBarcode
{

    /// <summary>
    /// 二维码图像的变长
    /// </summary>
    public const int IMAGE_SIZE = 600;

    /// <summary>
    /// 图形密码
    /// </summary>
    public static CipherType Type => CipherType.Image;

    /// <summary>
    /// 图片保存的格式
    /// </summary>
    public static ImageFormat ImgFormat { get; set; } = ImageFormat.Png;

    /// <summary>
    /// 编码彩色二维码并保存
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    /// <param name="imagePath">图片保存路径</param>
    /// <param name="useBase64">是否使用base64</param>
    public static void SaveBarcode(string text, string imagePath, bool useBase64 = false)
    {
        using var bitmap = Encode(text, useBase64);
        bitmap.Save(imagePath, ImgFormat);
    }

    /// <summary>
    /// 编码彩色二维码
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    /// <param name="useBase64">是否使用base64</param>
    public unsafe static Bitmap Encode(string text, bool useBase64 = false)
    {
        var inputString = useBase64 ? text.ToBase64() : text;
        int index = inputString.Length / 3;
        var text1 = inputString[..index];
        var text2 = inputString[index..(index + index)];
        var text3 = inputString[(index + index)..];
        var bitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);
        var writer = new QRCodeWriter();
        var bits1 = writer.encode(text1, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE);
        var bits2 = writer.encode(text2, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE);
        var bits3 = writer.encode(text3, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE);
        bits2.rotate90();
        bits3.rotate180();

        var rect = new Rectangle(0, 0, IMAGE_SIZE, IMAGE_SIZE);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var dataSpan = new Span<int>((void*)data.Scan0, IMAGE_SIZE * IMAGE_SIZE);
        for (int x = 0; x < IMAGE_SIZE; x++)
        {
            for (int y = 0; y < IMAGE_SIZE; y++)
            {
                dataSpan[x + y * IMAGE_SIZE] = Color.FromArgb(255,
                    bits1[x, y] ? 0 : 255,
                    bits2[x, y] ? 0 : 255,
                    bits3[x, y] ? 0 : 255).ToArgb();
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }

    /// <summary>
    /// 编码6种基础颜色组合的彩色二维码并保存
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    /// <param name="imagePath">图片保存路径</param>
    /// <param name="useBase64">是否使用base64</param>
    public static void SaveSixColorBarcode(string text, string imagePath, bool useBase64 = false)
    {
        using var bitmap = EncodeSixColor(text, useBase64);
        bitmap.Save(imagePath, ImgFormat);
    }

    /// <summary>
    /// 编码6种基础颜色组合的彩色二维码
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    /// <param name="useBase64">是否使用base64</param>
    public unsafe static Bitmap EncodeSixColor(string text, bool useBase64 = false)
    {
        var inputString = useBase64 ? text.ToBase64() : text;
        int index = inputString.Length / 6;
        var text1 = inputString[..index];
        var text2 = inputString[index..(index + index)];
        var text3 = inputString[(index + index)..(index * 3)];
        var text4 = inputString[(index * 3)..(index * 4)];
        var text5 = inputString[(index * 4)..(index * 5)];
        var text6 = inputString[(index * 5)..];
        var bitmap = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);
        var writer = new QRCodeWriter();
        var option = new Dictionary<EncodeHintType, object>
        {
            { EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.M }
        };
        var bits1 = writer.encode(text1, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE, option);
        var bits2 = writer.encode(text2, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE, option);
        var bits3 = writer.encode(text3, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE, option);
        var bits4 = writer.encode(text4, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE, option);
        var bits5 = writer.encode(text5, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE, option);
        var bits6 = writer.encode(text6, BarcodeFormat.QR_CODE, IMAGE_SIZE, IMAGE_SIZE, option);
        bits2.rotate90();
        bits3.rotate180();
        bits4.rotate90();
        bits4.rotate180();
        bits5.rotate180();
        bits6.rotate90();
        const byte HColor = 0B1010_1010;
        const byte LColor = unchecked((byte)~HColor);

        var rect = new Rectangle(0, 0, IMAGE_SIZE, IMAGE_SIZE);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

        var dataSpan = new Span<int>((void*)data.Scan0, IMAGE_SIZE * IMAGE_SIZE);
        for (int x = 0; x < IMAGE_SIZE; x++)
        {
            for (int y = 0; y < IMAGE_SIZE; y++)
            {
                int red, green, blue;
                red = bits1[x, y] ? 0 : HColor;
                red += bits2[x, y] ? 0 : LColor;
                green = bits3[x, y] ? 0 : HColor;
                green += bits4[x, y] ? 0 : LColor;
                blue = bits5[x, y] ? 0 : HColor;
                blue += bits6[x, y] ? 0 : LColor;
                dataSpan[x + y * IMAGE_SIZE] = Color.FromArgb(255, red, green, blue).ToArgb();
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }

    /// <summary>
    /// 识别(3色的)彩色二维码
    /// </summary>
    public unsafe static string Recognize(Bitmap bitmap)
    {
        using var redBitmap = new Bitmap(bitmap.Width, bitmap.Height);
        using var greenBitmap = new Bitmap(bitmap.Width, bitmap.Height);
        using var blueBitmap = new Bitmap(bitmap.Width, bitmap.Height);

        int white = Color.White.ToArgb();
        int black = Color.Black.ToArgb();
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var orgbBitmaps = new[] { bitmap, redBitmap, greenBitmap, blueBitmap };
        var orgbDatas = new BitmapData[orgbBitmaps.Length];
        var orgbDataSpan = new int*[orgbBitmaps.Length];
        for (int i = 0; i < orgbBitmaps.Length; i++)
        {
            orgbDatas[i] = orgbBitmaps[i].LockBits(rect, ImageLockMode.ReadWrite, orgbBitmaps[i].PixelFormat);
            orgbDataSpan[i] = (int*)orgbDatas[i].Scan0;
        }

        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                var color = Color.FromArgb(orgbDataSpan[0][x + y * bitmap.Width]);
                int index = x + y * bitmap.Width;
                orgbDataSpan[1][index] = color.R > 127 ? white : black;
                orgbDataSpan[2][index] = color.G > 127 ? white : black;
                orgbDataSpan[3][index] = color.B > 127 ? white : black;
            }
        }

        for (int i = 0; i < orgbBitmaps.Length; i++)
        {
            orgbBitmaps[i].UnlockBits(orgbDatas[i]);
        }

        var reader = new ZXing.Windows.Compatibility.BarcodeReader();
        var result = reader.Decode(redBitmap).Text;
        result += reader.Decode(greenBitmap).Text;
        result += reader.Decode(blueBitmap).Text;
        return result;
    }

    /// <summary>
    /// 识别(6色的)彩色二维码
    /// </summary>
    public unsafe static string RecognizeSixColor(Bitmap bitmap)
    {
        using var redBitmap1 = new Bitmap(bitmap.Width, bitmap.Height);
        using var redBitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
        using var greenBitmap1 = new Bitmap(bitmap.Width, bitmap.Height);
        using var greenBitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
        using var blueBitmap1 = new Bitmap(bitmap.Width, bitmap.Height);
        using var blueBitmap2 = new Bitmap(bitmap.Width, bitmap.Height);

        int white = Color.White.ToArgb();
        int black = Color.Black.ToArgb();
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var orgbBitmaps = new[] { bitmap, redBitmap1, redBitmap2,
            greenBitmap1, greenBitmap2, blueBitmap1, blueBitmap2 };
        var orgbDatas = new BitmapData[orgbBitmaps.Length];

        var orgbDataSpan = new int*[orgbBitmaps.Length];
        for (int i = 0; i < orgbBitmaps.Length; i++)
        {
            orgbDatas[i] = orgbBitmaps[i].LockBits(rect, ImageLockMode.ReadWrite, orgbBitmaps[i].PixelFormat);
            orgbDataSpan[i] = (int*)orgbDatas[i].Scan0;
        }

        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                var color = Color.FromArgb(orgbDataSpan[0][x + y * bitmap.Width]);
                int index = x + y * bitmap.Width;
                switch (color.R)
                {
                    case >= 192:
                        orgbDataSpan[1][index] = white;
                        orgbDataSpan[2][index] = white;
                        break;
                    case >= 128:
                        orgbDataSpan[1][index] = white;
                        orgbDataSpan[2][index] = black;
                        break;
                    case >= 64:
                        orgbDataSpan[1][index] = black;
                        orgbDataSpan[2][index] = white;
                        break;
                    default:
                        orgbDataSpan[1][index] = black;
                        orgbDataSpan[2][index] = black;
                        break;
                }

                switch (color.G)
                {
                    case >= 192:
                        orgbDataSpan[3][index] = white;
                        orgbDataSpan[4][index] = white;
                        break;
                    case >= 128:
                        orgbDataSpan[3][index] = white;
                        orgbDataSpan[4][index] = black;
                        break;
                    case >= 64:
                        orgbDataSpan[3][index] = black;
                        orgbDataSpan[4][index] = white;
                        break;
                    default:
                        orgbDataSpan[3][index] = black;
                        orgbDataSpan[4][index] = black;
                        break;
                }

                switch (color.B)
                {
                    case >= 192:
                        orgbDataSpan[5][index] = white;
                        orgbDataSpan[6][index] = white;
                        break;
                    case >= 128:
                        orgbDataSpan[5][index] = white;
                        orgbDataSpan[6][index] = black;
                        break;
                    case >= 64:
                        orgbDataSpan[5][index] = black;
                        orgbDataSpan[6][index] = white;
                        break;
                    default:
                        orgbDataSpan[5][index] = black;
                        orgbDataSpan[6][index] = black;
                        break;
                }
            }
        }

        for (int i = 0; i < orgbBitmaps.Length; i++)
        {
            orgbBitmaps[i].UnlockBits(orgbDatas[i]);
        }

        var reader = new ZXing.Windows.Compatibility.BarcodeReader();
        var result = reader.Decode(redBitmap1).Text;
        result += reader.Decode(redBitmap2).Text;
        result += reader.Decode(greenBitmap1).Text;
        result += reader.Decode(greenBitmap2).Text;
        result += reader.Decode(blueBitmap1).Text;
        result += reader.Decode(blueBitmap2).Text;
        return result;
    }

}