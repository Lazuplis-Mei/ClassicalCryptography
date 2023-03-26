using ClassicalCryptography.Encoder;
using ClassicalCryptography.Encoder.BaseEncodings;
using ClassicalCryptography.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class BaseEncodingsTest
    {
        [TestMethod]
        [DataRow("goodjob", "👞👦👦👛👡👦👙")]
        [DataRow("测试中文", "📝💬💂📟💦💌📛💯💤📝💍👾")]
        public void TestBase100(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToBase100(text));
            Assert.AreEqual(text, BaseEncoding.FromBase100(encodingText));
        }

        [TestMethod]
        [DataRow(new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 }, "GƸOʜeҩ")]
        public void TestBase2048(byte[] bytes, string encodingText)
        {
            Assert.AreEqual(encodingText, Base2048Encoding.Encode(bytes));
            Assert.IsTrue(bytes.SequenceEqual(Base2048Encoding.Decode(encodingText)));
        }

        [TestMethod]
        [DataRow("hello world", "媒腻㐤┖ꈳ埳")]
        public void TestBase32768(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToBase32768(text));
            Assert.AreEqual(text, BaseEncoding.FromBase32768(encodingText));
        }

        [TestMethod]
        [DataRow("goodjob", "M5XW6ZDKN5RA====")]
        [DataRow("测试中文", "422YX2FPSXSLRLPGS2DQ====")]
        public void TestBase32(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToBase32(text));
            Assert.AreEqual(text, BaseEncoding.FromBase32(encodingText));
        }

        [TestMethod]
        [DataRow("hello world", "驨ꍬ啯𒁷ꍲᕤ")]
        public void TestBase65536(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToBase65536(text));
            Assert.AreEqual(text, BaseEncoding.FromBase65536(encodingText));
        }

        [TestMethod]
        [DataRow("GOOD很好", "51NEyzu5kapWnp")]
        public void TestBase58(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToBase(text, GlobalTables.Base58));
            Assert.AreEqual(text, BaseEncoding.FromBase(encodingText, GlobalTables.Base58));
        }

        [TestMethod]
        [DataRow("NICETRY", ":.[fA<)Qj")]
        [DataRow("是的", "k*W@8RY1")]
        public void TestBase85(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToBase85(text));
            Assert.AreEqual(text, BaseEncoding.FromBase85(encodingText));
        }

        [TestMethod]
        [DataRow("012=Q是的ABC", "012=3DQ=E6=98=AF=E7=9A=84ABC")]
        public void TestQuotedPrintable(string text, string encodingText)
        {
            Assert.AreEqual(encodingText, BaseEncoding.ToQuotedPrintable(text));
            Assert.AreEqual(text, BaseEncoding.FromQuotedPrintable(encodingText));
        }

    }
}
