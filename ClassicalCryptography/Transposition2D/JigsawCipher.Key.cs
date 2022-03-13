using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Numerics;

namespace ClassicalCryptography.Transposition2D;

public partial class JigsawCipher
{
    /// <summary>
    /// 锯齿分割密码的密钥
    /// </summary>
    public class Key : IKey<ushort[]>
    {
        /// <summary>
        /// 整数的一个分拆
        /// </summary>
        public ushort[] KeyValue { get; }
        private Key(ushort[] keyValue) => KeyValue = keyValue;
        /// <summary>
        /// 不可逆密钥
        /// </summary>
        public bool CanInverse => false;
        /// <summary>
        /// 不存在逆向密钥
        /// </summary>
        public IKey<ushort[]>? InversedKey => null;
        /// <summary>
        /// 从文本格式创建密钥
        /// </summary>
        /// <param name="strKey">文本类型的密钥</param>
        public static IKey<ushort[]> FromString(string strKey)
        {
            var nums = strKey.Split(',', (StringSplitOptions)3);
            ushort[] vals = new ushort[nums.Length];
            for (int i = 0; i < vals.Length; i++)
                vals[i] = ushort.Parse(nums[i]);
            return new Key(vals);
        }
        /// <summary>
        /// 产生随机密钥
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static IKey<ushort[]> GenerateKey(int textLength)
        {
            int N = textLength.SqrtCeil();
            int n = Random.Shared.Next(N) + 1;
            ushort[] partition = new ushort[n];
            for (int i = 0; i < n - 1; i++)
            {
                partition[i] = (ushort)(Random.Shared.Next(N - (n - i - 1)) + 1);
                N -= partition[i];
            }
            partition[n - 1] = (ushort)N;
            return new Key(partition);
        }

        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength)
        {
            int N = textLength.SqrtCeil();
            var arr = new BigInteger[N];
            arr[0] = BigInteger.One;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = 1, ri = 0; ; j++)
                {
                    ri += 2 * j - 1;
                    if (i < ri)
                        break;
                    arr[i] += (j % 2 == 0) ? -arr[i - ri] : arr[i - ri];
                    ri += j;
                    if (i < ri)
                        break;
                    arr[i] += (j % 2 == 0) ? -arr[i - ri] : arr[i - ri];
                }
            }
            return arr[N - 1];
        }
        /// <summary>
        /// 字符串形式
        /// </summary>
        public string GetString() => ToString();
        /// <summary>
        /// 字符串形式
        /// </summary>
        public override string ToString()
        {
            return string.Join(',', KeyValue);
        }
    }
}