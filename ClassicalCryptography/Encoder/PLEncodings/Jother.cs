namespace ClassicalCryptography.Encoder.PLEncodings;

[ReferenceFrom("https://github.com/ezeeo/ctf-tools/blob/095808a84d34e7ebfa6bcbe063275da24563f092/Library/jotherencode/jother-1.0.rc.js", ProgramingLanguage.JavaScript)]
internal static class Jother
{
    private static readonly Dictionary<char, string> characterMap;
    private static readonly string function;
    private static readonly string localString;
    private static readonly string invokeUnescape;
    private static readonly string invokeEscape;

    static Jother()
    {
        characterMap = new()
            {
                {'0', "(+[]+[])" },
                {'1', "(+!![]+[])" },
                {'2', "(!![]+!![]+[])" },
                {'3', "(!![]+!![]+!![]+!![]+[])" },
                {'4', "(!![]+!![]+!![]+!![]+!![]+[])" },
                {'5', "(!![]+!![]+!![]+!![]+!![]+[])" },
                {'6', "(!![]+!![]+!![]+!![]+!![]+!![]+[])" },
                {'7', "(!![]+!![]+!![]+!![]+!![]+!![]+!![]+[])" },
                {'8', "(!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+[])" },
                {'9', "(!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+[])" },
                {'a', "(![]+[])[+!![]]" },
                {'b', "({}+[])[!![]+!![]]" },
                {'c', "({}+[])[!![]+!![]+!![]+!![]+!![]]" },
                {'d', "({}[[]]+[])[!![]+!![]]" },
                {'e', "(!![]+[])[!![]+!![]+!![]]" },
                {'f', "(![]+[])[+[]]" },
                {'i', "({}[[]]+[])[!![]+!![]+!![]+!![]+!![]]" },
                {'j', "({}+[])[!![]+!![]+!![]]" },
                {'l', "(![]+[])[!![]+!![]]" },
                {'n', "({}[[]]+[])[+!![]]" },
                {'o', "({}+[])[+!![]]" },
                {'r', "(!![]+[])[+!![]]" },
                {'s', "(![]+[])[!![]+!![]+!![]]" },
                {'t', "(!![]+[])[+[]]" },
                {'u', "({}[[]]+[])[+[]]" },
                {'y', "((+(+!![]+(!![]+[])[!![]+!![]+!![]]+(+!![]+[])+(+[]+[])+(+[]+[])+(+[]+[])+[])+[])+[])[!![]+!![]+!![]+!![]+!![]+!![]+!![]]" },
                {'I', "((+(+!![]+(!![]+[])[!![]+!![]+!![]]+(+!![]+[])+(+[]+[])+(+[]+[])+(+[]+[])+[])+[])+[])[+[]]" },
                {'N', "(+{}+[])[+[]]" },
                {'O', "({}+[])[!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]]" },
                {' ', "({}+[])[!![]+!![]+!![]+!![]+!![]+!![]+!![]]" },
                {'[', "({}+[])[+[]]" },
                {']', "({}+[])[!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]+!![]]" },
                {'-', "(~[]+[])[+[]]" },
                {'+', "(+(+!![]+(!![]+[])[!![]+!![]+!![]]+(+!![]+[])+(+[]+[])+(+[]+[])+[])+[])[!![]+!![]]" }//1e+100
            };

        localString = Resources.JotherLocalString;
        function = Resources.JotherFunction;
        invokeUnescape = ToScript("return unescape");
        invokeEscape = ToScript("return escape");

        characterMap['h'] = $"({localString})[+[]]";
        characterMap['p'] = $"({localString})[!![]+!![]+!![]]";
        characterMap[':'] = $"({localString})[!![]+!![]+!![]+!![]+!![]]";
        characterMap['/'] = $"({localString})[!![]+!![]+!![]+!![]+!![]+!![]]";
        characterMap['%'] = $"{invokeEscape}({ToString("[")}){"[+[]]"}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToScript(string script) => $"{function}({ToString(script)})()";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToUnescape(int charCode) => $"{invokeUnescape}({ToString($"%{charCode:x2}")})";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToHexs(int charCode) => ToString($@"\x{charCode:x2}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToUnicode(int charCode) => ToString($@"\u{charCode:x4}");

    public static string GetChar(char character)
    {
        if (characterMap.TryGetValue(character, out string? value))
            return value;

        if ((character == '\\') || (character == 'x'))
            return characterMap[character] = ToUnescape(character);

        var codeText = ToUnicode(character);
        if (character < 0x80)
        {
            //原本这里是追求用更长的字符来表达相同的内容，我更改了条件
            var unescapeText = ToUnescape(character);
            if (codeText.Length <= unescapeText.Length)
                codeText = unescapeText;
            var hexText = ToHexs(character);
            if (codeText.Length <= hexText.Length)
                codeText = hexText;
        }
        return characterMap[character] = codeText;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(string text) => string.Join('+', text.Select(GetChar));
}
