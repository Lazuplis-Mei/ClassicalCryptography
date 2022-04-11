using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;

namespace ClassicalCryptography.Undefined;

/// <summary>
/// 旗语路径密码
/// </summary>
[Introduction("旗语路径密码", "以字母代表的旗语作为路径的指向，去除路径上的字母。")]
public partial class SemaphorePathCipher : ICipher<string, string[]>
{
    /// <summary>
    /// 旗语字母记号(忽略作用)
    /// </summary>
    public const char FLetter = 'J';
    /// <summary>
    /// 旗语数字记号(忽略作用)
    /// </summary>
    public const char FDigits = '%';
    /// <summary>
    /// 旗语取消记号(忽略作用)
    /// </summary>
    public const char FCancel = '#';

    /// <summary>
    /// 旗语字母
    /// </summary>
    public static readonly string FLetters = $"ABCDEFGHIJKLMNOPQRSTUVWXYZ{FDigits}{FCancel}";

#pragma warning disable IDE1006 // 命名样式
    private static readonly (int dx, int dy) U = (0, -1);
    private static readonly (int dx, int dy) D = (0, 1);
    private static readonly (int dx, int dy) L = (-1, 0);
    private static readonly (int dx, int dy) R = (1, 0);
    private static readonly (int dx, int dy) UL = (-1, -1);
    private static readonly (int dx, int dy) UR = (1, -1);
    private static readonly (int dx, int dy) DL = (-1, 1);
    private static readonly (int dx, int dy) DR = (1, 1);
#pragma warning restore IDE1006 // 命名样式

    private static readonly (int dx, int dy)[] diffs = { U, D, L, R, UL, UR, DL, DR };

    private static readonly ((int dx, int dy) In, (int dx, int dy) Out)[] semaphores =
    {
        (DL, D),      //A(1)
        (L, D),       //B(2)
        (UL, D),      //C(3)
        (U, D),       //D(4)
        (D, UR),      //E(5)
        (D, R),       //F(6)
        (D, DR),      //G(7)
        (L, DL),      //H(8)
        (UL, DL),     //I(9)
        (U, R),       //J(字母记号)
        (DL, U),      //K(0)
        (DL, UR),     //L
        (DL, R),      //M
        (DL, DR),     //N
        (UL, L),      //O
        (L, U),       //P
        (L, UR),      //Q
        (L, R),       //R
        (L, DR),      //S
        (UL, U),      //T
        (UL, UR),     //U
        (U, DR),      //V
        (UR, R),      //W
        (UR, DR),     //X
        (UL, R),      //Y
        (DR, R),      //Z
        (U, UR),      //数字记号(%)
        (UL, DR),     //取消(#)
    };
    /// <summary>
    /// 未定义的密码类型
    /// </summary>
    public CipherType Type => CipherType.Undefined;

    /// <summary>
    /// 生成字符矩阵
    /// </summary>
    /// <param name="textLength">文字长度</param>
    /// <param name="height">矩阵高度</param>
    /// <param name="width">矩阵宽度</param>
    /// <param name="tolerance">结果的容忍度</param>
    /// <param name="retry">重试次数</param>
    public static char[,] GenerateMatrix(int textLength, int height, int width, int tolerance = 0, int retry = 200)
    {
        int length = width * height;
        var block = new List<(int x, int y)>(length);
        var queue = new Queue<(int x, int y)>();
        while (retry != 0)
        {
            RandomHelper.RandomSample(length - 1, textLength)
            .ForEach(i => block.Add(((i + 1) % height, (i + 1) / height)));

            /***路径生成***/
            var (x, y) = (0, 0);
            queue.Enqueue((x, y));
            while (queue.Count > 0)
            {
                (int x, int y) npos = queue.Dequeue();
                if ((x, y) == (0, 0) || diffs.Contains((npos.x - x, npos.y - y)))
                {
                    (x, y) = npos;
                    block.Add((x, y));
                    if (block.Count >= length - tolerance && block.Count <= length)
                    {
                        if (x == 0)
                            block.Add((-1, y));
                        else if (x == width - 1)
                            block.Add((width, y));
                        else if (y == 0)
                            block.Add((x, -1));
                        else if (y == height - 1)
                            block.Add((x, height));
                        else if (block.Count == length)
                            break;
                        else
                            goto LContinue;
                        char[,] matrix = new char[width, height];
                        FillMatrix(matrix, block, textLength);
                        return matrix;
                    }
                LContinue:
                    foreach (var (dx, dy) in diffs)
                    {
                        npos = (x + dx, y + dy);
                        if (npos.x >= 0 && npos.y >= 0 &&
                            npos.x < width && npos.y < height &&
                            !queue.Contains(npos) && !block.Contains(npos))
                            queue.Enqueue(npos);
                    }
                }
            }

            block.Clear();
            queue.Clear();
            retry--;
        }
        throw new Exception("超过重试次数");
    }

    private static void FillMatrix(char[,] matrix, List<(int x, int y)> block, int textLength)
    {
        (int x, int y) from = (-1, 0);

        for (int i = textLength; i < block.Count - 1; i++)
        {
            var flag1 = (from.x - block[i].x, from.y - block[i].y);
            from = block[i];
            var flag2 = (block[i + 1].x - from.x, block[i + 1].y - from.y);
            int fi = Array.IndexOf(semaphores, (flag1, flag2));
            if (fi == -1)
                fi = Array.IndexOf(semaphores, (flag2, flag1));
            matrix[from.x, from.y] = FLetters[fi];
        }
    }

    /// <summary>
    /// 填充文本到字符矩阵
    /// </summary>
    /// <param name="text"></param>
    /// <param name="cMatrix"></param>
    [SkipLocalsInit]
    public static string[] FillText(string text, char[,] cMatrix)
    {
        int width = cMatrix.GetLength(0);
        int height = cMatrix.GetLength(1);
        Span<char> span = stackalloc char[width];
        string[] result = new string[height];
        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                span[x] = cMatrix[x, y] == '\0' ?
                    (i >= text.Length ? TranspositionHelper.GetPaddingChar() : text[i++])
                    : cMatrix[x, y];
            result[y] = new string(span);
        }
        return result;
    }

    /// <summary>
    /// 提取其中的文本
    /// </summary>
    public static string GetText(string[] strMatrix)
    {
        int height = strMatrix.Length;
        int width = strMatrix[0].Length;
        var strBuilder = new StringBuilder();
        var visited = new BitMatrix(width, height);
        (int x, int y) from = (-1, 0);
        var (x, y) = (0, 0);
        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            int si = FLetters.IndexOf(strMatrix[y][x]);
            if (si != -1 && !visited[x, y])
            {
                visited[x, y] = true;
                var (In, Out) = semaphores[si];
                var flag = (from.x - x, from.y - y);
                from = (x, y);
                if (flag == In)
                {
                    x += Out.dx;
                    y += Out.dy;
                }
                else if (flag == Out)
                {
                    x += In.dx;
                    y += In.dy;
                }
                else
                    throw new FormatException("不正确的字符矩阵");
            }
            else
                throw new FormatException("不正确的字符矩阵");
        }
        for (y = 0; y < height; y++)
            for (x = 0; x < width; x++)
                if (!visited[x, y])
                    strBuilder.Append(strMatrix[y][x]);

        return strBuilder.ToString();
    }

    /// <summary>
    /// 加密指定的文本
    /// </summary>
#if DEBUG
    [SupportedOSPlatform("windows")]
#endif
    public string[] Encrypt(string plainText)
    {
        int len = (plainText.Length * 3).SqrtCeil();
        char[,]? cMatrix = null;
        int t = 0;
        while (t <= 10)
        {
            try
            {
                cMatrix = GenerateMatrix(plainText.Length, len, len, t);
#if DEBUG
                DrawSemaphores(cMatrix, $"temp_{plainText.GetHashCode()}.png");
#endif
                break;
            }
            catch
            {
                t += 2;
            }
        }
        if (cMatrix is null)
            throw new Exception("超过重试容忍次数");
        return FillText(plainText, cMatrix);
    }

    /// <summary>
    /// 解密字符矩阵
    /// </summary>
    public string Decrypt(string[] cipherText) => GetText(cipherText);


    private const int lineWidth = 5;
    private const int figureSize = 60;
    private const int midPosition = figureSize / 2;
    private static readonly Point centerPoint = new(midPosition, midPosition);
    private static readonly Point[] points = { default, centerPoint, default };

    /// <summary>
    /// 图片保存的格式
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static ImageFormat ImgFormat { get; set; } = ImageFormat.Png;

    /// <summary>
    /// 绘制旗语路径
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static void DrawSemaphores(char[,] cMatrix, string imagePath)
    {
        int width = cMatrix.GetLength(0);
        int height = cMatrix.GetLength(1);
        using var bitmap = new Bitmap(figureSize * width, figureSize * height);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(Color.Black, lineWidth);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int fi = FLetters.IndexOf(cMatrix[x, y]);
                if (fi != -1)
                {
                    var (In, Out) = semaphores[fi];
                    points[0] = centerPoint;
                    points[0].X += In.dx * midPosition;
                    points[0].Y += In.dy * midPosition;
                    points[2] = centerPoint;
                    points[2].X += Out.dx * midPosition;
                    points[2].Y += Out.dy * midPosition;
                    graphics.DrawLines(pen, points);
                }
                graphics.TranslateTransform(figureSize, 0);
            }
            graphics.TranslateTransform(-bitmap.Width, figureSize);
        }
        bitmap.Save(imagePath, ImgFormat);
    }
}
