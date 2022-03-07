using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class ReverseTest
    {
        [TestMethod]
        [DataRow("012345", "543210")]
        [DataRow("qwertyuiop", "poiuytrewq")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA")]
        [DataRow("When I look at myself,I feel I'm so cute that I can't divert my eyes.",
                 ".seye ym trevid t'nac I taht etuc os m'I leef I,flesym ta kool I nehW")]
        public void TestReverse(string plainText, string cipherText)
        {
            var cipher = new ReverseCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText));
        }

    }
}