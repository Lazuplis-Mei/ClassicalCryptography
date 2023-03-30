using System.Drawing;

namespace ClassicalCryptography.Image;

/// <summary>
/// 加密区域信息
/// </summary>
public class EncryptRegions
{
    /// <summary>
    /// 是否存在密码
    /// </summary>
    public bool IncludePassword { get; set; }

    /// <summary>
    /// 矩形区域
    /// </summary>
    public Rectangle[] Regions { get; set; }

    /// <summary>
    /// 加密区域信息
    /// </summary>
    public EncryptRegions(Rectangle[] rectangles, bool password = false)
    {
        Array.Sort(rectangles, (r1, r2) => r1.Left.CompareTo(r2.Left));
        Regions = rectangles;
        IncludePassword = password;
    }

    /// <summary>
    /// 是否存在重叠
    /// </summary>
    public bool HasOverlap
    {
        get
        {
            for (int i = 0; i < Regions.Length - 1; i++)
                if (Regions[i].Right > Regions[i + 1].Left &&
                    Regions[i].Bottom > Regions[i + 1].Top &&
                    Regions[i].Top < Regions[i + 1].Bottom)
                    return true;
            return false;
        }
    }

    /// <summary>
    /// ToBytes
    /// </summary>
    public byte[] ToBytes()
    {
        using var memory = new MemoryStream();
        using var writer = new BinaryWriter(memory);
        writer.Write(IncludePassword);
        writer.Write(Regions.Length);
        foreach (var rect in Regions)
        {
            writer.Write(rect.X);
            writer.Write(rect.Y);
            writer.Write(rect.Width);
            writer.Write(rect.Height);
        }
        return memory.ToArray();
    }

    /// <summary>
    /// FromBytes
    /// </summary>
    public static EncryptRegions FromBytes(byte[] bytes)
    {
        using var memory = new MemoryStream(bytes);
        using var reader = new BinaryReader(memory);
        bool includePassword = reader.ReadBoolean();
        int count = reader.ReadInt32();
        var regions = new Rectangle[count];
        for (int i = 0; i < count; i++)
        {
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            regions[i] = new Rectangle(x, y, width, height);
        }
        return new EncryptRegions(regions, includePassword);
    }

}