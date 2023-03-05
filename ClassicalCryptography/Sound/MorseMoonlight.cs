using ClassicalCryptography.Interfaces;
using Midi;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ClassicalCryptography.Sound;
//used <see href="https://www.codeproject.com/Articles/5272315/Midi-A-Windows-MIDI-Library-in-Csharp"/>

/// <summary>
/// 将摩斯密码掺入月光奏鸣曲第一乐章的节奏中
/// </summary>
[Introduction("摩斯月光奏鸣曲", "将摩斯密码掺入月光奏鸣曲第一乐章的节奏中")]
public static partial class MorseMoonlight
{
    private static readonly string notes = """
        Ab3Db4E4Ab3Db4E4Ab3Db4E4Ab3Db4E4,Ab3Db4E4Ab3Db4E4Ab3Db4E4Ab3Db4E4,
        A3Db4E4A3Db4E4A3D4F#4A3D4F#4,Ab3C4F#4Ab3Db4E4Ab3Db4Eb4F#3C4Eb4,
        E3Ab3Db4Ab3Db4E4Ab3Db4E4Ab3Db4E4,Ab3Eb4F#4Ab3Eb4F#4Ab3Eb4F#4Ab3Eb4F#4,
        Ab3Db4E4Ab3Db4E4A3Db4F#4A3Db4F#4,Ab3B3E4Ab3B3E4A3B3Eb4B4B3Eb4,
        Ab3B3E4Ab3B3E4Ab3B3E4Ab3B3E4,G3B3E4G3B3E4G3B3E4G3B3E4,G3B3F4G3B3F4G3B3F4G3B3F4,
        G3C4E4G3B3E4G3Db4E4F#3Db4E4,F#3B3D4F#3B3D4G3B3Db4E4B3Db4,F#3B3D4F#3B3D4F#3Bb3Db4F#3Bb3Db4,
        B3D4F#4B3D4F#4B3Eb4F#4B3Eb4F#4,B3E4G4B3E4G4B3E4G4B3E4G4,B3Eb4F#4B3Eb4F#4B3Eb4F#4B3Eb4F#4,
        B3E4G4B3E4G4B3E4G4B3E4G4,B3Eb4F#4B3Eb4F#4B3D4F4B3D4F4,B3Db4Ab4B3Db4Ab4A3Db4F#4A3Db4F#4,
        G3B3D4G3B3D4F#3A3Eb4F#3A3Eb4,Db3F#3A3Db3F#3A3Db3F#3Ab3Db3F3Ab3,F#3A3Db4A3Db4F#4Db4F#4A4Db4F#4A4,
        Db4Ab4B4Db4Ab4B4Db4Ab4B4Db4Ab4B4,Db4F#4A4Db4F#4A4C4F#4A4Db4F#4A4,Eb4F#4Ab4Eb4F#4Ab4Eb4F#4Ab4Eb4F#4Ab4
        """;
    private static readonly MatchCollection matches = SoundNoteRegex().Matches(notes);

    private static readonly (double start, double end, string[] notes)[] notes2 = new (double, double, string[])[]
    {
        (0, 4, new[]{"Db2", "Db3"}), (4, 8, new[]{"B1", "B2"}), (8, 10, new[]{"A1", "A2"}),
        (10, 12, new[] { "F#1", "F#2" }), (12, 14, new[] { "Ab1", "Ab2" }), (14, 16, new[] { "Ab1", "Ab2" }),
        (16, 19, new[] { "Db2", "Ab2", "Db3" }), (19, 19.9, new[] { "Ab4" }), /*(19.9, 20, new[] { "Ab4" }),*/
        (20, 23, new[] { "C#2", "G#2", "C#3", "Ab4" }), (23, 23.9, new[] { "Ab4" }), /*(23.9, 24, new[] { "Ab4" }),*/
        (24, 26, new[] { "Db2", "Db3", "Ab3" }), (26, 28, new[] { "F#1", "F#2", "A4" }),
        (28, 30, new[] { "B1", "B2", "Ab4" }), (30, 32, new[] { "B1", "B2", "F#4" }),
        (32, 36, new[] { "E2", "E3", "E4" }), (36, 39, new[] { "E2", "E3" }), (39, 39.9, new[] { "G4" }),
        /*(39.9, 40, new[] { "G4" }),*/ (40, 43, new[] { "D2", "D3", "G4" }), (43, 43.9, new[] { "G4" }),
        /*(43.9, 44, new[] { "G4" }),*/ (44, 45, new[] { "C2", "C3", "G4" }), (45, 46, new[] { "B1", "B2" }),
        (46, 47, new[] { "Bb1", "Bb2" }), (47, 48, new[] { "F#4" }), (48, 50, new[] { "B1", "B2", "F#4" }),
        (50, 51, new[] { "E2", "G4" }), (51, 52, new[] { "G2", "E3" }), (52, 54, new[] { "F#2", "F#4" }),
        (54, 56, new[] { "F#1", "F#2", "F#4" }), (56, 59, new[] { "B1", "B2" }), (59, 60, new[] { "B4" }),
        (60, 61, new[] { "C5" }), (61, 62, new[] { "E2", "E3" }), (62, 63, new[] { "G2", "G3" }),
        (63, 64, new[] { "E2", "E3", "Bb4" }), (64, 67, new[] { "B1", "B2", "B4" }), (67, 68, new[] { "B4" }),
        (68, 69, new[] { "C5" }), (69, 70, new[] { "E2", "E3" }), (70, 71, new[] { "G2", "G3" }),
        (71, 72, new[] { "E2", "E3", "Bb4" }), (72, 74, new[] { "B1", "B2", "B4" }),
        (74, 76, new[] { "Ab1", "Ab2", "B4" }), (76, 78, new[] { "F1", "F2", "B4" }),
        (78, 80, new[] { "F#1", "F#2", "A4" }), (80, 82, new[] { "B1", "B2", "G4" }),
        (82, 84, new[] { "C2", "C3", "F#4" }), (84, 86, new[] { "Db2", "Db4" }), (86, 87, new[] { "Db2", "Db4" }),
        (87, 88, new[] { "Db2", "Db4" }), (88, 91, new[] { "F#1", "Db2", "F#2" }), (91, 91.9, new[] { "Db5" }),
        /*(91.9, 92, new[] { "Db5" }),*/ (92, 95, new[] { "F2", "Db3", "F3", "Db5" }), (95, 95.9, new[] { "Db5" }),
        /*(95.9, 96, new[] { "Db5" }),*/ (96, 98, new[] { "F#2", "F#3", "Db5" }), (98, 99, new[] { "Eb2", "Eb3", "C5" }),
        (99, 100, new[] { "Db2", "Db3", "Db5" }), (100, 103, new[] { "C2", "Ab2", "C3", "Eb5" }),
        (103, 104, new[] { "C2", "Ab2", "C3", "Eb5" }), (104, 106,new[]  {"Db2", "Ab2", "Db3", "E5"})};

    private const int T = 70;
    private const int M = 400;
    private const int S = M - T;
    private const int L = M + T;

    /// <summary>
    /// 使用timidity编码为wav格式
    /// </summary>
    public static async void ExportWav(string morseCode, string filePath, string timidityPath = "timidity.exe")
    {
        var temp = Path.GetTempPath();
        var midPath = Path.Combine(temp, Path.GetFileNameWithoutExtension(filePath) + ".mid");
        ExportMidi(morseCode, midPath);
        await Process.Start(timidityPath, $@"""{midPath}"" -Ow -o ""{filePath}""").WaitForExitAsync();
        File.Delete(midPath);
    }

    /// <summary>
    /// 导出midi文件
    /// </summary>
    /// <param name="morseCode">摩斯密码</param>
    /// <param name="filePath">midi文件路径</param>
    public static void ExportMidi(string morseCode, string filePath)
    {
        if (morseCode.Length > matches.Count)
            throw new ArgumentException($"摩斯密码太长，应小于{matches.Count}", nameof(morseCode));

        var notelist = new List<MidiNote>();

        int start = 0, time = 0;
        int i = 0, j = 0;
        while (i < morseCode.Length)
        {
            if (notes2[j].end * 3 <= i)
            {
                if (notes2[j].end * 3 < i)
                {
                    int t = (int)((start - time) * 0.8);
                    foreach (var note in notes2[j].notes)
                    {
                        notelist.Add(new(time, 0, note, 127, t));
                        notelist.Add(new(time + t, 0, note, 127, start - time - t));
                    }
                }
                else
                {
                    foreach (var note in notes2[j].notes)
                    {
                        notelist.Add(new(time, 0, note, 127, start - time));
                    }
                }
                j++;
            }

            if (notes2[j].start * 3 >= i)
                time = start;

            switch (morseCode[i])
            {
                case '.':
                    notelist.Add(new(start, 0, matches[i].Value, 127, S));
                    start += S;
                    break;
                case '-':
                    notelist.Add(new(start, 0, matches[i].Value, 127, M));
                    start += M;
                    break;
                case '/':
                    notelist.Add(new(start, 0, matches[i].Value, 127, L));
                    start += L;
                    break;
                default:
                    break;
            }
            i++;
        }

        if ((notes2[j].start * 3 <= i && notes2[j].end * 3 > i)
            || (notes2[j].end * 3 <= i))
        {
            foreach (var note in notes2[j].notes)
            {
                notelist.Add(new(time, 0, note, 127, start - time));
            }
        }

        var file = new MidiFile();
        file.Tracks.Add(MidiSequence.FromNoteMap(notelist));
        file.WriteTo(filePath);
    }

    [GeneratedRegex("[CDEFGAB][#b]?[12345]")]
    private static partial Regex SoundNoteRegex();
}
