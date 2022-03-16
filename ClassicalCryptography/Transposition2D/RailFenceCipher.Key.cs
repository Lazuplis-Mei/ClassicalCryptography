using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System;
using System.Numerics;

namespace ClassicalCryptography.Transposition2D;

public partial class RailFenceCipher
{
    /// <summary>
    /// 栅栏密码的密钥
    /// </summary>
    public class Key : IKey<ushort[]>
    {
        /// <summary>
        /// 默认密钥
        /// </summary>
        public static readonly Key Default = new(new ushort[] { 2 }, 0);

        private readonly ushort[] keyValue;
        private readonly int textLength;

        private Key(ushort[] key, int textLength)
        {
            keyValue = key;
            this.textLength = textLength;
        }
        /// <summary>
        /// 密钥值
        /// </summary>
        public ushort[] KeyValue => keyValue;
        /// <summary>
        /// 密钥是否可逆(仅生成的1位密钥可逆)
        /// </summary>
        public bool CanInverse => textLength != 0;
        /// <summary>
        /// 逆向密钥
        /// </summary>
        public IKey<ushort[]>? InversedKey
        {
            get
            {
                if (!CanInverse)
                    throw new KeyCannotInverseException<ushort[]>(this);

                return new Key(new[] { (ushort)(textLength / keyValue[0]) }, textLength);
            }
        }

        /// <summary>
        /// 从文本格式创建密钥(该方式产生的密钥不可逆)
        /// </summary>
        /// <param name="strKey">文本类型的密钥</param>
        public static IKey<ushort[]> FromString(string strKey)
        {
            var nums = strKey.Split(',', (StringSplitOptions)3);
            ushort[] vals = new ushort[nums.Length];
            int max = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = ushort.Parse(nums[i]);
                max = Math.Max(max, vals[i]);
                vals[i]--;
            }
            if (vals.Length == 1)
            {
                vals[0]++;
                return new Key(vals, 0);
            }
            if (max != vals.Length)
                throw new ArgumentOutOfRangeException(nameof(strKey), "密钥值超过范围。");
            return new Key(vals, 0);
        }

        /// <summary>
        /// 产生随机密钥(将有50%的可能产生扩展密钥[不可逆])
        /// </summary>
        /// <param name="textLength">加密内容的长度(不足将会补齐)</param>
        public static IKey<ushort[]> GenerateKey(int textLength)
        {
            int width = Random.Shared.Next(2, textLength / 2 + 1);
            textLength += textLength % width;//补足密钥长度
            if (RandomHelper.TrueOrFalse)
                return new Key(new[] { (ushort)width }, textLength);

            ushort[] permutation = RandomHelper.RandomPermutation(width);
            return new Key(permutation, 0);//扩展密钥不可逆
        }

        /// <summary>
        /// 获得密钥的空间
        /// </summary>
        /// <param name="textLength">加密内容的长度</param>
        public static BigInteger GetKeySpace(int textLength)
        {
            return MathExtension.Factorial(textLength);
        }
        /// <summary>
        /// 字符串形式
        /// </summary>
        public override string ToString()
        {
            return string.Join(',', keyValue);
        }
        /// <summary>
        /// 字符串形式
        /// </summary>
        public string GetString() => ToString();
    }
}
