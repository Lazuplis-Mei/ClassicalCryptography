using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class TwiddlePuzzleTest
    {
        [TestMethod]
        public void TestTwiddlePuzzle()
        {
            string plainText = "0123456789ABCDEF";
            string cipherText = "412059638A7BCDEF";
            var cipher = new TwiddlePuzzle();
            var key = TwiddlePuzzle.Key.FromString("1,2,3,5");

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestTwiddlePuzzle_G(int textLength)
        {
            var cipher = new TwiddlePuzzle();
            for (int i = 0; i < 100; i++)
            {
                var key = TwiddlePuzzle.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}