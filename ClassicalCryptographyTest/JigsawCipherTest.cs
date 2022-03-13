using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class JigsawCipherTest
    {
        [TestMethod]
        [DataRow("0123456789ABCDEF", "045C167D289E3ABF", "1,2,1")]
        [DataRow("0123456789ABCDEFGHIJKLMNO", "01ABC23FDE54GHI67KLJ89MNO", "2,3")]
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                 "0123OP6745QR89ABSTCDEFUVIJGHWXKLMNYZ", "4,2")]
        public void TestJigsawCipher(string plainText, string cipherText, string keyStr)
        {
            var cipher = new JigsawCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            var key = JigsawCipher.Key.FromString(keyStr);
            Assert.IsFalse(key.CanInverse);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestJigsawCipher_G(int textLength)
        {
            var cipher = new JigsawCipher();
            for (int i = 0; i < 100; i++)
            {
                var key = JigsawCipher.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }

            cipher.ByColumn = true;
            for (int i = 0; i < 100; i++)
            {
                var key = JigsawCipher.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}