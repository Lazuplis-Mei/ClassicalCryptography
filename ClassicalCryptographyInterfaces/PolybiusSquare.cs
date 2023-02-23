using ClassicalCryptography.Interfaces;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 波利比奥斯方阵
/// </summary>
public class PolybiusSquare : IKey<(string Padding, string Alphabet)>
{
    private readonly (string Padding, string Alphabet) keyValue;

    /// <summary>
    /// 波利比奥斯方阵
    /// </summary>
    public (string Padding, string Alphabet) KeyValue => keyValue;

    /// <summary>
    /// 密钥不可逆
    /// </summary>
    public bool CanInverse => false;

    /// <summary>
    /// 密钥不可逆，将为null
    /// </summary>
    public IKey<(string Padding, string Alphabet)>? InversedKey => null;

    /// <summary>
    /// 波利比奥斯方阵
    /// </summary>
    public PolybiusSquare(string padding, string alphabet)
    {
        if (alphabet.Length != padding.Length * padding.Length)
            throw new ArgumentException("长度不匹配", nameof(alphabet));
        keyValue = (padding, alphabet);
    }

    /// <summary>
    /// 从文本格式创建密钥例如(ADFGX,BTALPDHOZKQFVSNGICUXMREWY)
    /// </summary>
    public static IKey<(string Padding, string Alphabet)> FromString(string strKey)
    {
        var keys = strKey.Split(new[] { ' ', ',' });
        if (keys.Length != 2)
            throw new ArgumentException("应为2组字符", nameof(strKey));
        return new PolybiusSquare(keys[0], keys[1]);
    }

    /// <summary>
    /// 产生随机密钥(5*5的方阵)
    /// </summary>
    public unsafe static IKey<(string Padding, string Alphabet)> GenerateKey(int textLength)
    {
        Span<char> alphabet = stackalloc char[25];
        var list = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        for (int i = 0; i < 25; i++)
        {
            int t = Random.Shared.Next(list.Count);
            alphabet[i] = list[t];
            list.RemoveAt(t);
        }
        list.Clear();
        Span<char> padding = stackalloc char[5];
        list.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        for (int i = 0; i < 5; i++)
        {
            int t = Random.Shared.Next(list.Count);
            padding[i] = list[t];
            list.RemoveAt(t);
        }

        return new PolybiusSquare(new(padding), new(alphabet));
    }

    /// <summary>
    /// 获得密钥的空间
    /// </summary>
    /// <param name="textLength">加密内容的长度</param>
    public static BigInteger GetKeySpace(int textLength)
    {
        //25*25!*C(26,5)
        return BigInteger.Parse("25508184916257806450688000000000");
    }

    /// <summary>
    /// 字符串形式
    /// </summary>
    public override string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append("  ");
        strBuilder.AppendJoin(' ', keyValue.Padding.ToCharArray());
        strBuilder.AppendLine();
        int n = keyValue.Padding.Length;
        for (int i = 0; i < n; i++)
        {
            strBuilder.Append(keyValue.Padding[i]);
            strBuilder.Append(' ');
            strBuilder.AppendJoin(' ', keyValue.Alphabet.Substring(n * i, n).ToCharArray());
            strBuilder.AppendLine();
        }
        strBuilder.Remove(strBuilder.Length - 2, 2);
        return strBuilder.ToString();
    }

    /// <summary>
    /// 字符串形式
    /// </summary>
    public string GetString() => ToString();

}
