using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System.Text;
using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Undefined;

/// <summary>
/// <para>弦艺术密码，使用22个针脚的欧拉字体(也可以更换为<see cref="nonEulerianFont"/>)</para>
/// <see href="https://en.wikipedia.org/wiki/String_art"/>
/// </summary>
[Introduction("弦艺术密码", "https://en.wikipedia.org/wiki/String_art")]
public class StringArtCipher
{
    //2和Z在事实上是相同的
    private static readonly string[] eulerianFont = new[]
    {
        "tucglmdbopth", "bdfhsomkitbopab", "bdfvomkqobvqb", "hboigcapnji",
        "tsihtabvfebolkqops", "sihtabvfebopt", "nikqobvfdbqvolk", 
        "isabopshlmdehti", "mdefvadopqklobm", "qolkmdekqsn", "tdfsmktabopt",
        "qklpabo", "vbilmdsfdeivabopv", "blabopamledm", "vboqkmdfvomfkdbqv",
        "thispobhfdbas",  "obfvbdfkmoqvdmskq", "thispobhfdbasmku",
        "uvbcdfuijkmnoqjtu", "bavfebomd", "obaqomdekmqko", "nuabndegn",
        "qohedmtkmlhqpobaq", "bvmkboqdfodmb", "dehtabomd", "gvaehrplkrsg",
        "bvqomkfdbfvdmqkob", "cugecnlg", "gvaehrklprsg", "histhfdbvfmoqkmdki",
        "hedmlhtubctsi", "ufeatjqomkiu", "oqabomkis", "fvaefnod",
        "vboqkmdfvdbfhsqmokitv", "dmlfdbvth"
    };

    //0和O在事实上是相同的
    private static readonly string[] nonEulerianFont = new[]
    {
        "tucglmkmdboqopth", "bdfhstikmpabo", "bvfdbomkqoqv", "pacghjnobahipo",
        "befvabopqklpthisv", "befvabopthisv", "kqvbdfvboniloq", "isabopthlmdeh",
        "defvadmpqklob", "nsqoledmlkq", "tdfrsmkuabopu", "bavbopqkmklpv",
        "dmlhbaiedtpobase", "bopamledmlba", "boqkmdfvbdfkmoqv", "spobasihtadfh",
        "mdfvboqksmkfdbvqom", "spobasihtukmsadfh", "nhtscdfvbcbvsihkmoqk",
        "vcavfecfedmob", "obaqomdekmkq", "vbavnfedfdnb", "mdeiophlmsaboptl",
        "bvmkboqdfoqdm", "degnuabmodmoob", "cavfechfisplkqpnsth",
        "vqomkfdbvfdmkqob", "cugecnlgenlc", "fvaehtsihtqklpq",
        "cbvfdchtsinoqkmnmkf", "hedmlhtubcstsi", "cavfecsvthkmoqkmnhis",
        "oqabomkisthi", "fvbvaefnod", "dmkitvbdfvboqkmoqshf", "delmdbvthist"
    };

    private static readonly BidirectionalDictionary<char, string> fontMap;

    static StringArtCipher()
    {
        fontMap = new();
        for (int i = 0; i < 36; i++)
            fontMap.Add(Base36_LD[i], eulerianFont[i]);
    }

    /// <summary>
    /// 使用欧拉字体加密
    /// </summary>
    public static string Encrypt(string text)
    {
        var result = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
            if (fontMap.TryGetValue(char.ToLower(text[i]), out string value))
                result.Append(value).Append('/');
        result.RemoveLast();
        return result.ToString();
    }

    /// <summary>
    /// 使用欧拉字体解密
    /// </summary>
    public static string Decrypt(string text)
    {
        var pairs = text.Split('/', StringSplitOptions.RemoveEmptyEntries);
        Span<char> span = pairs.Length <= StackLimit.MaxCharSize ?
            stackalloc char[pairs.Length] : new char[pairs.Length];
        for (int i = 0; i < pairs.Length; i++)
        {
            if (fontMap.Inverse.TryGetValue(pairs[i], out char value))
                span[i] = value;
            else
                span[i] = '?';
        }
        return new(span);
    }

}