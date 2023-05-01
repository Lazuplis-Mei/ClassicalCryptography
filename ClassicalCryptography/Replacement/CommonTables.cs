using SR = ClassicalCryptography.Interfaces.SingleReplacementCipher;

namespace ClassicalCryptography.Replacement;

/// <summary>
/// 常见的单表和方阵
/// </summary>
public static partial class CommonTables
{
    /// <summary>
    /// qwer键盘表
    /// </summary>
    public static readonly SR Keyboard = new(U_Letters, QWER_Keyboard, true);

    /// <summary>
    /// Atbash
    /// </summary>
    public static readonly SR Atbash = new(U_Letters, "ZYXWVUTSRQPONMLKJIHGFEDCBA", true);

    /// <summary>
    /// 汉语拼音
    /// </summary>
    public static readonly SR PinYin = new(U_Letters, "AOEIUVBPMFDTNLGKHJQXZCSRYW", true);

    /// <summary>
    /// Rot5
    /// </summary>
    public static readonly SR Rot5 = new(Digits, "5678901234");

    /// <summary>
    /// Rot13
    /// </summary>
    public static readonly SR Rot13 = new(U_Letters, "NOPQRSTUVWXYZABCDEFGHIJKLM", true);

    /// <summary>
    /// Rot18
    /// </summary>
    public static readonly SR Rot18 = new(U_Base36, "5678901234NOPQRSTUVWXYZABCDEFGHIJKLM");

    /// <summary>
    /// Rot47
    /// </summary>
    public static readonly SR Rot47 = new(PrintableAsciis,
        "PQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNO");

    /// <summary>
    /// 游戏《最终幻想X》中的密码语言Al Bhed
    /// </summary>
    public static readonly SR Al_Bhed = new(U_Letters + Hiragana,
        "YPLTAVKREZGMSHUBXNCDIJFQOWワミフネトアチルテヨラキヌヘホサヒユセソハシスメオマリクケロヤイツレコタヲモナニウエノカムン");

    /// <summary>
    /// Dvorak键盘布局
    /// </summary>
    public static readonly SR Dvorak = new(U_Letters, "PYFGCRLAOEUIDHTNSQJKXBMWVZ", true);

    /// <summary>
    /// Dvorak左手键盘布局
    /// </summary>
    public static readonly SR DvorakLeftHand = new(U_Letters, "PFMLJQBYURSOKCDTHEAZXGVWNI", true);

    /// <summary>
    /// Dvorak右手键盘布局
    /// </summary>
    public static readonly SR DvorakRightHand = new(U_Letters, "JLMFPQORSUYBZAEHTDCKXINWVG", true);

    /// <summary>
    /// Colemak键盘布局
    /// </summary>
    public static readonly SR Colemak = new(U_Letters, "QWFPGJLUYARSTDHNEIOZXCVBKM", true);

    /// <summary>
    /// LIIGOL键盘布局
    /// </summary>
    public static readonly SR LIIGOLLayout = new(U_Letters, "QWLFKJYUPASTNRDIEOHZXCVBGM", true);

    /// <summary>
    /// AZERTY键盘布局
    /// </summary>
    public static readonly SR AZERTY = new(U_Letters, "AZERTYUIOPQSDFGHJKLMWXCVBN", true);

    /// <summary>
    /// Asset键盘布局
    /// </summary>
    public static readonly SR Asset = new(U_Letters, "QWJFGYPULASETDHNIORZXCVBKM", true);

    /// <summary>
    /// Carpalx键盘布局
    /// </summary>
    public static readonly SR Carpalx = new(U_Letters, "QFMLWYUOBJDSTNRIAEHZVGCXPK", true);

    /// <summary>
    /// Minimak4Keys键盘布局
    /// </summary>
    public static readonly SR Minimak4Keys = new(U_Letters, "QWDRKYUIOPASTFGHJELZXCVBNM", true);

    /// <summary>
    /// Minimak8Keys键盘布局
    /// </summary>
    public static readonly SR Minimak8Keys = new(U_Letters, "QWDRKYUILPASTFGHNEOZXCVBJM", true);

    /// <summary>
    /// Minimak12Keys键盘布局
    /// </summary>
    public static readonly SR Minimak12Keys = new(U_Letters, "QWDFKYUILASTRGHNEOPZXCVBJM", true);

    /// <summary>
    /// Norman键盘布局
    /// </summary>
    public static readonly SR Norman = new(U_Letters, "QWDFKJURLASETGYNIOHZXCVBPM", true);

    /// <summary>
    /// Workman键盘布局
    /// </summary>
    public static readonly SR Workman = new(U_Letters, "QDRWBJFUPASHTGYNEOIZXMCVKL", true);

    /// <summary>
    /// 我曾用过的密码表，可以由<see cref="Transposition.TakeTranslateCipher"/>得到
    /// </summary>
    public static readonly SR MTOHOEM = new(U_Letters, "ANBYCODUEPFXGQHVIRJZKSLWMT", true);

    /// <summary>
    /// 星际火狐中的蜥蜴人语言(严格意义上专有名词应保持不变，且y应为0)
    /// </summary>
    public static readonly SR Saurian = new(U_Letters, "URSTOVWXAZBCMDEFGHJKILNPYQ", true);

    /// <summary>
    /// 《打工吧！魔王大人》中使用的语言
    /// </summary>
    public static readonly SR PartTimerDevil = new(U_Letters, "AZYXEWVTISRLPNOMQKJHUGFDCB", true);

    /// <summary>
    /// 易经八卦和Base64
    /// </summary>
    public static readonly SR IChingEightTrigramsBase64 = new(Base64,
        "䷁䷗䷆䷒䷎䷣䷭䷊䷏䷲䷧䷵䷽䷶䷟䷡䷇䷂䷜䷻䷦䷾䷯䷄䷬䷐䷮䷹䷞䷰䷛䷪䷖䷚䷃䷨䷳䷕䷑䷙䷢䷔䷿䷥䷷䷝䷱䷍䷓䷩䷺䷼䷴䷤䷸䷈䷋䷘䷅䷉䷠䷌䷫䷀☯");

    /// <summary>
    /// 火星文
    /// </summary>
    public static readonly SR MarsLanguage = new(Resources.MarsLanguagePlain, Resources.MarsLanguageText);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard = new(QWER_Keyboard, "1234567890!@#$%&*()'/-_:;?", true);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard2 = new(QWER_Keyboard, "1234567890_:\"…;%~()-!/.、?@", true);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard3 = new(QWER_Keyboard, "1234567890~!@#$%^&*()_+:“”", true);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard4 = new(QWER_Keyboard, "1234567890~!@#%'&*?()-_:;/", true);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard5 = new(QWER_Keyboard, "1234567890~@#$%&*()'/-_:;`", true);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard6 = new(QWER_Keyboard, "1234567890-/:;()_$&\"~,…@!'", true);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard7 = new(QWER_Keyboard, "1234567890~@#%+-_/.()?!:;`", true);

    /// <summary>
    /// 手机26键输入法的符号对应
    /// </summary>
    public static readonly SR PhoneKeyboard8 = new(QWER_Keyboard, "1234567890~@#$%&*(),/-_:;、", true);
}
