using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModbusRtuLib.Common;
using ModbusRtuLib.Contracts.Ascii;
using ModbusRtuLib.Contracts.Rtu;

IServiceProvider Service = new ServiceCollection()
    .AddSingleton(p =>
    {
        return ModBusFactory
            .CreateDefaultRtuClient("COM6")
            .AddSlave(p =>
            {
                p.SlaveId = 1;
                p.IsStartZero = true;
                p.IsCheckSlave = true;
                p.DataFormat = ModbusRtuLib.Models.Enums.DataFormat.DCBA;
                p.StringEncoding = Encoding.UTF8;
            })
            .SetupStart();
        ;
    })
    .AddSingleton(p =>
    {
        return ModBusFactory
            .CreateDefaultAsciiClient("COM2")
            .AddSlave(p =>
            {
                p.SlaveId = 1;
                p.IsStartZero = true;
                p.IsCheckSlave = true;
                p.DataFormat = ModbusRtuLib.Models.Enums.DataFormat.DCBA;
                p.StringEncoding = Encoding.ASCII;
            })
            .SetupStart();
    })
    .BuildServiceProvider();

var result = Service.GetService<IModbusAsciiClient>()!.GetSlave(1).ReadDiscrete(0x0001);
Console.ReadKey();
