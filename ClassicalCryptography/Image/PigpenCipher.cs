using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Image;

/// <summary>
/// 猪圈密码
/// </summary>
[SupportedOSPlatform("windows")]
[Introduction("猪圈密码", "一种以格子为基础的替代式密码。")]
public partial class PigpenCipher
{
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

    #region 常量

    private const int LINE_WIDTH = 5;
    private const int FIGURE_SIZE = 60;
    private const int PADDING = 7;
    private const int NEGATIVE_PADDING = FIGURE_SIZE - PADDING;
    private const int CHECK_STEP = (NEGATIVE_PADDING - PADDING) / 5;
    private const int CENTER_POSITION = FIGURE_SIZE / 2;
    private const int DOT_SIZE = 10;
    private const int DOT_POSITION = CENTER_POSITION - DOT_SIZE / 2;
    private static readonly Point LT = new(PADDING, PADDING);
    private static readonly Point RT = new(NEGATIVE_PADDING, PADDING);
    private static readonly Point LB = new(PADDING, NEGATIVE_PADDING);
    private static readonly Point RB = new(NEGATIVE_PADDING, NEGATIVE_PADDING);
    private static readonly Point ML = new(PADDING, CENTER_POSITION);
    private static readonly Point MT = new(CENTER_POSITION, PADDING);
    private static readonly Point MR = new(NEGATIVE_PADDING, CENTER_POSITION);
    private static readonly Point MB = new(CENTER_POSITION, NEGATIVE_PADDING);

    private static readonly Rectangle rect = Rectangle.FromLTRB(PADDING, PADDING, NEGATIVE_PADDING, NEGATIVE_PADDING);

    private static readonly Rectangle dotRect = new(DOT_POSITION, DOT_POSITION, DOT_SIZE, DOT_SIZE);

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

    #endregion 常量

    /// <summary>
    /// 加密成图像
    /// </summary>
    public static Bitmap Encrypt(string plainText, bool variant = false)
    {
        plainText = Purify(plainText, out int line);
        int length = Math.Min(LetterPerLine, plainText.Length);
        line += plainText.Length.DivCeil(LetterPerLine);
        var bitmap = new Bitmap(FIGURE_SIZE * length, FIGURE_SIZE * line);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(Foreground, LINE_WIDTH);
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
                int fi = U_Letters.IndexOf(plainText[i]);
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
                graphics.Translate(FIGURE_SIZE);
            }
            graphics.TranslateBreakLine(FIGURE_SIZE);
        }
        return bitmap;
    }

    /// <summary>
    /// 解密图像(只支持标准形状)
    /// </summary>
    public static string Decrypt(Bitmap bitmap)
    {
        var strBuilder = new StringBuilder();

        for (int y = 0; y < bitmap.Height; y += FIGURE_SIZE)
        {
            for (int x = 0; x < bitmap.Width; x += FIGURE_SIZE)
            {
                if (CheckBlackBox(bitmap, x, y))
                {
                    strBuilder.Append('?');
                    continue;
                }
                var hasDot = CheckDot(bitmap, x, y);
                if (CheckFigureLeft(bitmap, x, y))
                    strBuilder.Append(hasDot ? 'Y' : 'U');
                else if (CheckFigureUp(bitmap, x, y))
                    strBuilder.Append(hasDot ? 'Z' : 'V');
                else if (CheckFigureRight(bitmap, x, y))
                    strBuilder.Append(hasDot ? 'X' : 'T');
                else if (CheckFigureDown(bitmap, x, y))
                    strBuilder.Append(hasDot ? 'W' : 'S');
                else
                    strBuilder.Append(figureDict[(CheckLines(bitmap, x, y), hasDot)]);
            }
            strBuilder.AppendLine();
        }
        return strBuilder.Remove(strBuilder.Length - 2, 2).ToString();
    }

    [SkipLocalsInit]
    private static string Purify(string text, out int line)
    {
        int length = text.Length;
        Span<char> str = length.CanAllocString() ? stackalloc char[length] : new char[length];
        int count = 0;
        line = 0;
        foreach (var character in text)
        {
            if (character is >= 'a' and <= 'z')
                str[count++] = (char)(character ^ 0x20);//大小写转换
            else if (character == '\n')
            {
                line++;
                str[count++] = character;
            }
            else if (character.IsPrintable())
                str[count++] = character;
        }
        return new string(str[..count]);
    }
}
