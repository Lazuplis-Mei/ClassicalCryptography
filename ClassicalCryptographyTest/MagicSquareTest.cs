using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class MagicSquareTest
    {
        [TestMethod]
        [DataRow("123456789", "816357492")]//奇数阶
        [DataRow("0123456789ABCDEF", "0ED3B56879A4C21F")]//双偶阶
        //单偶阶
        [DataRow("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", "Y05PIN2V6KMOU81LQJ7RWG9ET4XBDF3ZSCHA")]
        public void TestMagicSquare(string plainText, string cipherText)
        {
            var cipher = new MagicSquareCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText)[..plainText.Length]);
        }
    }
}