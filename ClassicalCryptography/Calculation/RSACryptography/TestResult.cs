namespace ClassicalCryptography.Calculation.RSACryptography;

public static partial class MillerRabinPrimalityTester
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
}
