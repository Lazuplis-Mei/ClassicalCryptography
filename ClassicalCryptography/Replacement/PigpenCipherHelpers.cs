﻿using System.Drawing;
namespace ClassicalCryptography.Replacement;

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
        for (int y = posPadding; y < negPadding; y += checkStep / 2)
        {
            for (int x = posPadding; x < negPadding; x += checkStep / 2)
            {
                color = bitmap.GetPixel(startX + x, startY + y);
                tA += color.A;
                tR += color.R;
                tG += color.G;
                tB += color.B;
                t++;
            }
        }
        color = Color.FromArgb(tA / t, tR / t, tG / t, tB / t);
        return color.GetBrightness() <= 0.4f;
    }

    private static bool CheckDot(Bitmap bitmap, int startX, int startY)
    {
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color = bitmap.GetPixel(startX + midPosition - 2, startY + midPosition);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + midPosition, startY + midPosition - 2);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + midPosition + 2, startY + midPosition);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + midPosition, startY + midPosition + 2);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = bitmap.GetPixel(startX + midPosition, startY + midPosition);
        tA += color.A; tR += color.R; tG += color.G; tB += color.B;
        color = Color.FromArgb(tA / 5, tR / 5, tG / 5, tB / 5);
        return color.GetBrightness() <= 0.1f;
    }

    private static bool CheckFigureDown(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = y = posPadding; y < negPadding; y += checkStep, x += checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y + 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l1 = color.GetBrightness() <= 0.1f;
        if (!l1) return false;

        tA = tR = tG = tB = 0;
        for (x = negPadding, y = posPadding; y < negPadding; y += checkStep, x -= checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y + 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l2 = color.GetBrightness() <= 0.1f;
        return l2;
    }

    private static bool CheckFigureLeft(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = negPadding, y = posPadding; x > posPadding; x -= checkStep, y += checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y + 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l1 = color.GetBrightness() <= 0.1f;
        if (!l1) return false;

        tA = tR = tG = tB = 0;
        for (x = y = negPadding; x > posPadding; x -= checkStep, y -= checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y - 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l2 = color.GetBrightness() <= 0.1f;
        return l2;
    }

    private static bool CheckFigureRight(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = y = posPadding; x < negPadding; x += checkStep, y += checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y + 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l1 = color.GetBrightness() <= 0.1f;
        if (!l1) return false;

        tA = tR = tG = tB = 0;
        for (x = posPadding, y = negPadding; x < negPadding; x += checkStep, y -= checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y - 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l2 = color.GetBrightness() <= 0.1f;
        return l2;
    }

    private static bool CheckFigureUp(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = posPadding, y = negPadding; y > posPadding; y -= checkStep, x += checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x + 2, startY + y - 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l1 = color.GetBrightness() <= 0.1f;
        if (!l1) return false;

        tA = tR = tG = tB = 0;
        for (x = y = negPadding; y > posPadding; y -= checkStep, x -= checkStep / 2)
        {
            color = bitmap.GetPixel(startX + x - 2, startY + y - 2);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var l2 = color.GetBrightness() <= 0.1f;
        return l2;
    }

    private static (bool Left, bool Top, bool Right, bool Bottom) CheckLines(Bitmap bitmap, int startX, int startY)
    {
        int x, y;
        int tA = 0, tR = 0, tG = 0, tB = 0;
        Color color;
        for (x = y = posPadding; y < negPadding; y += checkStep)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var left = color.GetBrightness() <= 0.1f;


        tA = tR = tG = tB = 0;
        for (x = y = posPadding; x < negPadding; x += checkStep)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var top = color.GetBrightness() <= 0.1f;


        tA = tR = tG = tB = 0;
        for (x = negPadding, y = posPadding; y < negPadding; y += checkStep)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var right = color.GetBrightness() <= 0.1f;


        tA = tR = tG = tB = 0;
        for (x = posPadding, y = negPadding; x < negPadding; x += checkStep)
        {
            color = bitmap.GetPixel(startX + x, startY + y);
            tA += color.A;
            tR += color.R;
            tG += color.G;
            tB += color.B;
        }
        color = Color.FromArgb(tA / 6, tR / 6, tG / 6, tB / 6);
        var bottom = color.GetBrightness() <= 0.1f;

        return (left, top, right, bottom);
    }
}