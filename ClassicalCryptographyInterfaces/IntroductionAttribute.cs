namespace ClassicalCryptography.Interfaces;

/// <summary>
/// 描述性信息
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class IntroductionAttribute : Attribute
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 简介
    /// </summary>
    public string Introduction { get; }

    /// <summary>
    /// 描述性信息
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="intro">简介</param>
    public IntroductionAttribute(string name, string intro)
    {
        Name = name;
        Introduction = intro;
    }
}
