using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Collections;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;


namespace ClassicalCryptography.Transposition2D;


public partial class CycleTranspose
{
    /// <summary>
    /// 周期/列置换密码的密钥
    /// </summary>
    [Introduction("周期/列置换密码的密钥", @"正确的格式为多个正则匹配项/(\d+)/")]
    public partial class Key : IKey<ushort[][]>
    {
        private readonly ushort[][] keyValue;
        /// <summary>
        /// 置换对数组
        /// </summary>
        public ushort[][] KeyValue => keyValue;
        /// <summary>
        /// 可逆密钥
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
        private Key(ushort[][] key) => keyValue = key;
        /// <summary>
        /// 字符串形式
        /// </summary>
        public override string ToString()
        {
            if (keyValue.Length == 1 && keyValue[0].Length == 1)
                return keyValue[0][0].ToString();
            var strBuilder = new StringBuilder();
            for (int i = 0; i < keyValue.Length; i++)
            {
                strBuilder.Append('(');
                strBuilder.AppendJoin(',', keyValue[i]);
                strBuilder.Append(')');
            }
            return strBuilder.ToString();
        }
        /// <summary>
        /// 字符串形式
        /// </summary>
        public string GetString() => ToString();

        [GeneratedRegex("(?<=\\()[\\d\\,]+(?=\\))")]
        private static partial Regex CycleKeyRegex();

        /// <summary>
        /// 从文本格式创建密钥
        /// </summary>
        /// <param name="strKey">文本类型的密钥</param>
        public static IKey<ushort[][]> FromString(string strKey)
        {
            if (ushort.TryParse(strKey, out ushort k))
                return new Key(new[] { new[] { k } });

            var matches = CycleKeyRegex().Matches(strKey);
            ushort[][] keyValue = new ushort[matches.Count][];
            for (int i = 0; i < keyValue.Length; i++)
            {
                var match = matches[i];
                var nums = match.Value.Split(',', (StringSplitOptions)3);
                keyValue[i] = new ushort[nums.Length];
                for (int j = 0; j < nums.Length; j++)
                    keyValue[i][j] = ushort.Parse(nums[j]);
            }
            return new Key(keyValue);
        }
        /// <summary>
        /// 产生随机密钥
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static IKey<ushort[][]> GenerateKey(int textLength)
        {
            /*
            var permutIndex = PermutHelper.RandomBigInt(GetKeySpace(textLength));
            Span<ushort> permutation = stackalloc ushort[textLength];
            permutation.FillPermutation(permutIndex);
            */
            ushort[] permutation = RandomHelper.RandomPermutation(textLength);

            var used = new BitArray(textLength);
            var cycleList = new List<ushort[]>();
            for (int i = 0; i < textLength; i++)
            {
                if (used[i])
                    continue;
                var cycle = new List<ushort>();
                ushort j = (ushort)i;
                while (!used[j])
                {
                    used[j] = true;
                    j = (ushort)Array.IndexOf(permutation, j);
                    cycle.Add((ushort)(j + 1));
                }
                if (cycle.Count > 1)
                    cycleList.Add(cycle.ToArray());
            }
            if (cycleList.Count == 0)
                cycleList.Add(new ushort[] { 1 });
            return new Key(cycleList.ToArray());
        }
        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength)
        {
            return MathExtension.Factorial(textLength);
        }
    }
}
