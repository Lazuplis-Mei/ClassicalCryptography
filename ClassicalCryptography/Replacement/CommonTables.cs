using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using static ClassicalCryptography.Utils.Globals;

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
            new(ULLetters, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm");

        /// <summary>
        /// Atbash
        /// </summary>
        public static readonly SingleReplacementCipher Atbash = 
            new(ULLetters, "ZYXWVUTSRQPONMLKJIHGFEDCBAzyxwvutsrqponmlkjihgfedcba");
        
        /// <summary>
        /// 汉语拼音
        /// </summary>
        public static readonly SingleReplacementCipher PinYin =
            new(ULLetters, "AOEIUVBPMFDTNLGKHJQXZCSRYWaoeiuvbpmfdtnlgkhjqxzcsryw");

        /// <summary>
        /// Rot5
        /// </summary>
        public static readonly SingleReplacementCipher Rot5 = 
            new(Digits, "5678901234");

        /// <summary>
        /// Rot13
        /// </summary>
        public static readonly SingleReplacementCipher Rot13 = 
            new(ULLetters, "NOPQRSTUVWXYZABCDEFGHIJKLMnopqrstuvwxyzabcdefghijklm");

        /// <summary>
        /// Rot18
        /// </summary>
        public static readonly SingleReplacementCipher Rot18 = 
            new(ULLetterDigits, "NOPQRSTUVWXYZABCDEFGHIJKLMnopqrstuvwxyzabcdefghijklm5678901234");
        
        /// <summary>
        /// Rot47
        /// </summary>
        public static readonly SingleReplacementCipher Rot47 = 
            new(PrintableAsciis, "PQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNO");

        /// <summary>
        /// 游戏《最终幻想X》中的密码语言Al Bhed
        /// </summary>
        public static readonly SingleReplacementCipher Al_Bhed =
            new(ULLetters + Hiragana, "YPLTAVKREZGMSHUBXNCDIJFQOWypltavkrezgmshubxncdijfqowワミフネトアチルテヨラキヌヘホサヒユセソハシスメオマリクケロヤイツレコタヲモナニウエノカムン");

        /// <summary>
        /// Dvorak键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher Dvorak =
            new(ULLetters, "PYFGCRLAOEUIDHTNSQJKXBMWVZpyfgcrlaoeuidhtnsqjkxbmwvz");

        /// <summary>
        /// Dvorak左手键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher DvorakLeftHand =
            new (ULLetters, "PFMLJQBYURSOKCDTHEAZXGVWNIpfmljqbyursokcdtheazxgvwni");

        /// <summary>
        /// Dvorak右手键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher DvorakRightHand =
            new(ULLetters, "JLMFPQORSUYBZAEHTDCKXINWVGjlmfpqorsuybzaehtdckxinwvg");

        /// <summary>
        /// Colemak键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher Colemak =
            new(ULLetters, "QWFPGJLUYARSTDHNEIOZXCVBKMqwfpgjluyarstdhneiozxcvbkm");

        /// <summary>
        /// AZERTY键盘布局
        /// </summary>
        public static readonly SingleReplacementCipher AZERTY =
            new(ULLetters, "AZERTYUIOPQSDFGHJKLMWXCVBNazertyuiopqsdfghjklmwxcvbn");

        /// <summary>
        /// 我曾用过的密码表，可以由<see cref="TakeTranslateCipher"/>得到
        /// </summary>
        public static readonly SingleReplacementCipher MTOHOEM =
            new(ULLetters, "ANBYCODUEPFXGQHVIRJZKSLWMTanbycoduepfxgqhvirjzkslwmt");

        /// <summary>
        /// 星际火狐中的蜥蜴人语言(严格意义上专有名词应保持不变，且y应为0)
        /// </summary>
        public static readonly SingleReplacementCipher Saurian =
            new(ULLetters, "URSTOVWXAZBCMDEFGHJKILNPYQurstovwxazbcmdefghjkilnpyq");

        /// <summary>
        /// 《打工吧！魔王大人》中使用的语言
        /// </summary>
        public static readonly SingleReplacementCipher PartTimerDevil =
            new(ULLetters, "AZYXEWVTISRLPNOMQKJHUGFDCBazyxewvtisrlpnomqkjhugfdcb");
    }
}
