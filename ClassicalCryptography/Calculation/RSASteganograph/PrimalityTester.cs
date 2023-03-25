using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Calculation.RSASteganograph;

/// <summary>
/// 使用<a href="https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test">
/// MillerRabin算法
/// </a>检测质数<br/>
/// </summary>
/// <remarks>
/// <a href="https://planetcalc.com/8995/">在线工具</a>
/// </remarks>
[ReferenceFrom("https://github.com/CharlesWilliams127/MillerRabinAlgorithm/blob/master/MillerRabinAlgorithm/Program.cs")]
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
        IS_PRIME,
    }

    /// <summary>
    /// 默认的重复次数，这将提供0.9999999999854481的准确率
    /// </summary>
    public const int TEST_REPEAT_COUNT = 18;

    /// <summary>
    /// 求大于<paramref name="number"/>的下一个质数
    /// </summary>
    public static BigInteger NextPrime(this BigInteger number)
    {
        if (number.IsEven)
            number++;
        const int PARALLEL_NOT_FOUND = -1;
        int n = PARALLEL_NOT_FOUND;
        Parallel.For(0, 10000, (i, loop) =>
        {
            if ((number + (i << 1)).IsPrime())
            {
                n = i << 1;
                loop.Break();
            }
        });
        if (n != PARALLEL_NOT_FOUND)
            return number + n;
        number += 20000;
        while (!number.IsPrime())
            number += 2;
        return number;
    }

    /// <summary>
    /// 并行查找大于<paramref name="number"/>的质数(不保证是第一个质数)
    /// </summary>
    public static BigInteger ParallelFindPrime(this BigInteger number)
    {
        if (number.IsEven)
            number--;
        const int PARALLEL_NOT_FOUND = -1;
        int n = PARALLEL_NOT_FOUND;
        Parallel.For(0, 10000, (i, loop) =>
        {
            if ((number + (i << 1)).IsPrime())
            {
                n = i << 1;
                loop.Stop();
            }
        });
        if (n != PARALLEL_NOT_FOUND)
            return number + n;
        number += 20000;
        while (!number.IsPrime())
            number += 2;
        return number;
    }

    /// <summary>
    /// 获得指定重复次数下进行质数检测的准确率
    /// </summary>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetProbability(int testRepeatCount)
    {
        return 1 - 1 / Math.Pow(4, testRepeatCount);
    }

    /// <summary>
    /// 检测数值是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPrime(this BigInteger number)
    {
        return IsPrime(number, TEST_REPEAT_COUNT) switch
        {
            TestResult.NOT_PRIME => false,
            TestResult.TRUSTED_PRIME => true,
            TestResult.IS_PRIME => true,
            _ => false,
        };
    }

    /// <summary>
    /// 检测数值是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    public static TestResult IsPrime(BigInteger number, int testRepeatCount)
    {
        Guard.IsTrue(BigInteger.IsPositive(number));

        if (number < 100_0000UL)
            return IsPrime((ulong)number) ? TestResult.IS_PRIME : TestResult.NOT_PRIME;

        if (number.IsEven)
            return TestResult.NOT_PRIME;

        if (number % 03 == 0 || number % 05 == 0 || number % 07 == 0 ||
            number % 11 == 0 || number % 13 == 0 || number % 17 == 0 ||
            number % 19 == 0 || number % 23 == 0 || number % 29 == 0 ||
            number % 31 == 0 || number % 37 == 0 || number % 41 == 0 ||
            number % 43 == 0 || number % 47 == 0 || number % 53 == 0 ||
            number % 59 == 0 || number % 61 == 0 || number % 67 == 0 ||
            number % 71 == 0 || number % 73 == 0 || number % 79 == 0 ||
            number % 83 == 0 || number % 89 == 0 || number % 97 == 0)
            return TestResult.NOT_PRIME;

        return MillerRabin(number, testRepeatCount);
    }

    /// <summary>
    /// 检测是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    public static bool IsPrime(ulong number)
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
    /// MillerRabin算法的概率性质数测试
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    public static TestResult MillerRabin(BigInteger number, int testRepeatCount)
    {
        var m = number - 1;

        if (!BigInteger.ModPow(210, m, number).IsOne)
            return TestResult.NOT_PRIME;

        BigInteger q = m;
        int k = 0;
        while (q.IsEven)
        {
            k++;
            q >>= 1;
        }

        var testResult = TestResult.TRUSTED_PRIME;
        for (int i = 0; i < testRepeatCount && testResult != TestResult.NOT_PRIME; i++)
        {
            //如果有误，请更换为RandomBigInteger
            var x = RandomHelper.RandomBigInt(m);
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
