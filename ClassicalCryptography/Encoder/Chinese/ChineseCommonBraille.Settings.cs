namespace ClassicalCryptography.Encoder.Chinese;

public partial class ChineseCommonBraille
{
    /// <summary>
    /// 国家通用盲文方案的选项
    /// </summary>
    /// <param name="CheckPinyin">指示编码时会检查提供的拼音是否正确<br/>
    /// <see cref="EncodeTonenote"/>须为<see langword="true"/></param>
    /// <param name="AutoSimplify">指示编码时将繁体字对应为简体字</param>
    /// <param name="EncodeTonenote">指示应当编码声调(轻声声调5将被忽略)</param>
    /// <param name="EncodePunctuation">指示应当编码如下的标点符号<br/>
    /// <c>。，、；？！：“”‘’（）【】—…《》〈〉</c></param>
    /// <param name="EncodeNumber">指示应当编码数字</param>
    /// <param name="EncodeLetters">指示应当编码英文字母</param>
    /// <param name="DistinguishThird">指示应当区分`他它她`(来源于规则9.4)<br/>
    /// <see cref="EncodeTonenote"/>须为<see langword="true"/></param>
    /// <param name="UseAbbreviations">使用针对声调的简写(来源于规则10.2.1~10.2.6)<br/>
    /// <see cref="EncodeTonenote"/>须为<see langword="true"/></param>
    /// <param name="OutputPinyins">指示是否在编码过程中记录拼音作为输出参数<br/>
    /// 如果该项被设置为<see langword="false"/>，则<see cref="Encode(string, out string?)"/>的参数2不起作用</param>
    public record struct Settings(
        bool CheckPinyin,
        bool AutoSimplify,
        bool EncodeTonenote,
        bool EncodePunctuation,
        bool EncodeNumber,
        bool EncodeLetters,
        bool DistinguishThird,
        bool UseAbbreviations,
        bool OutputPinyins)
    {
        /// <summary>
        /// 国家通用盲文方案的选项
        /// </summary>
        public static Settings FromCode(int code)
        {
            Span<bool> bools = stackalloc bool[9];
            BitsHelper.ToBitArray(code, bools);
            return new(bools[0], bools[1], bools[2], bools[3], bools[4], bools[5], bools[6], bools[7], bools[8]);
        }

        /// <summary>
        /// 是否可以解码拼音
        /// </summary>
        ///<remarks>
        /// <see cref="DistinguishThird"/><br/>
        /// <see cref="UseAbbreviations"/><br/>
        /// 需为<see langword="false"/>，否则解码盲文会产生非预期行为<br/>
        /// </remarks>
        public bool CanDecodePinyin => !DistinguishThird && !UseAbbreviations;

        /// <summary>
        /// 是否可以解析拼音
        /// </summary>
        /// <remarks>
        /// <see cref="CanDecodePinyin"/><br/>
        /// <see cref="EncodeTonenote"/><br/>
        /// 需为<see langword="true"/>，否则解析拼音会产生非预期行为<br/>
        /// </remarks>
        public bool CanResolvePinyin => CanDecodePinyin && EncodeTonenote;

        /// <inheritdoc/>
        [SkipLocalsInit]
        public override int GetHashCode()
        {
            Span<bool> bools = stackalloc bool[9];
            bools[0] = CheckPinyin;
            bools[1] = AutoSimplify;
            bools[2] = EncodeTonenote;
            bools[3] = EncodePunctuation;
            bools[4] = EncodeNumber;
            bools[5] = EncodeLetters;
            bools[6] = DistinguishThird;
            bools[7] = UseAbbreviations;
            bools[8] = OutputPinyins;
            return BitsHelper.ToInt32(bools);
        }
    }
}
