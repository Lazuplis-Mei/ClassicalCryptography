using ClassicalCryptography.Transposition;
using ClassicalCryptography.Transposition2D;
using ClassicalCryptography.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class RotatingGrillesTest
    {
        [TestMethod]
        public void TestRotatingGrilles()
        {
            string plainText = "meetmeattwelvepm";
            string cipherText = "tmmveeewepeatlmt";
            var qArr = new QuaterArray(4);
            qArr[0] = 2;
            qArr[1] = 3;
            qArr[2] = 1;
            qArr[3] = 0;

            var cipher = new RotatingGrillesCipher();
            var key = new RotatingGrillesCipher.Key(qArr);

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        public void TestRotatingGrillesAntiClockwise()
        {
            string @plainText = "thelordismyshepherdishallnotbeinwant";
            string cipherText = "mtdhyeisthlbesoehpirhadnelwrailnnsot";

            var qArr = new QuaterArray(9);
            qArr[0] = 1;
            qArr[1] = 0;        //.H.H.H
            qArr[2] = 2;        //....H.
            qArr[3] = 2;        //..H...
            qArr[4] = 1;        //.H..H.
            qArr[5] = 3;        //.....H
            qArr[6] = 1;        //...H..
            qArr[7] = 2;        //
            qArr[8] = 0;

            var keyStr = qArr.ToString();
            var cipher = new RotatingGrillesCipher
            {
                AntiClockwise = true//逆时针转动
            };

            var key = RotatingGrillesCipher.Key.FromString(keyStr);
            Assert.AreEqual(key.ToString(),
                ".H.H.H" + Environment.NewLine +
                "....H." + Environment.NewLine +
                "..H..." + Environment.NewLine +
                ".H..H." + Environment.NewLine +
                ".....H" + Environment.NewLine +
                "...H.." + Environment.NewLine);
            Assert.AreEqual(keyStr, key.GetString());

            Assert.AreEqual(cipherText, cipher.Encrypt(plainText, key));
            Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..plainText.Length]);
        }

        [TestMethod]
        [DataRow(5), DataRow(17), DataRow(23), DataRow(32), DataRow(45)]
        public void TestRotatingGrilles_G(int textLength)
        {
            var cipher = new RotatingGrillesCipher();
            for (int i = 0; i < 100; i++)
            {
                var key = RotatingGrillesCipher.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }

            cipher.AntiClockwise = true;

            for (int i = 0; i < 100; i++)
            {
                var key = RotatingGrillesCipher.Key.GenerateKey(textLength);
                var plainText = RandomString.Generate(textLength);
                var cipherText = cipher.Encrypt(plainText, key);
                Assert.AreEqual(plainText, cipher.Decrypt(cipherText, key)[..textLength]);
            }
        }
    }
}