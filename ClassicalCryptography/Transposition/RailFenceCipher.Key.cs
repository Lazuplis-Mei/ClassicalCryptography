namespace ClassicalCryptography.Transposition;

public partial class RailFenceCipher
{
    /// <summary>
    /// 栅栏密码的密钥
    /// </summary>
    [Introduction("栅栏密码的密钥", "代表每组字数。")]
    public class Key : IKey<ushort>
    {
        /// <summary>
        /// 栅栏密码的密钥
        /// </summary>
        public Key(ushort m) => KeyValue = m;

        /// <summary>
        /// 每组字数
        /// </summary>
        public ushort KeyValue { get; }

        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;

        /// <summary>
        /// 密钥不可逆，将始终为null
        /// </summary>
        public IKey<ushort>? InversedKey => null;

        /// <inheritdoc/>
        public static IKey<ushort> FromString(string strKey)
        {
            return new Key(ushort.Parse(strKey));
        }

        /// <inheritdoc/>
        public static IKey<ushort> GenerateKey(int textLength)
        {
            int m = Random.Shared.Next(2, textLength);
            return new Key((ushort)m);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength) => textLength - 3;

        /// <inheritdoc/>
        public string GetString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override string ToString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => KeyValue.GetHashCode();
    }
}
