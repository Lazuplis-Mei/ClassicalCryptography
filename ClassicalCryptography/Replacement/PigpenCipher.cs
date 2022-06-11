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
    /// <summary>
    /// 文字笔刷
    /// </summary>
    public static Brush Foreground { get; set; } = Brushes.Black;
    /// <summary>
    /// 背景色
    /// </summary>
    public static Color? Background { get; set; } = Color.White;
    /// <summary>
    /// 额外字符笔刷
    /// </summary>
    public static Brush Extraground { get; set; } = Brushes.White;

    private const int lineWidth = 5;
    private const int figureSize = 60;
    private const int posPadding = 7;
    private const int negPadding = figureSize - posPadding;
    private const int midPosition = figureSize / 2;
    private const int dotSize = 10;
    private const int dotPosition = midPosition - dotSize / 2;
    private static readonly Point LT = new(posPadding, posPadding);
    private static readonly Point RT = new(negPadding, posPadding);
    private static readonly Point LB = new(posPadding, negPadding);
    private static readonly Point RB = new(negPadding, negPadding);
    private static readonly Point ML = new(posPadding, midPosition);
    private static readonly Point MT = new(midPosition, posPadding);
    private static readonly Point MR = new(negPadding, midPosition);
    private static readonly Point MB = new(midPosition, negPadding);
    private static readonly Rectangle rect = Rectangle.FromLTRB(
    posPadding, posPadding, negPadding, negPadding);
    private static readonly Rectangle dotRect = new(
        dotPosition, dotPosition, dotSize, dotSize);
    private static readonly Font font = new("Consolas", 26);
    private static readonly StringFormat format = new()
    {
        Alignment = StringAlignment.Center,
        LineAlignment = StringAlignment.Center
    };

    private static readonly (Point[] Points, bool HasDot)[] figures =
    {
        (new[] { LB, RB, RT }, false),             //A
        (new[] { LT, LB, RB, RT }, false),         //B
        (new[] { LT, LB, RB }, false),             //C
        (new[] { LT, RT, RB, LB }, false),         //D
        (new[] { LT, RT, RB, LB, LT, RT }, false), //E
        (new[] { RT, LT, LB, RB }, false),         //F
        (new[] { LT, RT, RB }, false),             //G
        (new[] { LB, LT, RT, RB }, false),         //H
        (new[] { LB, LT, RT }, false),             //I

        (new[] { LB, RB, RT }, true),              //J
        (new[] { LT, LB, RB, RT }, true),          //K
        (new[] { LT, LB, RB }, true),              //L
        (new[] { LT, RT, RB, LB }, true),          //M
        (new[] { LT, RT, RB, LB, LT, RT }, true),  //N
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
        (new[] { ML, MT, MR, MB, ML, MT }, false), //N
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
        using var pen = new Pen(Foreground, lineWidth);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        if (Background.HasValue)
            graphics.Clear(Background.Value);

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
                        graphics.FillEllipse(Foreground, dotRect);
                }
                else if (plainText[i] != ' ')
                {
                    graphics.FillRectangle(Foreground, rect);
                    graphics.DrawString(plainText[i].ToString(), font, Extraground, rect, format);
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
        foreach (char c in text)
        {
            if (c is >= 'a' and <= 'z')
                str[count++] = (char)(c ^ 0x20);//大小写转换
            else if (c == '\n')
            {
                line++;
                str[count++] = c;
            }
            else if (c.IsPrintable())
                str[count++] = c;
        }
        return new string(str[..count]);
    }
}