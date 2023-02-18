using ClassicalCryptography.Interfaces;
using static ClassicalCryptography.Utils.Globals;

namespace ClassicalCryptography.Replacement
{
    /// <summary>
    /// 其他常见的单表
    /// </summary>
    public static class CommonTables
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

    }
}
