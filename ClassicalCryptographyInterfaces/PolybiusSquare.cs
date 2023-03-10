using ClassicalCryptography.Interfaces;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
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
            throw new ArgumentException($"长度不匹配，{nameof(alphabet)}长度应为{nameof(padding)}长度的平方", nameof(alphabet));
        keyValue = (padding, alphabet);
    }

    /// <summary>
    /// 从文本格式创建密钥，例如(ADFGX,BTALPDHOZKQFVSNGICUXMREWY)
    /// </summary>
    public static IKey<(string Padding, string Alphabet)> FromString(string strKey)
    {
        var keys = strKey.Split(new[] { ' ', ',' });
        if (keys.Length != 2)
            throw new ArgumentException("应为2组字符", nameof(strKey));
        return new PolybiusSquare(keys[0], keys[1]);
    }

    /// <summary>
    /// 产生随机5*5的密钥方阵(<see href="Padding"/>也为大写字母)
    /// </summary>
    /// <param name="textLength">参数将被忽略</param>
    [SkipLocalsInit]
    public unsafe static IKey<(string Padding, string Alphabet)> GenerateKey(int textLength)
    {
        const string ULetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        Span<char> alphabet = stackalloc char[25];
        var list = new List<char>(ULetters);
        for (int i = 0; i < alphabet.Length; i++)
        {
            int index = Random.Shared.Next(list.Count);
            alphabet[i] = list[index];
            list.RemoveAt(index);
        }
        list.Clear();

        Span<char> padding = stackalloc char[5];
        list.AddRange(ULetters);
        for (int i = 0; i < padding.Length; i++)
        {
            int index = Random.Shared.Next(list.Count);
            padding[i] = list[index];
            list.RemoveAt(index);
        }

        return new PolybiusSquare(new(padding), new(alphabet));
    }

    /// <summary>
    /// 获得密钥的空间
    /// </summary>
    /// <param name="textLength">参数将被忽略</param>
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
        var strBuilder = new StringBuilder(78);//可能的大小
        strBuilder.Append("  ");
        strBuilder.AppendJoin(' ', (IEnumerable<char>)keyValue.Padding);
        strBuilder.AppendLine();
        int paddingLen = keyValue.Padding.Length;
        for (int i = 0; i < paddingLen; i++)
        {
            strBuilder.Append(keyValue.Padding[i]);
            strBuilder.Append(' ');
            strBuilder.AppendJoin(' ', (IEnumerable<char>)keyValue.Alphabet.Substring(paddingLen * i, paddingLen));
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
