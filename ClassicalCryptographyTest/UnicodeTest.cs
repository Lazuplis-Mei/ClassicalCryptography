using ClassicalCryptography.Encoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class UnicodeTest
    {
        [TestMethod]
        [DataRow("DataRow", @"\u0044\u0061\u0074\u0061\u0052\u006f\u0077")]
        [DataRow("中文测试", @"\u4e2d\u6587\u6d4b\u8bd5")]
        public void TestUnicode(string text, string unicode)
        {
            Assert.AreEqual(unicode, UnicodeEncoding.Encode(text));
            Assert.AreEqual(text, UnicodeEncoding.Decode(unicode));
        }
    }
}
