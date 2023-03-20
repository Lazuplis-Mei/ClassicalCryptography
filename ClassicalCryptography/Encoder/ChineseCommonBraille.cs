using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;

namespace ClassicalCryptography.Encoder;

/// <summary>
/// <a href="http://www.moe.gov.cn/jyb_sjzl/ziliao/A19/201807/W020180725666187054299.pdf">国家通用盲文方案</a>
/// </summary>
public static class ChineseCommonBraille
{
    /// <summary>
    /// 编码国家通用的盲文
    /// </summary>
    /// <param name="text">中文文字</param>
    /// <param name="autoSimplify">繁体字自动转简体字</param>
    public static string Encode(string text, bool autoSimplify = true)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 解码国家通用的盲文
    /// </summary>
    /// <param name="text">盲文文本</param>
    /// <returns>可能的拼音文本</returns>
    public static string Decode(string text)
    {
        throw new NotImplementedException();
    }
}
