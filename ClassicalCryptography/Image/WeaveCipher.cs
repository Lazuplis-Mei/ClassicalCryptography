using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;

namespace ClassicalCryptography.Image;

/// <summary>
/// 编织图形密码
/// </summary>
[Introduction("编织图形密码", "https://tieba.baidu.com/p/7814788182")]
[SupportedOSPlatform("windows")]
public static partial class WeaveCipher
{
    /// <summary>
    /// 图形密码
    /// </summary>
    public static CipherType Type => CipherType.Image;

    /// <summary>
    /// 前景颜色
    /// </summary>
    public static Brush Foreground { get; set; } = Brushes.Black;
    /// <summary>
    /// 背景颜色
    /// </summary>
    public static Brush Background { get; set; } = Brushes.White;
    /// <summary>
    /// 图片保存的格式
    /// </summary>
    public static ImageFormat ImgFormat { get; set; } = ImageFormat.Png;

    private const int blockSize = 16;
    /// <summary>
    /// 核心加密代码
    /// </summary>
    public static BitMatrix EncryptBits(BitArray bits1, BitArray bits2)
    {
        var matrix = new BitMatrix(bits1.Size + 1, bits2.Size + 1);

        for (int x = 1; x < matrix.Width; x++)
            matrix[x, 0] = bits1[x - 1] ? !matrix[x - 1, 0] : matrix[x - 1, 0];

        for (int y = 1; y < matrix.Height; y++) 
            for (int x = 0; x < matrix.Width; x++)
                matrix[x, y] = (x % 2 == 0 ? bits2[y - 1] : !bits2[y - 1])
                    ? !matrix[x, y - 1] : matrix[x, y - 1];

        return matrix;
    }

    internal static Bitmap BitsToImage(BitMatrix matrix)
    {
        var bitmap = new Bitmap(matrix.Width * blockSize, matrix.Height * blockSize);
        using var graphics = Graphics.FromImage(bitmap);
        for (int x = 0; x < matrix.Width; x++)
        {
            for (int y = 0; y < matrix.Height; y++)
            {
                graphics.FillRectangle(matrix[x, y] ? Foreground : Background,
                    x * blockSize, y * blockSize, blockSize, blockSize);
            }
        }
        return bitmap;
    }

    /// <summary>
    /// 转换为图像
    /// </summary>
    public static void BitsToImage(BitMatrix matrix,string filePath)
    {
        using var bitmap = BitsToImage(matrix);
        bitmap.Save(filePath, ImgFormat);
    }

    /// <summary>
    /// 加密为图像
    /// </summary>
    public static void Encrypt(byte[] bytes1, byte[] bytes2, string filePath)
    {
        Encrypt(bytes1.AsSpan(), bytes2.AsSpan(), filePath);
    }

    /// <summary>
    /// 加密为图像
    /// </summary>
    public static void Encrypt(Span<byte> bytes1, Span<byte> bytes2, string filePath)
    {
        var bits1 = new BitArray();
        var bits2 = new BitArray();

        for (int i = 0; i < bytes1.Length; i++)
            bits1.appendBits(bytes1[i], 8);
        for (int i = 0; i < bytes2.Length; i++)
            bits2.appendBits(bytes2[i], 8);

        BitsToImage(EncryptBits(bits1, bits2), filePath);
    }

    /// <summary>
    /// 加密为图像
    /// </summary>
    public static void Encrypt(string text, string filePath)
    {
        var bytes = Encoding.UTF8.GetBytes(text).AsSpan();
        var bytes1 = bytes[..(bytes.Length / 2)];
        var bytes2 = bytes[(bytes.Length / 2)..];
        Encrypt(bytes1, bytes2, filePath);
    }

}