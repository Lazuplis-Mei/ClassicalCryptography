using System.Reflection.Emit;
using System.Reflection;
using System.Text;

namespace ClassicalCryptography.Encoder.PLEncodings;

//copy from https://www.bilibili.com/read/cv15123678

internal class Brainfuck
{

    private static char GetDChar(byte v1, byte v2)
    {
        return (char)((v1 << 8) + v2);
    }

    public static string RunFile(string code, string input = "")
    {
        var result = new StringBuilder();
        var buffer = new byte[256];
        var stack = new Stack<int>();
        byte ptr = 0;
        int l = 0;
        if (Encoding.UTF8.GetByteCount(input) != input.Length)
            throw new ArgumentException("输入必须是单字节字符", nameof(input));
        for (int i = 0; i < code.Length; i++)
        {
            switch (code[i])
            {
                case '>':
                    ptr++;
                    break;
                case '<':
                    ptr--;
                    break;
                case '+':
                    buffer[ptr]++;
                    break;
                case '-':
                    buffer[ptr]--;
                    break;
                case '.':
                    result.Append((char)buffer[ptr]);
                    break;
                case ':':
                    result.Append(GetDChar(buffer[ptr], buffer[ptr + 1]));
                    break;
                case ',':
                    buffer[ptr] = (byte)input[l++];
                    break;
                case '[':
                    if (buffer[ptr] != 0)
                        stack.Push(i);
                    else
                    {
                        int j = i, k = 0;
                        while (j < code.Length)
                        {
                            if (code[j] == '[')
                                k++;
                            if (code[j] == ']')
                                k--;
                            if (k == 0)
                                break;
                            j++;
                        }
                        if (k == 0)
                            i = j;
                        else
                            throw new Exception("error: no match for block");
                    }
                    break;
                case ']':
                    i = stack.Pop() - 1;
                    break;
                default:
                    break;
            }
        }
        return result.ToString();
    }


}
