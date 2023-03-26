using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class VBakersMapTest
    {
        [TestMethod]
        [DataRow("0123456789ABCDEF", "04158C9D2637AEBF")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUWXYZ",
                 "AFBGCKPLQMUWXYZHDIEJRNSOT")]
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                 "061728CIDJEKOUPVQW394A5BFLGMHNRXSYTZ")]
        public void TestVBakersMap(string plainText, string cipherText)
        {
            var cipher = new VBakersMapCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText)[..plainText.Length]);
        }
    }
}