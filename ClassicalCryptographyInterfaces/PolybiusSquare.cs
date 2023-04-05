using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

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
    public PolybiusSquare(string padding, string alphabet)
    {
        if (alphabet.Length != padding.Length * padding.Length)
            throw new ArgumentException($"长度不匹配，alphabet长度应为padding长度的平方", nameof(alphabet));
        keyValue = (padding, alphabet);
    }

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
    /// 从文本格式创建密钥，例如(ADFGX,BTALPDHOZKQFVSNGICUXMREWY)
    /// </summary>
    public static IKey<(string Padding, string Alphabet)> FromString(string strKey)
    {
        char[] separator = { ' ', ',', ':' };
        var keys = strKey.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if (keys.Length != 2)
            throw new ArgumentException("应为2组字符", nameof(strKey));
        return new PolybiusSquare(keys[0], keys[1]);
    }

    /// <summary>
    /// 产生随机5*5的密钥方阵(<see href="Padding"/>也为大写字母)
    /// </summary>
    /// <param name="textLength">参数将被忽略</param>
    [SkipLocalsInit]
    public static IKey<(string Padding, string Alphabet)> GenerateKey(int textLength)
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
        (string padding, string alphabet) = keyValue;
        var result = new StringBuilder(80);//可能的大小
        result.Append("  ").AppendJoin(' ', (IEnumerable<char>)padding).AppendLine();
        int length = padding.Length;
        for (int i = 0; i < length; i++)
        {
            result.Append(padding[i]).Append(' ');
            result.AppendJoin(' ', (IEnumerable<char>)alphabet.Substring(length * i, length));
            result.AppendLine();
        }
        result.Remove(result.Length - 2, 2);
        return result.ToString();
    }

    /// <summary>
    /// 字符串形式
    /// </summary>
    public string GetString() => ToString();
}
