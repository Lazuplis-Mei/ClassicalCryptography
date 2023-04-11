namespace ClassicalCryptography.Transposition2D;

public partial class TwiddlePuzzle
{
    /// <summary>
    /// 旋转阵列密码的密钥
    /// </summary>
    [Introduction("旋转阵列密码的密钥", "密钥的数字代表第几个旋转的位置")]
    public class Key : IKey<ushort[]>
    {
        /// <summary>
        /// 旋转阵列密码的密钥
        /// </summary>
        private Key(ushort[] keyValue) => KeyValue = keyValue;

        /// <inheritdoc/>
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
            ushort[] array = new ushort[nums.Length];
            for (int i = 0; i < array.Length; i++)
                if (ushort.TryParse(nums[i], out ushort value))
                    array[i] = value;

            return new Key(array);
        }

        /// <inheritdoc/>
        public static IKey<ushort[]> GenerateKey(int textLength)
        {
            int n = textLength.SqrtCeil() - 1;
            int count = Random.Shared.Next(textLength);
            int max = n * n;
            ushort[] result = new ushort[count];
            for (int i = 0; i < count; i++)
                result[i] = (ushort)(Random.Shared.Next(1, max));
            return new Key(result);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            int n = textLength.SqrtCeil() - 1;
            return BigInteger.Pow(n * n, textLength);
        }

        /// <inheritdoc/>
        public string GetString() => string.Join(',', KeyValue);
    }
}
