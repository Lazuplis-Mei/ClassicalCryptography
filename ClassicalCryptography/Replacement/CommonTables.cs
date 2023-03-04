using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using static ClassicalCryptography.Utils.GlobalTables;

namespace ClassicalCryptography.Replacement
{
    /// <summary>
    /// 常见的单表和方阵
    /// </summary>
    public static partial class CommonTables
    {
        /// <summary>
        /// qwer键盘表
        /// </summary>
        public static readonly SingleReplacementCipher Keyboard =
            new(UL_Letters, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm");

        /// <summary>
        /// Atbash
        /// </summary>
        public static readonly SingleReplacementCipher Atbash =
            new(UL_Letters, "ZYXWVUTSRQPONMLKJIHGFEDCBAzyxwvutsrqponmlkjihgfedcba");

        /// <summary>
        /// 汉语拼音
        /// </summary>
        public static readonly SingleReplacementCipher PinYin =
            new(UL_Letters, "AOEIUVBPMFDTNLGKHJQXZCSRYWaoeiuvbpmfdtnlgkhjqxzcsryw");

        /// <summary>
        /// Rot5
        /// </summary>
        public static readonly SingleReplacementCipher Rot5 =
            new(Digits, "5678901234");

        /// <summary>
        /// Rot13
        /// </summary>
        public static readonly SingleReplacementCipher Rot13 =
            new(UL_Letters, "NOPQRSTUVWXYZABCDEFGHIJKLMnopqrstuvwxyzabcdefghijklm");

        /// <summary>
        /// Rot18
        /// </summary>
        public static readonly SingleReplacementCipher Rot18 =
            new(UL_Letter_Digits, "NOPQRSTUVWXYZABCDEFGHIJKLMnopqrstuvwxyzabcdefghijklm5678901234");

        /// <summary>
        /// Rot47
        /// </summary>
        public static readonly SingleReplacementCipher Rot47 =
            new(PrintableAsciis, "PQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNO");

        /// <summary>
        /// 游戏《最终幻想X》中的密码语言Al Bhed
        /// </summary>
        public static readonly SingleReplacementCipher Al_Bhed =
            new(UL_Letters + Hiragana, "YPLTAVKREZGMSHUBXNCDIJFQOWypltavkrezgmshubxncdijfqowワミフネトアチルテヨラキヌヘホサヒユセソハシスメオマリクケロヤイツレコタヲモナニウエノカムン");

        /// <summary>
        /// Dvorak键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher Dvorak =
            new(UL_Letters, "PYFGCRLAOEUIDHTNSQJKXBMWVZpyfgcrlaoeuidhtnsqjkxbmwvz");

        /// <summary>
        /// Dvorak左手键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher DvorakLeftHand =
            new(UL_Letters, "PFMLJQBYURSOKCDTHEAZXGVWNIpfmljqbyursokcdtheazxgvwni");

        /// <summary>
        /// Dvorak右手键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher DvorakRightHand =
            new(UL_Letters, "JLMFPQORSUYBZAEHTDCKXINWVGjlmfpqorsuybzaehtdckxinwvg");

        /// <summary>
        /// Colemak键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher Colemak =
            new(UL_Letters, "QWFPGJLUYARSTDHNEIOZXCVBKMqwfpgjluyarstdhneiozxcvbkm");

        /// <summary>
        /// AZERTY键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher AZERTY =
            new(UL_Letters, "AZERTYUIOPQSDFGHJKLMWXCVBNazertyuiopqsdfghjklmwxcvbn");

        /// <summary>
        /// 我曾用过的密码表，可以由<see cref="TakeTranslateCipher"/>得到
        /// </summary>
        public static readonly SingleReplacementCipher MTOHOEM =
            new(UL_Letters, "ANBYCODUEPFXGQHVIRJZKSLWMTanbycoduepfxgqhvirjzkslwmt");

        /// <summary>
        /// 星际火狐中的蜥蜴人语言(严格意义上专有名词应保持不变，且y应为0)
        /// </summary>
        public static readonly SingleReplacementCipher Saurian =
            new(UL_Letters, "URSTOVWXAZBCMDEFGHJKILNPYQurstovwxazbcmdefghjkilnpyq");

        /// <summary>
        /// 《打工吧！魔王大人》中使用的语言
        /// </summary>
        public static readonly SingleReplacementCipher PartTimerDevil =
            new(UL_Letters, "AZYXEWVTISRLPNOMQKJHUGFDCBazyxewvtisrlpnomqkjhugfdcb");

        /// <summary>
        /// 易经八卦和Base64
        /// </summary>
        public static readonly SingleReplacementCipher IChingHexagramsBase64 =
            new(Base64, "䷁䷗䷆䷒䷎䷣䷭䷊䷏䷲䷧䷵䷽䷶䷟䷡䷇䷂䷜䷻䷦䷾䷯䷄䷬䷐䷮䷹䷞䷰䷛䷪䷖䷚䷃䷨䷳䷕䷑䷙䷢䷔䷿䷥䷷䷝䷱䷍䷓䷩䷺䷼䷴䷤䷸䷈䷋䷘䷅䷉䷠䷌䷫䷀☯");


    }
}
