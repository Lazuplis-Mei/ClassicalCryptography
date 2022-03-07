# ClassicalCryptography
古典密码
* ## Transposition
  一维置换密码，仅改变字符排列顺序，不改变内容。
  * ### ReverseCipher
    * 无密钥
    * 倒序密码
    1. 文字从右向左读出
    ```
    12345678

    87654321
    ```
  * ### TakeTranslateCipher
    * 有密钥:<font color="#00DDFF">**(N,K)**</font>
    * 密钥不可逆
    * 取平移密码
    1. 先取出<font color="#00DDFF">**N**</font>个字符
    2. 对剩余的字符向左平移<font color="#00DDFF">**K**</font>个单位
    3. 回到步骤1，直到剩余字符不足<font color="#00DDFF">**K**</font>个
    4. 剩余字符(如果有)直接连接
    ```
    12345678
    N=2,K=3

    12 345678
    12 678345
    1267 8345
    1267 5834
    126758 34

    12675834
    ```
  * ### TriangleCipher
    * 无密钥
    * 可能需要补充字符(默认`)
    * 三角排列密码
    1. 文字按行排列成(等腰直角)三角形
    3. 按列读出文字
    ```
    123456789

      1
     234
    56789

    526137489
    ```
* ## Transposition2D
  二维置换密码，仅(使用二维的矩形空间)改变字符排列顺序，不改变内容。
  * ### CycleTranspose
    * 有密钥(多组排列对)
    * 可能需要补充字符(默认`)
    * 周期置换密码
    * 列置换密码
    1. 将文字排列成一个矩形(宽度即为排列长度)
    2. 根据排列对交换文字的列
    3. 依据排列数的顺序依次读出文字(按行按列都可以)
    ```
    Sitdownplease!
    (1,2,4)(3,5)

    S i t d o
    w n p l e
    a s e ! `
    (1,2,4)    //1->2,2->4,4->1

    d S t i o
    l w p n e
    ! a e s `

    (3,5)      //3->5,5->3
    d S o i t
    l w e n p
    ! a ` s e

    dSoitlwenp!a`se
    ```
  * ### RailFenceCipher
    * 有密钥(每组字数或一个排列)
    * 可能需要补充字符(默认`)
    * 栅栏密码
    1. 宽度为分组数(或排列长度)
    2. 将文字排列成一个矩形
    3. (依据排列的顺序)按列依次读出文字
     ```
    RailFenceCipherTest
    3

    R a i
    l F e
    n c e
    C i p
    h e r
    T e s
    t ` `

    RlnChTtaFciee`ieeprs`
    ```
  * ### RotatingGrillesCipher
    * 有密钥(一个正方形的栅格,用4进制数组表示挖洞的情况)
    * 可能需要补充字符(默认`)
    * 旋转栅格密码
    1. 准备一个4N^2的矩形
    2. 在栅格对应的位置填入文字
    3. 旋转栅格(默认顺时针旋转)
    4. 再次在栅格对应的位置填入文字
    5. 重复填入4次文字
    6. 按行读出
     ```
    meetmeattwelvepm
    {2,3,1,0}

    左上角的1/4，共有N^2个位置
    如果洞是在这些位置里，那么数组值为0
    如果洞在顺时针旋转后的位置里，那么值为旋转次数
    H...
    ....
    ....
    ....
    这个位置顺时针旋转2次，到了这里，代表最终这里有一个洞
    ....
    ....
    ....
    ...H

    {2,3,1,0}
    ..H.
    .H..
    H...
    ...H
    对应的栅格是这样(H代表洞)
    写入文字meet meattwelvepm
    ..m.
    .e..
    e...
    ...t
    旋转栅格
    .Hm.
    .eH.
    e..H
    H..t
    写入文字meat twelvepm
    .mm.
    .ee.
    e..a
    t..t
    旋转栅格
    Hmm.
    .eeH
    e.Ha
    tH.t
    写入文字twel vepm
    tmm.
    .eew
    e.ea
    tl.t
    旋转栅格
    tmmH
    Heew
    eHea
    tlHt
    写入文字vepm
    tmmv
    eeew
    epea
    tlmt
    得到结果
    tmmveeewepeatlmt
    ```
  * ### MagicSquareCipher
    * 无密钥
    * 可能需要补充字符(默认`)
    * 幻方正序密码
    1. 文字写成一个N^2的矩形
    2. 用特定的方法构造N阶幻方
    3. 根据幻方的顺序，读出文字
     ```
    0123456789ABCDEF

    0123
    4567
    89AB
    CDEF

    对应的幻方
    01 15 14 04
    12 06 07 09
    08 10 11 05
    13 03 02 16

    根据顺序读出
    0ED3B56879A4C21F
    ```