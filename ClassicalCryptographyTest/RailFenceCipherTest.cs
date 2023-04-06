using ClassicalCryptography.Transposition;
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
        public void TestAdvancedRailFence(string plainText, string cipherText, string keyStr)
        {
            var cipher = new AdvancedRailFenceCipher();

            var key = AdvancedRailFenceCipher.Key.FromString(keyStr);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow("RailFenceCipherTest", "RlnChTtaFcieeieeprs", "3")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "AHOVBIPWCJQXDKRYELSZFMTGNU", "7")]
        public void TestRailFence(string plainText, string cipherText, string keyStr)
        {
            var cipher = new RailFenceCipher();

            var key = RailFenceCipher.Key.FromString(keyStr);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow("WEAREDISCOVEREDRUNATONCE", "WECRUOERDSOEERNTNEAIVDAC", "3")]
        [DataRow("WEAREDISCOVEREDRUNATONCE", "WVOEOETNACRACRSENEEIDUDR", "6")]
        [DataRow("WEAREDISCOVERED", "WVEOEACRRSEEIDD", "6")]
        public void TestOriginalRailFence(string plainText, string cipherText, string keyStr)
        {
            var cipher = new OriginalRailFenceCipher();
            var key = RailFenceCipher.Key.FromString(keyStr);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestAdvancedRailFence_G(int textLength)
        {
            var cipher = new AdvancedRailFenceCipher();
            for (int i = 0; i < 100; i++)
            {
                var key = AdvancedRailFenceCipher.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}