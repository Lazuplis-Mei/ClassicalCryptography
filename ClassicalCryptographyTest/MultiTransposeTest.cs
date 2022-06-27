using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Transposition2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static ClassicalCryptography.Interfaces.TranspositionHelper;

namespace ClassicalCryptographyTest
{
    [TestClass]
    public class MultiTransposeTest
    {
        [TestMethod]
        [DataRow(1, 5, 3)]
        [DataRow(2, 4, 6)]
        [DataRow(4, 20, 12)]
        [DataRow(10, 20, 90)]
        public void TestGCD(int gcd, int n, int m)
        {
            Assert.AreEqual(gcd, GCD(n, m));
        }

        [TestMethod]
        [DataRow(120, 2, 3, 5, 4, 8)]
        public void TestLCM(int lcm, params int[] args)
        {
            Assert.AreEqual(lcm, LCM(args.AsSpan()));
        }

        [TestMethod]
        [DataRow(5, 0, 5, 6, 3, 2, 4, 1)]
        [DataRow(12, 1, 2, 7, 6, 4, 3, 5, 0)]
        public void TestGetPeriod(int p, params int[] args)
        {
            Assert.AreEqual(p, GetPeriod(args.Select(n => (ushort)n).ToArray()));

            ushort[,] order = { { 1, 2, 7, 6 }, { 4, 3, 5, 0 } };
            Assert.AreEqual(12, GetPeriod(order));
        }

        [TestMethod]
        public void TestMultiTranspose()
        {
            ushort[] order = { 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2 };
            Assert.IsTrue(new ushort[]
            {
                8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7
            }.SequenceEqual(order.MultiTranspose(10)));
        }

        [TestMethod]
        [DataRow("123456789ABCDEFGHIJKLMNO", "147ADGJM258BEHKN369CFILO")]
        public void TestRailFence(string plainText, string cipherText)
        {
            var cipher = new AdvancedRailFenceCipher();

            var key = AdvancedRailFenceCipher.Key.FromString("4");

            Assert.AreEqual(cipherText, cipher.MultiEncrypt(plainText, key, 4));
            Assert.AreEqual(plainText, cipher.MultiDecrypt(cipherText, key, 4));
        }

        [TestMethod]
        [DataRow("123456789", "294753618")]
        public void TestMagicSquare(string plainText, string cipherText)
        {
            var cipher = new MagicSquareCipher();

            Assert.AreEqual(cipherText, cipher.MultiEncrypt(plainText, 3));
            Assert.AreEqual(plainText, cipher.MultiDecrypt(cipherText, 3));
        }
    }
}