using ClassicalCryptography.Encoder;
using CommunityToolkit.HighPerformance;
using System.Diagnostics;
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
public class ColorfulBarcode
{
    /// <summary>
    /// 二维码图像的边长
    /// </summary>
    public const int IMAGE_SIZE = 600;

    /// <summary>
    /// 图形密码
    /// </summary>
    private static CipherType Type => CipherType.Image;

    /// <summary>
    /// 编码彩色二维码
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    /// <param name="useBase64">是否使用base64</param>
    public static unsafe Bitmap Encode(string text, bool useBase64 = false)
    {
        var inputString = useBase64 ? BaseEncoding.ToBase64(text) : text;
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

        const byte ZERO = 0;
        const byte MAX = 255;

        var data = bitmap.LockBits();
        var dataSpan = data.AsSpan2D<int>();

        for (int x = 0; x < IMAGE_SIZE; x++)
        {
            for (int y = 0; y < IMAGE_SIZE; y++)
            {
                byte red = bits1[x, y] ? ZERO : MAX;
                byte green = bits2[x, y] ? ZERO : MAX;
                byte blue = bits3[x, y] ? ZERO : MAX;
                dataSpan[x, y] = BitsHelper.CombineInt32(MAX, red, green, blue);
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }

    /// <summary>
    /// 编码6种基础颜色组合的彩色二维码
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    /// <param name="useBase64">是否使用base64</param>
    public static unsafe Bitmap EncodeSixColor(string text, bool useBase64 = false)
    {
        var inputString = useBase64 ? BaseEncoding.ToBase64(text) : text;
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

        var data = bitmap.LockBits();
        var dataSpan = data.AsSpan2D<int>();

        for (int x = 0; x < IMAGE_SIZE; x++)
        {
            for (int y = 0; y < IMAGE_SIZE; y++)
            {
                int red, green, blue;
                red = (bits1[x, y] ? 0 : HColor) + (bits2[x, y] ? 0 : LColor);
                green = (bits3[x, y] ? 0 : HColor) + (bits4[x, y] ? 0 : LColor);
                blue = (bits5[x, y] ? 0 : HColor) + (bits6[x, y] ? 0 : LColor);
                dataSpan[x, y] = BitsHelper.CombineInt32(255, (byte)red, (byte)green, (byte)blue);
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }

    /// <summary>
    /// 识别(3色的)彩色二维码
    /// </summary>
    public static unsafe string Recognize(Bitmap bitmap)
    {
        using var redBitmap = new Bitmap(bitmap.Width, bitmap.Height);
        using var greenBitmap = new Bitmap(bitmap.Width, bitmap.Height);
        using var blueBitmap = new Bitmap(bitmap.Width, bitmap.Height);

        const uint WHITE = 0xFFFFFFFF;
        const uint BLACK = 0xFF000000;
        const byte HALF = 0B01111111;

        var orgbBitmaps = new[] { bitmap, redBitmap, greenBitmap, blueBitmap };
        var orgbDatas = new BitmapData[orgbBitmaps.Length];
        var orgbDataSpan = new uint*[orgbBitmaps.Length];

        for (int i = 0; i < orgbBitmaps.Length; i++)
        {
            orgbDatas[i] = orgbBitmaps[i].LockBits();
            orgbDataSpan[i] = (uint*)orgbDatas[i].Scan0;
        }

        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                int index = x + y * bitmap.Width;
                BitsHelper.DecomposeUInt32(orgbDataSpan[0][index], out _, out byte red, out byte green, out byte blue);
                orgbDataSpan[1][index] = red > HALF ? WHITE : BLACK;
                orgbDataSpan[2][index] = green > HALF ? WHITE : BLACK;
                orgbDataSpan[3][index] = blue > HALF ? WHITE : BLACK;
            }
        }

        for (int i = 0; i < orgbBitmaps.Length; i++)
            orgbBitmaps[i].UnlockBits(orgbDatas[i]);

        var reader = new ZXing.Windows.Compatibility.BarcodeReader();
        var result = new StringBuilder();
        result.Append(reader.Decode(redBitmap).Text);
        result.Append(reader.Decode(greenBitmap).Text);
        result.Append(reader.Decode(blueBitmap).Text);
        return result.ToString();
    }

    /// <summary>
    /// 识别(6色的)彩色二维码
    /// </summary>
    public static unsafe string RecognizeSixColor(Bitmap bitmap)
    {
        using var redBitmap1 = new Bitmap(bitmap.Width, bitmap.Height);
        using var redBitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
        using var greenBitmap1 = new Bitmap(bitmap.Width, bitmap.Height);
        using var greenBitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
        using var blueBitmap1 = new Bitmap(bitmap.Width, bitmap.Height);
        using var blueBitmap2 = new Bitmap(bitmap.Width, bitmap.Height);

        const uint WHITE = 0xFFFFFFFF;
        const uint BLACK = 0xFF000000;

        var orgbBitmaps = new[] { bitmap, redBitmap1, redBitmap2, greenBitmap1, greenBitmap2, blueBitmap1, blueBitmap2 };
        var orgbDatas = new BitmapData[orgbBitmaps.Length];
        var orgbDataSpan = new uint*[orgbBitmaps.Length];
        for (int i = 0; i < orgbBitmaps.Length; i++)
        {
            orgbDatas[i] = orgbBitmaps[i].LockBits();
            orgbDataSpan[i] = (uint*)orgbDatas[i].Scan0;
        }

        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                int index = x + y * bitmap.Width;
                BitsHelper.DecomposeUInt32(orgbDataSpan[0][index], out _, out byte red, out byte green, out byte blue);
                SetBlackWhite(red, 1);
                SetBlackWhite(green, 3);
                SetBlackWhite(blue, 5);

                void SetBlackWhite(byte colorValue, int i)
                {
                    int j = i + 1;
                    switch (colorValue)
                    {
                        case >= 192:
                            orgbDataSpan[i][index] = WHITE;
                            orgbDataSpan[j][index] = WHITE;
                            break;
                        case >= 128:
                            orgbDataSpan[i][index] = WHITE;
                            orgbDataSpan[j][index] = BLACK;
                            break;
                        case >= 64:
                            orgbDataSpan[i][index] = BLACK;
                            orgbDataSpan[j][index] = WHITE;
                            break;
                        default:
                            orgbDataSpan[i][index] = BLACK;
                            orgbDataSpan[j][index] = BLACK;
                            break;
                    }
                }
            }
        }

        for (int i = 0; i < orgbBitmaps.Length; i++)
            orgbBitmaps[i].UnlockBits(orgbDatas[i]);

        var reader = new ZXing.Windows.Compatibility.BarcodeReader();
        var result = new StringBuilder();
        result.Append(reader.Decode(redBitmap1).Text);
        result.Append(reader.Decode(redBitmap2).Text);
        result.Append(reader.Decode(greenBitmap1).Text);

        var mayBeNullResult = reader.Decode(greenBitmap2);
        if (mayBeNullResult is null)
        {
            greenBitmap2.RotateFlip(RotateFlipType.Rotate270FlipNone);
            mayBeNullResult = reader.Decode(greenBitmap2);
            Debug.WriteLine($"`{mayBeNullResult.Text}`二维码在旋转后无法识别");
        }

        result.Append(mayBeNullResult.Text);
        result.Append(reader.Decode(blueBitmap1).Text);
        result.Append(reader.Decode(blueBitmap2).Text);
        return result.ToString();
    }
}
