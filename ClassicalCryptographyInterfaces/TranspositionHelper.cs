namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 换位密码使用的扩展方法
/// </summary>
public static class TranspositionHelper
{
    /// <summary>
    /// 补充字符
    /// </summary>
    public static char PaddingChar { get; set; } = '`';
    /// <summary>
    /// 字符不足时使用的补充字符
    /// </summary>
    public static PaddingMode PaddingMode { get; set; } = PaddingMode.SingleChar;

    private static char GetPaddingChar()
    {
        return PaddingMode switch
        {
            PaddingMode.SingleChar => PaddingChar,
            PaddingMode.Digit => (char)('0' + Random.Shared.Next(10)),
            PaddingMode.LowerLetter => (char)('a' + Random.Shared.Next(26)),
            PaddingMode.UpperLetter => (char)('A' + Random.Shared.Next(26)),
            _ => ' ',
        };
    }

    /// <summary>
    /// 填充顺序
    /// </summary>
    /// <param name="order">顺序数组</param>
    public static void FillOrder(this ushort[] order)
    {
        for (ushort i = 0; i < order.Length; i++)
            order[i] = i;
    }

    /// <summary>
    /// 填充二维数组的顺序(按行填充)
    /// </summary>
    /// <param name="order">顺序数组</param>
    public static void FillOrderByRow(this ushort[,] order)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                order[x, y] = (ushort)(x + y * width);
    }

    /// <summary>
    /// 填充二维数组的顺序(按列填充)
    /// </summary>
    /// <param name="order">顺序数组</param>
    public static void FillOrderByColumn(this ushort[,] order)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                order[x, y] = (ushort)(x * height + y);
    }

    /// <summary>
    /// 依据顺序拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    public static string AssembleText(this ushort[] order, string text)
    {
        Span<char> buffer = stackalloc char[order.Length];
        for (int i = 0; i < order.Length; i++)
            buffer[i] = order[i] < text.Length ? text[order[i]] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据逆向顺序拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    public static string AssembleTextInverse(this ushort[] order, string text)
    {
        Span<char> buffer = stackalloc char[order.Length];
        for (int i = 0; i < order.Length; i++)
            buffer[order[i]] = i < text.Length ? text[i] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据顺序按行拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    public static string AssembleTextByRow(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        Span<char> buffer = stackalloc char[width * height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[x + y * width] = order[x, y] < text.Length ?
                    text[order[x, y]] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据逆向顺序按行拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    public static string AssembleTextByRowInverse(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        Span<char> buffer = stackalloc char[width * height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[order[x, y]] = x + y * width < text.Length ?
                        text[x + y * width] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据顺序按列拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    public static string AssembleTextByColumn(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        Span<char> buffer = stackalloc char[width * height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[x * height + y] = order[x, y] < text.Length ?
                    text[order[x, y]] : GetPaddingChar();
        return new string(buffer);
    }

    /// <summary>
    /// 依据逆向顺序按列拼合文本
    /// </summary>
    /// <param name="text">参考文本</param>
    /// <param name="order">顺序</param>
    public static string AssembleTextByColumnInverse(this ushort[,] order, string text)
    {
        int width = order.GetLength(0);
        int height = order.GetLength(1);
        Span<char> buffer = stackalloc char[width * height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                buffer[order[x, y]] = x * height + y < text.Length ?
                        text[x * height + y] : GetPaddingChar();
        return new string(buffer);
    }

}
