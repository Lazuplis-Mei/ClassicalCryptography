using ClassicalCryptography.Encoder;
using ClassicalCryptography.Encoder.BaseEncodings;
using ClassicalCryptography.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class BaseEncodingsTest
    {
        [TestMethod]
        [DataRow("goodjob", "👞👦👦👛👡👦👙")]
        [DataRow("测试中文", "📝💬💂📟💦💌📛💯💤📝💍👾")]
        [DataRow("the quick brown fox jumped over the lazy dog\n", "👫👟👜🐗👨👬👠👚👢🐗👙👩👦👮👥🐗👝👦👯🐗👡👬👤👧👜👛🐗👦👭👜👩🐗👫👟👜🐗👣👘👱👰🐗👛👦👞🐁")]
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
        [DataRow("caseDemo"), DataRow("caseEmpty")]
        [DataRow("every-byte"), DataRow("every-pair-of-bytes")]
        [DataRow("hatetris-wr"), DataRow("hatetris-wr-rle")]
        [DataRow("hatetris-wr-rle2"), DataRow("lena_std.tif")]
        public void TestBase2048Data(string dataName)
        {
            var bytes = File.ReadAllBytes($"BaseXXXXTestData/{dataName}.bin");
            var encodingText = File.ReadAllText($"Base2048TestData/{dataName}.txt");
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
        [DataRow("caseDemo"), DataRow("caseEmpty")]
        [DataRow("every-byte"), DataRow("every-pair-of-bytes")]
        [DataRow("hatetris-wr"), DataRow("hatetris-wr-rle")]
        [DataRow("hatetris-wr-rle2"), DataRow("lena_std.tif")]
        public void TestBase32768Data(string dataName)
        {
            var bytes = File.ReadAllBytes($"BaseXXXXTestData/{dataName}.bin");
            var encodingText = File.ReadAllText($"Base32768TestData/{dataName}.txt");
            Assert.AreEqual(encodingText, Base32768Encoding.Encode(bytes));
            Assert.IsTrue(bytes.SequenceEqual(Base32768Encoding.Decode(encodingText)));
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
        [DataRow("Man is distinguished, not only by his reason, but by this singular passion from other animals, which is a lust of the mind, that by a perseverance of delight in the continued and indefatigable generation of knowledge, exceeds the short vehemence of any carnal pleasure.", "9jqo^BlbD-BleB1DJ+*+F(f,q/0JhKF<GL>Cj@.4Gp$d7F!,L7@<6@)/0JDEF<G%<+EV:2F!,O<DJ+*.@<*K0@<6L(Df-\\0Ec5e;DffZ(EZee.Bl.9pF\"AGXBPCsi+DGm>@3BB/F*&OCAfu2/AKYi(DIb:@FD,*)+C]U=@3BN#EcYf8ATD3s@q?d$AftVqCh[NqF<G:8+EV:.+Cf>-FD5W8ARlolDIal(DId<j@<?3r@:F%a+D58'ATD4$Bl@l3De:,-DJs`8ARoFb/0JMK@qB4^F!,R<AKZ&-DfTqBG%G>uD.RTpAKYo'+CT/5+Cei#DII?(E,9)oF*2M7/c")]
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
