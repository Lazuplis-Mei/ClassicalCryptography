namespace ClassicalCryptography.Transposition2D;

public partial class TwiddlePuzzle
{
    /// <summary>
    /// 旋转阵列密码的密钥
    /// </summary>
    public class Key : IKey<ushort[]>
    {
        /// <inheritdoc/>
        public ushort[] KeyValue { get; }

        /// <inheritdoc/>
        public bool CanInverse => false;

        /// <inheritdoc/>
        public IKey<ushort[]>? InversedKey => null;

        /// <summary>
        /// 旋转阵列密码的密钥
        /// </summary>
        private Key(ushort[] keyValue) => KeyValue = keyValue;

        /// <inheritdoc/>
        public static IKey<ushort[]> FromString(string strKey)
        {
            var nums = strKey.Split(',');
            ushort[] array = new ushort[nums.Length];
            for (int i = 0; i < array.Length; i++)
                array[i] = ushort.Parse(nums[i]);
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
