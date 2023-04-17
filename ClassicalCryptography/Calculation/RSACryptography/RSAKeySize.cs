namespace ClassicalCryptography.Calculation.RSACryptography;

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
        RSA1024 = 0x40,

        /// <summary>
        /// 2048位RSA
        /// </summary>
        RSA2048 = 0x80,

        /// <summary>
        /// 3072位RSA
        /// </summary>
        RSA3072 = 0xC0,

        /// <summary>
        /// 4096位RSA
        /// </summary>
        RSA4096 = 0x100,
    }
}
