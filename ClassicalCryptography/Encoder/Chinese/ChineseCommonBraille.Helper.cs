using static ClassicalCryptography.Encoder.Chinese.ChineseHelper;

namespace ClassicalCryptography.Encoder.Chinese;

public partial class ChineseCommonBraille
{

    internal static partial class Helper
    {
        public const char ULETTER_SIGN = '⠠';

        public const char LLETTER_SIGN = '⠰';

        public const char NUMBER_SIGN = '⠼';

        public static readonly Dictionary<string, char> brailles = new()
        {
            { "B", '⠃' },
            { "P", '⠏' },
            { "M", '⠍' },
            { "F", '⠋' },
            { "D", '⠙' },
            { "T", '⠞' },
            { "N", '⠝' },
            { "L", '⠇' },
            { "G", '⠛' },{ "J", '⠛' },
            { "K", '⠅' },{ "Q", '⠅' },
            { "H", '⠓' },{ "X", '⠓' },
            { "ZH", '⠌' },
            { "CH", '⠟' },
            { "SH", '⠱' },
            { "R", '⠚' },
            { "Z", '⠵' },
            { "C", '⠉' },
            { "S", '⠎' },
            { "A", '⠔' },
            { "O", '⠢' },{ "E", '⠢' },
            { "I", '⠊' },
            { "U", '⠥' },
            { "V", '⠬' },
            { "ER", '⠗' },
            { "AI", '⠪' },
            { "AO", '⠖' },
            { "EI", '⠮' },
            { "OU", '⠷' },
            { "IA", '⠫' },
            { "IAO", '⠜' },
            { "IE", '⠑' },
            { "IOU", '⠳' },{ "IU", '⠳' },
            { "UA", '⠿' },
            { "UAI", '⠽' },
            { "UEI", '⠺' },{ "UI", '⠺' },
            { "UO", '⠕' },
            { "VE", '⠾' },
            { "AN", '⠧' },
            { "ANG", '⠦' },
            { "EN", '⠴' },
            { "ENG", '⠼' },
            { "IAN", '⠩' },
            { "IANG", '⠭' },
            { "IN", '⠣' },
            { "ING", '⠡' },
            { "UAN", '⠻' },
            { "UANG", '⠶' },
            { "UEN", '⠒' },{ "UN", '⠒' },
            { "ONG", '⠲' },{ "UENG", '⠲' },
            { "VAN", '⠯' },
            { "VN", '⠸' },
            { "IONG", '⠹' },
        };

        public static readonly Dictionary<char, string> braillesInverse = new()
        {
            { '⠃', "B" },
            { '⠏', "P" },
            { '⠍', "M" },
            { '⠋', "F" },
            { '⠙', "D" },
            { '⠞', "T" },
            { '⠝', "N" },
            { '⠇', "L" },
            { '⠛', "[G/J]" },
            { '⠅', "[K/Q]" },
            { '⠓', "[H/X]" },
            { '⠌', "ZH(I)" },
            { '⠟', "CH(I)" },
            { '⠱', "SH(I)" },
            { '⠚', "R(I)" },
            { '⠵', "Z(I)" },
            { '⠉', "C(I)" },
            { '⠎', "S(I)" },
            { '⠔', "A" },
            { '⠢', "[O/E]" },
            { '⠊', "[I/YI]" },
            { '⠥', "[U/WU]" },
            { '⠬', "[V/YU]" },
            { '⠗', "ER" },
            { '⠪', "AI" },
            { '⠖', "AO" },
            { '⠮', "EI" },
            { '⠷', "OU" },
            { '⠫', "[IA/YA]" },
            { '⠜', "[IAO/YAO]" },
            { '⠑', "[IE/YE]"},
            { '⠳', "[IU/YOU]" },
            { '⠿', "[UA/WA]" },
            { '⠽', "[UAI/WAI]" },
            { '⠺', "[UI/WEI]" },
            { '⠕', "[UO/WO]" },
            { '⠾', "[VE/YUE]" },
            { '⠧', "AN" },
            { '⠦', "ANG" },
            { '⠴', "EN" },
            { '⠼', "ENG" },
            { '⠩', "[IAN/YAN]" },
            { '⠭', "[IANG/YANG]" },
            { '⠣', "[IN/YIN]" },
            { '⠡', "[ING/YING]" },
            { '⠻', "[UAN/WAN]" },
            { '⠶', "[UANG/WANG]" },
            { '⠒', "[UN/WEN]" },
            { '⠲', "[ONG/WENG]" },
            { '⠯', "[VAN/YUAN]" },
            { '⠸', "[VN/YUN]" },
            { '⠹', "[IONG/YONG]" },
            { '⠁', "1" },
            { '⠂', "2" },
            { '⠄', "3" },
            { '⠆', "4" },
        };

        public static readonly Dictionary<char, string> punctuations = new()
        {
            { '。', "⠐⠆" },
            { '，', "⠐" },
            { '、', "⠈" },
            { '；', "⠰" },
            { '？', "⠐⠄" },{ '〈', "⠐⠄" },
            { '！', "⠰⠂" },
            { '：', "⠤" },
            { '“', "⠘" },{ '”', "⠘" },
            { '‘', "⠘⠘" },{ '’', "⠘⠘" },
            { '（', "⠰⠄" },
            { '）', "⠠⠆" },
            { '【', "⠰⠆" },{ '】', "⠰⠆" },
            { '—', "⠠⠤" },
            { '…', "⠐⠐⠐" },
            { '《', "⠐⠤" },
            { '》', "⠤⠂" },
            { '〉', "⠠⠂" },
        };

        public static readonly string letters = "⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵";
        public static readonly string toneNotes = "⠁⠂⠄⠆";

        public static void AppendCase1(StringBuilder result, ReadOnlySpan<char> brailles, ref int i)
        {
            if (++i >= brailles.Length)
            {
                result.Append('，');
                return;
            }
            switch (brailles[i])
            {
                case '⠆':
                    result.Append('。');
                    break;
                case '⠄':
                    result.Append('？');
                    break;
                case '⠤':
                    result.Append('《');
                    break;
                case '⠐':
                    if (++i < brailles.Length && brailles[i] == '⠐')
                    {
                        result.Append('…');
                        break;
                    }
                    i--;
                    result.Append('，').Append('，');
                    break;
                default:
                    i--;
                    result.Append('，');
                    break;
            }
        }

        public static void AppendCase2(StringBuilder result, ReadOnlySpan<char> brailles, ref int i, ref bool bracketsFlag)
        {
            if (++i >= brailles.Length)
            {
                result.Append('；');
                return;
            }
            switch (brailles[i])
            {
                case '⠂':
                    result.Append('！');
                    break;
                case '⠄':
                    result.Append('（');
                    break;
                case '⠆':
                    result.Append((bracketsFlag ^= true) ? '【' : '】');
                    break;
                default:
                    i--;
                    result.Append('；');
                    break;
            }
        }

        public static void AppendCase3(StringBuilder result, ReadOnlySpan<char> brailles, ref int i)
        {
            if (++i >= brailles.Length)
            {
                result.Append('：');
                return;
            }
            if (brailles[i] == '⠂')
            {
                result.Append('》');
                return;
            }
            i--;
            result.Append('：');
        }

        public static void AppendCase4(StringBuilder result, ReadOnlySpan<char> brailles, ref int i, ref bool dQuotesFlag, ref bool sQuotesFlag)
        {
            if (++i >= brailles.Length)
            {
                result.Append((dQuotesFlag ^= true) ? '“' : '”');
                return;
            }
            if (brailles[i] == '⠘')
            {
                result.Append((sQuotesFlag ^= true) ? '‘' : '’');
                return;
            }
            i--;
            result.Append((dQuotesFlag ^= true) ? '“' : '”');
        }

        public static void AppendCase5(StringBuilder result, ReadOnlySpan<char> brailles, ref int i)
        {
            if (++i >= brailles.Length)
                return;
            switch (brailles[i])
            {
                case '⠆':
                    result.Append('）');
                    break;
                case '⠤':
                    result.Append('—');
                    break;
                case '⠂':
                    result.Append('〉');
                    break;
                default:
                    i--;
                    break;
            }
        }

        public static void ResolveTonenote(string pinYin, ref int tonenote)
        {
            char firstCharacter = pinYin[0];
            if (tonenote == 1 && firstCharacter == 'F')
                tonenote = 0;
            else if ("PMTNHQCR".Contains(firstCharacter) && tonenote == 2 && pinYin is not "TOU")
                tonenote = 0;
            else if ("BDLGKJXZS".Contains(firstCharacter) && tonenote == 4 && (pinYin is not "LE" and not "ZI"))
                tonenote = 0;
            else if (SingleRhymes.ContainsKey(pinYin) && tonenote == 4 && !(pinYin is "YI" or "ER" or "WO" or "YE" or "YOU" or "E"))
                tonenote = 0;
            else if (tonenote == 1 && pinYin is "YI")
                tonenote = 0;
            else if (tonenote == 2 && pinYin is "ER")
                tonenote = 0;
            else if (tonenote == 3 && (pinYin is "WO" or "YE" or "YOU"))
                tonenote = 0;
            else if (pinYin is "O")
                tonenote = 0;
        }

        public static string ResolvePinyinInternal(Match match)
        {
            if (match.Groups["V_RP"].Success)
                return AlternateRegex().Replace(match.Value, "${First}");
            if (match.Groups["RP"].Success)
                return AlternateRegex().Replace(match.Value, "${Second}");
            if (match.Groups["VI_R"].Success)
                return match.Value.Replace("(I)", string.Empty);
            if (match.Groups["VI"].Success)
                return match.Value.Replace("(I)", "I");
            if (match.Groups["VP_R"].Success)
            {
                var m = AlternateRegex().Match(match.Value);
                var rhyme = match.ValueSpan[(m.Index + m.Length)..];
                var character = rhyme[0];
                if (character is 'I' or 'V')
                    return m.Groups["Second"].Value + new string(rhyme);
                if (character is 'A' or 'E')
                    return m.Groups["First"].Value + new string(rhyme);
                return match.Value;
            }
            if (match.Groups["VP_RP"].Success)
            {
                var m = AlternateRegex().Matches(match.Value);
                var vowel = m[0].ValueSpan;
                var rhyme = m[1].Groups["Second"].ValueSpan;
                var character = rhyme[0];
                if (character is 'I' or 'V')
                    vowel = m[0].Groups["Second"].ValueSpan;
                else if (character is 'A' or 'E')
                    vowel = m[0].Groups["First"].ValueSpan;
                else
                {
                    rhyme = m[1].Groups["First"].ValueSpan;
                    character = rhyme[0];
                    if (character is 'I' or 'V')
                        vowel = m[0].Groups["Second"].ValueSpan;
                    else if (character is 'A' or 'E')
                        vowel = m[0].Groups["First"].ValueSpan;
                }
                return new string(vowel) + new string(rhyme) + match.ValueSpan[^1];
            }
            if (match.Groups["VI_RP"].Success)
            {
                var m = AlternateRegex().Match(match.Value);
                var vowel = match.Value[..m.Index].Replace("(I)", string.Empty);
                var rhyme = m.Groups["First"].Value;
                return vowel + rhyme + match.ValueSpan[^1];
            }
            return string.Empty;
        }

        [GeneratedRegex(@"(?<Punctuations>[。，、；？！：“”‘’（）【】—…《》〈〉])|(?<LLetters>[a-z]+)|(?<ULetters>[A-Z]+)|(?<Numbers>[0-9]+)|([〇\u4E00-\u9FA5\uE81A-\uE863](\((?<Pinyin>[A-Za-zāáǎàōóǒòēéěèīíǐìūúǔùǖǘǚǜ]{1,6}[1-5]?)\))?)")]
        public static partial Regex ChineseCharWithPinyin();

        [GeneratedRegex(@"(⠠(?<ULetters>[⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵]+) )|(⠰(?<LLetters>[⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵]+) )|(⠼(?<Numbers>[⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚]+) )|(?<Others>(⠠[\u2800-\u283F])|(⠰[\u2800-\u283F])|[\u2800-\u283F-[⠠⠰⠼]]+)")]
        public static partial Regex ChineseCommonBrailleRegex();

        [GeneratedRegex(@"(?<VI>[A-Z]+\(I\))[1-4]|(?<VP_R>\[[A-Z]+/[A-Z]+\][A-Z]+)[1-4]|(?<VI_R>[A-Z]+\(I\)[A-Z]+)[1-4]|((?<![A-Z\]\)])(?<RP>\[[A-Z]+/[A-Z]+\])[1-4])|([A-Z]+(?<V_RP>\[[A-Z]+/[A-Z]+\])[1-4])|(?<VI_RP>[A-Z]+\(I\)\[[A-Z]+/[A-Z]+\])[1-4]|(?<VP_RP>(\[[A-Z]+/[A-Z]+\]){2})[1-4]")]
        public static partial Regex PinyinResolveRegex();

        [GeneratedRegex(@"\[(?<First>[A-Z]+)/(?<Second>[A-Z]+)\]")]
        public static partial Regex AlternateRegex();
    }
}
