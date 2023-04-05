using System.Drawing;
using System.Runtime.Versioning;

namespace ClassicalCryptography.Undefined;

public static partial class SemaphorePathCipher
{

    private const int LINE_WIDTH = 5;
    private const int FIGURE_SIZE = 60;
    private const int CENTER_POSITION = FIGURE_SIZE / 2;
    private static readonly Point centerPoint = new(CENTER_POSITION, CENTER_POSITION);
    private static readonly Point[] linePoints = { default, centerPoint, default };

    /// <summary>
    /// 绘制旗语路径
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static Bitmap DrawSemaphores(char[,] characterMatrix)
    {
        int width = characterMatrix.GetLength(0);
        int height = characterMatrix.GetLength(1);
        var bitmap = new Bitmap(FIGURE_SIZE * width, FIGURE_SIZE * height);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(Color.Black, LINE_WIDTH);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int i = FLetters.IndexOf(characterMatrix[x, y]);
                if (i != -1)
                {
                    var (In, Out) = semaphores[i];
                    linePoints[0] = centerPoint;
                    linePoints[0].X += In.dx * CENTER_POSITION;
                    linePoints[0].Y += In.dy * CENTER_POSITION;
                    linePoints[2] = centerPoint;
                    linePoints[2].X += Out.dx * CENTER_POSITION;
                    linePoints[2].Y += Out.dy * CENTER_POSITION;
                    graphics.DrawLines(pen, linePoints);
                }
                graphics.Translate(FIGURE_SIZE);
            }
            graphics.TranslateBreakLine(FIGURE_SIZE);
        }
        return bitmap;
    }
}