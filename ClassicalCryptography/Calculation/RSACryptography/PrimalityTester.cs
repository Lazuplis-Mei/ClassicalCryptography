namespace ClassicalCryptography.Calculation.RSACryptography;

/// <summary>
/// 使用MillerRabin算法的质数检测工具
/// </summary>
/// <remarks>
/// <list type="bullet">
///     <item>
///         <term>参考资料</term>
///         <description>
///             <see href="https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test">wikipedia/Miller–Rabin_primality_test</see>
///         </description>
///     </item>
///     <item>
///         <term>在线工具</term>
///         <description>
///             <see href="https://planetcalc.com/8995/">Miller–Rabin primality test</see>
///         </description>
///     </item>
/// </list>
/// </remarks>
[ReferenceFrom("https://github.com/IOL0ol1/PrimalityTest/blob/0faadb9d39fb1dcb1c5218654a601ccf56730773/PrimalityTest/Utils/BigIntegerEx.cs")]
public static partial class MillerRabinPrimalityTester
{
    #region 常量

    /// <summary>
    /// 默认的重复次数，这将提供 <c>99.99999999854481%</c> 的准确率
    /// </summary>
    public const int TEST_REPEAT_COUNT = 18;

    /// <summary>
    /// 并行查找质数的范围
    /// </summary>
    private const int SEARCH_END = 10000;

    /// <summary>
    /// 在并行查找质数的范围中是否找到质数
    /// </summary>
    private const int NOT_FOUND = -1;

    /// <summary>
    /// 使用简单方法检测质数的阈值
    /// </summary>
    private const ulong SMALL_PRIME_THRESHOLD = 100_0000UL;

    #endregion

    private static readonly int[] smallPrimes =
    {
        02, 03, 05, 07, 11,
        13, 17, 19, 23, 29,
        31, 37, 41, 43, 47,
        53, 59, 61, 67, 71,
        73, 79, 83, 89, 97,
    };

    /// <summary>
    /// 并行查找大于等于<paramref name="number"/>的下一个质数
    /// </summary>
    public static BigInteger NextPrime(this BigInteger number)
    {
        if (number < 2)
            return 2;

        if (number.IsEven) number++;

        int n = NOT_FOUND;
        Parallel.For(0, SEARCH_END, (i, loop) =>
        {
            int di = 2 * i;
            if ((number + di).IsPrime())
            {
                n = di;
                loop.Break();
            }
        });

        if (n is not NOT_FOUND)
            return number + n;

        number += 2 * SEARCH_END;
        while (!number.IsPrime())
            number += 2;

        return number;
    }

    /// <summary>
    /// 并行查找大于<paramref name="number"/>的某个质数
    /// </summary>
    /// <remarks>
    /// 相比较<see cref="NextPrime"/><br/>
    /// 此方法的唯一区别在于使用<see cref="ParallelLoopState.Stop"/><br/>
    /// 而不是<see cref="ParallelLoopState.Break"/><br/>
    /// 因此它在相同情况下可能会更快地得到结果，尽管结果可能会不同
    /// </remarks>
    public static BigInteger FindPrime(this BigInteger number)
    {
        if (number < 2)
            return 2;

        if (number.IsEven) number++;

        int n = NOT_FOUND;
        Parallel.For(0, SEARCH_END, (i, loop) =>
        {
            int di = 2 * i;
            if ((number + di).IsPrime())
            {
                n = di;
                loop.Stop();
            }
        });

        if (n is not NOT_FOUND)
            return number + n;

        number += 2 * SEARCH_END;
        while (!number.IsPrime())
            number += 2;

        return number;
    }

    /// <summary>
    /// 获得指定重复次数下进行质数检测的准确率
    /// </summary>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetAccuracy(int testRepeatCount)
    {
        return 1 - 1 / Math.Pow(4, testRepeatCount);
    }

    /// <summary>
    /// 检测数值是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <returns>
    /// <see langword="true"/> 如果<paramref name="number"/>是(或<strong>非常可能</strong>是)质数<br/>
    /// <see langword="false"/> 如果<paramref name="number"/>不是质数
    /// </returns>
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
    /// <returns>
    /// <see cref="TestResult.NOT_PRIME"/> 如果<paramref name="number"/>不是质数<br/>
    /// <see cref="TestResult.TRUSTED_PRIME"/> 如果<paramref name="number"/><strong>非常可能</strong>是质数<br/>
    /// <see cref="TestResult.IS_PRIME"/> 如果<paramref name="number"/>是质数
    /// </returns>
    public static TestResult IsPrime(BigInteger number, int testRepeatCount)
    {
        GuardEx.IsPositive(number);

        if (number < SMALL_PRIME_THRESHOLD)
            return IsPrime((ulong)number) ? TestResult.IS_PRIME : TestResult.NOT_PRIME;

        if (number.IsEven)
            return TestResult.NOT_PRIME;

        if (smallPrimes.Any(p => (number % p).IsZero))
            return TestResult.NOT_PRIME;

        return MillerRabin(number, testRepeatCount);
    }

    /// <summary>
    /// 检测是否为质数
    /// </summary>
    /// <param name="number">待检测的数</param>
    /// <returns>
    /// <see langword="true"/> 如果<paramref name="number"/>是(或<strong>非常可能</strong>是)质数<br/>
    /// <see langword="false"/> 如果<paramref name="number"/>不是质数
    /// </returns>
    public static bool IsPrime(ulong number)
    {
        if (number < 3 || ulong.IsEvenInteger(number))
            return number is 2;

        ulong quotient, remainder, divisor;
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
    /// <remarks>
    /// 其中重复过程是并行计算的
    /// </remarks>
    /// <param name="number">待检测的数</param>
    /// <param name="testRepeatCount">测试过程的重复次数</param>
    /// <returns>
    /// <see cref="TestResult.NOT_PRIME"/> 如果<paramref name="number"/>不是质数<br/>
    /// <see cref="TestResult.TRUSTED_PRIME"/> 如果<paramref name="number"/><strong>非常可能</strong>是质数<br/>
    /// <see cref="TestResult.IS_PRIME"/> 如果<paramref name="number"/>是质数
    /// </returns>
    private static TestResult MillerRabin(BigInteger number, int testRepeatCount)
    {
        var m = number - 1;

        if (!BigInteger.ModPow(210, m, number).IsOne)
            return TestResult.NOT_PRIME;

        BigInteger q = m;
        int k;
        for (k = 0; q.IsEven; k++)
            q >>>= 1;

        var testResult = TestResult.TRUSTED_PRIME;
        Parallel.For(0, testRepeatCount, (_, loop) =>
        {
            if (testResult is TestResult.NOT_PRIME)
                loop.Stop();
            var x = RandomHelper.RandomBigInteger(m);
            testResult = MillerRabinInternal(number, x, q, k);
        });
        return testResult;
    }

    private static TestResult MillerRabinInternal(BigInteger n, BigInteger x, BigInteger q, int k)
    {
        BigInteger y = BigInteger.ModPow(x, q, n), m = n - 1;

        if (y.IsOne || y == m)
            return TestResult.TRUSTED_PRIME;

        for (int i = 1; i < k; i++)
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
