using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class SixteenPuzzleTest
    {
        [TestMethod]
        public void TestSixteenPuzzle()
        {
            string plainText = "0123456789ABCDEF";
            string cipherText = "F01E3426795B8CDA";
            var cipher = new SixteenPuzzle();
            var key = SixteenPuzzle.Key.FromString("2,-3,1,4,-1");

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestSixteenPuzzle_G(int textLength)
        {
            var cipher = new SixteenPuzzle();
            for (int i = 0; i < 100; i++)
            {
                var key = SixteenPuzzle.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}