using ClassicalCryptography.Transposition;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicalCryptographyTest
{

    [TestClass]
    public class CycleTransposeTest
    {
        [TestMethod]
        [DataRow("StateKeyLaboratoryofNetworkingandSwitching",
                 "aKttSeLoyaebtyaorrNwfeotkgriondinSawhgcitn", "(1,5,6,2,3)")]
        [DataRow("Sitdownplease!`", "dSoitlwenp!a`se", "(1,2,4)(3,5)")]
        [DataRow("abcdefgh", "cdabghef", "(1,3)(2,4)")]
        public void TestCycleTranspose(string plainText, string cipherText, string keyStr)
        {
            var cipher = new CycleTranspose();

            var key = CycleTranspose.Key.FromString(keyStr);
            Assert.IsTrue(key.CanInverse);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key));
        }

        [TestMethod]
        [DataRow("abcdefgh", "cgdhaebf", "(1,3)(2,4)")]
        public void TestColumnTranspose(string plainText, string cipherText, string keyStr)
        {
            var cipher = new CycleTranspose
            {
                ByColumn = true//列置换
            };

            var key = CycleTranspose.Key.FromString(keyStr);
            Assert.IsTrue(key.CanInverse);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key));
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestCycleTranspose_G(int textLength)
        {
            var cipher = new CycleTranspose();
            for (int i = 0; i < 100; i++)
            {
                var key = CycleTranspose.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }

            cipher.ByColumn = true;
            for (int i = 0; i < 100; i++)
            {
                var key = CycleTranspose.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}