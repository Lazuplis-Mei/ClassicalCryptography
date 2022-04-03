using ClassicalCryptography.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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
            Assert.AreEqual(gcd, TranspositionHelper.GCD(n, m));
        }

        [TestMethod]
        [DataRow(120, 2, 3, 5, 4, 8)]
        public void TestLCM(int lcm, params int[] args)
        {
            Assert.AreEqual(lcm, TranspositionHelper.LCM(args.AsSpan()));
        }

        [TestMethod]
        [DataRow(5, 0, 5, 6, 3, 2, 4, 1)]
        [DataRow(12, 1, 2, 7, 6, 4, 3, 5, 0)]
        public void TestGetPeriod(int p, params int[] args)
        {
            Assert.AreEqual(p, TranspositionHelper
                .GetPeriod(args.Select(n => (ushort)n).ToArray()));
        }

        [TestMethod]
        public void TestMultiTranspose()
        {
            ushort[] order = { 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2 };
            Assert.IsTrue(new ushort[] { 8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7 }
                .SequenceEqual(order.MultiTranspose(10)));
        }
    }
}