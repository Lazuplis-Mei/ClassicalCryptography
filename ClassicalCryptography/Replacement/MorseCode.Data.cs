﻿namespace ClassicalCryptography.Replacement;

public partial class MorseCode
{
    /// <summary>
    /// 英文(字母+数字+符号)
    /// </summary>
    public static readonly MorseCode English = new(new()
    {
        { 'A', ".-" },
        { 'B', "-..." },
        { 'C', "-.-." },
        { 'D', "-.."},
        { 'E', "." },
        { 'F', "..-." },
        { 'G', "--." },
        { 'H', "...." },
        { 'I', ".." },
        { 'J', ".---" },
        { 'K', "-.-" },
        { 'L', ".-.." },
        { 'M', "--" },
        { 'N', "-." },
        { 'O', "---" },
        { 'P', ".--." },
        { 'Q', "--.-" },
        { 'R', ".-." },
        { 'S', "..." },
        { 'T', "-" },
        { 'U', "..-" },
        { 'V', "...-" },
        { 'W', ".--"},
        { 'X', "-..-" },
        { 'Y', "-.--" },
        { 'Z', "--.." },

        { '1', ".----" },
        { '2', "..---" },
        { '3', "...--" },
        { '4', "....-" },
        { '5', "....." },
        { '6', "-...." },
        { '7', "--..." },
        { '8', "---.." },
        { '9', "----." },
        { '0', "-----" },

        { '.', ".-.-.-" },
        { ',', "--..--" },
        { '?', "..--.." },
        { ';', "-.-.-." },
        { ':', "---..." },
        { '/', "-..-."},
        { '\'', ".----." },
        { '\"', ".-..-." },

        { '_', "..--.-" },
        { '+', ".-.-." },
        { '-', "-....-" },
        { '=', "-...-" },
        { ')', "-.--.-" },
        { '(', "-.--." },
        { '$', "...-..-" },
        { '¿', "..-.-" },
        { '¡', "--...-" },
        { '&', ".-..." },
        { '@', ".--.-." },
});

    /// <summary>
    /// 数字短码
    /// </summary>
    public static readonly MorseCode ShortDigit = new(new()
    {
        { '1', ".-" },
        { '2', "..-" },
        { '3', "...-" },
        { '4', "....-" },
        { '5', "....." },
        { '6', "-...." },
        { '7', "-..." },
        { '8', "-.." },
        { '9', "-." },
        { '0', "-" },
    });

    /// <summary>
    /// 注音符号
    /// </summary>
    public static readonly MorseCode ChineseZhuyin = new(new()
    {
        { 'ㄅ', "-..." },
        { 'ㄆ', ".--.." },
        { 'ㄇ', ".--." },
        { 'ㄈ', "..-.-" },
        { 'ㄉ', ".--" },
        { 'ㄊ', "--.-" },
        { 'ㄋ', "---." },
        { 'ㄌ', "..." },
        { 'ㄍ', "-..-" },
        { 'ㄎ', "--..-" },
        { 'ㄏ', "..--" },
        { 'ㄐ', "-.." },
        { 'ㄑ', ".-..-" },
        { 'ㄒ', "..--." },
        { 'ㄓ', "...." },
        { 'ㄔ', "-.-." },
        { 'ㄕ', ".-." },
        { 'ㄖ', ".---" },
        { 'ㄗ', "...-" },
        { 'ㄘ', "-.-.." },
        { 'ㄙ', "-.-" },
        { 'ㄧ', "." },
        { 'ㄨ', "-" },
        { 'ㄩ', "..-" },
        { 'ㄚ', "---" },
        { 'ㄛ', "--" },
        { 'ㄜ', "-.--" },
        { 'ㄝ', "--.." },
        { 'ㄞ', ".-.-" },
        { 'ㄟ', "..-.." },
        { 'ㄠ', ".-.." },
        { 'ㄡ', "..-." },
        { 'ㄢ', ".-" },
        { 'ㄣ', "-." },
        { 'ㄤ', "--." },
        { 'ㄥ', ".." },
        { 'ㄦ', ".---." },
    });

    /// <summary>
    /// 日语
    /// </summary>
    public static readonly MorseCode Japanese = new(new()
    {
        { 'ア' , "--.--" },
        { 'カ' , ".-.." },
        { 'サ' , "-.-.-" },
        { 'タ' , "-." },
        { 'ナ' , ".-." },
        { 'ハ' , "-..." },
        { 'マ' , "-..-" },
        { 'ヤ' , ".--" },
        { 'ラ' , "..." },
        { 'ワ' , "-.-" },
        { 'イ' , ".-" },
        { 'キ' , "-.-.." },
        { 'シ' , "--.-." },
        { 'チ' , "..-." },
        { 'ニ' , "-.-." },
        { 'ヒ' , "--..-" },
        { 'ミ' , "..-.-" },
        { 'リ' , "--." },
        { 'ヰ' , ".-..-" },
        { 'ウ' , "..-" },
        { 'ク' , "...-" },
        { 'ス' , "---.-" },
        { 'ツ' , ".--." },
        { 'ヌ' , "...." },
        { 'フ' , "--.." },
        { 'ム' , "-" },
        { 'ユ' , "-..--" },
        { 'ル' , "-.--." },
        { 'ン' , ".-.-." },
        { 'エ' , "-.---" },
        { 'ケ' , "-.--" },
        { 'セ' , ".---." },
        { 'テ' , ".-.--" },
        { 'ネ' , "--.-" },
        { 'ヘ' , "." },
        { 'メ' , "-...-" },
        { 'レ' , "---" },
        { 'ヱ' , ".--.." },
        { 'オ' , ".-..." },
        { 'コ' , "----" },
        { 'ソ' , "---." },
        { 'ト' , "..-.." },
        { 'ノ' , "..--" },
        { 'ホ' , "-.." },
        { 'モ' , "-..-." },
        { 'ヨ' , "--" },
        { 'ロ' , ".-.-" },
        { 'ヲ' , ".---" },
        { '゛' , ".." },
        { '゜' , "..--." },

        { 'ー' , ".--.-" },
        { '、' , ".-.-.-" },
        { '」' , ".-.-.." },
        { '（' , "-.--.-" },
        { '）' , ".-..-." },
    });
}
