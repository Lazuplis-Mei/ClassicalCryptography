using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class SpiralCurveTest
    {
        [TestMethod]
        [DataRow("0123456789ABCDEF", "0123BCD4AFE59876", "4")]
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                 "01234567LMNOPQR8KZ````S9JYXWVUTAIHGFEDCB", "8")]
        public void TestSpiralCurve(string plainText, string cipherText, string keyStr)
        {
            var cipher = new SpiralCurveCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            var key = WidthKey.FromString(keyStr);
            Assert.IsFalse(key.CanInverse);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestSpiralCurve_G(int textLength)
        {
            var cipher = new SpiralCurveCipher();
            for (int i = 0; i < 100; i++)
            {
                var key = WidthKey.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }

            cipher.ByColumn = true;
            for (int i = 0; i < 100; i++)
            {
                var key = WidthKey.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}