﻿namespace ClassicalCryptography.Encoder.PLEncodings;

internal static partial class Constants
{

    public static readonly string[] jjCodes = new[]
    {
        "___", "__$", "_$_", "_$$", "$__", "$_$", "$$_", "$$$",
        "$___", "$__$", "$_$_", "$_$$", "$$__", "$$_$", "$$$_", "$$$$"
    };

    public static readonly string[] aaCodes = new[]
    {
        "(c^_^o)",
        "(ﾟΘﾟ)",
        "((o^_^o) - (ﾟΘﾟ))",
        "(o^_^o)",
        "(ﾟｰﾟ)",
        "((ﾟｰﾟ) + (ﾟΘﾟ))",
        "((o^_^o) +(o^_^o))",
        "((ﾟｰﾟ) + (o^_^o))",
        "((ﾟｰﾟ) + (ﾟｰﾟ))",
        "((ﾟｰﾟ) + (ﾟｰﾟ) + (ﾟΘﾟ))",
        "(ﾟДﾟ) .ﾟωﾟﾉ",
        "(ﾟДﾟ) .ﾟΘﾟﾉ",
        "(ﾟДﾟ) ['c']",
        "(ﾟДﾟ) .ﾟｰﾟﾉ",
        "(ﾟДﾟ) .ﾟДﾟﾉ",
        "(ﾟДﾟ) [ﾟΘﾟ]"
    };

    public static readonly string[] ppWords = new[]
    {
        "abs", "accept", "alarm", "and", "bind", "binmode", "bless", "caller",
        "chdir", "chmod", "chomp", "chop", "chown", "chr", "chroot", "close",
        "closedir", "cmp", "connect", "continue", "cos", "crypt", "dbmclose",
        "dbmopen", "defined", "delete", "die", "do", "dump", "each", "else",
        "elsif", "endgrent", "endhostent", "endnetent", "endprotoent", "endpwent",
        "endservent", "eof", "eq", "eval", "exec", "exists", "exit", "exp", "fcntl",
        "fileno", "flock", "for", "foreach", "fork", "formline", "ge", "getc",
        "getgrent", "getgrgid", "getgrnam", "gethostbyaddr", "gethostbyname",
        "gethostent", "getlogin", "getnetbyaddr", "getnetbyname", "getnetent",
        "getpeername", "getpgrp", "getppid", "getpriority", "getprotobyname",
        "getprotobynumber", "getprotoent", "getpwent", "getpwnam", "getpwuid",
        "getservbyname", "getservbyport", "getservent", "getsockname", "getsockopt",
        "glob", "gmtime", "goto", "grep", "gt", "hex", "if", "import", "index",
        "int", "ioctl", "join", "keys", "kill", "last", "lc", "lcfirst", "le",
        "length", "link", "listen", "local", "localtime", "log", "lstat", "lt",
        "m", "map", "mkdir", "msgctl", "msgget", "msgrcv", "msgsnd", "my", "ne",
        "next", "no", "not", "oct", "open", "opendir", "or", "ord", "pack", "pipe",
        "pop", "pos", "print", "printf", "push", "q", "qq", "qr", "quotemeta", "qw",
        "qx", "rand", "read", "readdir", "readlink", "recv", "redo", "ref",
        "rename", "require", "reset", "return", "reverse", "rewinddir", "rindex",
        "rmdir", "s", "scalar", "seek", "seekdir", "select", "semctl", "semget",
        "semop", "send", "setgrent", "sethostent", "setnetent", "setpgrp",
        "setpriority", "setprotoent", "setpwent", "setservent", "setsockopt",
        "shift", "shmctl", "shmget", "shmread", "shmwrite", "shutdown", "sin",
        "sleep", "socket", "socketpair", "sort", "splice", "split", "sprintf",
        "sqrt", "srand", "stat", "study", "substr", "symlink", "syscall", "sysread",
        "system", "syswrite", "tell", "telldir", "tie", "time", "times", "tr",
        "truncate", "uc", "ucfirst", "umask", "undef", "unlink", "unpack", "unshift",
        "untie", "use", "utime", "values", "vec", "wait", "waitpid", "wantarray",
        "warn", "while", "write", "x", "xor", "y" };

    //但它就是这样......
    public static readonly string[,] ppCodes = new string[,]
    {
        {"8182","8282","798182"},
        {"75755479817C","75755479811E","61817C"},
        {"61811E","61817069","61818127"},
        {"618164","618195","61812734"},
        {"61810D5E","61810DC7","6181115E"},
        {"618189","61810B5E","618118"},
        {"6181085E","618108C7","6181095E"},
        {"6181044F","618193","6181075E"},
        {"6181064F","618107D2","61810829"},
        {"61810729","61810735","61810E29"},
        {"6181054F","618110D2","618113D2"},
        {"61810084","61810384","61811029"},
        {"61811609","618116B7","6181176F"},
        {"6181166F","61811742","618117AB"},
        {"61810184","61811642","618116AB"},
        {"75755479816B69","61811649","61813966"},
        {"61814466","61818739","61818839"},
        {"61818AB1","61818E3D","61819039"},
        {"61814566","75757579816902","75757579818127"},
        {"61818A3D","618181392681","6181813A7C81"},
        {"61818A39","6181817662C681","618181196ABD81"},
        {"618181A8AB0681","61818106658D81","6181819BA20B81"},
        {"757579816902","757579818127","61818174284D81"},
        {"61818120D1B981","618181B84581","618181751D017E81"},
        {"618181510EA681","61818170804481","618181AF0ED2B481"},
        {"546182813B0781","54618181392681","546181813A7C81"},
        {"7579816902","7579818127","618181D648903281"},
        {"6181D99203688331D9","61818174253B81","618181ABABA181"},
        {"618181B991641881","618181B7AF92B381","61818172634481"},
        {"618181D54C5081","6181D9A9B91F5681D9","61818149D9B96B5A81"},
        {"61818176B9663981","6181813512A9D37981","6181811CB20E35BE81"},
        {"6181D90282D5A0D9","6181DB6A728982739ADB","61818151C69A1381"},
        {"79818181","79828181","79818127"},
        {"54618181A8AB0681","5461818106658D81","546181819BA20B81"},
        {"5461818174284D81","5461828144D781","6181813435437FB481"},
        {"75757579C7816A69","75757579C7811D71","75757579C7811D34"},
        {"75757579C785810481","75757579C785810581","75757579C785810681"},
        {"75757579C7815E28","75757579C781751B","75757579C7817573"},
        {"75757579C785813081","75757579C785813281","75757579C785812D81"},
        {"75757579C785813481","75757579C785815381","75757579C785814F81"},
        {"75757579C7811E","755479816902","755479818127"},
        {"75757579C781115E","75757579C785816981","75757579C78195"},
        {"7575757985811C81","7575757985811B81","7575757985811A81"},
        {"757579C7816A69","757579C7811D60","757579C7811D71"},
        {"757579C785810481","7575757985813081","757579C785810681"},
        {"757579C7815E28","757579C781751B","757579C7817573"},
        {"757579C785813081","757579C785813281","757579C785812D81"},
        {"757579C785813481","757579C785815381","75757579816B69"},
        {"75757579817673","7575757981B496","7575757981761B"},
        {"75757579812760","75757579812771","75757579812734"},
        {"757579C7811E","5479816902","5479818127"},
        {"757579C785816981","757579C781115E","757579C78195"},
        {"75757985811C81","75757985811B81","75757985811A81"},
        {"7579C7816A69","7579C7811D60","7579C7811D71"},
        {"7579C785810481","7579C785810681","75757985813081"},
        {"7579C7815E28","7579C781751B","7579C7817573"},
        {"7579C785813081","7579C785813281","7579C785812D81"},
        {"7579C785813481","7579C785815381","757579816B69"},
        {"7579C7810DC7","7579C7810BC7","7579C7810D5E"},
        {"7575798164","7579C781AE96","757579817C"},
        {"757579817673","75757981B496","75757981761B"},
        {"757579812760","757579812734","757579812771"},
        {"7579C7811E","757579818381","757579817873"},
        {"7579C78195","7579C781115E","7579C785816981"},
        {"757985811C81","757985811B81","757985811A81"},
        {"79C7816A69","79C7811D60","79C7811D71"},
        {"79C785810481","79C785810681","757985813081"},
        {"79C7815E28","79C781751B","79C7817573"},
        {"7579810D5E","7579810DC7","79C785811B81"},
        {"79C7813466","79C78171AE","79C7818D78"},
        {"79C785813281","79C785813081","79C785812F81"},
        {"79C785813481","79C785815381","7579816B69"},
        {"79C7810DC7","79C7810BC7","79C7810D5E"},
        {"75798164","79C781AE96","7579817C"},
        {"7579817673","757981B496","757981761B"},
        {"7579812760","7579812734","7579812771"},
        {"79C7811E","7579818381","7579817873"},
        {"79C78195","79C781115E","79C785816981"},
        {"7579815366","79C785817381","79C785817181"},
        {"79C7817C","79C78164","79C781DAD9"},
        {"7579818627","7579818681","7579812C60"},
        {"79C7812760","79C7812771","79C7812734"},
        {"79C7818381","79C7817873","79C7818327"},
        {"79C785819681","79C78581AE81","79C7816C69"},
        {"79C781B896","79C7816828","79C7815366"},
        {"79C78581C781","79C78581CF81","79C7811C03"},
        {"79C7812860","79C7812834","79C7812871"},
        {"79C7818581","79C78581D381","79C7818527"},
        {"79C7818627","79C7818681","79C7812C60"},
        {"79C7817002","79C78581DB81","79C7817069"},
        {"755479C781B896","755479C7816828","755479C7815366"},
        {"755479C78581C781","755479C78581CF81","755479C7811C03"},
        {"755479C7812860","755479C7812834","755479C7812871"},
        {"755479C7818581","755479C78581D381","755479C7818527"},
        {"755479C7818627","755479C7818681","755479C7812C60"},
        {"755479C7817069","755479C78581DB81","755479C7817002"},
        {"61818111B35D3F9A9A981A108C9A965C1901AE81","6181812CCACC200A3D6D5799534A90D881","6181D9B818492699A9AFD5228081694DBAD9"},
        {"79816A69","79811D60","79811D71"},
        {"7985810481","79811703","7985810681"},
        {"79815E28","7981751B","79817573"},
        {"7985811C81","7985811B81","7985811A81"},
        {"79813466","798171AE","79818D78"},
        {"7985813081","7985813281","7985812D81"},
        {"7985813481","7985815381","7985814F81"},
        {"79810DC7","79810BC7","79810D5E"},
        {"7981AE96","7981C253","7981C268"},
        {"75547985810481","7985815A81","75547985810681"},
        {"7985815B81","7985815C81","79816B69"},
        {"79811E","7985816081","7985815E81"},
        {"7981115E","7985816981","798195"},
        {"7985817481","7985817181","7985817381"},
        {"79817C","798164","7981DAD9"},
        {"7981761B","7981B496","79817673"},
        {"79812760","79812771","79812734"},
        {"79818381","79817873","79818327"},
        {"798581AE81","79816C69","7985819681"},
        {"7981B896","79816828","79815366"},
        {"798581CF81","798581C781","79811C03"},
        {"79812834","79812860","79812871"},
        {"79818581","798581D681","79818527"},
        {"79818627","79818681","798581D981"},
        {"798581DB81","79817002","79817069"},
        {"618181B3B74AA171902331979314A1741E4481","6181D98EA0B77A7BA5BC1E27B4AF25858FBB715B798FD9","618181468D4B9759375C2B482574729D079F1281"},
        {"618181D8352CB4C710D76C5F192E0BD7BD9E95781C015DAE81","618181B09410389B6BC28A91D6804748524BA181","61818189340B62BC0E053061A617C73E488EB8B97A81"},
        {"7575547985811C81","7575547985811B81","7575547985811A81"},
        {"75755479813466","757554798171AE","75755479818D78"},
        {"75755479810DC7","75755479810BC7","75755479810D5E"},
        {"7575547981AE96","7575547981C253","7575547981C268"},
        {"5479C785817A81","5479C785817B81","5479C785817D81"},
        {"5479C7812760","5479C7812771","5479C7812734"},
        {"5479C7818381","5479C7817873","5479C7818327"},
        {"5479C785819681","5479C78581AE81","5479C7816C69"},
        {"5479C781B896","5479C7816828","5479C7815366"},
        {"5479C78581C781","5479C78581CF81","5479C7811C03"},
        {"5479C7812860","5479C7812834","5479C7812871"},
        {"5479C7818581","5479C78581D381","5479C7818527"},
        {"5479C7818627","5479C7818681","5479C7812C60"},
        {"5479C7817069","5479C78581DB81","5479C7817002"},
        {"61818109CC1913AD90361A4C34C916873159DABEAA901881","6181DB6EA6623BAF9C73B6C57ECB288C8513B76ADA31565DAF79DB","618181A6D5AECE246FC67D1EAA1CC4B1D0C65107220881"},
        {"6181D93BA7BB2DB98174444444A06D9D23115DD9","6181813C798B9F902A412C3EA841A0D63CACD7C04A81","6181D934A1257A5245B71892AAC5469DC7820D95638EBED9"},
        {"6181815C2156A1C91C8AC3B564CF9932681023D96318B87481","618181040B380353725762034F12591425A55C0C1507BE62D95F7D81","618181A4002317D4BC880BCF1191381436919A753888B581"},
        {"61818135BDB0AC4DAC80C54AABD1139851AFBB01180B81","618181BDC0B85851B7B895DBBD4899C223308E314E762F28B381","61818194C84AD6AA4EBE7188B041928C0A7A2275A9A89A81"},
        {"618181BD36023291B44D0C80DB6D013F038A56B2599A2C1381","618181CA65C64F260A4F69C4020CB50897AD0EBA11CB6BBEBC81","6181D900B35ED31E169BBF7A1F4E816AD49B53B133233AD9"},
        {"61818198B15FC910399CA68E9F3BABD53E0BD481","6181814EB841C47E65686C73B54763339E424F63A769AE9281","618181DBA6751F8A4E9E13BE8E2106D791894EC4A62881"},
        {"6181D903791D3B4C99383D0F91BC41813FA269378ACB96D9","618181755BCAC808DB996D026945AACA2132A9D4090458052981","6181D98527213EB77D0C4E0B0E796FCF9AC0694B14300E3F701BD9"},
        {"6181813E742A7C1E648B766B2E23D03155A128BBC6A70A5481","618181CB89971E03AD6D0F5A1D66391B5D662FA8CFAF096C38115381","6181D9BB702E85B884437D0DD8BF46CDB47F9D7E1602CD51D9"},
        {"618181977D563DA105A47B501E06BA06AD656A259F0B81","6181D99626630F3B9D15A406272FA6441C9913A17A88C1D9","6181814C8090A0342E3747B88A1CC2A9647106A8199A904CDB81"},
        {"61818150392ECF05453077C70016353159CB5A929512A281","6181810DCA750732C68C164A3F46605A19A33C13DA010B81","6181D9AB83611A89628A91B31DA4D444360AA0288F38D9"},
        {"6181D9710AA5B924482110852FD2D5B823CF468EC13AD9","6181D947A2BD3D69892084D888AC4B38115FC16BB89DD9","618181CABD646D25670FC58BCE67145AC85E2A4DB7254C15C281"},
        {"6181D91427476A4DB73956267DAC882FD313398A8D599F5CD9","618181419A6FA553D578BFBD265D72D7D573B9B11350A3A881","618181366F32BA3D8996926880938A39BDD82E2B4D9381"},
        {"546181D9B818492699A9AFD5228081694DBAD9","5461818111B35D3F9A9A981A108C9A965C1901AE81","546181812CCACC200A3D6D5799534A90D881"},
        {"5479816A69","5479811D60","5479811D71"},
        {"547985810481","5479811703","547985810681"},
        {"5479815E28","547981751B","5479817573"},
        {"6181DBB6C22E29729F0075385C8B53DA037DC0117DC77136A193ADAEB49FDB","61818116737228516148B4095189D77E313F87C44D53A69AA781","6181810E179B2CCF33B0536539948CA22DB7524D5E36D6B00E81"},
        {"6181D94FC81CBA217B9F6E81CB090E138A01B8062D7BC5DBCD36D0D9","6181813C11360FC5350D68A942C79B54B27C6D98BF3D07D0BA289B0781","6181D966DB85B48996230FB3184E8249259CA88D269C485FA9A296D9"},
        {"6181DB5182D9D108044D1A300B820F2C09772A969B1CD7C4C597325E2AB1961ADB","6181810794CE9DA1D7BD491BD4A32BC3C64BBA6F764A0181","6181816D99BC401453CC0CC0AD7B4654D9C62064C65CBD525AA981"},
        {"6181D9A3747FA71A1232981D373F4645008255271E2EAE3CA5D9","618181906DB3B01795219EBCD55749B32AD441AC1D655A0781","6181818C5799720B713D00A54413497A5A78A969BF991BB9D74E81"},
        {"6181D9334CBF9E55A8C407A0A83F39D5AA83BFD5D1757C7ED9","6181811B55AF3B5E93CB4B9D970BCA6E7E36D6B7AAD0465D2E9081","618181BCA5370A9F392A699661C4492DA7C29F485C11C2C2BCB881"},
        {"6181813DD6A780A81E19C010B88A0E8EAA80877DA8C52B1A8B7AD40181","6181D93F891DCE5D03387AD2381BB37D31B7B353C9BC2E85745C3E10D9","6181DB1B6C80AFCE024919BAD950C772939E27148D1113D19F954F11C910DB"},
        {"618181A6BB263B8E5A44C349D417377F48374EB08E9E8D81","618181B5030B18C97DCCB4C02C446D6C04BE7BAAAE80A7A8D6922B8B81","6181818D12B4409CA8AB5860631187DB80D5C0D2B4D0BE4470529B763581"},
        {"6181810F33984A2452455AD846A3C94ACC0168A0198C81","618181D40B4D893C6932BDBC147896389C05D438397769AE8AB581","61818170AD8B00B336D261A346575FD1A513D7A4337F0CAD81"},
        {"6181810465313E9C227047C2A830A17A679278393B760C597781","618181ADCF03C272BA9497D44A7C554BC73C748C4A991E115B7A4781","61818165B707D48E6CAD3F7180C76F0AA410177D31731E40A481"},
        {"6181815E5C107D2ECC40B93655AC0AAAD6C8CE5A7A40713B15A58C81","6181818EAA55C1D9A0457DD52046880530D2787270D159161D477481","6181D9B78F16A8060C36248248B84E6FC4C2014022B4404ED9"},
        {"6181D9B77800D0551F6103B1851FC291912220443DCFCA6980936F52D9","6181D9848414645A38892E7B129544C30A673DA6A6837C87C4D9","618181BA5C7D5124BFCE41A68B23AE8855802D1A5505BC5F186028B09E81"},
        {"618181D23A727C61BE93B990660F42793096371BCB3D569AA844B981","61818142C6303C6066B5BF7812420C61243BB503D79D6C1ED37ECA8081","61818159236F4F7232D4560274224A2FD5136DAC6AB95E0A704381"},
        {"618181176D0D505C044F0DBB20AB49BE758E89254F0A7417B43BC25D81","6181810C0E1AC97868075480928CB13CAF68070EC9A9B1D8BABD1D644281","6181D956A7B06017469903927E9510934FCB8527C410340BC376836038D9"},
        {"6181D97B6E27C62F1C689C15536EC8B69A6898B4536F4406BFA14C3FD9","6181D9623FA412670417875A95719C891A270E8A71CA8BBD236CA6C5D9","618181D4733D0EDA79B3AF32221B2BCBB58EAF50669DC73D660D1DC472CE81"},
        {"618181D1804EDBBD66C3C76AC77557AE7925C2B5C9D04DC1B57E7A6EA10981","618181492E733EB85A45366176A475AA2D0F8D0C0EBAB0474381","618181D0C8B97A55D66C099C56C187CA8D7FD426D80F55C7ABBE081232649481"},
        {"61818102A83DB2397418035646B47CB746163A032D52C209C1C381","61818180BCCD7046AF8AD1247D00C0C9B4770ED87BCBAA33B2AD772B81","618181C1036B22269EAF4D16C5096C795865BD2C2AAED7B75E76C9D793D681"},
        {"6181D942209A3CB6D3B408B43C97184B1CC7D3A3AB4F6FCBD3D617D9","6181DBB14D87012C366377698150955CC0ABB426D97A58D3306F542BD3D98CC59771DB","6181D938AC0D96C1478DC76350A2698110438AB29888DB3C0CBD7725CED9"},
        {"6181810AA3A9D33D0DA923C329B106938E0758AB78D88E90513A81","6181D9953FA7531A5B712DAE815E0F8D0C0613876F2369556FBA746AA87C61CBBAD9","6181819B4E2C3BA34128DB44217F10C6D7161DC25A18C334883881"},
        {"6181813E3A458B5A055CB26D787CDAA26C7A01630DC179684EB17E81","6181D9CB431FA997500A5FB20AA82EC3B580A032B57127CB217359B69AD9","6181D9A1710271CF812283422D98438F4E8F1F348F951F6F6EA15B9810D9"},
        {"618181A8B73D0E955F516C21067601C315266F501D756EA5033F773781","6181813A9FB4A1D035A02290C1446CA92F3109D3CAD26476B2299681","6181812E5A353D8792882198236BAE0A37BD721FA70E457CB76381"},
        {"75547985811C81","75547985811B81","75547985811A81"},
        {"755479813466","7554798171AE","755479818D78"},
        {"755479810DC7","755479810BC7","755479810D5E"},
        {"75547981AE96","75547981C253","75547981C268"},
        {"75547985815A81","6181DB972F9C62A62699C04F7C3CAED2AA7E57A921CA1B8F1E6D57D925B2ABDB","6181D9A08585325B62B22893C61A37658767772D911849668710AB76C3ABD9"},
        {"75547985815B81","75547985815C81","755479816B69"},
        {"755479811E","75547985816081","75547985815E81"},
        {"75547981115E","75547985816981","7554798195"},
        {"6181D9AE18ACA1004CACA181D06A5E3D1891B661D4922FB51001C74785D9","6181D90D08D4B623451A3E66240DA6BC6AC21C9C7C06844944AAC3D9","6181DB9466CA959C22495F33D47397C5D78F37C919A81C4E567F3C72D4DB"},
        {"6181DB24C22F65608B4C9D3B82477C50557861DA10B56B738191AF511A9F177280DB","61818164141F34519FC1C276AB3D22AA6A3326BA888A0F6A1F88AF03CB750EAA81","618181627E7303DAB24699CE4B77BB3936511CCE5752B7685E8B2C3C3F4781"},
        {"618181688C9CAC33A18A62C707C3C4B5A0B3C5C8AAD514BACE03DB068969519781","618181050D097ACAB762D1992D440575479F74906B465F05A49060BDCF81","6181812FD39279CB1F8DA01270332525BBC869AE7E3BD72AB5A51C1B64CC81"},
        {"6181D97E353BA906D36FC8C00EDB05D127B0747E21881CB5920D6E7EBEC735B1D9","6181D9897E9CD4B327894C5BA2DB9361B799CC6F469A2DC90E52B9ACC1D9","6181D9233A1739C98F8D991888BC4761B9D73A931EA33832AFD9"},
        {"75547985817481","75547985817181","75547985817381"},
        {"755479817C","7554798164","75547981DAD9"},
        {"75547981761B","755479817673","75547981B496"},
        {"755479812760","755479812771","755479812734"},
        {"755479818381","755479817873","755479818327"},
        {"75547985819681","7554798581AE81","755479816C69"},
        {"618181569864B0719337D7DA1A7752663B9A240C49B3553870B3235B153CD581","6181813591D1AF7978BB512A143A984AC17AC3702113086340648A4E260881","6181D94348C3963FA91645C45ACA44833779C18A646B253A79665BD3D9"},
        {"6181D9CCA79BA6A46946AC8A45427A812E52A58B8356CA390DCAC4BDD9","61818141490229AAC1AE684BD60479CF4760182F723DC7030C500F9C11DA0C5756D781","618181690B9B6BBE312A1687AE407E2A069BBD227F119F71D3916A925FBBCBA881"},
        {"618181AB461FB93AB11B93D8541C936238B456A54B454E36576281","6181816048BEAA6E605087701F0A2428C9241A6F039E8EDA5AB5C62CAFA46E6C00B581","618181A68835BE9B137BA27B18911024C34307247C37346F5CB98EC41460B41E81"},
        {"6181D9A91F6E7F8E9F882751BD936A8BCD9843988999C94A315AC40737D826D9","6181D97E211DC27469AA88CF99B60E6842C2B999BFBE8D877C369EC4871EB0A1B46881D351D9","6181D9B45A6C2087CF198EC232BDC9835C80D0811C8F7B1FA1876E4B7E404214C47CD9"},
        {"6181D9663007D49B5B429E5C62651B818B401C05190D56BF7CDBD82E5B3A75249ADB4FD9","6181D961328C4952956C3D70AB5D90640B3C0D651B840B956E5E6652920F894F3F81D9","618181341CC73A5F9892C7058CBD80D1904F6CB2C567C88B246D2FD15D5672D6BC9B6981"},
        {"6181817E21BE5368CCB9353C0F9D010332902E50075466AF9BC04B704737C86FBC81","618181729E8C919D37B91A9879444AC1486AC06C962F19D8D5C31B14954AC98A81","6181812A4D665AD99F735298A62C47CE76B3155BD62A050EBC87B390A53BAA2581"},
        {"6181D9D34148BB997F314A823F6A786E61C5170D15C5643D9C0E5C61875A74B7CBD9","6181814726371B9E2F909C56477A6B196B7D505BBA89014C6C28537F4D28CF427C7C81","618181969A5A2CCB644DBC3762C07DDA4C6E874DCF23B851C7D9CD2B7F0DB4177CAE1981"},
        {"61818158A318C1442674C7141C1AA2CA4B4F4E0CCCB0D6B56A4C348A3A550881","618181066A9B7FCF888AD4CC61204C2F7544A9871307565E5D400A4E3CB581","61818135BDBA596C5D1F989E96C8932B4324A3BF4C7134B30860A2A05906D2021B81"},
        {"6181D94A7BBD9F3A6B1289251A9B7BC18C95956E7679ACAAA09EA4519C82D9","6181818E5EA2047B3547150041C59017B24BBA6D29331E880B3A347FC9CA179E760681","6181814D0CB91AD04D543FBA0E0C9AA09C263DAB629771DA3A19ACD6A7D281"},
        {"6181D9208426CEC3A904BE33B33FB9C1526000584191CD046A1A1AC90709A551D7305171D9","61818171B27B6B49BCAB0D0C10CE5125630FA7ACA828B4C241194B79C28AAEA081","6181812DB96642293C6FA0906E4979B08A0D8BD253BB04BE5D70451F17D749D85981"},
        {"755461818109CC1913AD90361A4C34C916873159DABEAA901881","75546181DB6EA6623BAF9C73B6C57ECB288C8513B76ADA31565DAF79DB","7554618181A6D5AECE246FC67D1EAA1CC4B1D0C65107220881"},
        {"75546181813C798B9F902A412C3EA841A0D63CACD7C04A81","75546181D93BA7BB2DB98174444444A06D9D23115DD9","75546181D934A1257A5245B71892AAC5469DC7820D95638EBED9"},
        {"6181D9BF3B38221EC4BAB26555D5839D6CB61BCD1A30CE2D2E0D6ECE9A901436BFA9D9","6181811C65CA714AD65F49246E2DA91E2445A946667B6F162172311F5C7D81","618181BFDA9A1B55D86F1614772515167F7E21B00E080DCBA9A53E87423E6381"},
        {"618181AE2C62D7D94B0B57074F577502871DA35863365E654267C828B35A249DAD5E4981","618181986D0D8B00609168AF58A41C890ED128A4CE290036BB52327EB0C9C6493EC881","6181D9C955163D17060D0EA02889059E356617A518856C0461C0B4A2AC84A767D9"},
        {"6181D9AFB550308E9B16C6CEB78D893103986F096D6381B4BA20C5AEBB5FC9ADAAAFC3D9","618181A14555D28B97133AD48964AA28D1C1253347618EA1B4969F1E6A8981","6181817157DB8D471B8A77806255D57A8CADBD6F32B4080C74664702B33D4C0ACBA81781"},
        {"618181304A629D905E47741F5F2A6BD58C2C7F60B756C8CC337A527A8E455D60ADB781","61818176963C4D3C653B66412F9F48C159D5472A0DA43552A1D50ABD8C0181","6181812899626B791A8A617A55410E925B36B18C1993AE4F77B18E6F6C390CDBAC81"},
        {"7554618181A4002317D4BC880BCF1191381436919A753888B581","75546181815C2156A1C91C8AC3B564CF9932681023D96318B87481","7554618181040B380353725762034F12591425A55C0C1507BE62D95F7D81"},
        {"755461818135BDB0AC4DAC80C54AABD1139851AFBB01180B81","7554618181BDC0B85851B7B895DBBD4899C223308E314E762F28B381","755461818194C84AD6AA4EBE7188B041928C0A7A2275A9A89A81"},
        {"7554618181CA65C64F260A4F69C4020CB50897AD0EBA11CB6BBEBC81","7554618181BD36023291B44D0C80DB6D013F038A56B2599A2C1381","75546181D900B35ED31E169BBF7A1F4E816AD49B53B133233AD9"},
        {"755461818198B15FC910399CA68E9F3BABD53E0BD481","75546181814EB841C47E65686C73B54763339E424F63A769AE9281","7554618181DBA6751F8A4E9E13BE8E2106D791894EC4A62881"},
        {"75546181D903791D3B4C99383D0F91BC41813FA269378ACB96D9","7554618181755BCAC808DB996D026945AACA2132A9D4090458052981","75546181D98527213EB77D0C4E0B0E796FCF9AC0694B14300E3F701BD9"},
        {"7554618181CB89971E03AD6D0F5A1D66391B5D662FA8CFAF096C38115381","75546181813E742A7C1E648B766B2E23D03155A128BBC6A70A5481","75546181D9BB702E85B884437D0DD8BF46CDB47F9D7E1602CD51D9"},
        {"7554618181977D563DA105A47B501E06BA06AD656A259F0B81","75546181D99626630F3B9D15A406272FA6441C9913A17A88C1D9","75546181814C8090A0342E3747B88A1CC2A9647106A8199A904CDB81"},
        {"755461818150392ECF05453077C70016353159CB5A929512A281","75546181810DCA750732C68C164A3F46605A19A33C13DA010B81","75546181D9AB83611A89628A91B31DA4D444360AA0288F38D9"},
        {"7575546181818E5EA2047B3547150041C59017B24BBA6D29331E880B3A347FC9CA179E760681","7575546181814D0CB91AD04D543FBA0E0C9AA09C263DAB629771DA3A19ACD6A7D281","7575546181D94A7BBD9F3A6B1289251A9B7BC18C95956E7679ACAAA09EA4519C82D9"},
        {"75755461818171B27B6B49BCAB0D0C10CE5125630FA7ACA828B4C241194B79C28AAEA081","7575546181812DB96642293C6FA0906E4979B08A0D8BD253BB04BE5D70451F17D749D85981","7575546181D9208426CEC3A904BE33B33FB9C1526000584191CD046A1A1AC90709A551D7305171D9"},
        {"7575546181D981401675424926699952905844D1003007D23A13038D2E891AA11A243AD9","757554618281D8B469A570040CC60B5AA75615ADCA772B5E61D41B9C7658CA204620472581","7575546181813D1A1678B3585AA31CD66F077D7C4791AAA929D630CB2002A638B489289181"},
        {"757554618181231C7705CFD3643E667AD83854C5CCB19896787DAE501B4CC5D01D95CB7345AF81","75755461828130143906699374D51831DBA589020A22340A1523D8D7702962CE9B03A1A91E81","61818194A29A3EADDA552F004E239816B37338A3D56DD200729E1D76C13B1B29732E9781"},
        {"7575546181D9BF3B38221EC4BAB26555D5839D6CB61BCD1A30CE2D2E0D6ECE9A901436BFA9D9","7575546181811C65CA714AD65F49246E2DA91E2445A946667B6F162172311F5C7D81","757554618181BFDA9A1B55D86F1614772515167F7E21B00E080DCBA9A53E87423E6381"},
        {"757554618181AE2C62D7D94B0B57074F577502871DA35863365E654267C828B35A249DAD5E4981","757554618181986D0D8B00609168AF58A41C890ED128A4CE290036BB52327EB0C9C6493EC881","7575546181D9C955163D17060D0EA02889059E356617A518856C0461C0B4A2AC84A767D9"},
        {"7554618181D163BA2963A6302D0AB40ACB6C01C8157F481369A2B381","75546182DB1D2F9CC32D9C4DC769722249CE1130417BC0481E878FDA76D2DB","7554618181CF4462DAC6AD106F6204463F6ACFC32FCD8C795978D1C581"},
        {"757554618181285BB32E22963AA7BE45712FB29658AE9FC90B183EC85D93370DC53EB981","6181814EAE2499911C05B44973CBAA9351621D73BF260553687B205E36D64116CD7567C417C781","7575546182D9135E9FB80B806D8708BD556A9860029585310C463A8F1499592E016C60B80C3C1181D9"},
        {"75546181810E179B2CCF33B0536539948CA22DB7524D5E36D6B00E81","75546181DBB6C22E29729F0075385C8B53DA037DC0117DC77136A193ADAEB49FDB","755461818116737228516148B4095189D77E313F87C44D53A69AA781"},
        {"75546181D94FC81CBA217B9F6E81CB090E138A01B8062D7BC5DBCD36D0D9","75546181813C11360FC5350D68A942C79B54B27C6D98BF3D07D0BA289B0781","75546181D966DB85B48996230FB3184E8249259CA88D269C485FA9A296D9"},
        {"75546181DB5182D9D108044D1A300B820F2C09772A969B1CD7C4C597325E2AB1961ADB","75546181810794CE9DA1D7BD491BD4A32BC3C64BBA6F764A0181","75546181816D99BC401453CC0CC0AD7B4654D9C62064C65CBD525AA981"},
        {"75546181818C5799720B713D00A54413497A5A78A969BF991BB9D74E81","7554618181906DB3B01795219EBCD55749B32AD441AC1D655A0781","75546181D9A3747FA71A1232981D373F4645008255271E2EAE3CA5D9"},
        {"75546181D9334CBF9E55A8C407A0A83F39D5AA83BFD5D1757C7ED9","75546181811B55AF3B5E93CB4B9D970BCA6E7E36D6B7AAD0465D2E9081","7554618181BCA5370A9F392A699661C4492DA7C29F485C11C2C2BCB881"},
        {"75546181813DD6A780A81E19C010B88A0E8EAA80877DA8C52B1A8B7AD40181","75546181D93F891DCE5D03387AD2381BB37D31B7B353C9BC2E85745C3E10D9","75546181DB1B6C80AFCE024919BAD950C772939E27148D1113D19F954F11C910DB"},
        {"757554618281D01FCE479038CE2CAC717D10A2369A5C5E254634558EDA5CA64800BD0C7160BF1581","6181815A10DAAB7057C14D9E1B4249AF649301C7609B08D016A3AF4A221F154A38328D81","6181811BCC75021F3B21AB50BC332C70BABCCB7E146702738E78963E6389B25400562D2D410A3681"},
        {"61818152958C6A4872807D5521D843BD3A605D645F8060B78D1C9DD33D75718C99CC53CF0A38B0BB81","7575546181819F942CA07D00892DD6029B34233E6EB80E6AD9771E91571746C24A948C12696A81","6181D99F59799976820093895FBB03CA803A5088912FA92EB4D1BE0E769F580B684A14DB88B1D9"},
        {"7575546182D96E43C7BF9A5FBC4580CB9D7FA607C06F1D36A0C00FD13F844F7474C95A0DD9","61818198381EBEA8A2745580ACDBBD56157F40435A57BD9D49A69D6202B11FCEA63D6381","618181D5265E71B2679E34802EA22A5862925FCA50D3D06672BAC96F0EA4D9BAA74AA16910599B81"},
        {"6181813D78A2490AC65EB07EBB00B3603C67AE68C66507ABCF4302D7C6BF7EA09C2989B081","6181813F4955D56D144853C605DA4657351E992EB4B213DB186EB998B117C03E3C07CB1E81","75755461818194A29A3EADDA552F004E239816B37338A3D56DD200729E1D76C13B1B29732E9781"},
        {"75546181818D12B4409CA8AB5860631187DB80D5C0D2B4D0BE4470529B763581","7554618181A6BB263B8E5A44C349D417377F48374EB08E9E8D81","7554618181B5030B18C97DCCB4C02C446D6C04BE7BAAAE80A7A8D6922B8B81"},
        {"75546181810F33984A2452455AD846A3C94ACC0168A0198C81","7554618181D40B4D893C6932BDBC147896389C05D438397769AE8AB581","755461818170AD8B00B336D261A346575FD1A513D7A4337F0CAD81"},
        {"75546181810465313E9C227047C2A830A17A679278393B760C597781","7554618181ADCF03C272BA9497D44A7C554BC73C748C4A991E115B7A4781","755461818165B707D48E6CAD3F7180C76F0AA410177D31731E40A481"},
        {"75546181D9B78F16A8060C36248248B84E6FC4C2014022B4404ED9","75546181815E5C107D2ECC40B93655AC0AAAD6C8CE5A7A40713B15A58C81","75546181818EAA55C1D9A0457DD52046880530D2787270D159161D477481"},
        {"75546181D9848414645A38892E7B129544C30A673DA6A6837C87C4D9","7554618181BA5C7D5124BFCE41A68B23AE8855802D1A5505BC5F186028B09E81","75546181D9B77800D0551F6103B1851FC291912220443DCFCA6980936F52D9"},
        {"755461818142C6303C6066B5BF7812420C61243BB503D79D6C1ED37ECA8081","7554618181D23A727C61BE93B990660F42793096371BCB3D569AA844B981","755461818159236F4F7232D4560274224A2FD5136DAC6AB95E0A704381"},
        {"7554618181D1804EDBBD66C3C76AC77557AE7925C2B5C9D04DC1B57E7A6EA10981","7554618181D0C8B97A55D66C099C56C187CA8D7FD426D80F55C7ABBE081232649481","7554618181492E733EB85A45366176A475AA2D0F8D0C0EBAB0474381"},
        {"755461818102A83DB2397418035646B47CB746163A032D52C209C1C381","755461818180BCCD7046AF8AD1247D00C0C9B4770ED87BCBAA33B2AD772B81","7554618181C1036B22269EAF4D16C5096C795865BD2C2AAED7B75E76C9D793D681"},
        {"7575546181814A358E2B5654C6AF6B880D47CFBA683277B10AB98D00299BA00068A48817415796A24981","7575546182812018111BD56B5217076639C57E4E498CBCA4186505268B9ED0888BA39B36CC8D81","61818136008A4ED4646ED2B11A8B1F9E45094F4CAE0F56588936C2AF0B5E032FBE383C5E74659881"},
        {"6181D9D5CE7A33B9BA80BFA2BB3982D21A4201C07A6BADCDB926368E92279EDB7CC6694E46AF41D9","618181B180CEA924218C584995333D16CD6290D91035C875B2C16B31BC42C5DB630E0AC97287700381","61818195BA240E497D6064C280A72FD1A301CB9E2C077232A53F9BB3773D7EBB0D3E8C32A781"},
        {"6181810F67BA7C938094CEBF4D3B4B549F13032C2201131D13711CB02692693B5D38A32EC781","618181A02535BEC159C848396A6D8993048AD1A4CB7B055C996BA1106E52481B221D5D81","618181789449A05860AFC4C034523E1EB18E5958B134D8262179CA04CA89A5A7AA6EABA847A96E81"},
        {"6181D90E4BB6BF3FA5A566061F281058AAD89336460F37D00708678F8F76338122267F7BB8D9","6181D92F8288448ACC79A0922802A91D713DA4104BD2B8820378566A0267136C4D67A35B49D9","618181C3141664D4086F0FD764BC4126034C58A68D208B3576BFCCC9CFD5C1969F882F136309C9A581"},
        {"6181D9A40CD1CC1B4176D2857799C9120F3BA3BD0EAD14BDA53185B541556FAC0F302455411109D9","6181DBC83C84B6789188D8359B2E99A41116038F2FB07E249A6B61921E821CB272B4093B7F1D2AB0DB","6181816500A2A491AB3E4D541191484AD09C9075D830AC9C34AC20590BD2B879639953B43497D681"},
        {"6181819A9512416E558C71621104B39C3B17B13C13A1BCCFCCB540B9734C331D60D2321270A19481","6181815CD58E48801E569A91532147CE58769B67C17F17C8048DCD003210D36A75794E98C6A6BF8C81","618181AE6CC7BD9C7C8D655FD835463D19172C65360E096B75287FAA328DC3AD5F2C1266C73D313081"},
        {"75546181D9A1710271CF812283422D98438F4E8F1F348F951F6F6EA15B9810D9","75546181D9CB431FA997500A5FB20AA82EC3B580A032B57127CB217359B69AD9","75546181813E3A458B5A055CB26D787CDAA26C7A01630DC179684EB17E81"},
        {"7554618181A8B73D0E955F516C21067601C315266F501D756EA5033F773781","75546181812E5A353D8792882198236BAE0A37BD721FA70E457CB76381","75546181813A9FB4A1D035A02290C1446CA92F3109D3CAD26476B2299681"},
        {"7575546181810208217A7916BBBD389C1EC2C2A158D9347E6EB81EB40336DB296031B125679F00030F1F177081","7575546182D9D5C18353C728BA8B8FC384B59149217E0BA77BBEBD8256D5C01D43AD3AA30CD7D9","6181814D6FA0796B4E7F4F49CB99D342B25697143E2D4598CF1B0976C02E2F6A517E0C9FA30E81"},
        {"6181819437D3930D97AFAF5F488B0D391BCC7A52B154056CBB18D602B2B0BB8A00A37007162CB181","6181D93504089E853A7C9719481363639341394937615C2F12C524AF43CFD73432C0B864D61D6B6BD9","6181816866573DC8AF6024BE13187677BAD9C245C92471D05A871A0BD174505C497BAF57044BC4CA8981"},
        {"757554618281063F6421BB9E9CC955547BB73D7B0A77B825512EB97414D2C847D974111029D56D12D74281","61818189DB7805474E7B3EC5C833CA28072C1D9E340D4744887F6E6091CAAD560CBBCD4790623C0181","6181D901738BAEC7147DA49F9FD6B6D17B283D898D0637B68E34AD4DD4B988C43332357BD8AD0DC88097D9"},
        {"7575546181DB229521B36477038791B0756224B07A85147A5788C8087647B21C5E7C284ECBD6CD06629BD007DB","6181D91B4B2222B4606FB8B39BB8902375D4BF6B434F825BB62058B125807D38C08EC29996CA8D838FD9","6181816AB2AE2E7839D69B18CB5F6D9601AC7E3B16DB3A9F902278C2399AD40BB21240000D29BF81"},
        {"75546181D9A08585325B62B22893C61A37658767772D911849668710AB76C3ABD9","75546182817BD043C0109DA7112AC83A0E598A427C7E989D7B8B1A64A74281","75546181DB972F9C62A62699C04F7C3CAED2AA7E57A921CA1B8F1E6D57D925B2ABDB"},
        {"6181DB628D58AB076DB1247A6C57D76F1258257C1C2093B73573006FC557467784864E3C1D03898121DB","6181D9B87F7B6D1276B1A6203C56A93AC8098AD01E6E0A875A8C39832E388BC0AA6F91AB3809D9","6181812A5E7B60146CD68965BD918DD9348C5C45BC365C249D67329A205AA402CA0D7B28A3D76DAB4E81"}};

}
