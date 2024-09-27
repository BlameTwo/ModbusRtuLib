## Ascii传输

接口：IModbusAsciiClient

实现：ModbusAsciiClient

应用层协议：modbus ascii (Ascii 字符型协议)

物理介质：串口

Ascii的写入线圈和输入状态字符顺序如下

| 名称      | 例子        | 属性      | 占用字节                          |
| ------- | --------- | ------- | ----------------------------- |
| 首位符号    | :         | 头       | 1                             |
| SlaveID | 0x01      | 站号      | 1                             |
| method  | 0x01      | 功能码     | 1                             |
| start   | 0x0001    | 开始寄存器索引 | 2                             |
| length  | 0x0001    | 读取长度    | 2 |
| LRC校验   | 0xDF(不固定) | 字符串16进制 | 1                             |
| 尾部符号    | \r\n      | 回车换行    | 1                             |

有了以上的表格我们可以组成以下的字节，标识读取一个线圈

```powershell
:01 01 00 01 00 01 FC\r\n 
头部:,01标识站号，01标识功能吗，00和01标识起始索引，后面的00和01表示读取长度 FC标识LRC（纵向冗余校验），尾部\r\n
```

