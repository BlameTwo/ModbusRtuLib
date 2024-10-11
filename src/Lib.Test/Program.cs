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
using ModbusRtuLib.Services.Mitsubishi;

McNetSerialPort mcnet = new McNetSerialPort();
mcnet.OpenSerial(
    new ModbusRtuLib.Models.SerialPortConfig()
    {
        SerialPortName = "COM2",
        BaudRate = 9600,
        DataBit = 8,
        Parity = System.IO.Ports.Parity.None,
        StopBit = System.IO.Ports.StopBits.One,
    }
);

//var result = mcnet.ReadBit("X1A0");
var write = mcnet.WriteBit("B1A0", false);
var read = mcnet.ReadBit("B1A0");
Console.WriteLine(write.Data);
Console.WriteLine(read.Data);
Console.ReadKey();
