using System.Collections;

namespace ClassicalCryptography.Transposition2D;

public partial class CycleTranspose
{
    /// <summary>
    /// 周期/列置换密码的密钥
    /// </summary>
    [Introduction("周期/列置换密码的密钥", """正确的格式为形如"(1,2,4)(3,5)"的字符串""")]
    public partial class Key : IKey<ushort[][]>
    {
        private readonly ushort[][] keyValue;

        private Key(ushort[][] key) => keyValue = key;

        /// <summary>
        /// 置换对数组
        /// </summary>
        public ushort[][] KeyValue => keyValue;

        /// <summary>
        /// 密钥可逆
        /// </summary>
        public bool CanInverse => true;

        /// <summary>
        /// 逆向密钥
        /// </summary>
        public IKey<ushort[][]>? InversedKey
        {
            get
            {
                var inverseKey = new ushort[keyValue.Length][];
                for (int i = 0; i < inverseKey.Length; i++)
                {
                    ushort[] cycle = keyValue[i];
                    ushort[] temp = new ushort[cycle.Length];
                    for (int j = 0; j < temp.Length;)
                        temp[j++] = cycle[^j];
                    inverseKey[i] = temp;
                }
                return new Key(inverseKey);
            }
        }

        /// <inheritdoc/>
        public static IKey<ushort[][]> FromString(string strKey)
        {
            var matches = CycleKeyRegex().Matches(strKey);
            Guard.IsGreaterThan(matches.Count, 0);
            ushort[][] keyValue = new ushort[matches.Count][];
            var hashset = new HashSet<ushort>();
            for (int i = 0; i < keyValue.Length; i++)
            {
                var match = matches[i];
                var nums = match.Value.Split(',', (StringSplitOptions)3);
                keyValue[i] = new ushort[nums.Length];
                for (int j = 0; j < nums.Length; j++)
                {
                    ushort value = ushort.Parse(nums[j]);
                    Guard.IsTrue(hashset.Add(value));
                    keyValue[i][j] = value;
                }
            }
            return new Key(keyValue);
        }

        /// <inheritdoc/>
        public static IKey<ushort[][]> GenerateKey(int textLength)
        {
            ushort[] permutation = RandomHelper.RandomPermutation(textLength);
            var used = new BitArray(textLength);
            var cycleList = new List<ushort[]>();
            var cycle = new List<ushort>();
            for (int i = 0; i < textLength; i++)
            {
                if (used[i])
                    continue;
                ushort j = (ushort)i;
                while (!used[j])
                {
                    used[j] = true;
                    j = (ushort)Array.IndexOf(permutation, j);
                    cycle.Add((ushort)(j + 1));
                }
                if (cycle.Count > 1)
                    cycleList.Add(cycle.ToArray());
                cycle.Clear();
            }
            if (cycleList.Count == 0)
                cycleList.Add(new ushort[] { 1 });
            return new Key(cycleList.ToArray());
        }

        /// <inheritdoc/>
        public static BigInteger GetKeySpace(int textLength)
        {
            return MathExtension.Factorial(textLength);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            foreach (ushort[] value in keyValue)
                strBuilder.Append('(').AppendJoin(',', value).Append(')');
            return strBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GetString() => ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => keyValue.GetHashCode();

        [GeneratedRegex(@"(?<=\()\s*\d+\s*(\,\s*\d+\s*)*(?=\))")]
        private static partial Regex CycleKeyRegex();
    }
}
