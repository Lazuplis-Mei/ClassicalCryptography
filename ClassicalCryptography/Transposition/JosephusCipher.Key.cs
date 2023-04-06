namespace ClassicalCryptography.Transposition;

public partial class JosephusCipher
{
    /// <summary>
    /// 约瑟夫置换密码的密钥
    /// </summary>
    [Introduction("约瑟夫置换密码的密钥", "代表第几个人出列。")]
    public class Key : IKey<ushort>
    {
        /// <summary>
        /// 默认密钥7
        /// </summary>
        public static readonly Key Default = new(7);

        /// <summary>
        /// 约瑟夫置换密码的密钥
        /// </summary>
        public Key(ushort m)
        {
            Guard.IsGreaterThan(m, (ushort)0);
            KeyValue = m;
        }

        /// <summary>
        /// 第几个人出列
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
            int m = Random.Shared.Next(1, textLength);
            return new Key((ushort)(m + 1));
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength) => textLength;

        /// <inheritdoc/>
        public string GetString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override string ToString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => KeyValue.GetHashCode();
    }
}
