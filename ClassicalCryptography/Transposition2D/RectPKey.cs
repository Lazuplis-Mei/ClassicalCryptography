using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Globalization;
using System.Numerics;

namespace ClassicalCryptography.Transposition2D;

/// <summary>
/// 宽度分割密钥,可用于<see cref="SpiralCurveCipher"/>
/// </summary>
[Introduction("宽度分割密钥", "文本表示成指定宽度的矩形。")]
public class WidthKey : IKey<int>
{
    /// <summary>
    /// 宽度分割密钥,可用于<see cref="SpiralCurveCipher"/>
    /// </summary>
    public WidthKey(int width) => KeyValue = width;

    /// <summary>
    /// 宽度值
    /// </summary>
    public int KeyValue { get; }
    /// <summary>
    /// 密钥不可逆
    /// </summary>
    public bool CanInverse => false;
    /// <summary>
    /// 不存在可逆密钥
    /// </summary>
    public IKey<int>? InversedKey => null;
    /// <summary>
    /// 从文本格式创建密钥
    /// </summary>
    /// <param name="strKey">文本类型的密钥</param>
    public static IKey<int> FromString(string strKey)
    {
        return new WidthKey(ushort.Parse(strKey));
    }
    /// <summary>
    /// 产生随机密钥
    /// </summary>
    /// <param name="textLength">加密内容的长度</param>
    public static IKey<int> GenerateKey(int textLength)
    {
        int width = Random.Shared.Next(1, textLength.DivCeil(2));
        return new WidthKey(width + 1);
    }
    /// <summary>
    /// 获得密钥的空间
    /// </summary>
    /// <param name="textLength">加密内容的长度</param>
    public static BigInteger GetKeySpace(int textLength) => textLength >> 1;
    /// <summary>
    /// 字符串形式
    /// </summary>
    public string GetString() => KeyValue.ToString();

    /// <summary>
    /// 字符串形式
    /// </summary>
    public override string ToString() => KeyValue.ToString();
}