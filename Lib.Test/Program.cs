using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModbusRtuLib.Common;
using ModbusRtuLib.Common.Factorys;
using ModbusRtuLib.Contracts.Ascii;
using ModbusRtuLib.Contracts.Rtu;
using ModbusRtuLib.Contracts.Tcp;

internal class Program
{
    private static void Main(string[] args)
    {
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
    }
}
