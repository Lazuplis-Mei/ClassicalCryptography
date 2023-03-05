using ClassicalCryptography.Encoder;
using ClassicalCryptography.Encoder.BaseEncodings;
using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class EncodingsTest
    {

        [TestMethod]
        [DataRow("98765", "X̅C̅V̅MMMDCCLXV")]
        public void TestRomanNumerals(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, RomanNumerals.ArabicToRoman(text));
            Assert.AreEqual(text, RomanNumerals.RomanToArabic(encodingText));
        }

        [TestMethod]
        [DataRow(new byte[] { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 },
                 "驨ꍬ啯𒁷ꍲᕤ")]
        public void TestBase65536(byte[] bytes, string encodingText)
        {
            Assert.AreEqual(encodingText, Base65536Encoding.Encode(bytes));
            Assert.IsTrue(bytes.SequenceEqual(Base65536Encoding.Decode(encodingText)));
        }

        [TestMethod]
        [DataRow(new byte[] { 104, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 },
                 "媒腻㐤┖ꈳ埳")]
        public void TestBase32768(byte[] bytes, string encodingText)
        {
            Assert.AreEqual(encodingText, Base32768Encoding.Encode(bytes));
            Assert.IsTrue(bytes.SequenceEqual(Base32768Encoding.Decode(encodingText)));
        }

        [TestMethod]
        [DataRow(new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 },
                 "GƸOʜeҩ")]
        public void TestBase2048(byte[] bytes, string encodingText)
        {
            Assert.AreEqual(encodingText, Base2048Encoding.Encode(bytes));
            Assert.IsTrue(bytes.SequenceEqual(Base2048Encoding.Decode(encodingText)));
        }

        [TestMethod]
        [DataRow("goodjob", "👞👦👦👛👡👦👙")]
        public void TestBase100(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToBase100(text));
            Assert.AreEqual(text, BaseEncoding.FromBase100(encodingText));
        }

    }
}
