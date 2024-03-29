﻿using System.IO.Compression;

namespace ClassicalCryptography.Utils;

internal static class GZip
{
    /// <summary>
    /// 压缩数据
    /// </summary>
    public static byte[] Compress(byte[] bytes)
    {
        var memory = new MemoryStream();
        GZipStream gzip = new(memory, CompressionMode.Compress);
        gzip.Write(bytes, 0, bytes.Length);
        gzip.Close();
        return memory.ToArray();
    }

    /// <summary>
    /// 解压数据
    /// </summary>
    public static byte[] Decompress(byte[] bytes) => DecompressToStream(bytes).ToArray();

    /// <summary>
    /// 解压数据
    /// </summary>
    public static MemoryStream DecompressToStream(byte[] bytes)
    {
        var memory = new MemoryStream(bytes);
        GZipStream gzip = new(memory, CompressionMode.Decompress);
        memory = new MemoryStream();
        gzip.CopyTo(memory);
        gzip.Close();
        memory.Seek(0, SeekOrigin.Begin);
        return memory;
    }

}