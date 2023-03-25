using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml;
using static ClassicalCryptography.Utils.MathExtension;

namespace ClassicalCryptography.Calculation.RSASteganograph;

/// <summary>
/// 以指定的前缀字节生成质数并计算RSA私钥
/// </summary>
[Introduction("RSA隐写术", "以指定的前缀字节生成质数并计算RSA私钥")]
public class RSASteganograph : IStaticCipher<string, string>
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

    #region 常数

    /// <summary>
    /// 短密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_SHORT = 1024;

    /// <summary>
    /// 中等密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_MEDIUM = RSA_KEYSIZE_SHORT * 2;

    /// <summary>
    /// 长密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_LONG = RSA_KEYSIZE_SHORT * 3;

    /// <summary>
    /// 最长密钥长度
    /// </summary>
    public const int RSA_KEYSIZE_VERYLONG = RSA_KEYSIZE_SHORT * 4;

    /// <summary>
    /// 用于生成质数时预留的字节长度(只是粗略的估计)
    /// </summary>
    public const int REMAIN_BYTESCOUNT = 5;

    /// <summary>
    /// 标记前缀字节的结尾
    /// </summary>
    public const byte PREFIX_END_FLAG = 0;

    /// <summary>
    /// 默认的RSA指数
    /// </summary>
    public const int RSA_EXPONENT = 65537;

    /// <summary>
    /// 默认的RSA指数字符串形式
    /// </summary>
    public const string RSA_EXPONENT_STRING = "AQAB";

    #endregion

    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    static CipherType IStaticCipher<string, string>.Type => CipherType.Calculation;

    private static readonly RSACryptoServiceProvider RSA_CSP = new();

    /// <summary>
    /// 以指定前缀生成指定RSA密钥长度的质数
    /// </summary>
    /// <param name="prefix">前缀字节</param>
    /// <param name="keySize">密钥长度</param>
    [SkipLocalsInit]
    public static BigInteger GeneratePrime(Span<byte> prefix, RSAKeySize keySize)
    {
        Guard.IsTrue(Enum.IsDefined(keySize));
        int bytesCount = (int)keySize;
        Guard.HasSizeLessThanOrEqualTo(prefix, bytesCount - REMAIN_BYTESCOUNT);
        Guard.IsFalse(prefix.Contains(PREFIX_END_FLAG));

        Span<byte> bigIntegerBytes = bytesCount.CanAllocate()
            ? stackalloc byte[bytesCount]
            : new byte[bytesCount];

        prefix.CopyTo(bigIntegerBytes);
        int prefixRegionCount = prefix.Length + 1;
        bigIntegerBytes[prefix.Length] = PREFIX_END_FLAG;
        Random.Shared.NextBytes(bigIntegerBytes[prefixRegionCount..]);
        //为了避免生成质数时，数值的增长覆盖了前缀字节结尾的标记，扩大了寻找质数的范围
        bigIntegerBytes[prefixRegionCount] >>= 1;

        return new BigInteger(bigIntegerBytes, true, true).ParallelFindPrime();
    }

    /// <summary>
    /// xml格式的转换为pem格式
    /// </summary>
    /// <param name="xmlKey">xml格式的密钥</param>
    /// <param name="PKCS8Format">是否使用PKCS8格式</param>
    /// <returns>pem格式的密钥</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string XmlToPem(string xmlKey, bool PKCS8Format = false)
    {
        RSA_CSP.FromXmlString(xmlKey);
        return PKCS8Format ? RSA_CSP.ExportPkcs8PrivateKeyPem() : RSA_CSP.ExportRSAPrivateKeyPem();
    }

    /// <summary>
    /// pem格式的转换为xml格式
    /// </summary>
    /// <param name="pemKey">pem格式的密钥</param>
    /// <returns>xml格式的密钥</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string PemToXml(string pemKey)
    {
        RSA_CSP.ImportFromPem(pemKey);
        return RSA_CSP.ToXmlString(true);
    }

    /// <summary>
    /// 使用指定的质数生成RSA私钥
    /// </summary>
    /// <param name="P">质数P</param>
    /// <param name="Q">质数Q</param>
    /// <returns>xml格式的RSA私钥</returns>
    public static string CreateRSAPrivateKey(BigInteger P, BigInteger Q)
    {
        var Modulus = P * Q;
        var Exponent = RSA_EXPONENT;
        var D = ModularInverse(Exponent, Modulus - (P + Q - 1));
        var DP = D % (P - 1);
        var DQ = D % (Q - 1);
        var InverseQ = ModularInverse(Q, P);

        var xmlKey = new StringBuilder();
        using (var writer = XmlWriter.Create(xmlKey, new()
        {
            Indent = true,
            OmitXmlDeclaration = true
        }))
        {
            writer.WriteStartElement("RSAKeyValue");

            writer.WriteElement(nameof(Modulus), Modulus);
            writer.WriteElement(nameof(Exponent), RSA_EXPONENT_STRING);
            writer.WriteElement(nameof(D), D);
            writer.WriteElement(nameof(P), P);
            writer.WriteElement(nameof(Q), Q);
            writer.WriteElement(nameof(DP), DP);
            writer.WriteElement(nameof(DQ), DQ);
            writer.WriteElement(nameof(InverseQ), InverseQ);

            writer.WriteEndElement();
        }
        return xmlKey.ToString();
    }

    /// <summary>
    /// 以指定的前缀字节生成RSA私钥
    /// </summary>
    /// <param name="prefixP">用于生成质数P的前缀字节</param>
    /// <param name="prefixQ">用于生成质数Q的前缀字节</param>
    /// <returns>xml格式的RSA私钥</returns>
    public static string GenerateRSAPrivateKey(Span<byte> prefixP, Span<byte> prefixQ)
    {
        int prefixRegionCount = Math.Max(prefixP.Length, prefixQ.Length) + REMAIN_BYTESCOUNT;
        var keySize = GetKeySize(prefixRegionCount);

        var P = GeneratePrime(prefixP, keySize);
        var Q = GeneratePrime(prefixQ, keySize);
        return CreateRSAPrivateKey(P, Q);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static RSAKeySize GetKeySize(int prefixRegionCount)
        {
            return (prefixRegionCount << 4) switch
            {
                <= RSA_KEYSIZE_SHORT => RSAKeySize.RSA1024,
                <= RSA_KEYSIZE_MEDIUM => RSAKeySize.RSA2048,
                <= RSA_KEYSIZE_LONG => RSAKeySize.RSA3072,
                <= RSA_KEYSIZE_VERYLONG => RSAKeySize.RSA4096,
                _ => throw new ArgumentException("前缀字节的长度过长", nameof(prefixRegionCount)),
            };
        }
    }

    /// <summary>
    /// 以指定的文本作为前缀生成RSA私钥
    /// </summary>
    /// <remarks>
    /// <paramref name="text"/>使用<see cref="Encoding"/>转换成字节。<br/>
    /// 再对半分为两部分，分别作为质数P和Q的前缀字节。
    /// </remarks>
    /// <param name="text">作为前缀的文本</param>
    /// <param name="pemFormat">是否以pem格式导出</param>
    /// <returns>xml或pem格式的密钥</returns>
    public static string GenerateRSAPrivateKey(string text, bool pemFormat = false)
    {
        var prefix = Encoding.GetBytes(text).AsSpan();
        int halfLength = prefix.Length >> 1;
        var xmlKey = GenerateRSAPrivateKey(prefix[..halfLength], prefix[halfLength..]);
        return pemFormat ? XmlToPem(xmlKey) : xmlKey;
    }

    /// <summary>
    /// 从密钥中获得前缀字节
    /// </summary>
    /// <remarks>
    /// 值得注意的是，如果前缀字节中包含0，则获取的内容会提前截断。
    /// </remarks>
    /// <param name="privateKey">RSA私钥</param>
    /// <returns>质数P和Q中的前缀字节</returns>
    public static (ArraySegment<byte>, ArraySegment<byte>) GetPrifix(string privateKey)
    {
        Guard.HasSizeGreaterThan(privateKey, 0);
        var xmlDocument = new XmlDocument();
        if (privateKey[0] == '<')
            xmlDocument.LoadXml(privateKey);
        else if (privateKey[0] == '-')
            xmlDocument.LoadXml(PemToXml(privateKey));
        else
        {
            try
            {
                xmlDocument.LoadXml(privateKey);
            }
            catch (XmlException)
            {
                xmlDocument.LoadXml(PemToXml(privateKey));
            }
        }

        var keyValueNode = xmlDocument.FirstChild;
        Guard.IsNotNull(keyValueNode);
        Guard.IsEqualTo(keyValueNode.Name, "RSAKeyValue");

        string? base64P = keyValueNode.SelectSingleNode("P")?.InnerText;
        string? base64Q = keyValueNode.SelectSingleNode("Q")?.InnerText;
        Guard.IsNotNull(base64P);
        Guard.IsNotNull(base64Q);

        byte[] pBytes = Convert.FromBase64String(base64P);
        byte[] qBytes = Convert.FromBase64String(base64Q);

        int prifixEndofP = Array.IndexOf(pBytes, PREFIX_END_FLAG);
        if (prifixEndofP == -1)
            prifixEndofP = pBytes.Length - REMAIN_BYTESCOUNT;
        int prifixEndofQ = Array.IndexOf(qBytes, PREFIX_END_FLAG);
        if (prifixEndofQ == -1)
            prifixEndofQ = qBytes.Length - REMAIN_BYTESCOUNT;

        return (pBytes.SubArray(0, prifixEndofP), qBytes.SubArray(0, prifixEndofQ));
    }

    /// <summary>
    /// 从密钥中获得前缀文本
    /// </summary>
    /// <remarks>
    /// 从<paramref name="privateKey"/>中获取前缀字节并组合。<br/>
    /// 再使用<see cref="Encoding"/>转换成文本。
    /// </remarks>
    /// <param name="privateKey">RSA私钥</param>
    /// <returns>P和Q中前缀字节组合而成的文本</returns>
    [SkipLocalsInit]
    public static string GetTextFrom(string privateKey)
    {
        var (prifixP, prifixQ) = GetPrifix(privateKey);
        int length = prifixP.Count + prifixQ.Count;
        Span<byte> prifix = length.CanAllocate() ? stackalloc byte[length] : new byte[length];
        prifixP.CopyTo(prifix);
        prifixQ.CopyTo(prifix[prifixP.Count..]);
        return Encoding.GetString(prifix);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static string IStaticCipher<string, string>.Encrypt(string plainText) => GenerateRSAPrivateKey(plainText);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static string IStaticCipher<string, string>.Decrypt(string cipherText) => GetTextFrom(cipherText);
}
