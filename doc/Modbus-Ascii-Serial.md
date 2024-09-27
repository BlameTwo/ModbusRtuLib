## Ascii传输

接口：IModbusAsciiClient

实现：ModbusAsciiClient

应用层协议：modbus ascii (Ascii 字符型协议)

物理介质：串口

Ascii的写入线圈和输入状态字符顺序如下

| 名称      | 例子            | 属性         | 占用字节                          |
| ------- | ------------- | ---------- | ----------------------------- |
| 首位符号    | :             | 头          | 1                             |
| SlaveID | 0x01          | 站号         | 1 |
| method  | 0x01          | 功能码        | 1                             |
| 数据      | 0x00FF/0x0000 | true/false | 2                             |
| LRC校验   | 0xDF(不固定)     | 8位16禁止     | 1                             |


