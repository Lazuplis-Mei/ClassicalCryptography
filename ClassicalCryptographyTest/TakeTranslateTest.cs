using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{

    [TestClass]
    public class TakeTranslateTest
    {
        [TestMethod]
        [DataRow("012345", "024153")]
        [DataRow("qwertyuiop", "qetuowypir")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ACEGIKMOQSUWYBFJNRVZHPXLDT")]
        [DataRow("When I look at myself,I feel I'm so cute that I can't divert my eyes.",
                 "We  oka yefIfe ' oct htIcntdvr yee.nl ml lm ea 'it sIt,Iu  mhssayeeto")]
        public void TestTakeTranslate_D(string plainText, string cipherText)
        {
            var cipher = new TakeTranslateCipher();
            Assert.AreEqual(CipherType.Transposition, cipher.Type);

            var defaultKey = TakeTranslateCipher.Key.Default;
            Assert.IsFalse(defaultKey.CanInverse);
            Assert.AreEqual("11", defaultKey.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, defaultKey));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, defaultKey));
        }

        [TestMethod]
        [DataRow("012345", "015234", "23")]
        [DataRow("qwertyuiop", "qweyuirtop", "32")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDLMNOWXYZPQRSIJKTUVEFGH", "47")]
        [DataRow("When I look at myself,I feel I'm so cute that I can't divert my eyes.",
                 "When I  myself I'm sohat I cvert myook at,cute taeyes.lI't di  feel n", "77")]
        public void TestTakeTranslate(string plainText, string cipherText, string keyStr)
        {
            var cipher = new TakeTranslateCipher();

            var key = TakeTranslateCipher.Key.FromString(keyStr);
            Assert.IsFalse(key.CanInverse);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key));
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestTakeTranslate_G(int textLength)
        {
            var cipher = new TakeTranslateCipher();
            for (int i = 0; i < 100; i++)
            {
                var key = TakeTranslateCipher.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key));
            }
        }
    }
}