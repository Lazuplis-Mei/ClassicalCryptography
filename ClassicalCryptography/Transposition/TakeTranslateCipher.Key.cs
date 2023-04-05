using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Transposition;

public partial class TakeTranslateCipher
{
    /// <summary>
    /// 取后平移密码的密钥
    /// </summary>
    [Introduction("取后平移密码的密钥", "n为每次取出个数，k为每次平移个数")]
    public class Key : IKey<(int N, int K)>
    {
        /// <summary>
        /// 默认密钥(1,1)
        /// </summary>
        public static readonly Key Default = new(1, 1);

        private readonly (int N, int K) keyValue;

        private Key(int N, int K) => keyValue = (N, K);

        /// <summary>
        /// (N,K)
        /// </summary>
        public (int N, int K) KeyValue => keyValue;

        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;

        /// <summary>
        /// 密钥不可逆，将始终为null
        /// </summary>
        public IKey<(int N, int K)>? InversedKey => null;

        /// <summary>
        /// 从文本格式创建密钥(10~//)参考<see cref="VChar64"/>
        /// </summary>
        /// <param name="strKey">密钥文本</param>
        public static IKey<(int N, int K)> FromString(string strKey)
        {
            if (strKey.Length != 2)
                throw new ArgumentException("应为2个字符", nameof(strKey));
            int n = VChar64.IndexOf(strKey[0]);
            int k = VChar64.IndexOf(strKey[1]);
            if (n <= 0 || k == -1)
                throw new FormatException("不正确的格式");
            return new Key(n, k);
        }

        /// <summary>
        /// 产生随机密钥(值的范围小于64)
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static IKey<(int N, int K)> GenerateKey(int textLength)
        {
            int length = Math.Min(64, textLength);
            int n = Random.Shared.Next(1, length);
            int k = Random.Shared.Next(0, length);
            return new Key(n, k);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            return Math.Min(textLength * textLength, 4032);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{VChar64[KeyValue.N]}{VChar64[KeyValue.K]}";
        }

        /// <inheritdoc/>
        public string GetString() => ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => KeyValue.GetHashCode();
    }
}
