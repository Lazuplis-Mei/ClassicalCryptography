using ClassicalCryptography.Encoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class EncodingsTest
    {

        [TestMethod]
        [DataRow(98765UL, "X̅C̅V̅MMMDCCLXV")]
        [DataRow(12345UL, "X̅MMCCCXLV")]

        [DataRow(9007199254740991UL, "M̅̅̅̅X̅̅̅̅̅V̅̅̅̅M̅̅̅M̅̅̅C̅̅̅X̅̅̅C̅̅̅M̅̅X̅̅̅C̅̅C̅̅L̅̅M̅V̅̅D̅C̅C̅X̅L̅CMXCI")]
        public void TestRomanNumerals(ulong number, string encodingText)
        {
            Assert.AreEqual(encodingText, RomanNumerals.ArabicToRoman(number));
            Assert.AreEqual(number, RomanNumerals.RomanToArabic(encodingText));
        }

        [TestMethod]
        [DataRow("golden traum", "⢶⢾⠞⠖⢖⠾⠄⡖⡦⢆⣖⢞")]
        public void TestBrailleEncoding(string text, string encodingText)
        {
            var bytes = new byte[]{
                0xd4, 0x1d, 0x8c, 0xd9, 0x8f, 0x00, 0xb2, 0x04,
                0xe9, 0x80, 0x09, 0x98, 0xec, 0xf8, 0xf1, 0x1f };
            Assert.AreEqual("⡓⣘⠙⣋⢹⠀⡥⠐⢏⠁⢈⡉⠟⡏⣇⣸", BrailleEncoding.EncodeBytes(bytes));

            Assert.AreEqual(encodingText, BrailleEncoding.Encode(text));
            Assert.AreEqual(text, BrailleEncoding.Decode(encodingText));
        }

    }
}
