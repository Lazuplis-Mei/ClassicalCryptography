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
            /*
            int n = (N >= 5 && RandomHelper.TrueOrFalse) ?
                N / 4 + Random.Shared.Next(N / 4) : 
                Random.Shared.Next(N) + 1;
            ushort[] partition = new ushort[n];
            for (int i = 0; i < n - 1; i++)
            {
                partition[i] = (ushort)(Random.Shared.Next(N - (n - i - 1)) + 1);
                N -= partition[i];
            }
            partition[n - 1] = (ushort)N;
            return new Key(partition);
            */
            var partition = new List<ushort>();
            while (N > 0)
            {
                ushort n = (ushort)Random.Shared.Next(1, N);
                partition.Add(n);
                N -= n;
            }
            return new Key(partition.ToArray());
        }

        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength)
        {
            int N = textLength.SqrtCeil();
            return BigInteger.One << (N - 1);
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