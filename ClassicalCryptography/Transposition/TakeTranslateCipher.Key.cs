using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Numerics;

namespace ClassicalCryptography.Transposition;


public partial class TakeTranslateCipher
{
    /// <summary>
    /// 取后平移密码的密钥
    /// </summary>
    public class Key : IKey<(int N, int K)>
    {
        /// <summary>
        /// 默认密钥
        /// </summary>
        public static readonly Key Default = new(1, 1);
        private readonly (int N, int K) keyValue;
        /// <summary>
        /// (N,K)
        /// </summary>
        public (int N, int K) KeyValue => keyValue;

        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;
        /// <summary>
        /// 密钥不可逆，将为null
        /// </summary>
        public IKey<(int N, int K)>? InversedKey => null;
        private Key(int N, int K) => keyValue = (N, K);

        /// <summary>
        /// 从文本格式创建密钥(10~//)参考<see cref="Globals.VBase64"/>
        /// </summary>
        /// <param name="strKey">密钥文本</param>
        public static IKey<(int N, int K)> FromString(string strKey)
        {
            if (strKey.Length != 2)
                throw new ArgumentException("应为2个字符", nameof(strKey));
            int n = Globals.VBase64.IndexOf(strKey[0]);
            int k = Globals.VBase64.IndexOf(strKey[1]);
            if (n <= 0 || k == -1)
                throw new FormatException("不正确的格式");
            return new Key(n, k);
        }
        /// <summary>
        /// 产生随机密钥(值不会大于64)
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static IKey<(int N, int K)> GenerateKey(int textLength)
        {
            int length = Math.Min(64, textLength);
            int n = Random.Shared.Next(1, length);
            int k = Random.Shared.Next(0, length);
            return new Key(n, k);
        }
        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength)
        {
            return Math.Min(textLength * textLength, 4032);
        }
        /// <summary>
        /// 字符串形式
        /// </summary>
        public override string ToString()
        {
            return $"{Globals.VBase64[KeyValue.N]}{Globals.VBase64[KeyValue.K]}";
        }
        /// <summary>
        /// 字符串形式
        /// </summary>
        public string GetString() => ToString();
    }
}
