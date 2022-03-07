using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;

namespace ClassicalCryptography.Utils
{
    /// <summary>
    /// 随机数
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 随机的字节值
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static byte RandomByte(byte maxValue)
        {
            return (byte)Random.Shared.Next(maxValue);
        }
        /// <summary>
        /// 随机的真值
        /// </summary>
        public static bool TrueOrFalse => Random.Shared.NextDouble() <= 0.5;
        /// <summary>
        /// 随机的4进制数
        /// </summary>
        public static byte TwoBits => (byte)(Random.Shared.Next() & 0B11);

        /// <summary>
        /// 获得随机的排列
        /// </summary>
        /// <param name="count">排列长度</param>
        public static ushort[] RandomPermutation(int count)
        {
            var permutation = new ushort[count];
            permutation.FillOrder();
            var list = permutation.ToList();
            for (int i = 0; i < count; i++)
            {
                int t = Random.Shared.Next(list.Count);
                permutation[i] = list[t];
                list.RemoveAt(t);
            }
            return permutation;
        }

        /// <summary>
        /// 获得随机的排列
        /// </summary>
        /// <param name="count">排列长度</param>
        public static QuaterArray RandomQuaterArray(int count)
        {
            var array = new QuaterArray(count);
            for (int i = 0; i < count; i++)
                array[i] = TwoBits;
            return array;
        }
    }
}
