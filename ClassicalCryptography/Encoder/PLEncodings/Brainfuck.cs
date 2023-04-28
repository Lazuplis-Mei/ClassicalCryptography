using CommunityToolkit.HighPerformance;
using System.Runtime.CompilerServices;

namespace ClassicalCryptography.Encoder.PLEncodings;

/// <summary>
/// <see href="https://en.wikipedia.org/wiki/Brainfuck">Brainfuck程序</see>
/// </summary>
/// <remarks>
/// 在线工具:<see href="https://www.splitbrain.org/services/ook">Brainfuck</see>
/// </remarks>
public class Brainfuck
{
    /// <summary>
    /// 内存空间大小
    /// </summary>
    public const int MEMORY_SIZE = 255;

    /// <summary>
    /// 字符编码
    /// </summary>
    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 解释执行brainfuck代码
    /// </summary>
    /// <param name="brainfuckCode">brainfuck代码</param>
    /// <param name="input">输入</param>
    [ReferenceFrom("https://www.bilibili.com/read/cv15123678")]
    public static string Interpret(string brainfuckCode, string input = "")
    {
        var inputBytes = Encoding.GetBytes(input);
        Span<byte> memoryBuffer = stackalloc byte[MEMORY_SIZE];
        var stack = new Stack<int>();
        int memoryPointer = 0, inputPointer = 0;
        var result = new List<byte>();
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
                    if (k != 0)
                        throw new ArgumentException("语句块不匹配", nameof(brainfuckCode));
                    i = j;
                    break;
                case ']':
                    i = stack.Pop() - 1;
                    break;
                case ',':
                    memoryBuffer[memoryPointer] = inputBytes[inputPointer++];
                    break;
                case '!':
                    memoryBuffer[memoryPointer] = (byte)~memoryBuffer[memoryPointer++];
                    break;
                case '&':
                    memoryBuffer[memoryPointer] &= inputBytes[inputPointer++];
                    break;
                case '|':
                    memoryBuffer[memoryPointer] |= inputBytes[inputPointer++];
                    break;
                case '^':
                    memoryBuffer[memoryPointer] ^= inputBytes[inputPointer++];
                    break;
                case '*':
                    memoryBuffer[memoryPointer] *= inputBytes[inputPointer++];
                    break;
                case '/':
                    memoryBuffer[memoryPointer] /= inputBytes[inputPointer++];
                    break;
                case '%':
                    memoryBuffer[memoryPointer] %= inputBytes[inputPointer++];
                    break;
                case '@':
                    memoryBuffer[memoryPointer] = memoryBuffer[inputBytes[inputPointer++]];
                    break;
                case '#':
                    memoryBuffer[inputBytes[inputPointer++]] = memoryBuffer[memoryPointer];
                    break;
                case '$':
                    inputBytes.CopyTo(memoryBuffer[inputPointer..]);
                    break;
                case '(':
                    inputPointer -= 2;
                    break;
                case ')':
                    inputPointer++;
                    break;
                case '~':
                    memoryPointer = 0;
                    break;
            }
        }
        return Encoding.GetString(result.AsSpan());
    }

    /// <summary>
    /// 为文本生成brainfuck代码
    /// </summary>
    /// <param name="text">要编码的字符串</param>
    [ReferenceFrom("https://github.com/splitbrain/ook/blob/master/util.php", ProgramingLanguage.PHP, License.GPL2_0)]
    public static string GenerateCode(string text)
    {
        int value = 0;
        var bytes = Encoding.GetBytes(text);
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
                    result.Append(GetPlus(difference));
                else if (difference < 0)
                    result.Append(GetMinus(absDifference));
            }
            else
            {
                var loop = (int)Math.Sqrt(absDifference);

                result.Append(GetPlus(loop));

                if (difference > 0)
                {
                    result.Append($"[->{GetPlus(loop)}<]");
                    result.Append('>').Append(GetPlus(difference - loop * loop));
                }
                else if (difference < 0)
                {
                    result.Append($"[->{GetMinus(loop)}<]");
                    result.Append('>').Append(GetMinus(absDifference - loop * loop));
                }
            }

            result.Append(".<");
        }

        result.Replace("<>", string.Empty);
        return result.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string GetPlus(int count) => count switch
        {
            1 => "+",
            2 => "++",
            3 => "+++",
            4 => "++++",
            5 => "+++++",
            _ => '+'.Repeat(count)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string GetMinus(int count) => count switch
        {
            1 => "-",
            2 => "--",
            3 => "---",
            4 => "----",
            5 => "-----",
            _ => '-'.Repeat(count)
        };
    }

}
