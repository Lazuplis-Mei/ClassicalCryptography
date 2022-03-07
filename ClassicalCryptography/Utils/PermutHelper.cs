using ClassicalCryptography.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptography.Utils
{
    /// <summary>
    /// 组合工具类
    /// </summary>
    public static class PermutHelper
    {

        /// <summary>
        /// 产生不大于<paramref name="maxValue"/>的随机数
        /// </summary>
        /// <param name="maxValue">最大值</param>
        public static BigInteger RandomBigInt(BigInteger maxValue)
        {
            if (maxValue < int.MaxValue)
                return Random.Shared.Next((int)maxValue);
            int byteCount = maxValue.GetByteCount(true);
            Span<byte> buffer = stackalloc byte[byteCount];
            byte firstBit = (byte)(maxValue >> ((byteCount - 1) << 3));
            buffer[0] = RandomHelper.RandomByte(firstBit);
            Random.Shared.NextBytes(buffer[1..]);
            return new BigInteger(buffer, true, true);
        }

        /// <summary>
        /// 计算x的阶乘(朴素方法)
        /// </summary>
        public static BigInteger Factorial(int x)
        {
            var result = BigInteger.One;
            for (int i = 2; i <= x; i++)
                result *= i;
            return result;
        }

        /// <summary>
        /// 获得<paramref name="count"/>个数的第n个排列
        /// </summary>
        /// <param name="n">第n个排列</param>
        /// <param name="count">排列的数的个数</param>
        public static ushort[] GetPermutation(BigInteger n, int count)
        {
            var arr = new ushort[count];
            var arr2 = new ushort[count + 1];
            arr2.FillOrder();
            var list = arr2.ToList();
            for (int i = 0; i < count; i++)
            {
                arr[i] = list[(int)BigInteger.DivRem(n, Factorial(count - i - 1), out n)];
                list.Remove(arr[i]);
            }
            return arr;
        }

        /// <summary>
        /// 填充span为第n个排列
        /// </summary>
        /// <param name="n">第n个排列</param>
        /// <param name="span">写入的内存</param>
        public static void FillPermutation(this Span<ushort> span, BigInteger n)
        {
            var arr = new ushort[span.Length + 1];
            arr.FillOrder();
            var list = arr.ToList();
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = list[(int)BigInteger.DivRem(n, Factorial(span.Length - i - 1), out n)];
                list.Remove(span[i]);
            }
        }

        /// <summary>
        /// 获得一个排列的序号
        /// </summary>
        public static BigInteger GetPermutIndex(ushort[] arr)
        {
            var result = BigInteger.Zero;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int t = arr.Length - i - 1;
                result += arr.TakeLast(t).Count(j => j < arr[i]) * Factorial(t);
            }
            return result;
        }

    }
}
