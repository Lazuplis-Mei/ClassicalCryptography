namespace ClassicalCryptography.Encoder.PLEncodings;

internal static partial class Constants
{
    [TranslatedFrom("JavaScript")]
    public static class Jother
    {
        public static readonly string[] jotherBase = new[]
        {
            "[]",          //空,相当于""
	        "{}",          //[object Object]
	        "![]",         //false
	        "!![]",        //true
	        "~[]",         //-1
	        "+{}",         //NaN
	        "{}[[]]",      //undefined
            //Infinity
            "(+(+!![]+(!![]+[])[!![]+!![]+!![]]+(+!![]+[])+(+[]+[])+(+[]+[])+(+[]+[])+[])+[])",
        };

        public static readonly string[] jotherDigits = new[]
        {
            "+[]",                                             //0
            "+!![]",                                           //1
            "!![]+!![]",                                       //2
            "!![]+!![]+!![]",                                  //3
            "!![]+!![]+!![]+!![]",                             //4
            "!![]+!![]+!![]+!![]+!![]",                        //5
            "!![]+!![]+!![]+!![]+!![]+!![]",                   //6
            "!![]+!![]+!![]+!![]+!![]+!![]+!![]",              //7
            "!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]",         //8
            "!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]"     //9
        };


        public static readonly Dictionary<char, string> characterMap;

        public static readonly string function;
        public static readonly string localString;
        static Jother()
        {
            characterMap = new()
            {
                {'0', $"({jotherDigits[0]}+[])" },
                {'1', $"({jotherDigits[1]}+[])" },
                {'2', $"({jotherDigits[2]}+[])" },
                {'3', $"({jotherDigits[3]}+[])" },
                {'4', $"({jotherDigits[4]}+[])" },
                {'5', $"({jotherDigits[5]}+[])" },
                {'6', $"({jotherDigits[6]}+[])" },
                {'7', $"({jotherDigits[7]}+[])" },
                {'8', $"({jotherDigits[8]}+[])" },
                {'9', $"({jotherDigits[9]}+[])" },
                {'a', $"({jotherBase[2]}+[])" + $"[{jotherDigits[1]}]" },
                {'b', $"({jotherBase[1]}+[])" + $"[{jotherDigits[2]}]" },
                {'c', $"({jotherBase[1]}+[])" + $"[{jotherDigits[5]}]" },
                {'d', $"({jotherBase[6]}+[])" + $"[{jotherDigits[2]}]" },
                {'e', $"({jotherBase[3]}+[])" + $"[{jotherDigits[3]}]" },
                {'f', $"({jotherBase[2]}+[])" + $"[{jotherDigits[0]}]" },
                {'i', $"({jotherBase[6]}+[])" + $"[{jotherDigits[5]}]" },
                {'j', $"({jotherBase[1]}+[])" + $"[{jotherDigits[3]}]" },
                {'l', $"({jotherBase[2]}+[])" + $"[{jotherDigits[2]}]" },
                {'n', $"({jotherBase[6]}+[])" + $"[{jotherDigits[1]}]" },
                {'o', $"({jotherBase[1]}+[])" + $"[{jotherDigits[1]}]" },
                {'r', $"({jotherBase[3]}+[])" + $"[{jotherDigits[1]}]" },
                {'s', $"({jotherBase[2]}+[])" + $"[{jotherDigits[3]}]" },
                {'t', $"({jotherBase[3]}+[])" + $"[{jotherDigits[0]}]" },
                {'u', $"({jotherBase[6]}+[])" + $"[{jotherDigits[0]}]" },
                {'y', $"({jotherBase[7]}+[])" + $"[{jotherDigits[7]}]" },
                {'I', $"({jotherBase[7]}+[])" + $"[{jotherDigits[0]}]" },
                {'N', $"({jotherBase[5]}+[])" + $"[{jotherDigits[0]}]" },
                {'O', $"({jotherBase[1]}+[])" + $"[{jotherDigits[8]}]" },
                {' ', $"({jotherBase[1]}+[])" + $"[{jotherDigits[7]}]" },
                {'[', $"({jotherBase[1]}+[])" + $"[{jotherDigits[0]}]" },
                {']', $"({jotherBase[1]}+[])" + Q2(jotherDigits[7] + Q4(jotherDigits[7])) },
                {'-', $"({jotherBase[4]}+[])" + $"[{jotherDigits[0]}]" },
                {'+', Q1(Q3(jotherDigits[1] + Q4($"({jotherBase[3]}+[])" + $"[{jotherDigits[3]}]") + Q3(jotherDigits[1]) + Q3(jotherDigits[0]) + Q3(jotherDigits[0]))) + Q2(jotherDigits[2]) }//1e+100
            };

            function = $"[][{ToString("sort")}][{ToString("constructor")}]";
            localString = $"[]+{ToScript("return location")}";
            characterMap['h'] = Q5(localString) + $"[{jotherDigits[0]}]";
            characterMap['p'] = Q5(localString) + $"[{jotherDigits[3]}]";
            characterMap[':'] = Q5(localString) + $"[{jotherDigits[4]}]";
            characterMap['/'] = Q5(localString) + $"[{jotherDigits[6]}]";
            rc_unescape = ToScript("return unescape");
            rc_escape = ToScript("return escape");
            characterMap['%'] = $"{rc_escape}({ToString("[")}){Q2(jotherDigits[0])}";
        }
        public static readonly string rc_unescape;
        public static readonly string rc_escape;
        public static string Q1(string s) => $"({s}+[])";
        public static string Q2(string s) => $"[{s}]";
        public static string Q3(string s) => $"+({s}+[])";
        public static string Q4(string s) => $"+{s}";
        public static string Q5(string s) => $"({s})";
        public static string ToScript(string script) => $"{function}({ToString(script)})()";
        public static string ToUnescape(int charCode) => $"{rc_unescape}({ToString("%" + ToHex(charCode, 2))})";
        public static string ToHexs(int charCode) => ToString($"\\x{ToHex(charCode, 2)}");
        public static string ToUnicode(int charCode) => ToString($"\\u{ToHex(charCode, 4)}");
        public static string ToHex(int number, int digitCount)
        {
            var hex = number.ToString("x");
            while (hex.Length < digitCount)
                hex = $"0{hex}";
            return hex;
        }
        public static string ToChar(char ch)
        {
            int charCode = ch;
            string unis, unes, hexs;
            if (characterMap.TryGetValue(ch, out string? value))
                return value;

            if ((ch == '\\') || (ch == 'x'))
            {
                characterMap[ch] = ToUnescape(charCode);
                return characterMap[ch];
            }
            unis = ToUnicode(charCode);
            if (charCode < 128)
            {
                unes = ToUnescape(charCode);
                if (unis.Length > unes.Length)
                    unis = unes;
                hexs = ToHexs(charCode);
                if (unis.Length > hexs.Length)
                    unis = hexs;
            }
            characterMap[ch] = unis;
            return unis;
        }

        public static string ToString(string text)
        {
            var result = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                if (i > 0)
                    result += '+';
                result += ToChar(text[i]);
            }
            return result;
        }

    }

}
