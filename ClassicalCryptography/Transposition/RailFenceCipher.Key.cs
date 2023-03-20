namespace ClassicalCryptography.Transposition;


public partial class RailFenceCipher
{
    /// <summary>
    /// 栅栏密码的密钥
    /// </summary>
    [Introduction("栅栏密码的密钥", "代表每组字数。")]
    public class Key : IKey<int>
    {
        /// <summary>
        /// 栅栏密码的密钥
        /// </summary>
        public Key(int m) => KeyValue = m;

        /// <summary>
        /// 每组字数
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
            int m = Random.Shared.Next(2, textLength);
            return new Key(m);
        }
        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength) => textLength - 3;
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
