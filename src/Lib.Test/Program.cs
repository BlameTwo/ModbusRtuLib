using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModbusRtuLib.Common;
using ModbusRtuLib.Common.Factorys;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Contracts.Modbus.Ascii;
using ModbusRtuLib.Contracts.Modbus.Rtu;
using ModbusRtuLib.Contracts.Modbus.Tcp;
using ModbusRtuLib.Models;
using ModbusRtuLib.Services.Mitsubishi;

IServiceProvider Service = new ServiceCollection()
    .AddSingleton(p =>
    {
        return DeviceFactory
            .CreateDefaultRtuClient()
            .AddSlave(p =>
            {
                p.SlaveId = 1;
                p.IsCheckSlave = true;
                p.WriteTimepan = 100;
                p.ReadTimeSpan = 100;
                p.DataFormat = ModbusRtuLib.Models.Enums.DataFormat.CDAB;
                p.StringEncoding = Encoding.ASCII;
            })
            .SetupStart();
    })
    .BuildServiceProvider();
var tcp = Service.GetService<IModbusRtuClient>();
var result = tcp!.GetSlave(1).ReadDouble(1000, ModbusRtuLib.Models.Enums.ReadType.InputRegister);

Console.WriteLine();
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

//IMcNetTcp mctcp = new McNetTcp();
//var connect = await mctcp.OpenDeviceAsync("127.0.0.1");
//var result = mctcp.ReadBit("D100.5");
//Console.ReadKey();
