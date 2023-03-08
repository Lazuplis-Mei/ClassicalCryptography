namespace ClassicalCryptography.Image;

/// <summary>
/// 摩尔条纹的类型(用于为单个字符串绘制条纹)
/// </summary>
public static class MoirePatternTypes
{
    /// <summary>
    /// 没有条纹
    /// </summary>
    public static Func<int, int, bool> NoPatten => (_, _) => false;


    /// <summary>
    /// 竖直线条纹
    /// </summary>
    public static Func<int, int, bool> VerticalPatten => (x, _) => (x % 2) == 0;

    /// <summary>
    /// 横直线条纹
    /// </summary>
    public static Func<int, int, bool> HorizontalPatten => (_, y) => (y % 2) == 0;

    /// <summary>
    /// 对角线条纹
    /// </summary>
    public static Func<int, int, bool> DiagonalPatten => (x, y) => (x + y) % 4 < 2;

    /// <summary>
    /// 对角线条纹2
    /// </summary>
    public static Func<int, int, bool> DiagonalPatten2 => (x, y) => Math.Abs(x - y) % 4 < 2;

    /// <summary>
    /// 正弦曲线条纹(建议使用嵌入式)
    /// </summary>
    public static Func<int, int, bool> SinWavePatten => (x, y) => (int)(200 * Math.Sin(x * Math.PI / 180) + y) % 3 == 0;
}