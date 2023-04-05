using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Image;

/// <summary>
/// <a href="https://en.wikipedia.org/wiki/Cistercian_numerals">僧侣密码数字</a>，又叫西多会修道士数字系统
/// </summary>
[SupportedOSPlatform("windows")]
public class CisterciansNumerals
{
    /// <summary>
    /// 前景色
    /// </summary>
    public static Brush Foreground { get; set; } = Brushes.Black;

    /// <summary>
    /// 背景色
    /// </summary>
    public static Color? Background { get; set; } = Color.White;

    #region 常量

    private const int UNIT_SIZE = 40;
    private const int PADDING = 4;
    private const int FIGURE_WIDTH = UNIT_SIZE * 2 + PADDING * 2;
    private const int FIGURE_HEIGHT = UNIT_SIZE * 3 + PADDING * 2;
    private const int LINE_WIDTH = 3;

    private static readonly Point LT = new(PADDING, PADDING);
    private static readonly Point LU = new(PADDING, PADDING + UNIT_SIZE);
    private static readonly Point LD = new(PADDING, PADDING + UNIT_SIZE * 2);
    private static readonly Point LB = new(PADDING, FIGURE_HEIGHT - PADDING);
    private static readonly Point MT = new(PADDING + UNIT_SIZE, PADDING);
    private static readonly Point MU = new(PADDING + UNIT_SIZE, PADDING + UNIT_SIZE);
    private static readonly Point MD = new(PADDING + UNIT_SIZE, PADDING + UNIT_SIZE * 2);
    private static readonly Point MB = new(PADDING + UNIT_SIZE, FIGURE_HEIGHT - PADDING);
    private static readonly Point RT = new(PADDING + UNIT_SIZE * 2, PADDING);
    private static readonly Point RU = new(PADDING + UNIT_SIZE * 2, PADDING + UNIT_SIZE);
    private static readonly Point RD = new(PADDING + UNIT_SIZE * 2, PADDING + UNIT_SIZE * 2);
    private static readonly Point RB = new(PADDING + UNIT_SIZE * 2, FIGURE_HEIGHT - PADDING);

    private static readonly Point[][] figures1 = new[]
    {
        new[] { MT, RT },
        new[] { MU, RU },
        new[] { MT, RU },
        new[] { MU, RT },
        new[] { MU, RT, MT },
        new[] { RT, RU },
        new[] { MT, RT, RU },
        new[] { MU, RU, RT },
        new[] { MU, RU, RT, MT },
    };

    private static readonly Point[][] figures2 = new[]
    {
        new[] { MT, LT },
        new[] { MU, LU },
        new[] { MT, LU },
        new[] { MU, LT },
        new[] { MU, LT, MT },
        new[] { LT, LU },
        new[] { MT, LT, LU },
        new[] { MU, LU, LT },
        new[] { MU, LU, LT, MT },
    };

    private static readonly Point[][] figures3 = new[]
    {
        new[] { MB, RB },
        new[] { MD, RD },
        new[] { MB, RD },
        new[] { MD, RB },
        new[] { MD, RB, MB },
        new[] { RB, RD },
        new[] { MB, RB, RD },
        new[] { MD, RD, RB },
        new[] { MD, RD, RB, MB },
    };

    private static readonly Point[][] figures4 = new[]
    {
        new[] { MB, LB },
        new[] { MD, LD },
        new[] { MB, LD },
        new[] { MD, LB },
        new[] { MD, LB, MB },
        new[] { LB, LD },
        new[] { MB, LB, LD },
        new[] { MD, LD, LB },
        new[] { MD, LD, LB, MB },
    };

    #endregion 常量

    /// <summary>
    /// 就数字转换成僧侣密码
    /// </summary>
    public static Bitmap Encode(ulong number)
    {
        var digits = BaseConverter.ToBase10000(number);
        int length = digits.Length;
        var bitmap = new Bitmap(FIGURE_WIDTH * length, FIGURE_HEIGHT);
        using var pen = new Pen(Foreground, LINE_WIDTH);
        using var graphics = Graphics.FromImage(bitmap);
        if (Background.HasValue)
            graphics.Clear(Background.Value);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        for (int i = 0; i < length; i++)
        {
            var digit = digits[i];
            var n4 = digit / 1000;
            var n3 = (digit / 100) % 10;
            var n2 = (digit / 10) % 10;
            var n1 = digit % 10;

            graphics.DrawLine(pen, MT, MB);
            if (n1 != 0)
                graphics.DrawLines(pen, figures1[n1 - 1]);
            if (n2 != 0)
                graphics.DrawLines(pen, figures2[n2 - 1]);
            if (n3 != 0)
                graphics.DrawLines(pen, figures3[n3 - 1]);
            if (n4 != 0)
                graphics.DrawLines(pen, figures4[n4 - 1]);
            graphics.Translate(FIGURE_WIDTH);
        }
        return bitmap;
    }
}
