using ClassicalCryptography.Utils;
using System.Numerics;

namespace ClassicalCryptography.Calculation.CustomRSAPrivateKey;

/// <summary>
/// <para>使用MillerRabin算法检测质数</para>
/// <seealso href="https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test"/>
/// <para>参考的代码片段</para>
/// <seealso href="https://github.com/CharlesWilliams127/MillerRabinAlgorithm/blob/master/MillerRabinAlgorithm/Program.cs"/>
/// <para>在线的工具</para>
/// <seealso href="https://planetcalc.com/8995/"/>
/// </summary>
public static class MillerRabinPrimalityTester
{
    /// <summary>
    /// 检测结果
    /// </summary>
    public enum TestResult
    {
        /// <summary>
        /// 不是质数
        /// </summary>
        NOT_PRIME,
        /// <summary>
        /// 通过检测的质数
        /// </summary>
        TRUSTED_PRIME,
        /// <summary>
        /// 一定是质数
        /// </summary>
        SURELY_PRIME,
    }

    /// <summary>
    /// 默认的重复次数，这将提供0.9999999999854481的准确率
    /// </summary>
    public const int TEST_REPEAT_COUNT = 18;

    /// <summary>
    /// 求指定数的下一个质数
    /// </summary>
    public static BigInteger NextPrime(this BigInteger number)
    {
        if (number.IsEven)
            number--;
        while (!number.IsPrime())
            number += 2;
        return number;
    }

    /// <summary>
    /// 获得指定重复次数下进行质数检测的准确率
    /// </summary>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    /// <returns>质数检测的准确率</returns>
    public static double GetProbability(int testRepeatCount = TEST_REPEAT_COUNT)
    {
        return 1 - (1 / Math.Pow(4, testRepeatCount));
    }

    /// <summary>
    /// 检测数值是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <returns>检测结果</returns>
    public static bool IsPrime(this BigInteger number)
    {
        return IsPrime(number, TEST_REPEAT_COUNT) switch
        {
            TestResult.NOT_PRIME => false,
            TestResult.TRUSTED_PRIME => true,
            /* 原则上，这只是代表它很有可能是质数
             * 你也可以计算在特定重试次数下的准确率
             */
            TestResult.SURELY_PRIME => true,
            _ => false,
        };
    }

    /// <summary>
    /// 检测数值是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    /// <returns>检测结果</returns>
    public static TestResult IsPrime(BigInteger number, int testRepeatCount)
    {
        if (number.Sign == -1)
            throw new ArgumentException("无法检测负数", nameof(number));

        if (number < 1000000UL)
            return IsPrimeUInt64((ulong)number) ? TestResult.SURELY_PRIME : TestResult.NOT_PRIME;

        if (number.IsEven)
            return TestResult.NOT_PRIME;

        if (number % 03 == 0 || number % 05 == 0 || number % 07 == 0 || 
            number % 11 == 0 || number % 13 == 0 || number % 17 == 0 || 
            number % 19 == 0 || number % 23 == 0 || number % 29 == 0 || 
            number % 31 == 0 || number % 37 == 0 || number % 41 == 0 || 
            number % 43 == 0 || number % 47 == 0 || number % 53 == 0)
            return TestResult.NOT_PRIME;

        return MillerRabin(number, testRepeatCount);
    }

    /// <summary>
    /// 检测属于[0, 1000000)范围内的数是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <returns>检测结果</returns>
    public static bool IsPrimeUInt64(ulong number)
    {
        ulong quotient, remainder, divisor;

        if (number < 3 || (number & 1) == 0)
            return number == 2;

        for (divisor = 3, remainder = 1; remainder != 0; divisor += 2)
        {
            (quotient, remainder) = ulong.DivRem(number, divisor);
            if (quotient < divisor)
                return true;
        }
        return false;
    }

    /// <summary>
    /// miller-rabin算法的概率性质数测试
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    /// <returns>检测结果</returns>
    internal static TestResult MillerRabin(BigInteger number, int testRepeatCount)
    {
        var m = number - 1;

        // 进行费马检验
        if (!BigInteger.ModPow(210, m, number).IsOne)
            return TestResult.NOT_PRIME;

        //寻找奇数 q 使得 m = 2^k * q
        BigInteger q = m;
        int k = 0;
        while (q.IsEven)
        {
            k++;
            q >>= 1;
        }

        var testResult = TestResult.TRUSTED_PRIME;
        for (int i = 0; i < testRepeatCount && testResult > 0; i++)
        {
            var x = RandomHelper.RandomBigInteger(1, m);
            testResult = MillerRabinInternal(number, x, q, k);
        }
        return testResult;
    }

    private static TestResult MillerRabinInternal(BigInteger n, BigInteger x, BigInteger q, int k)
    {
        BigInteger m = n - 1;
        var y = BigInteger.ModPow(x, q, n);

        if (y.IsOne || y == m)
            return TestResult.TRUSTED_PRIME;

        for (var i = 1; i < k; i++)
        {
            y = BigInteger.ModPow(y, 2, n);
            if (y == m)
                return TestResult.TRUSTED_PRIME;
            if (y.IsOne)
                return TestResult.NOT_PRIME;
        }

        return TestResult.NOT_PRIME;
    }

}
