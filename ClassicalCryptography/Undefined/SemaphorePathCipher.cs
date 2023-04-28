using ZXing.Common;

namespace ClassicalCryptography.Undefined;

/// <summary>
/// 旗语路径密码
/// </summary>
[Introduction("旗语路径密码", "以字母代表的旗语作为路径的指向，去除路径上的字母。")]
public static partial class SemaphorePathCipher
{
    /// <summary>
    /// 旗语字母
    /// </summary>
    public static readonly string FLetters = $"{U_Letters}%#";

    #region 符号路径定义

    private static readonly (int dx, int dy) U = (0, -1);
    private static readonly (int dx, int dy) D = (0, 1);
    private static readonly (int dx, int dy) L = (-1, 0);
    private static readonly (int dx, int dy) R = (1, 0);
    private static readonly (int dx, int dy) UL = (-1, -1);
    private static readonly (int dx, int dy) UR = (1, -1);
    private static readonly (int dx, int dy) DL = (-1, 1);
    private static readonly (int dx, int dy) DR = (1, 1);
    private static readonly (int dx, int dy)[] directions = { U, D, L, R, UL, UR, DL, DR };

    private static readonly ((int dx, int dy) In, (int dx, int dy) Out)[] semaphores =
    {
        (DL, D),      //A
        (L, D),       //B
        (UL, D),      //C
        (U, D),       //D
        (D, UR),      //E
        (D, R),       //F
        (D, DR),      //G
        (L, DL),      //H
        (UL, DL),     //I
        (U, R),       //J
        (DL, U),      //K
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

    #endregion 符号路径定义

    /// <summary>
    /// 未定义的密码类型
    /// </summary>
    public static CipherType Type => CipherType.Undefined;

    /// <summary>
    /// 生成字符矩阵
    /// </summary>
    /// <param name="textLength">文字长度</param>
    /// <param name="height">矩阵高度</param>
    /// <param name="width">矩阵宽度</param>
    /// <param name="tolerance">结果长度允许的填充数</param>
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
                (int x, int y) next = queue.Dequeue();
                if ((x, y) == (0, 0) || directions.Contains((next.x - x, next.y - y)))
                {
                    (x, y) = next;
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
                    foreach (var (dx, dy) in directions)
                    {
                        next = (x + dx, y + dy);
                        if (next.x >= 0 && next.y >= 0 &&
                            next.x < width && next.y < height &&
                            !queue.Contains(next) && !block.Contains(next))
                            queue.Enqueue(next);
                    }
                }
            }

            block.Clear();
            queue.Clear();
            retry--;
        }
        throw new Exception("超过重试次数");
    }

    /// <summary>
    /// 填充文本到字符矩阵
    /// </summary>
    /// <param name="text"></param>
    /// <param name="characterMatrix"></param>
    [SkipLocalsInit]
    public static string[] FillText(string text, char[,] characterMatrix)
    {
        int width = characterMatrix.GetLength(0);
        int height = characterMatrix.GetLength(1);
        Span<char> span = stackalloc char[width];
        string[] result = new string[height];
        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                span[x] = characterMatrix[x, y];
                if (span[x] == '\0')
                    span[x] = i >= text.Length ? TranspositionHelper.GetPaddingChar() : text[i++];
            }
            result[y] = new string(span);
        }
        return result;
    }

    /// <summary>
    /// 提取其中的文本
    /// </summary>
    public static string GetText(string[] lines)
    {
        int height = lines.Length;
        int width = lines[0].Length;
        var result = new StringBuilder();
        var visited = new BitMatrix(width, height);

        (int x, int y) from = (-1, 0);
        var (x, y) = (0, 0);

        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            int i = FLetters.IndexOf(lines[y][x]);
            if (i == -1 || visited[x, y])
                throw new FormatException("不正确的字符矩阵");

            visited[x, y] = true;
            var (In, Out) = semaphores[i];
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
            {
                throw new FormatException("不正确的字符矩阵");
            }
        }

        for (y = 0; y < height; y++)
            for (x = 0; x < width; x++)
                if (!visited[x, y])
                    result.Append(lines[y][x]);

        return result.ToString();
    }

    /// <summary>
    /// 加密指定的文本
    /// </summary>
    public static string[] Encrypt(string plainText)
    {
        int size = (plainText.Length * 3).SqrtCeil();
        char[,]? characterMatrix = null;
        int tolerance = 0;
        while (tolerance <= 10)
        {
            try
            {
                characterMatrix = GenerateMatrix(plainText.Length, size, size, tolerance);
                break;
            }
            catch
            {
                tolerance += 2;
            }
        }
        if (characterMatrix is null)
            throw new Exception("超过重试容忍次数");
        return FillText(plainText, characterMatrix);
    }

    private static void FillMatrix(char[,] matrix, List<(int x, int y)> block, int textLength)
    {
        (int x, int y) from = (-1, 0);

        for (int i = textLength; i < block.Count - 1; i++)
        {
            var flag1 = (from.x - block[i].x, from.y - block[i].y);
            from = block[i];
            var flag2 = (block[i + 1].x - from.x, block[i + 1].y - from.y);

            int j = Array.IndexOf(semaphores, (flag1, flag2));
            if (j == -1)
                j = Array.IndexOf(semaphores, (flag2, flag1));

            matrix[from.x, from.y] = FLetters[j];
        }
    }
}
