namespace ClassicalCryptography.Transposition2D;

public partial class JigsawCipher
{
    /// <summary>
    /// 锯齿分割密码的密钥
    /// </summary>
    [Introduction("锯齿分割密码的密钥", "正确的格式为某数的一个分拆")]
    public class Key : IKey<ushort[]>
    {
        private Key(ushort[] keyValue) => KeyValue = keyValue;

        /// <summary>
        /// 整数的一个分拆
        /// </summary>
        public ushort[] KeyValue { get; }

        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;

        /// <summary>
        /// 密钥不可逆，将始终为null
        /// </summary>
        public IKey<ushort[]>? InversedKey => null;

        /// <inheritdoc/>
        public static IKey<ushort[]> FromString(string strKey)
        {
            var nums = strKey.Split(',', (StringSplitOptions)3);
            ushort[] vals = new ushort[nums.Length];
            for (int i = 0; i < vals.Length; i++)
                vals[i] = ushort.Parse(nums[i]);
            return new Key(vals);
        }

        /// <inheritdoc/>
        public static IKey<ushort[]> GenerateKey(int textLength)
        {
            int count = textLength.SqrtCeil();
            var partition = new List<ushort>();
            while (count > 0)
            {
                var n = (ushort)Random.Shared.Next(1, count);
                partition.Add(n);
                count -= n;
            }
            return new Key(partition.ToArray());
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            int n = textLength.SqrtCeil();
            return BigInteger.One << (n - 1);
        }

        /// <inheritdoc/>
        public string GetString() => ToString();

        /// <inheritdoc/>
        public override string ToString() => string.Join(',', KeyValue);

        /// <inheritdoc/>
        public override int GetHashCode() => KeyValue.GetHashCode();
    }
}
