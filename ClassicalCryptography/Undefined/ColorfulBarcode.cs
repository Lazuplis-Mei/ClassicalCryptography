using ClassicalCryptography.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;

namespace ClassicalCryptography.Undefined;

/// <summary>
/// 彩色二维码
/// </summary>
[Introduction("彩色二维码", "以颜色通道分别存储信息")]
public class ColorfulBarcode
{
    /// <summary>
    /// 默认编码
    /// </summary>
    public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 中文按UTF-8转换成Base64字符串
    /// </summary>
    /// <param name="chineseText">中文字符串</param>
    public static string CNToB64(string chineseText)
    {
        var bytes = DefaultEncoding.GetBytes(chineseText);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64字符串按UTF-8转换成中文
    /// </summary>
    /// <param name="base64">中文字符串</param>
    public static string B64ToCN(string base64)
    {
        var bytes = Convert.FromBase64String(base64);
        return DefaultEncoding.GetString(bytes);
    }

    /// <summary>
    /// 密码类型
    /// </summary>
    public static readonly CipherType Type = CipherType.Undefined;
    /// <summary>
    /// 彩色二维码加密
    /// </summary>
    /// <param name="plainText">的字符串</param>
    /// <param name="imagePath">图片保存路径</param>
    /// <param name="base64">是否使用base64</param>
    [SupportedOSPlatform("windows")]
    public static void Encrypt(string plainText, string imagePath, bool base64 = false)
    {
        var b64Str = base64 ? CNToB64(plainText) : plainText;
        int si = b64Str.Length / 3;
        var text1 = b64Str[..si];
        var text2 = b64Str[si..(si + si)];
        var text3 = b64Str[(si + si)..];
        using var bitmap = new Bitmap(600, 600);
        var writer = new QRCodeWriter();
        var bits1 = writer.encode(text1, BarcodeFormat.QR_CODE, 600, 600);
        var bits2 = writer.encode(text2, BarcodeFormat.QR_CODE, 600, 600);
        bits2.rotate90();
        var bits3 = writer.encode(text3, BarcodeFormat.QR_CODE, 600, 600);
        bits3.rotate180();
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
        bitmap.Save(imagePath, ImageFormat.Png);
    }

    /// <summary>
    /// 彩色二维码加密
    /// </summary>
    /// <param name="plainText">的字符串</param>
    /// <param name="imagePath">图片保存路径</param>
    /// <param name="base64">是否使用base64</param>
    [SupportedOSPlatform("windows")]
    public static void EncryptSixColor(string plainText, string imagePath, bool base64 = false)
    {
        var b64Str = base64 ? CNToB64(plainText) : plainText;
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
        var bits5 = writer.encode(text5, BarcodeFormat.QR_CODE, 600, 600);
        bits5.rotate180();
        var bits6 = writer.encode(text6, BarcodeFormat.QR_CODE, 600, 600);
        bits6.rotate90();
        for (int x = 0; x < 600; x++)
        {
            for (int y = 0; y < 600; y++)
            {
                var r = bits1[x, y] ? 0 : 170;
                r += bits2[x, y] ? 0 : 85;
                var g = bits3[x, y] ? 0 : 170;
                g += bits4[x, y] ? 0 : 85;
                var b = bits5[x, y] ? 0 : 170;
                b += bits6[x, y] ? 0 : 85;
                bitmap.SetPixel(x, y, Color.FromArgb(255, r, g, b));
            }
        }
        bitmap.Save(imagePath, ImageFormat.Png);
    }
}