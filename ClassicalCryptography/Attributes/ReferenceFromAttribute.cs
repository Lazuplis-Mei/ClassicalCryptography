namespace ClassicalCryptography.Attributes;

/// <summary>
/// 指示代码是参考其他代码或从其他语言的代码翻译而来，可能不符合当前项目代码的编写规范
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class ReferenceFromAttribute : Attribute
{
    /// <summary>
    /// 指示代码参考了其他来源
    /// </summary>
    /// <param name="sourceLink">参考代码的来源链接</param>
    /// <param name="sourceLanguage">参考代码的编程语言</param>
    /// <param name="sourceLicense">参考代码的开源协议</param>
    public ReferenceFromAttribute(string sourceLink, ProgramingLanguage sourceLanguage = ProgramingLanguage.CSharp, License sourceLicense = License.None)
    {
        SourceLink = sourceLink;
        SourceLanguage = sourceLanguage;
        SourceLicense = sourceLicense;
    }

    /// <summary>
    /// 参考代码的来源链接
    /// </summary>
    public string SourceLink { get; }

    /// <summary>
    /// 参考代码的编程语言
    /// </summary>
    public ProgramingLanguage SourceLanguage { get; }
    /// <summary>
    /// 参考代码的开源协议
    /// </summary>
    public License SourceLicense { get; }
}
