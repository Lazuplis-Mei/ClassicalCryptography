﻿using ClassicalCryptography.Interfaces;
using ClassicalCryptography.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalCryptography.Transposition2D
{
    /// <summary>
    /// 栅栏密码(包括扩展栅栏密码)
    /// </summary>
    [Introduction("栅栏密码", "按字数分割，按列读出，如果密钥是多位则代表读取顺序。")]
    public partial class RailFenceCipher : TranspositionCipher2D<ushort[]>
    {
        /// <summary>
        /// 划分二维顺序矩阵
        /// </summary>
        /// <param name="textLength">原文长度</param>
        /// <param name="key">密钥</param>
        protected override (int Width, int Height) Partition(int textLength, IKey<ushort[]> key)
        {
            int width = key.KeyValue.Length;
            if (width == 1)
                width = key.KeyValue[0];
            int height = textLength.DivCeil(width);
            return (width, height);
        }
        /// <summary>
        /// 栅栏密码(包括扩展栅栏密码)
        /// </summary>
        public RailFenceCipher()
        {
            FillOrder = false;
            ByColumn = true;
        }
        /// <summary>
        /// 转换顺序
        /// </summary>
        /// <param name="indexes">正常顺序</param>
        /// <param name="key">密钥</param>
        protected override ushort[,] Transpose(ushort[,] indexes, IKey<ushort[]> key)
        {
            ushort[] vals = key.KeyValue;
            int height = indexes.GetLength(1);
            int width = vals.Length;
            if (vals.Length == 1)
                indexes.FillOrderByRow();
            else
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        indexes[vals[x], y] = (ushort)(x + y * width);
            return indexes;
        }

    }
}
