using ClassicalCryptography.Interfaces;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ClassicalCryptography.Calculation.CustomRSAPK;

/// <summary>
/// 以指定的开始字符创建质数并计算RSA密钥
/// </summary>
public static class CustomRSA
{
    private static BigInteger StartsWith(Span<byte> startbytes, int size)
    {
        if (startbytes.Length > size / 16 - 5)//只是粗略的估计
            throw new ArgumentException("开始字节太长，请修改尺寸", nameof(startbytes));

        Span<byte> bytes = size / 16 <= StackLimit.MaxByteSize
            ? stackalloc byte[size / 16] : new byte[size / 16];
        startbytes.CopyTo(bytes);
        Random.Shared.NextBytes(bytes[(startbytes.Length + 1)..]);
        var num = new BigInteger(bytes, true, true);
        if (num.IsEven)
            num++;
        while (!num.IsPrime())
            num += 2;
        return num;
    }

    private static string ToBase64(this BigInteger bigInteger)
    {
        return Convert.ToBase64String(bigInteger.ToByteArray(true, true));
    }

    /// <summary>
    /// 获得密钥
    /// </summary>
    public static string FromStartText(string text, bool pem = false)
    {
        var bytes = Encoding.UTF8.GetBytes(text).AsSpan();
        var bytes1 = bytes[..(bytes.Length / 2)];
        var bytes2 = bytes[(bytes.Length / 2)..];
        var result = FromStartBytes(bytes1, bytes2);
        if (pem)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(result);
            return rsa.ExportRSAPrivateKeyPem();
        }
        return result;
    }

    /// <summary>
    /// 从xml格式密钥中获得可能的信息
    /// </summary>
    public static (byte[], byte[]) GetBytesFrom(string xmlKey)
    {
        var xml = new XmlDocument();
        xml.LoadXml(xmlKey);
        var rsa = xml.FirstChild;
        if (rsa is null || rsa.Name != "RSAKeyValue")
            throw new ArgumentException("不是RSAKeyValue", nameof(xmlKey));

        string? b64P = rsa.SelectSingleNode("P")?.InnerText;
        string? b64Q = rsa.SelectSingleNode("Q")?.InnerText;
        if (b64P is null || b64Q is null)
            throw new ArgumentException("无法读取因数", nameof(xmlKey));
        byte[] p = Convert.FromBase64String(b64P);
        byte[] q = Convert.FromBase64String(b64Q);

        int pi = Array.IndexOf(p, (byte)0);
        int qi = Array.IndexOf(q, (byte)0);

        return (p[..pi], q[..qi]);
    }

    /// <summary>
    /// 从密钥中获得可能的文本
    /// </summary>
    public static string GetTextFrom(string key, bool pem = false)
    {
        if (pem)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(key);
            key = rsa.ToXmlString(true);
        }
        var (p, q) = GetBytesFrom(key);
        var bytes = new byte[p.Length + q.Length];
        p.CopyTo(bytes, 0);
        Array.Copy(q, 0, bytes, p.Length, q.Length);
        return Encoding.UTF8.GetString(bytes);
    }


    /// <summary>
    /// 获得XML格式的密钥
    /// </summary>
    public static string FromStartBytes(Span<byte> bytes1, Span<byte> bytes2, int size = 1024)
    {
        var P = StartsWith(bytes1, size);
        var Q = StartsWith(bytes2, size);
        var Modulus = P * Q;
        var Exponent = 65537;
        var D = ModularInverse(Exponent, Modulus - (P + Q - 1));
        var DP = D % (P - 1);
        var DQ = D % (Q - 1);
        var InverseQ = ModularInverse(Q, P);

        var settings = new XmlWriterSettings
        {
            Indent = true,
            NewLineChars = Environment.NewLine,
            OmitXmlDeclaration = true
        };
        var xmlStr = new StringBuilder();
        using (var writer = XmlWriter.Create(xmlStr, settings))
        {
            writer.WriteStartElement("RSAKeyValue");

            writer.WriteStartElement(nameof(Modulus));
            writer.WriteString(Modulus.ToBase64());
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(Exponent));
            writer.WriteString("AQAB");
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(D));
            writer.WriteString(D.ToBase64());
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(P));
            writer.WriteString(P.ToBase64());
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(Q));
            writer.WriteString(Q.ToBase64());
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(DP));
            writer.WriteString(DP.ToBase64());
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(DQ));
            writer.WriteString(DQ.ToBase64());
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(InverseQ));
            writer.WriteString(InverseQ.ToBase64());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        return xmlStr.ToString();
    }

    private static BigInteger ModularInverse(BigInteger a, BigInteger n)
    {
        BigInteger q, t, m = n, x = 1, y = 0;
        if (n.IsOne)
            return 0;
        while (a > 1)
        {
            q = a / n;
            t = n;
            n = a % n;
            a = t;
            t = y;
            y = x - q * y;
            x = t;
        }
        if (x.Sign == -1)
            x += m;
        return x;
    }


}
