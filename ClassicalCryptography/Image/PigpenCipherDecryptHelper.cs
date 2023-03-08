using System.Drawing;
namespace ClassicalCryptography.Image;

public static partial class PigpenCipher
{

    private static readonly Dictionary<((bool, bool, bool, bool), bool), char> figureDict = new()
    {
        { ((false, false, true, true), false), 'A' },
        { ((true, false, true, true), false), 'B' },
        { ((true, false, false, true), false), 'C' },
        { ((false, true, true, true), false), 'D' },
        { ((true, true, true, true), false), 'E' },
        { ((true, true, false, true), false), 'F' },
        { ((false, true, true, false), false), 'G' },
        { ((true, true, true, false), false), 'H' },
        { ((true, true, false, false), false), 'I' },
        { ((false, false, true, true), true), 'J' },
        { ((true, false, true, true), true), 'K' },
        { ((true, false, false, true), true), 'L' },
        { ((false, true, true, true), true), 'M' },
        { ((true, true, true, true), true), 'N' },
        { ((true, true, false, true), true), 'O' },
        { ((false, true, true, false), true), 'P' },
        { ((true, true, true, false), true), 'Q' },
        { ((true, true, false, false), true), 'R' },
        { ((false, false, false, false), false), ' ' },
    };

    private static bool CheckBlackBox(Bitmap bitmap, int startX, int startY)
    {
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        int t = 0;
        for (int y = PADDING; y < NEGATIVE_PADDING; y += CHECK_STEP / 2)
        {
            for (int x = PADDING; x < NEGATIVE_PADDING; x += CHECK_STEP / 2)
            {
                color = bitmap.GetPixel(startX + x, startY + y);
                tA += color.A; tR += color.R; tG += color.G; tB += color.B;
                t++;
            }
        }
        color = Color.FromArgb(tA / t, tR / t, tG / t, tB / t);
        return color.GetBrightness() <= 0.4f;
    }

    private static bool CheckDot(Bitmap bitmap, int startX, int startY)
    {
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color = bitmap.GetPixel(startX + CENTER_POSITION - 2, startY + CENTER_POSITION);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + CENTER_POSITION, startY + CENTER_POSITION - 2);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + CENTER_POSITION + 2, startY + CENTER_POSITION);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + CENTER_POSITION, startY + CENTER_POSITION + 2);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + CENTER_POSITION, startY + CENTER_POSITION);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = Color.FromArgb(tA / 5, tR / 5, tG / 5, tB / 5);
        return color.GetBrightness() <= 0.1f;
    }

    private static bool CheckFigureDown(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = y = PADDING; y < NEGATIVE_PADDING; y += CHECK_STEP, x += CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y + 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        if (color.GetBrightness() > 0.1f)
            return false;

        tA = tR = tG = tB = 0;
        for (x = NEGATIVE_PADDING, y = PADDING; y < NEGATIVE_PADDING; y += CHECK_STEP, x -= CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y + 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        return color.GetBrightness() <= 0.1f;
    }

    private static bool CheckFigureLeft(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = NEGATIVE_PADDING, y = PADDING; x > PADDING; x -= CHECK_STEP, y += CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y + 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        if (color.GetBrightness() > 0.1f)
            return false;

        tA = tR = tG = tB = 0;
        for (x = y = NEGATIVE_PADDING; x > PADDING; x -= CHECK_STEP, y -= CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y - 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        return color.GetBrightness() <= 0.1f;
    }

    private static bool CheckFigureRight(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = y = PADDING; x < NEGATIVE_PADDING; x += CHECK_STEP, y += CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y + 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        if (color.GetBrightness() > 0.1f)
            return false;

        tA = tR = tG = tB = 0;
        for (x = PADDING, y = NEGATIVE_PADDING; x < NEGATIVE_PADDING; x += CHECK_STEP, y -= CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y - 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        return color.GetBrightness() <= 0.1f;
    }

    private static bool CheckFigureUp(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = PADDING, y = NEGATIVE_PADDING; y > PADDING; y -= CHECK_STEP, x += CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y - 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        if (color.GetBrightness() > 0.1f)
            return false;

        tA = tR = tG = tB = 0;
        for (x = y = NEGATIVE_PADDING; y > PADDING; y -= CHECK_STEP, x -= CHECK_STEP / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y - 2);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);

        return color.GetBrightness() <= 0.1f;
    }

    //left top right buttom
    private static (bool, bool, bool, bool) CheckLines(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = y = PADDING; y < NEGATIVE_PADDING; y += CHECK_STEP)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var left = color.GetBrightness() <= 0.1f;


        tA = tR = tG = tB = 0;
        for (x = y = PADDING; x < NEGATIVE_PADDING; x += CHECK_STEP)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var top = color.GetBrightness() <= 0.1f;


        tA = tR = tG = tB = 0;
        for (x = NEGATIVE_PADDING, y = PADDING; y < NEGATIVE_PADDING; y += CHECK_STEP)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var right = color.GetBrightness() <= 0.1f;


        tA = tR = tG = tB = 0;
        for (x = PADDING, y = NEGATIVE_PADDING; x < NEGATIVE_PADDING; x += CHECK_STEP)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var bottom = color.GetBrightness() <= 0.1f;

        return (left, top, right, bottom);
    }
}