using ClassicalCryptography.Interfaces;
using System.Numerics;

namespace ClassicalCryptography.Transposition;

public partial class JosephusCipher
{
    /// <summary>
    /// 约瑟夫置换密码的密钥
    /// </summary>
    [Introduction("约瑟夫置换密码的密钥", "第几个人出列。")]
    public class Key : IKey<int>
    {
        /// <summary>
        /// 约瑟夫置换密码的密钥
        /// </summary>
        public Key(int m) => KeyValue = m;

        /// <summary>
        /// 第几个人出列
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
            int m = Random.Shared.Next(1, textLength);
            return new Key(m + 1);
        }
        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength) => textLength;
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
