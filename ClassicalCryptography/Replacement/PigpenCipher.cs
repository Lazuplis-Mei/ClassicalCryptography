using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 猪圈密码
/// </summary>
[SupportedOSPlatform("windows")]
[Introduction("猪圈密码", "一种以格子为基础的替代式密码。")]
public class PigpenCipher
{
    /// <summary>
    /// 替代密码
    /// </summary>
    public static CipherType Type => CipherType.Replacement;

    /// <summary>
    /// 图片保存的格式
    /// </summary>
    public static ImageFormat ImgFormat { get; set; } = ImageFormat.Png;
    /// <summary>
    /// 每行字母数
    /// </summary>
    public static int LetterPerLine { get; set; } = 20;

    private const int lineWidth = 5;
    private const int figureSize = 60;
    private const int posPadding = 7;
    private const int negPadding = figureSize - posPadding;
    private const int midPosition = figureSize / 2;
    private const int dotSize = 10;
    private const int dotPosition = midPosition - dotSize / 2;

#pragma warning disable IDE1006 // 命名样式
    private static readonly Point LT = new(posPadding, posPadding);
    private static readonly Point RT = new(negPadding, posPadding);
    private static readonly Point LB = new(posPadding, negPadding);
    private static readonly Point RB = new(negPadding, negPadding);
    private static readonly Point ML = new(posPadding, midPosition);
    private static readonly Point MT = new(midPosition, posPadding);
    private static readonly Point MR = new(negPadding, midPosition);
    private static readonly Point MB = new(midPosition, negPadding);
#pragma warning restore IDE1006 // 命名样式

    private static readonly (Point[] Points, bool HasDot)[] figures =
    {
        (new[] { LB, RB, RT }, false),             //A
        (new[] { LT, LB, RB, RT }, false),         //B
        (new[] { LT, LB, RB }, false),             //C
        (new[] { LT, RT, RB, LB }, false),         //D
        (new[] { LT, RT, RB, LB, LT }, false),     //E
        (new[] { RT, LT, LB, RB }, false),         //F
        (new[] { LT, RT, RB }, false),             //G
        (new[] { LB, LT, RT, RB }, false),         //H
        (new[] { LB, LT, RT }, false),             //I

        (new[] { LB, RB, RT }, true),              //J
        (new[] { LT, LB, RB, RT }, true),          //K
        (new[] { LT, LB, RB }, true),              //L
        (new[] { LT, RT, RB, LB }, true),          //M
        (new[] { LT, RT, RB, LB, LT }, true),      //N
        (new[] { RT, LT, LB, RB }, true),          //O
        (new[] { LT, RT, RB }, true),              //P
        (new[] { LB, LT, RT, RB }, true),          //Q
        (new[] { LB, LT, RT }, true),              //R

        (new[] { LT, MB, RT }, false),             //S
        (new[] { LT, MR, LB }, false),             //T
        (new[] { RT, ML, RB }, false),             //U
        (new[] { LB, MT, RB }, false),             //V

        (new[] { LT, MB, RT }, true),              //S
        (new[] { LT, MR, LB }, true),              //T
        (new[] { RT, ML, RB }, true),              //U
        (new[] { LB, MT, RB }, true),              //V
    };

    private static readonly (Point[] Points, bool HasDot)[] vfigures =
    {
        //省略了重复的
        (new[] { LT, MB, RT }, false),             //J
        (new[] { ML, MB, MR, MT }, false),         //K
        (new[] { MT, ML, MB, MR }, false),         //L
        (new[] { LT, MR, LB }, false),             //M
        (new[] { ML, MT, MR, MB, ML }, false),     //N
        (new[] { RT, ML, RB }, false),             //O
        (new[] { ML, MT, MR, MB }, false),         //P
        (new[] { MB, ML, MT, MR }, false),         //Q
        (new[] { LB, MT, RB }, false),             //R

        (new[] { LB, RB, RT }, true),              //S
        (new[] { LT, LB, RB }, true),              //T
        (new[] { LT, RT, RB }, true),              //U
        (new[] { LB, LT, RT }, true),              //V
        //省略了重复的
    };

    /// <summary>
    /// 加密成图像
    /// </summary>
    public static void Encrypt(string plainText, string imagePath, bool variant = false)
    {
        plainText = Purify(plainText, out int line);
        int length = Math.Min(LetterPerLine, plainText.Length);
        line += plainText.Length.DivCeil(LetterPerLine);
        using var bitmap = new Bitmap(figureSize * length, figureSize * line);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(Color.Black, lineWidth);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.White);
        for (int y = 0, i = 0; y < line; y++)
        {
            for (int x = 0; x < length; x++, i++)
            {
                if (i == plainText.Length)
                    break;
                if (plainText[i] == '\n')
                {
                    i++;
                    break;
                }
                int fi = Globals.ULetters.IndexOf(plainText[i]);
                if (fi != -1)
                {
                    (Point[] points, bool hasDot) = variant && fi >= 9 && fi <= 21 ?
                        vfigures[fi - 9] : figures[fi];//省略了重复的
                    graphics.DrawLines(pen, points);
                    if (hasDot)
                        graphics.FillEllipse(pen.Brush, dotPosition, dotPosition, dotSize, dotSize);
                }
                graphics.TranslateTransform(figureSize, 0);
            }
            graphics.TranslateTransform(-graphics.Transform.OffsetX, figureSize);
        }
        bitmap.Save(imagePath, ImgFormat);
    }

    [SkipLocalsInit]
    private static string Purify(string text, out int line)
    {
        Span<char> str = stackalloc char[text.Length];
        int count = 0;
        line = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] is (>= 'A' and <= 'Z') or ' ')
                str[count++] = text[i];
            else if (text[i] is >= 'a' and <= 'z')
                str[count++] = (char)(text[i] - 'a' + 'A');
            else if (text[i] == '\n')
            {
                str[count++] = text[i];
                line++;
            }
        }
        return new string(str[..count]);
    }
}