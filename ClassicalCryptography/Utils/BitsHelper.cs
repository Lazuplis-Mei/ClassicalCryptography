using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Utils;

internal static class BitsHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static int CombineInt32(byte b1, byte b2, byte b3, byte b4)
    {
        int number;
        byte* pointer = (byte*)&number;
        pointer[3] = b1;
        pointer[2] = b2;
        pointer[1] = b3;
        pointer[0] = b4;
        return number;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static uint CombineUInt32(byte b1, byte b2, byte b3, byte b4)
    {
        uint number;
        byte* pointer = (byte*)&number;
        pointer[3] = b1;
        pointer[2] = b2;
        pointer[1] = b3;
        pointer[0] = b4;
        return number;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void DecomposeInt32(int number, out byte b1, out byte b2, out byte b3, out byte b4)
    {
        byte* pointer = (byte*)&number;
        b1 = pointer[3];
        b2 = pointer[2];
        b3 = pointer[1];
        b4 = pointer[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void DecomposeUInt32(uint number, out byte b1, out byte b2, out byte b3, out byte b4)
    {
        byte* pointer = (byte*)&number;
        b1 = pointer[3];
        b2 = pointer[2];
        b3 = pointer[1];
        b4 = pointer[0];
    }

}
