namespace ClassicalCryptography.Transposition2D;

public partial class FifteenPuzzle
{
    /// <summary>
    /// 数字华容道密码的密钥
    /// </summary>
    public class Key : IKey<QuaterArray>
    {
        /// <inheritdoc/>
        public QuaterArray KeyValue { get; }

        /// <inheritdoc/>
        public bool CanInverse => false;

        /// <inheritdoc/>
        public IKey<QuaterArray>? InversedKey => null;

        /// <summary>
        /// 数字华容道密码的密钥
        /// </summary>
        public Key(QuaterArray keyValue) => KeyValue = keyValue;

        /// <inheritdoc/>
        public static IKey<QuaterArray> FromString(string strKey)
        {
            return new Key(QuaterArray.FromString(strKey));
        }

        /// <inheritdoc/>
        public static IKey<QuaterArray> GenerateKey(int textLength)
        {
            int N = textLength.SqrtCeil();
            int count = Random.Shared.Next(textLength);
            int x = N - 1;
            int y = x;
            var result = new QuaterArray(count);
            for (int i = 0; i < count; i++)
            {
                int move = RandomHelper.TwoBits;
                switch (move)
                {
                    case 0:
                        if (x + 1 == N)
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
                        if (y + 1 == N)
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
        public static BigInteger GetKeySpace(int textLength)
        {
            return BigInteger.Pow(4, textLength);
        }

        /// <inheritdoc/>
        public string GetString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => GetString().GetHashCode();
    }
}
