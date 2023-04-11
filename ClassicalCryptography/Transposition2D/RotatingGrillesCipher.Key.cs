using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Transposition2D;

public partial class RotatingGrillesCipher
{
    /// <summary>
    /// 旋转栅格密码的密钥
    /// </summary>
    [Introduction("旋转栅格密码的密钥", """4进制数组的字符串形式""")]
    public class Key : IKey<QuaterArray>
    {
        /// <summary>
        /// 旋转栅格密码的密钥
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
            return new Key(QuaterArray.FromString(strKey));
        }

        /// <inheritdoc/>
        public static IKey<QuaterArray> GenerateKey(int textLength)
        {
            int n = textLength.DivCeil(4).SqrtCeil();
            return new Key(RandomHelper.RandomQuaterArray(n * n));
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            return BigInteger.Pow(4, textLength.DivCeil(4));
        }

        /// <summary>
        /// 栅格的字符串形式
        /// </summary>
        [SkipLocalsInit]
        public override string ToString()
        {
            int n = (int)Math.Sqrt(KeyValue.Count);
            int length = n << 1;
            int size = length * (length + 1);
            int index = 0;
            Span<char> span = size.CanAllocString() ? stackalloc char[size] : new char[size];
            Span<int> rot = stackalloc int[4];
            Span<int> pos = stackalloc int[KeyValue.Count];
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    rot[0] = x;
                    rot[3] = y;
                    rot[1] = length - y - 1;
                    rot[2] = length - x - 1;
                    int j = x + (n * y);
                    pos[j] = rot[KeyValue[j] & 0B11] + rot[(KeyValue[j] + 3) & 0B11] * length;
                }
            }
            pos.Sort();
            for (int i = 0, y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    if (i < pos.Length && pos[i] == x + y * length)
                    {
                        i++;
                        span[index++] = '█';
                        continue;
                    }
                    span[index++] = '.';
                }
                span[index++] = '\n';
            }
            return new string(span);
        }

        /// <inheritdoc/>
        public string GetString() => KeyValue.ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => KeyValue.GetHashCode();
    }
}
