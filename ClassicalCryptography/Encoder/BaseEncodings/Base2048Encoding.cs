namespace ClassicalCryptography.Encoder.BaseEncodings;

/// <summary>
/// from <see href="https://github.com/qntm/base2048"/>
/// </summary>
public static class Base2048Encoding
{
    const int BITS_PER_CHAR = 11; // Base2048 is an 11-bit encoding
    const int BITS_PER_BYTE = 8;

    private static readonly string[] pairStrings =
    {
        "89AZazÆÆÐÐØØÞßææððøøþþĐđĦħııĸĸŁłŊŋŒœŦŧƀƟƢƮƱǃǝǝǤǥǶǷȜȝȠȥȴʯͰͳͶͷ" +
            "ͻͽͿͿΑΡΣΩαωϏϏϗϯϳϳϷϸϺϿЂЂЄІЈЋЏИКикяђђєіјћџѵѸҁҊӀӃӏӔӕӘәӠӡӨөӶӷ" +
            "ӺԯԱՖաֆאתװײؠءاؿفي٠٩ٮٯٱٴٹڿہہۃےەەۮۼۿۿܐܐܒܯݍޥޱޱ߀ߪࠀࠕࡀࡘࡠࡪࢠࢴࢶࢽऄ" +
            "नपरलळवहऽऽॐॐॠॡ०९ॲঀঅঌএঐওনপরললশহঽঽৎৎৠৡ০ৱ৴৹ৼৼਅਊਏਐਓਨ" +
            "ਪਰਲਲਵਵਸਹੜੜ੦੯ੲੴઅઍએઑઓનપરલળવહઽઽૐૐૠૡ૦૯ૹૹଅଌଏଐଓନପର" +
            "ଲଳଵହଽଽୟୡ୦୯ୱ୷ஃஃஅஊஎஐஒஓககஙசஜஜஞடணதநபமஹௐௐ௦௲" +
            "అఌఎఐఒనపహఽఽౘౚౠౡ౦౯౸౾ಀಀಅಌಎಐಒನಪಳವಹಽಽೞೞೠೡ೦" +
            "೯ೱೲഅഌഎഐഒഺഽഽൎൎൔൖ൘ൡ൦൸ൺൿඅඖකනඳරලලවෆ෦෯" +
            "กะาาเๅ๐๙ກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອະາາຽຽເໄ໐໙ໞໟༀༀ༠༳ཀགངཇཉཌཎདནབམཛཝཨཪཬྈ" +
            "ྌကဥဧဪဿ၉ၐၕ",
        "07"
    };
    private static readonly BaseXXXXEncoding base2048 = new(BITS_PER_CHAR, BITS_PER_BYTE, pairStrings);

    /// <summary>
    /// encode Base2048
    /// </summary>
    public static string Encode(byte[] uint8Array) => base2048.Encode(uint8Array);



    /// <summary>
    /// decode Base2048
    /// </summary>
    public static byte[] Decode(string str) => base2048.Decode(str);
}