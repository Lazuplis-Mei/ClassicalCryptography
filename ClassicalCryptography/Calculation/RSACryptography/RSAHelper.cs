using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml;
using static ClassicalCryptography.Calculation.RSACryptography.RSASteganograph;
using static ClassicalCryptography.Utils.MathExtension;

namespace ClassicalCryptography.Calculation.RSACryptography;

/// <summary>
/// 生成RSA密钥的方法
/// </summary>
public static class RSAHelper
{

    /// <summary>
    /// 默认的RSA指数
    /// </summary>
    public const int RSA_EXPONENT = 65537;

    /// <summary>
    /// 默认的RSA指数字符串形式
    /// </summary>
    public const string RSA_EXPONENT_STRING = "AQAB";

    private static readonly RSACryptoServiceProvider RSA_CSP = new();

    /// <summary>
    /// 生成可被破解的弱RSA私钥
    /// </summary>
    /// <param name="keySize">密钥长度</param>
    /// <returns>xml格式的RSA私钥</returns>
    [SkipLocalsInit]
    public static string GenerateWeakRSAPrivateKey(RSAKeySize keySize)
    {
        GuardEx.IsDefined(keySize);
        int size = (int)keySize;
        Span<byte> buffer = stackalloc byte[size];
        Random.Shared.NextBytes(buffer);
        var p = new BigInteger(buffer, true, true);
        Random.Shared.NextBytes(buffer[^(size >> 2)..]);
        var q = new BigInteger(buffer, true, true);
        return GenerateRSAPrivateKey(p.FindPrime(), q.FindPrime());
    }

    /// <summary>
    /// 使用指定的质数生成RSA私钥
    /// </summary>
    /// <param name="P">质数P</param>
    /// <param name="Q">质数Q</param>
    /// <returns>xml格式的RSA私钥</returns>
    public static string GenerateRSAPrivateKey(BigInteger P, BigInteger Q)
    {
        var Modulus = P * Q;
        var Exponent = RSA_EXPONENT;
        var D = ModularInverse(Exponent, Modulus - (P + Q - 1));
        var DP = D % (P - 1);
        var DQ = D % (Q - 1);
        var InverseQ = ModularInverse(Q, P);

        var xml = new StringBuilder();
        using (var writer = XmlWriter.Create(xml, new() { Indent = true, OmitXmlDeclaration = true }))
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
        return xml.ToString();
    }

    /// <summary>
    /// pem格式的转换为xml格式
    /// </summary>
    /// <param name="pemKey">pem格式的密钥</param>
    /// <returns>xml格式的密钥</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string PemToXml(this string pemKey)
    {
        RSA_CSP.ImportFromPem(pemKey);
        return RSA_CSP.ToXmlString(true);
    }

    /// <summary>
    /// xml格式的转换为pem格式
    /// </summary>
    /// <param name="xmlKey">xml格式的密钥</param>
    /// <param name="PKCS8Format">是否使用PKCS8格式</param>
    /// <returns>pem格式的密钥</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string XmlToPem(this string xmlKey, bool PKCS8Format = false)
    {
        RSA_CSP.FromXmlString(xmlKey);
        return PKCS8Format ? RSA_CSP.ExportPkcs8PrivateKeyPem() : RSA_CSP.ExportRSAPrivateKeyPem();
    }
}