namespace ClassicalCryptography.Utils;

/// <summary>
/// 指定单词转换成排列的选项
/// </summary>
public enum WordToPermutationOptions
{
    /// <summary>
    /// 默认情况，重复的将被忽略
    /// </summary>
    IgnoreDuplicated,

    /// <summary>
    /// 允许重复的字母顺延
    /// </summary>
    AllowDuplicated,

    /// <summary>
    /// 允许重复字母拥有相同编号(结果不再是一个排列)
    /// </summary>
    Myszkowski,
}
