using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class RailFenceCipherTest
    {
        [TestMethod]
        [DataRow("RailFenceCipherTest", "RlnChTtaFciee`ieeprs`", "3")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "AHOVBIPWCJQXDKRYELSZFMT`GNU`", "7")]
        public void TestRailFence(string plainText, string cipherText, string keyStr)
        {
            var cipher = new RailFenceCipher();

            var key = RailFenceCipher.Key.FromString(keyStr);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestRailFence_G(int textLength)
        {
            var cipher = new RailFenceCipher();
            for (int i = 0; i < 100; i++)
            {
                var key = RailFenceCipher.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}