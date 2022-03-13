using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Numerics;

namespace ClassicalCryptography.Transposition2D;

public partial class SpiralCurveCipher
{
    /// <summary>
    /// 螺旋曲线密码的密钥
    /// </summary>
    [Introduction("螺旋曲线密码的密钥", "文本表示成指定宽度的矩形。")]
    public class Key : IKey<int>
    {
        /// <summary>
        /// 宽度分割密钥
        /// </summary>
        public Key(int width) => KeyValue = width;

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
            return new Key(ushort.Parse(strKey));
        }
        /// <summary>
        /// 产生随机密钥
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static IKey<int> GenerateKey(int textLength)
        {
            int width = Random.Shared.Next(1, textLength.DivCeil(2));
            return new Key(width + 1);
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
}
