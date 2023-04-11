namespace ClassicalCryptography.Transposition2D;

public partial class SixteenPuzzle
{
    /// <summary>
    /// 移动数字华容道密码
    /// </summary>
    [Introduction("移动数字华容道密码的密钥", "正确的格式为移动的行数(正数)和列数(负数)")]
    public partial class Key : IKey<short[]>
    {
        /// <inheritdoc/>
        public short[] KeyValue { get; }

        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;

        /// <summary>
        /// 密钥不可逆，将始终为null
        /// </summary>
        public IKey<short[]>? InversedKey => null;

        private Key(short[] sbytes) => KeyValue = sbytes;

        /// <summary>
        /// 移动数字华容道密码
        /// </summary>
        public Key(string key)
        {
            var nums = key.Split(',', (StringSplitOptions)3);
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
            int n = textLength.SqrtCeil();
            int count = Random.Shared.Next(textLength);
            var result = new short[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = (short)(Random.Shared.Next(n) + 1);
                if (RandomHelper.TrueOrFalse)
                    result[i] = (short)-result[i];
            }
            return new Key(result);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            int n = textLength.SqrtCeil();
            return BigInteger.Pow(n << 1, textLength);
        }

        /// <inheritdoc/>
        public string GetString() => string.Join(',', KeyValue);
    }
}