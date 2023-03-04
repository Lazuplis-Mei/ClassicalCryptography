using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Replacement;
using ClassicalCryptography.Image;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;

namespace ClassicalCryptographyTest
{
    [TestClass]
    [SupportedOSPlatform("windows")]
    public class WeaveCipherTest
    {
        [TestMethod]
        public void TestWeaveCipher()
        {
            var bits1 = new BitArray();
            bits1.appendBit(true);
            bits1.appendBit(false);
            bits1.appendBit(false);
            bits1.appendBit(true);
            bits1.appendBit(true);
            bits1.appendBit(false);
            bits1.appendBit(true);
            bits1.appendBit(false);
            bits1.appendBit(true);

            var bits2 = new BitArray();
            bits2.appendBit(false);
            bits2.appendBit(false);
            bits2.appendBit(false);
            bits2.appendBit(true);
            bits2.appendBit(true);
            bits2.appendBit(true);
            bits2.appendBit(false);
            bits2.appendBit(true);
            bits2.appendBit(false);

            var m = WeaveCipher.EncryptBits(bits1, bits2);

            //WeaveCipher.BitsToImage(m, @"E:/test.png");
            //WeaveCipher.Encrypt("Soul Undertone", @"E:/test.png");

            Debug.WriteLine(m);/*
  X X X   X X     X 
    X       X X     
  X X X   X X     X 
    X       X X     
X       X     X X   
    X       X X     
X       X     X X   
X X   X X X     X X 
  X X X   X X     X 
    X       X X     
*/
        }
        [TestMethod]
        public void TestWeaveCipher2()
        {
            var bits1 = new BitArray();
            bits1.appendBit(true);
            bits1.appendBit(true);
            bits1.appendBit(false);
            bits1.appendBit(true);
            bits1.appendBit(false);
            bits1.appendBit(false);

            var m = WeaveCipher.EncryptBitsExtend(bits1, bits1, bits1, bits1);
            WeaveCipher.EncryptExtend("Soul.Undertone.", @"E:/test.png");
            WeaveCipher.EncryptExtend("Taiji__no__yume", @"E:/test2.png");

            Debug.WriteLine(m);/*
  X   X       X   X X X X   
X X X X X X   X X   X X   X 
X   X   X       X X X X X   
X X X X X   X X X   X   X X 
    X X   X X X   X X       
  X X     X         X X     
X X     X X X   X     X X   
*/
        }
    }
}
