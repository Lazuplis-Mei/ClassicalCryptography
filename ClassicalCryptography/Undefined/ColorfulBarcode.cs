using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using ZXing;
using ZXing.QrCode;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClassicalCryptography.Undefined;

/// <summary>
/// 彩色二维码
/// </summary>
[Introduction("彩色二维码", "以颜色通道分别存储信息")]
[SupportedOSPlatform("windows")]
public static class ColorfulBarcode
{

    /// <summary>
    /// 未定义的密码类型
    /// </summary>
    public static readonly CipherType Type = CipherType.Undefined;

    /// <summary>
    /// 图片保存的格式
    /// </summary>
    public static ImageFormat ImgFormat { get; set; } = ImageFormat.Png;
    /// <summary>
    /// 彩色二维码加密
    /// </summary>
    /// <param name="plainText">的字符串</param>
    /// <param name="imagePath">图片保存路径</param>
    /// <param name="base64">是否使用base64</param>
    public static void Encrypt(string plainText, string imagePath, bool base64 = false)
    {
        using var bitmap = Encrypt(plainText, base64);
        bitmap.Save(imagePath, ImgFormat);
    }

    /// <summary>
    /// 彩色二维码加密
    /// </summary>
    /// <param name="plainText">的字符串</param>
    /// <param name="base64">是否使用base64</param>
    public static Bitmap Encrypt(string plainText, bool base64 = false)
    {
        var b64Str = base64 ? plainText.ToBase64() : plainText;
        int si = b64Str.Length / 3;
        var text1 = b64Str[..si];
        var text2 = b64Str[si..(si + si)];
        var text3 = b64Str[(si + si)..];
        var bitmap = new Bitmap(600, 600);
        var writer = new QRCodeWriter();
        var bits1 = writer.encode(text1, BarcodeFormat.QR_CODE, 600, 600);
        var bits2 = writer.encode(text2, BarcodeFormat.QR_CODE, 600, 600);
        bits2.rotate90();
        var bits3 = writer.encode(text3, BarcodeFormat.QR_CODE, 600, 600);
        bits3.rotate180();

        if (Debugger.IsAttached)
        {
            for (int x = 0; x < 600; x++)
            {
                for (int y = 0; y < 600; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(
                        255,
                        bits1[x, y] ? 0 : 255,
                        bits2[x, y] ? 0 : 255,
                        bits3[x, y] ? 0 : 255));
                }
            }
        }
        else
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int[] bits = new int[data.Stride / 4 * data.Height];
            Marshal.Copy(data.Scan0, bits, 0, bits.Length);
            for (int x = 0; x < 600; x++)
            {
                for (int y = 0; y < 600; y++)
                {

                    bits[x + y * 600] = Color.FromArgb(
                        255,
                        bits1[x, y] ? 0 : 255,
                        bits2[x, y] ? 0 : 255,
                        bits3[x, y] ? 0 : 255).ToArgb();
                }
            }
            Marshal.Copy(bits, 0, data.Scan0, bits.Length);
            bitmap.UnlockBits(data);
        }
        return bitmap;
    }

    /// <summary>
    /// 彩色二维码加密
    /// </summary>
    /// <param name="plainText">的字符串</param>
    /// <param name="imagePath">图片保存路径</param>
    /// <param name="base64">是否使用base64</param>
    public static void EncryptSixColor(string plainText, string imagePath, bool base64 = false)
    {
        var b64Str = base64 ? plainText.ToBase64() : plainText;
        int si = b64Str.Length / 6;
        var text1 = b64Str[..si];
        var text2 = b64Str[si..(si + si)];
        var text3 = b64Str[(si + si)..(si * 3)];
        var text4 = b64Str[(si * 3)..(si * 4)];
        var text5 = b64Str[(si * 4)..(si * 5)];
        var text6 = b64Str[(si * 5)..];
        using var bitmap = new Bitmap(600, 600);
        var writer = new QRCodeWriter();
        var bits1 = writer.encode(text1, BarcodeFormat.QR_CODE, 600, 600);
        var bits2 = writer.encode(text2, BarcodeFormat.QR_CODE, 600, 600);
        bits2.rotate90();
        var bits3 = writer.encode(text3, BarcodeFormat.QR_CODE, 600, 600);
        bits3.rotate180();
        var bits4 = writer.encode(text4, BarcodeFormat.QR_CODE, 600, 600);
        bits4.rotate90();
        bits4.rotate180();
        var bits5 = writer.encode(text5, BarcodeFormat.QR_CODE, 600, 600);
        bits5.rotate180();
        var bits6 = writer.encode(text6, BarcodeFormat.QR_CODE, 600, 600);
        bits6.rotate90();
        const byte HColor = 0B1010_1010;
        const byte LColor = unchecked((byte)~HColor);

        if (Debugger.IsAttached)
        {
            for (int x = 0; x < 600; x++)
            {
                for (int y = 0; y < 600; y++)
                {
                    int r, g, b;
                    r = bits1[x, y] ? 0 : HColor;
                    r &= bits2[x, y] ? 0 : LColor;
                    g = bits3[x, y] ? 0 : HColor;
                    g &= bits4[x, y] ? 0 : LColor;
                    b = bits5[x, y] ? 0 : HColor;
                    b &= bits6[x, y] ? 0 : LColor;
                    bitmap.SetPixel(x, y, Color.FromArgb(255, r, g, b));
                }
            }
        }
        else
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int[] bits = new int[data.Stride / 4 * data.Height];
            Marshal.Copy(data.Scan0, bits, 0, bits.Length);
            for (int x = 0; x < 600; x++)
            {
                for (int y = 0; y < 600; y++)
                {
                    int r, g, b;
                    r = bits1[x, y] ? 0 : HColor;
                    r &= bits2[x, y] ? 0 : LColor;
                    g = bits3[x, y] ? 0 : HColor;
                    g &= bits4[x, y] ? 0 : LColor;
                    b = bits5[x, y] ? 0 : HColor;
                    b &= bits6[x, y] ? 0 : LColor;
                    bits[x + y * 600] = Color.FromArgb(255, r, g, b).ToArgb();
                }
            }
            Marshal.Copy(bits, 0, data.Scan0, bits.Length);
            bitmap.UnlockBits(data);
        }
        bitmap.Save(imagePath, ImgFormat);
    }

    /// <summary>
    /// 识别3色的彩色二维码
    /// </summary>
    public static string Decrypt(Bitmap bitmap)
    {
        using var redBitmap = new Bitmap(bitmap.Width, bitmap.Height);
        using var greenBitmap = new Bitmap(bitmap.Width, bitmap.Height);
        using var blueBitmap = new Bitmap(bitmap.Width, bitmap.Height);

        if (Debugger.IsAttached)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var color = bitmap.GetPixel(x, y);
                    redBitmap.SetPixel(x, y, color.R > 200 ? Color.White : Color.Black);
                    greenBitmap.SetPixel(x, y, color.G > 200 ? Color.White : Color.Black);
                    blueBitmap.SetPixel(x, y, color.B > 200 ? Color.White : Color.Black);
                }
            }
        }
        else
        {
            int white = Color.White.ToArgb();
            int black = Color.Black.ToArgb();
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var orgbBitmaps = new[] { bitmap, redBitmap, greenBitmap, blueBitmap };
            var orgbdatas = new BitmapData[orgbBitmaps.Length];
            var orgbbits = new int[orgbBitmaps.Length][];

            for (int i = 0; i < orgbBitmaps.Length; i++)
            {
                orgbdatas[i] = orgbBitmaps[i].LockBits(rect, ImageLockMode.ReadWrite, orgbBitmaps[i].PixelFormat);
                orgbbits[i] = new int[orgbdatas[i].Stride / 4 * orgbdatas[i].Height];
                Marshal.Copy(orgbdatas[i].Scan0, orgbbits[i], 0, orgbbits[i].Length);
            }

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var color = Color.FromArgb(orgbbits[0][x + y * bitmap.Width]);
                    orgbbits[1][x + y * bitmap.Width] = color.R > 200 ? white : black;
                    orgbbits[2][x + y * bitmap.Width] = color.G > 200 ? white : black;
                    orgbbits[3][x + y * bitmap.Width] = color.B > 200 ? white : black;
                }
            }

            for (int i = 0; i < orgbBitmaps.Length; i++)
            {
                Marshal.Copy(orgbbits[i], 0, orgbdatas[i].Scan0, orgbbits[i].Length);
                orgbBitmaps[i].UnlockBits(orgbdatas[i]);
            }
        }

        var reader = new ZXing.Windows.Compatibility.BarcodeReader();
        var result = reader.Decode(redBitmap).Text;
        result += reader.Decode(greenBitmap).Text;
        result += reader.Decode(blueBitmap).Text;
        return result;
    }
}