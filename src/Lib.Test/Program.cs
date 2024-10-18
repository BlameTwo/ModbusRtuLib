//IServiceProvider Service = new ServiceCollection()
//    .AddSingleton(p =>
//    {
//        return DeviceFactory
//            .CreateDefaultAsciiClient("COM2")
//            .AddSlave(p =>
//            {
//                p.SlaveId = 1;
//                p.IsStartZero = true;
//                p.IsCheckSlave = true;
//                p.DataFormat = ModbusRtuLib.Models.Enums.DataFormat.ABCD;
//                p.StringEncoding = Encoding.ASCII;
//                p.ReverseString = false;
//            })
//            .SetupStart();
//    })
//    .BuildServiceProvider();
//var rtu = Service.GetService<IModbusAsciiClient>()!.GetSlave(1).ReadDouble(0x0000);
using System.Net.Sockets;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Services.Mitsubishi;

//IMcNetSerialPort mcnet = new McNetSerialPort();
//mcnet.OpenSerial(
//    new ModbusRtuLib.Models.SerialPortConfig()
//    {
//        SerialPortName = "COM2",
//        BaudRate = 9600,
//        DataBit = 8,
//        Parity = System.IO.Ports.Parity.None,
//        StopBit = System.IO.Ports.StopBits.One,
//    }
//);

IMcNetTcp mctcp = new McNetTcp();
var connect = await mctcp.OpenDeviceAsync("127.0.0.1");
short value = 123;
var result = await mctcp.WriteAsync(value, "D100");
var result2 = await mctcp.ReadInt16Async("D100");
Console.ReadKey();
