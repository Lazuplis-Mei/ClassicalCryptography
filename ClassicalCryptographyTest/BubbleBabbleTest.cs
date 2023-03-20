using ClassicalCryptography.Encoder;
using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class BubbleBabbleTest
    {
        [TestMethod]
        [DataRow("abcd", "ximek-domek-gyxox")]
        [DataRow("Pineapple", "xigak-nyryk-humil-bosek-sonax")]
        [DataRow("中文", "xunar-myruv-kahem-lyxox")]
        public void TestBubbleBabble(string plainText, string cipherText)
        {
            Assert.AreEqual(cipherText, BubbleBabble.EncodeString(plainText));
            Assert.AreEqual(plainText, BubbleBabble.DecodeString(cipherText));
        }
    }
}
