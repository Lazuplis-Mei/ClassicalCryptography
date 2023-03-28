using ClassicalCryptography.Transposition2D;
using ClassicalCryptography.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class FifteenPuzzleTest
    {
        [TestMethod]
        public void TestFifteenPuzzle()
        {
            string plainText = "0123456789ABCDEF";
            string cipherText = "012345678D9ACEFB";
            var qArr = new QuaterArray(5);
            qArr[0] = 1;
            qArr[1] = 2;
            qArr[2] = 2;
            qArr[3] = 3;
            qArr[4] = 0;

            var cipher = new FifteenPuzzle();
            var key = new FifteenPuzzle.Key(qArr);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestFifteenPuzzle_G(int textLength)
        {
            var cipher = new FifteenPuzzle();
            for (int i = 0; i < 100; i++)
            {
                var key = FifteenPuzzle.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}