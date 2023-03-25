# ClassicalCryptography

这是一个包含了众多古典密码实现的程序库，它是100%由C#编写的类库，框架版本为[.Net7](https://dotnet.microsoft.com/zh-cn/download/dotnet/7.0 "https://dotnet.microsoft.com/zh-cn/download/dotnet/7.0")。

## Transposition

这一类型的密码为一维置换密码，仅改变内容字符的排列顺序，不改变内容，排列顺序的决定方式是一维的。

---

### ReverseCipher

* 倒序密码
* 无密钥
  1. 文字从右向左读出

```csharp
var cipher = new ReverseCipher();
cipher.Encrypt("012345");//543210
```

---

### TakeTranslateCipher

* 取平移密码
* 密钥 ***(N,K)***
* 密钥不可逆
  1. 先取出***N***个字符
  2. 对剩余的字符向左平移 ***K*** 个单位
  3. 回到步骤1，直到剩余字符不足 ***K***个
  4. 剩余字符(如果有)直接连接
* 以下演示的文本为"12345678"
* 密钥为(2,3)
* 结果为"12675834"

|          |  1  |  2  |  3  |  4  |  5  |  6  |  7  |  8  |
| -------- | :-: | :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| 12       |     |     | [3] | [4] | [5] |  6  |  7  |  8  |
| 12       |     |     |  6  |  7  |  8  |  3  |  4  |  5  |
| 1267     |     |     |     |     | [8] | [3] | [4] |  5  |
| 1267     |     |     |     |     |  5  |  8  |  3  |  4  |
| 126758   |     |     |     |     |     |     |  3  |  4  |
| 12675834 |     |     |     |     |     |     |     |     |

```csharp
var cipher = new TakeTranslateCipher();
var key = TakeTranslateCipher.Key.FromString("23");
cipher.Encrypt("12345678", key)//12675834
```

---

### TriangleCipher

* 三角形排列密码
* 无密钥
* 可能需要补充字符(默认`)

  1. 文字按行排列成(等腰直角)三角形
  2. 按列读出文字
* 如下所示即是"123456789"的三角形排列

  |   |   | 1 |   |   |
  | - | - | - | - | - |
  |   | 2 | 3 | 4 |   |
  | 5 | 6 | 7 | 8 | 9 |

* 按列读出即为"526137489"

```csharp
var cipher = new TriangleCipher();
cipher.Encrypt("123456789");//526137489
```

---

### JosephusCipher

* 约瑟夫环密码
* 有密钥 ***M***
* 密钥不可逆
  1. 想象所有人围城一个环，开始报数
  2. 当报到第***M***个人，那个人就出列
  3. 接着以下一个为起点开始重新报数
  4. 重复步骤2和3，直到所有人都出列
* 以下演示的文本为"123456"
* 密钥为3
* 结果为"364251"

|        |  1  |  2  |  3  |  4  |  5  |  6  |
| ------ | :-: | :-: | :-: | :-: | :-: | :-: |
| 3      |  1  |  2  | [1] |  4  |  5  |  6  |
| 36     |  1  |  2  | [1] |  4  |  5  | [2] |
| 364    |  1  |  2  | [1] | [3] |  5  | [2] |
| 3642   |  1  | [4] | [1] | [3] |  5  | [2] |
| 36425  |  1  | [4] | [1] | [3] | [5] | [2] |
| 364251 | [6] | [4] | [1] | [3] | [5] | [2] |

```csharp
var cipher = new JosephusCipher();
var key = JosephusCipher.Key.FromString("3");
cipher.Encrypt("123456", key);//364251
```

---

### RailFenceCipher

* 栅栏密码(普通型，实际上这是个二维置换密码)
* 密钥为***每组字数N***

  1. 将文字按行排列成***N***列
  2. 按列依次读出文字
* 以下演示的文本为"RailFenceCipherTest"
* 每组字数为3
* 结果为"RlnChTtaFcieeieeprs"

  | R | a | i |
  | - | - | - |
  | l | F | e |
  | n | c | e |
  | C | i | p |
  | h | e | r |
  | T | e | s |
  | t |   |   |

```csharp
var cipher = new RailFenceCipher();
var key = RailFenceCipher.Key.FromString("3");
cipher.Encrypt("RailFenceCipherTest", key);//RlnChTtaFcieeieeprs
```

---

## Transposition2D

这一类型的密码为二维置换密码，仅改变内容字符的排列顺序，不改变内容，排列顺序的决定方式是二维的。

### CycleTranspose

* 周期/列置换密码
* 有密钥(多组排列对)
* 可能需要补充字符(默认`)
  1. 将文字排列成一个矩形(宽度即为排列长度)
  2. 根据排列对交换文字的列
  3. 依据排列数的顺序依次读出文字
* 以下演示的文本为"Sitdownplease!"
* 密钥为(1,2,4)(3,5)
* 结果为"dSoitlwenp!a`se"

| S | i | t | d | o |
| - | - | - | - | - |
| w | n | p | l | e |
| a | s | e | ! | ` |

> 第1列移动至第2列，第2列移动至第4列，第4列移动至第1列

| d | S | t | i | o |
| - | - | - | - | - |
| l | w | p | n | e |
| ! | a | e | s | ` |

> 第3列移动至第5列，第5列移动至第3列

| d | S | o | i | t |
| - | - | - | - | - |
| l | w | e | n | p |
| ! | a | ` | s | e |

```csharp
var cipher = new CycleTranspose();
var key = CycleTranspose.Key.FromString("(1,2,4)(3,5)");
cipher.Encrypt("Sitdownplease!", key);//dSoitlwenp!a`se
```

---

### AdvancedRailFenceCipher

* 扩展栅栏密码
* 密钥为可能的一个排列
* 可能需要补充字符(默认`)

  1. 将文字按行排列成一个矩形
  2. 矩形宽度为排列长度
  3. 依据排列的顺序按列读出文字
* 以下演示的文本为"eg1ML9mymEqtKzeN0"
* 密钥为7,4,3,5,6,2,1
* 结果为"mz\`9K\`1E0gmNMq\`Lt\`eye"

| e | g | 1 | M | L | 9 | m |
| - | - | - | - | - | - | - |
| y | m | E | q | t | K | z |
| e | N | 0 | ` | ` | ` | ` |

> 将列表按照7,4,3,5,6,2,1列的顺序填入

| m | 9 | 1 | g | M | L | e |
| - | - | - | - | - | - | - |
| z | K | E | m | q | t | y |
| ` | ` | 0 | N | ` | ` | e |

```csharp
var cipher = new AdvancedRailFenceCipher();
var key = AdvancedRailFenceCipher.Key.FromString("7,4,3,5,6,2,1");
cipher.Encrypt("eg1ML9mymEqtKzeN0", key);//mz\`9K\`1E0gmNMq\`Lt\`eye
```

---

### RotatingGrillesCipher

* 旋转栅格密码
* 有密钥(一个正方形的栅格,用4进制数组表示挖洞的情况)
* 可能需要补充字符(默认`)

  1. 准备一个4N^2的矩形
  2. 在指定的位置挖出洞
  3. 在栅格对应的位置填入文字
  4. 旋转栅格(默认顺时针旋转)
  5. 重复步骤填入4次
  6. 按行读出结果
* 如下所示即是一个栅格(H代表对应位置有洞)

  |     |     |  H  |     |
  | :-: | :-: | :-: | :-: |
  |     |  H  |     |     |
  |  H  |     |     |     |
  |     |     |     |  H  |

* 以下演示的文本为"meetmeattwelvepm"
* 密钥为"4:tA=="(即为上述的栅格)
* 结果为"tmmveeewepeatlmt"

> 将meet填入栅格位置，并旋转栅格

|     |  -  |  m  |     |
| :-: | :-: | :-: |  -  |
|     |  e  |  -  |     |
|  e  |     |     |  -  |
|  -  |     |     |  t  |

> 填入meat并旋转栅格

|  -  |  m  |  m  |     |
| :-: | :-: | :-: |  -  |
|     |  e  |  e  |  -  |
|  e  |     |  -  |  a  |
|  t  |  -  |     |  t  |

> 重复上述步骤

|  t  |  m  |  m  |  -  |
| :-: | :-: | :-: | :-: |
|  -  |  e  |  e  |  w  |
|  e  |  -  |  e  |  a  |
|  t  |  l  |  -  |  t  |

|  t  |  m  |  m  |  v  |
| :-: | :-: | :-: | :-: |
|  e  |  e  |  e  |  w  |
|  e  |  p  |  e  |  a  |
|  t  |  l  |  m  |  t  |

```csharp
var cipher = new RotatingGrillesCipher();
var qArr = new QuaterArray(4);
qArr[0] = 2;
qArr[1] = 3;
qArr[2] = 1;
qArr[3] = 0;
var keyStr = qArr.ToString();
var key = RotatingGrillesCipher.Key.FromString(keyStr);
cipher.Encrypt("meetmeattwelvepm", key);//tmmveeewepeatlmt
```

* 补充：你也可以设置`cipher.AntiClockwise`属性来让栅格逆时针旋转

---

### MagicSquareCipher

* 幻方序密码
* 无密钥
* 可能需要补充字符(默认`)

  1. 用特定的方法构造N阶幻方(幻方可以有很多种顺序，但这里只采用"标准方法"的顺序)
  2. 根据幻方的顺序，按行读出文字
* 如下所示即是一个3阶幻方(行列和对角线的和都相等)

  | 8 | 1 | 6 |
  | - | - | - |
  | 3 | 5 | 7 |
  | 4 | 9 | 2 |

* 以下描述了具体依据使用的方法，细节请查看代码

  1. 奇数阶幻方使用Louberel法
  2. 双偶阶幻方使用对称交换法
  3. 单偶阶幻方使用Strachey法

```csharp
var cipher = new MagicSquareCipher();
cipher.Encrypt("123456789");//816357492
```

---

### HilbertCurveCipher

* 希尔伯特曲线密码
* 无密钥
* 可能需要补充字符(默认`)
  1. 从左上角开始，到左下角的路径构造希尔伯特曲线
  2. 根据路径顺序依次填入字符
  3. 按行读出文字
* 前4阶的希尔伯特曲线如图所示
* ![希尔伯特曲线](Images/HilbertCurve.png)

> 4*4的曲线顺序为

|  1  |  4  |  5  |  6  |
| :-: | :-: | :-: | :-: |
|  2  |  3  |  8  |  7  |
| 15  | 14  |  9  | 10  |
| 16  | 13  | 12  | 11  |

```csharp
var cipher = new HilbertCurveCipher();
cipher.Encrypt("0123456789ABCDEF");//03451276ED89FCBA
```

---

### SpiralCurveCipher

* 螺旋曲线密码
* 有密钥(方形宽度)
* 可能需要补充字符(默认`)
  1. 从左上==>右上顺序
  2. 从右上==>右下顺序
  3. 从右下==>左下顺序
  4. 从左下==>左上顺序
  5. 不重复无遗漏地螺旋顺序排列文本

> 4*4时的顺序为

|  1  |  2  |  3  |  4  |
| :-: | :-: | :-: | :-: |
| 12  | 13  | 14  |  5  |
| 11  | 16  | 15  |  6  |
| 10  |  9  |  8  |  7  |

```csharp
var cipher = new HilbertCurveCipher();
cipher.Encrypt("0123456789ABCDEF");//0123BCD4AFE59876
```

---

### ArnoldCatMapCipher

* 猫映射变换密码(用于文本而不是图像)
* 无密钥
* 可能需要补充字符(默认`)
  1. 文字排列成N^2的方阵
  2. x=>2x+y(mod N)
  3. y=>x+y(mod N)
  4. 根据变换后的顺序加密文本
* 用图像解释操作的过程
* ![ArnoldCatMap](Images/ArnoldCatMap.png)

```csharp
var cipher = new ArnoldCatMapCipher();
cipher.Encrypt("0123456789ABCDEF");//0DA7B41E2F85963C
```

---

### VBakersMapCipher

* 魔改Baker's映射变换密码
* 无密钥
* 可能需要补充字符(默认`)
* 具体方法请前往[VBakersMapCipher.cs](https://github.com/Lazuplis-Mei/ClassicalCryptography/blob/main/ClassicalCryptography/Transposition2D/VBakersMapCipher.cs)查看代码

---

### JigsawCipher

* 锯齿分割密码
* 有密钥(文本方阵的一个整数分拆)
* 可能需要补充字符(默认`)
  1. 文字排列成N^2的方阵
  2. 取整数N的一个分拆{P1,P2,...,Pn}
  3. 方阵竖着划分成n个块
  4. 每一个块按顺序分割成Pi个面积为N的锯齿拼图
  5. 余项使用右对齐
  6. 根据**特定的顺序**写入文字
* 下图为使用2,5,1,3加密文本的图示
* ![JigsawCipher](Images/JigsawCipher.png)

---

### FifteenPuzzle

* 数字华容道密码
* 有密钥(移动步骤)
* 可能需要补充字符(默认`)
  1. 文字排列成N^2的方阵
  2. 右下角的位置空着
  3. 移动空位和相邻的内容交换
  4. 经过一系列移动后再读出方阵
* *此功能正在开发中*

---

### SixteenPuzzle

* 移动数字华容道密码
* 有密钥(移动步骤)
* 可能需要补充字符(默认`)
  1. 文字排列成N^2的方阵
  2. 首位循环地移动整行/列
  3. 经过一系列移动后再读出方阵
* *此功能正在开发中*

---

### TwiddlePuzzle

* 旋转阵列密码
* 有密钥(旋转步骤)
* 可能需要补充字符(默认`)
  1. 文字排列成N^2的方阵
  2. 以指定的4个格子为中心旋转
  3. 经过一系列旋转后再读出方阵
* *此功能正在开发中*

---

## Replacement

替换/代换密码(也包括输出图像)

### PigpenCipher

* 猪圈密码
* 无密钥
* 仅处理纯英文字母和空格
* 包含变体
  1. ![类型1](Images/PigpenCipher1.png)
  2. ![类型2](Images/PigpenCipher2.png)

---

### SingleReplacementCipher

* 单表替换密码

  1. 凯撒密码
  2. qwer键盘表
  3. Atbash
  4. 汉语拼音
  5. rot5
  6. rot13
  7. rot18(rot5+rot13)
  8. rot47
  9. Al Bhed
  10. 仿射密码
  11. 敲击码
* 更多可自定义

---

### MorseCode

* 摩斯密码
  1. 纯英文字母摩斯密码
  2. 扩展数字和符号的摩斯密码
  3. 数字短码

---

### CommercialCode

* 中文电码
  1. 标准中文电码(Chinese Commercial Code)
  2. 默认使用数字短码进行编码

---

## Calculation

自定义算法计算的密码。

### ShortHide5

* ShortHide5密码
* 无密钥
* [Standard Short Hide5(标准SH5)](https://www.bilibili.com/read/cv15660906)
* [标准的1组SH5推荐字母表](https://www.bilibili.com/read/cv15676311)

---

### RSASteganograph

* RSA隐写术
* 以指定的前缀字节生成质数并计算RSA私钥
* 生成的私钥可以像正常的私钥一样使用
* 可指定生成xml或pem格式的密钥

```csharp
var text = "天空不会一直都晴朗的，偶尔会下些雨滴，也有吹起暴风雨的时候，景色会渐渐的改变。";
var pemkey = RSASteganograph.GenerateRSAPrivateKey(text, true);
//将会生成一个pem格式的RSA私钥
RSASteganograph.GetTextFrom(pemkey);//获取其中的文本
```

---

### PerfectShuffle

* 完美洗牌密码
* 对于字母表进行2种交替式的完美洗牌
* 取指定的首字母作为结果
* 补充：结果有随机性
* *快速的插入查找方法正在开发中*

---

## Image

与图像密切相关的密码，或以图像形式呈现。

### ColorfulBarcode

* 彩色二维码
* 无密钥
* 中文可用base64代替
  1. 文字分成3部分/6部分
  2. 生成3个/6个二维码
  3. 二维码分别对应rgb色彩通道(3份)
  4. 如果是6组色彩,请查看[ColorfulBarcode.cs](https://github.com/Lazuplis-Mei/ClassicalCryptography/blob/main/ClassicalCryptography/Undefined/ColorfulBarcode.cs)

> 一个3色二维码的例子
>
> 内容为3oX+o67864CwiRYVndetxFyZuy7T59Rv0gc9eUSKwU/kWbNy1PGbtfJrrE/s9Can0N5sbPyOcXedYvwEom6qmQqMlfLCwkpZrp3j

![ColorfulBarcode](Images/ColorfulBarcode.png)

> 相同的内容的6色二维码为

![ColorfulBarcodeSixColor](Images/ColorfulBarcodeSixColor.png)

```csharp
var plainText = "你想编码的内容";
var bitmap = ColorfulBarcode.Encode(plainText);//生成3色二维码
var text = ColorfulBarcode.Recognize(bitmap);//识别3色二维码
bitmap = ColorfulBarcode.EncodeSixColor(plainText);//生成6色二维码
text = ColorfulBarcode.RecognizeSixColor(bitmap);//识别6色二维码
```

---

### MoirePattern

* 摩尔纹
* 支持非嵌入式和嵌入式
* 支持自定义条纹样式
* 支持静态内容和多帧内容

> 静态非嵌入式的例子

```csharp
MoirePattern.Font = new Font("微软雅黑", 36);
var bitmap = MoirePattern.DrawText("这是摩尔纹", 300, 80, MoirePatternTypes.DiagonalPatten);
bitmap.Save("E:/MoirePattern.png");
```

![MoirePattern](Images/MoirePattern.png)

```csharp
MoirePattern.FillPatten(bitmap, MoirePatternTypes.DiagonalPatten);
```

![MoirePatternFilled](Images/MoirePatternFilled.png)

```csharp
MoirePattern.FillPatten(bitmap, MoirePatternTypes.DiagonalPatten, removePatten: true);//指定去除条纹
```

![MoirePatternRemoved](Images/MoirePatternRemoved.png)

> 这是嵌入式的例子，更换了波形的条纹

```csharp
MoirePattern.Font = new Font("微软雅黑", 36);
var bitmap = MoirePattern.DrawText("这是摩尔纹", 300, 80, MoirePatternTypes.SinWavePatten, true);
bitmap.Save("E:/MoirePatternEmbedded.png");
```

![MoirePatternEmbedded](Images/MoirePatternEmbedded.png)

> 考虑到嵌入式的呈现效果，只选择条纹去除的效果

![MoirePatternEmbeddedFilled](Images/MoirePatternEmbeddedFilled.png)

> 多帧的摩尔纹示例

```csharp
MoirePattern.Font = new Font("微软雅黑", 36);
var bitmap = MoirePattern.DrawTexts(new[] { "这", "是", "摩", "尔", "纹" }, 80, 80);
bitmap.Save("E:/Pattens.png");
MoirePattern.FillAndSavePattens(bitmap, 5, "E:/Pattens");
```

![Pattens](Images/Pattens.png)
![5826912_0](Images/Pattens/5826912_0.png)![5826912_1](Images/Pattens/5826912_1.png)![5826912_2](Images/Pattens/5826912_2.png)![5826912_3](Images/Pattens/5826912_3.png)![5826912_4](Images/Pattens/5826912_4.png)

---

### WeaveCipher

* 编织图形密码
* [参考实现逻辑](https://tieba.baidu.com/p/7814788182)
* 扩展的(带有三角形的)图形的实现有所变化

> 正方形的例子

```csharp
var bitmap = WeaveCipher.Encrypt("GOOD");
bitmap.Save("E:/WeaveCipher.png");
WeaveCipher.Decrypt(bitmap);//GOOD
```

![WeaveCipher](Images/WeaveCipher.png)

> 带有三角形的例子

```csharp
var bitmap = WeaveCipher.EncryptExtend("GOOD Night");
bitmap.Save("E:/WeaveCipherExtend.png");
```

![WeaveCipherExtend](Images/WeaveCipherExtend.png)

* *解密三角形的功能正在开发中*

---

## Sound

与声音相关的密码，用声音表示。

### MorseMoonlight

* 摩斯月光奏鸣曲
* 将摩斯密码掺入月光奏鸣曲第一乐章的节奏中

```csharp
var morseCode = MorseCode.Standred.ToMorse("MorseMoonlight");
MorseMoonlight.ExportMidi(morseCode, "E:/MorseMoonlight.mid");
MorseMoonlight.ExportWav(morseCode, "E:/MorseMoonlight.wav");
//如果你希望能够编码为Wav，你需要提供soundfont文件，在TIMIDITY.CFG中编辑
```

> [MorseMoonlight.mid](https://github.com/Lazuplis-Mei/ClassicalCryptography/blob/main/Sound/MorseMoonlight.mid)

---

## Encoder

多种编码之间转换的实现

* Base100
* Base2048
* Base65536
* Base32768
* Base32
* Base85
* 2,4,8,16进制
* 盲文编码
* 易经八卦编码(等价于Base64)
* 罗马数字转换
* BubbleBabble
* 国家通用盲文方案(*正在开发中*)

---

### PLEncoding

编程语言相关的转换

* Python中的bytes
* Punycode
* PPEncode
* JJEncode
* Jother
* Brainfuck

---

## Undefined

未做具体归类的密码

### SemaphorePathCipher

* 旗语路径密码
* 无密钥
* ![旗语](Images/Semaphores.png)

  1. 从左往右进入左上角开始
  2. 根据旗语对应的符号连接写一个字母
  3. 删除所有路径上的字母
  4. 剩余的则为内容

---

### StringArtCipher

* 弦艺术密码
* 使用22个针脚的欧拉字体
* 使用22个针脚的非欧拉字体

---

### PascalianPuzzleCipher

* 帕斯卡谜题密码
* *此功能正在开发中*