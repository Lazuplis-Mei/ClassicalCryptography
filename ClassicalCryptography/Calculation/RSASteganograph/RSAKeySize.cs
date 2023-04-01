namespace ClassicalCryptography.Calculation.RSASteganograph;

public partial class RSASteganograph
{
    /// <summary>
    /// RSA密钥长度
    /// </summary>
    public enum RSAKeySize
    {
        /// <summary>
        /// 1024位RSA
        /// </summary>
        RSA1024 = 64,

        /// <summary>
        /// 2048位RSA
        /// </summary>
        RSA2048 = 128,

        /// <summary>
        /// 3072位RSA
        /// </summary>
        RSA3072 = 192,

        /// <summary>
        /// 4096位RSA
        /// </summary>
        RSA4096 = 256,
    }
}
