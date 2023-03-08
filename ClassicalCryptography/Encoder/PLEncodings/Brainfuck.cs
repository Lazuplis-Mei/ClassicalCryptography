using ClassicalCryptography.Utils;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ClassicalCryptography.Encoder.PLEncodings;

/// <summary>
/// <para>Brainfuck解释器，参考资料</para>
/// <seealso href="https://en.wikipedia.org/wiki/Brainfuck"/>
/// <para>在线工具</para>
/// <seealso href="https://www.splitbrain.org/services/ook"/>
/// </summary>
public class Brainfuck
{
    /// <summary>
    /// 内存空间大小
    /// </summary>
    public const int MEMORY_SIZE = 100;

    /// <summary>
    /// 解释执行brainfuck代码
    /// <para>代码片段来自</para>
    /// <see href="https://www.bilibili.com/read/cv15123678"/>
    /// </summary>
    /// <param name="brainfuckCode">brainfuck代码</param>
    /// <param name="input">输入</param>
    public static string Interpret(string brainfuckCode, string input = "")
    {
        var result = new List<byte>();
        Span<byte> memoryBuffer = stackalloc byte[MEMORY_SIZE];
        var stack = new Stack<int>();
        int memoryPointer = 0, inputPointer = 0;
        if (Encoding.UTF8.GetByteCount(input) != input.Length)
            throw new ArgumentException("输入必须是单字节字符", nameof(input));
        for (int i = 0; i < brainfuckCode.Length; i++)
        {
            switch (brainfuckCode[i])
            {
                case '>':
                    memoryPointer++;
                    break;
                case '<':
                    memoryPointer--;
                    break;
                case '+':
                    memoryBuffer[memoryPointer]++;
                    break;
                case '-':
                    memoryBuffer[memoryPointer]--;
                    break;
                case '.':
                    result.Add(memoryBuffer[memoryPointer]);
                    break;
                case '[':
                    if (memoryBuffer[memoryPointer] != 0)
                    {
                        stack.Push(i);
                        continue;
                    }
                    int j = i, k = 0;
                    while (j < brainfuckCode.Length)
                    {
                        if (brainfuckCode[j] == '[')
                            k++;
                        if (brainfuckCode[j] == ']')
                            k--;
                        if (k == 0)
                            break;
                        j++;
                    }
                    if (k == 0)
                        i = j;
                    else
                        throw new ArgumentException("语句块块不匹配", nameof(brainfuckCode));
                    break;
                case ']':
                    i = stack.Pop() - 1;
                    break;
                case ',':
                    memoryBuffer[memoryPointer] = (byte)input[inputPointer++];
                    break;
                default:
                    break;
            }
        }
        return Encoding.UTF8.GetString(result.ToArray());
    }

    /// <summary>
    /// 为文本生成brainfuck代码
    /// <para>参考代码</para>
    /// <see href="https://github.com/splitbrain/ook/blob/master/util.php"/>    
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    [TranslatedFrom("php")]
    public static string GenerateCode(string text)
    {
        int value = 0;
        var bytes = Encoding.UTF8.GetBytes(text);
        var result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            var difference = bytes[i] - value;
            var absDifference = Math.Abs(difference);
            value = bytes[i];
            if (difference == 0)
            {
                result.Append(">.<");
                continue;
            }

            if (absDifference < 10)
            {
                result.Append('>');

                if (difference > 0)
                    result.Append('+'.Repeat(difference));
                else if (difference < 0)
                    result.Append('-'.Repeat(absDifference));
            }
            else
            {
                var loop = (int)Math.Sqrt(absDifference);

                result.Append('+'.Repeat(loop));

                if (difference > 0)
                {
                    result.Append($"[->{'+'.Repeat(loop)}<]");
                    result.Append('>').Append('+'.Repeat(difference - loop * loop));
                }
                else if (difference < 0)
                {
                    result.Append($"[->{'-'.Repeat(loop)}<]");
                    result.Append('>').Append('-'.Repeat(absDifference - loop * loop));
                }
            }

            result.Append(".<");
        }

        result.Replace("<>", string.Empty);
        return result.ToString();
    }

}
