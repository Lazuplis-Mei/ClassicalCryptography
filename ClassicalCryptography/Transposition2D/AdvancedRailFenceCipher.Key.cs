using CommunityToolkit.HighPerformance;

namespace ClassicalCryptography.Transposition2D;

public partial class AdvancedRailFenceCipher
{
    /// <summary>
    /// 扩展栅栏密码的密钥
    /// </summary>
    [Introduction("扩展栅栏密码的密钥", """形式为一组合法的排列""")]
    public class Key : IKey<ushort[]>
    {
        private readonly ushort[] keyValue;
        private readonly int textLength;

        private Key(ushort[] key, int length)
        {
            keyValue = key;
            textLength = length;
        }

        /// <summary>
        /// 排列对(如果排列对只有一个元素，那么它将代表每组字数)
        /// </summary>
        public ushort[] KeyValue => keyValue;

        /// <summary>
        /// 逆向密钥仅在每组字数情况下存在
        /// </summary>
        public bool CanInverse => textLength != 0;

        /// <summary>
        /// 逆向密钥(如果存在)
        /// </summary>
        public IKey<ushort[]>? InversedKey
        {
            get
            {
                if (!CanInverse)
                    return null;

                return new Key(new[] { (ushort)(textLength / keyValue[0]) }, textLength);
            }
        }

        /// <inheritdoc/>
        public static IKey<ushort[]> FromString(string strKey)
        {
            var nums = strKey.Split(',', (StringSplitOptions)3);
            ushort[] permut = new ushort[nums.Length];
            int max = 0;
            var hashset = new HashSet<ushort>();
            for (int i = 0; i < permut.Length; i++)
            {
                if (ushort.TryParse(nums[i], out ushort value))
                {
                    Guard.IsGreaterThan(value, (ushort)0);
                    if (hashset.Add(value))
                    {
                        max = Math.Max(max, value);
                        permut[i] = (ushort)(value - 1);
                    }
                }
            }
            if (permut.Length == 1)
            {
                permut[0]++;
                return new Key(permut, 0);
            }
            Guard.IsEqualTo(max, permut.Length);
            return new Key(permut, 0);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <remarks>
        /// 密钥排列长度的范围为[2,n/2]<br/>
        /// 将有25%的可能不产生扩展密钥
        /// </remarks>
        /// <param name="textLength">长度将被自动补足</param>
        public static IKey<ushort[]> GenerateKey(int textLength)
        {
            int width = Random.Shared.Next(2, (textLength >> 1) + 1);
            textLength += textLength % width;
            if (RandomHelper.TwoBits == 0)
                return new Key(new[] { (ushort)width }, textLength);

            ushort[] permutation = RandomHelper.RandomPermutation(width);
            return new Key(permutation, 0);
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            return MathExtension.Factorial(textLength);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Join(',', keyValue);
        }

        /// <inheritdoc/>
        public string GetString() => ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => keyValue.GetDjb2HashCode();
    }
}
