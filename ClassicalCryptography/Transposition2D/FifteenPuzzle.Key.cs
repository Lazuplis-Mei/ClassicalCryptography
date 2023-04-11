namespace ClassicalCryptography.Transposition2D;

public partial class FifteenPuzzle
{
    /// <summary>
    /// 数字华容道密码的密钥
    /// </summary>
    [Introduction("数字华容道密码的密钥", "移动步骤0,1,2,3代表→↑←↓")]
    public partial class Key : IKey<QuaterArray>
    {
        /// <summary>
        /// 数字华容道密码的密钥
        /// </summary>
        public Key(QuaterArray keyValue) => KeyValue = keyValue;

        /// <inheritdoc/>
        public QuaterArray KeyValue { get; }

        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;

        /// <summary>
        /// 密钥不可逆，将始终为null
        /// </summary>
        public IKey<QuaterArray>? InversedKey => null;

        /// <inheritdoc/>
        public static IKey<QuaterArray> FromString(string strKey)
        {
            if (MoveStepRegex().IsMatch(strKey))
            {
                var array = new QuaterArray(strKey.Length);
                for (int i = 0; i < strKey.Length; i++)
                {
                    array[i] = strKey[i] switch
                    {
                        '→' => 0,
                        '↑' => 1,
                        '←' => 2,
                        '↓' => 3,
                        _ => -1
                    };
                }
                return new Key(array);
            }
            return new Key(QuaterArray.FromString(strKey));
        }

        /// <inheritdoc/>
        public static IKey<QuaterArray> GenerateKey(int textLength)
        {
            int n = textLength.SqrtCeil();
            int count = Random.Shared.Next(textLength);
            int x = n - 1;
            int y = x;
            var result = new QuaterArray(count);
            for (int i = 0; i < count; i++)
            {
                int move = RandomHelper.TwoBits;
                switch (move)
                {
                    case 0:
                        if (x + 1 == n)
                        {
                            move = 2;
                            goto case 2;
                        }
                        x++;
                        break;
                    case 1:
                        if (y == 0)
                        {
                            move = 3;
                            goto case 3;
                        }
                        y--;
                        break;
                    case 2:
                        if (x == 0)
                        {
                            move = 0;
                            goto case 0;
                        }
                        x--;
                        break;
                    case 3:
                        if (y + 1 == n)
                        {
                            move = 1;
                            goto case 1;
                        }
                        y++;
                        break;
                }
                result[i] = move;
            }
            return new Key(result);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength) => BigInteger.Pow(4, textLength);

        /// <inheritdoc/>
        public string GetString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => GetString().GetHashCode();

        [GeneratedRegex("^[→↑←↓]+$")]
        private static partial Regex MoveStepRegex();
    }
}
