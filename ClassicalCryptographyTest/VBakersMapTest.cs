using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class VBakersMapTest
    {
        [TestMethod]
        [DataRow("0123456789ABCDEF", "0E1F8697C2D34A5B")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUWXYZ",
                 "FYGZHPNQORUDWEXKILJMASBTC")]
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                 "0X1Y2ZCLDMENO9PAQBU3V4W5IFJGKH6R7S8T")]
        public void TestBakersMap(string plainText, string cipherText)
        {
            var cipher = new VBakersMapCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText)[..plainText.Length]);
        }
    }
}