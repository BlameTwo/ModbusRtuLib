using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModbusRtuLib.Services.Mitsubishi;

internal class Program
{
    private static void Main(string[] args)
    {
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
        mcnet.Write(1234, "W100");
    }
}
