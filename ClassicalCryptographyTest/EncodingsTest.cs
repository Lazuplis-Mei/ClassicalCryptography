using ClassicalCryptography.Encoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class EncodingsTest
    {

        [TestMethod]
        [DataRow("98765", "X̅C̅V̅MMMDCCLXV")]
        [DataRow("12345", "X̅MMCCCXLV")]
        public void TestRomanNumerals(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, RomanNumerals.ArabicToRoman(text));
            Assert.AreEqual(text, RomanNumerals.RomanToArabic(encodingText));
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
