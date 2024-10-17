# Communication Tools

#### 我是一名工控行业的普通开发者，目前仅入行不到一年，现如今已经积累了一些经验，我想将这些技术归类总结为工作积累，此项目由此诞生。

#### 首先项目现阶段仅处于实验阶段，在本ReadMe未更改之前切勿使用本仓库来进行开发。

#### 目前项目通信进展

- [ ] Modbus
  
  - [x] ModbusRtu-SerialPort
  
  - [x] ModbusAscii-SerialPort
  
  - [ ] ModbusTcp
  
  - [ ] ModbusUdp

- [ ] Mitsubishi(三菱通信库)
  
  - [ ] Qna3E
    
    - [ ] SerialPort
      
      - [x] ##### Bit
    
    - [ ] Tcp
    
    - [ ] Udp
      
      

#### 接下来是一段简单的ModbusAscii的示例代码：

```C#
using Microsoft.Extensions.DependencyInjection;
using ModbusRtuLib.Contracts.Modbus.Ascii;

IServiceProvider Service = new ServiceCollection()
    .AddSingleton(p =>
    {
        return DeviceFactory
            .CreateDefaultAsciiClient("COM2")
            .AddSlave(p =>
            {
                p.SlaveId = 1;
                p.IsStartZero = true;
                p.IsCheckSlave = true;
                p.DataFormat = ModbusRtuLib.Models.Enums.DataFormat.ABCD;
                p.StringEncoding = Encoding.ASCII;
                p.ReverseString = false;
            })
            .SetupStart();
    })
    .BuildServiceProvider();
var rtu = Service.GetService<IModbusAsciiClient>()!.GetSlave(1).ReadDouble(0x0000);

```

#### 这段代码展示了开启一个COM2端口，并设置相关参数，读取站号1的0x0000的double数据（长度为4个寄存器，长度为64个字节）。



## 另外！本项目提供了一款基础的测试工具，跟随核心代码一并更新

#### 测试工具使用WinUI1.6+.Net 8框架开发，使用AOT进行发布，可对于任意工控机（Windows Version > 10）进行快速测试通信。

<img title="Home" src="assets\home.png" alt="home" style="zoom:50%;">
