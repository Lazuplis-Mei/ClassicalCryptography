using ClassicalCryptography.Calculation.ShortHide5;
using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class JosephusCipherTest
    {
        [TestMethod]
        [DataRow("012345", "253140", "3")]
        [DataRow("123456", "546231", "5")]
        [DataRow("Josephus survived with his jewish friend.",
            "sh rv th whrnJpsiw jsi.uvis dsdeeuiohefei", "3")]
        public void TestJosephusCipher(string plainText, string cipherText, string keyStr)
        {
            var cipher = new JosephusCipher();

            var key = JosephusCipher.Key.FromString(keyStr);
            Assert.IsFalse(key.CanInverse);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key));
        }
    }
}