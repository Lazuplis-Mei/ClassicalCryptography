using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using ClassicalCryptography.Utils;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace ClassicalCryptography.Transposition2D;


public partial class RotatingGrillesCipher
{
    /// <summary>
    /// 旋转栅格密码的密钥
    /// </summary>
    public class Key : IKey<QuaterArray>
    {
        /// <summary>
        /// 密钥值
        /// </summary>
        public QuaterArray KeyValue { get; }
        private Key(QuaterArray keyValue) => KeyValue = keyValue;
        /// <summary>
        /// 密钥不可逆
        /// </summary>
        public bool CanInverse => false;
        /// <summary>
        /// 不存在逆向密钥
        /// </summary>
        public IKey<QuaterArray>? InversedKey => null;


        /// <summary>
        /// 从文本格式创建密钥
        /// </summary>
        /// <param name="strKey">文本类型的密钥</param>
        public static IKey<QuaterArray> FromString(string strKey)
        {
            return new Key(QuaterArray.FromString(strKey));
        }
        /// <summary>
        /// 产生随机密钥
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static IKey<QuaterArray> GenerateKey(int textLength)
        {
            double N = Math.Ceiling(Math.Sqrt(textLength / 4.0));
            return new Key(RandomHelper.RandomQuaterArray((int)(N * N)));
        }

        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength)
        {
            int N = textLength.DivCeil(4);
            return BigInteger.Pow(4, N);
        }

        /// <summary>
        /// 栅格的字符串形式，H代表对应的格子
        /// </summary>
        public override string ToString()
        {
            int N = (int)Math.Sqrt(KeyValue.Count);
            int length = N << 1;
            var strBuilder = new StringBuilder(length * (length + 2));
            Span<int> rot = stackalloc int[4];
            Span<int> pos = stackalloc int[KeyValue.Count];
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    rot[0] = x; rot[3] = y;
                    rot[1] = length - y - 1;
                    rot[2] = length - x - 1;
                    int j = x + (N * y);
                    pos[j] = rot[KeyValue[j] % 4] + rot[(KeyValue[j] + 3) % 4] * length;
                }
            }
            pos.Sort();
            for (int y, i = y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    int p = x + y * length;
                    if (i < pos.Length && pos[i] == x + y * length)
                    {
                        i++;
                        strBuilder.Append('H');
                        continue;
                    }
                    strBuilder.Append('.');
                }
                strBuilder.AppendLine();
            }
            return strBuilder.ToString();
        }
        /// <summary>
        /// 密钥的字符串形式
        /// </summary>
        public string GetString() => KeyValue.ToString();
    }
}
