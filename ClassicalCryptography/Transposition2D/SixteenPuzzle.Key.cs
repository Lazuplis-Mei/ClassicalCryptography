namespace ClassicalCryptography.Transposition2D;

public partial class SixteenPuzzle
{
    /// <summary>
    /// 移动数字华容道密码
    /// </summary>
    public partial class Key : IKey<short[]>
    {
        /// <inheritdoc/>
        public short[] KeyValue { get; }

        /// <inheritdoc/>
        public bool CanInverse => false;

        /// <inheritdoc/>
        public IKey<short[]>? InversedKey => null;

        private Key(short[] sbytes) => KeyValue = sbytes;

        /// <summary>
        /// 移动数字华容道密码
        /// </summary>
        public Key(string key)
        {
            var nums = key.Split(',');
            var array = new short[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                if (short.TryParse(nums[i], out short value))
                    array[i] = value;
            }
            KeyValue = array;
        }

        /// <inheritdoc/>
        public static IKey<short[]> FromString(string strKey) => new Key(strKey);

        /// <inheritdoc/>
        public static IKey<short[]> GenerateKey(int textLength)
        {
            int N = textLength.SqrtCeil();
            int count = Random.Shared.Next(textLength);
            var result = new short[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = (short)(Random.Shared.Next(N) + 1);
                if (RandomHelper.TrueOrFalse)
                    result[i] = (short)-result[i];
            }
            return new Key(result);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            int N = textLength.SqrtCeil();
            return BigInteger.Pow(N << 1, textLength);
        }

        /// <inheritdoc/>
        public string GetString() => string.Join(',', KeyValue);
    }
}