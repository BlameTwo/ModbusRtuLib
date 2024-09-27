using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModbusRtuLib.Common;
using ModbusRtuLib.Common.Factorys;
using ModbusRtuLib.Contracts.Tcp;

internal class Program
{
    private static void Main(string[] args)
    {
        IServiceProvider Service = new ServiceCollection()
            .AddSingleton(p =>
            {
                return DeviceFactory
                    .CreateDefaultTcpClient()
                    .InitServerPort(p =>
                    {
                        p.IpAddress = "127.0.0.1";
                        p.Port = 502;
                        p.IsReconnect = true;
                    });
            })
            .AddSingleton(p =>
            {
                return DeviceFactory
                    .CreateDefaultRtuClient("COM2")
                    .AddSlave(p =>
                    {
                        p.SlaveId = 1;
                        p.IsStartZero = true;
                        p.IsCheckSlave = true;
                        p.DataFormat = ModbusRtuLib.Models.Enums.DataFormat.CDAB;
                        p.StringEncoding = Encoding.UTF8;
                    })
                    .SetupStart();
                ;
            })
            .AddSingleton(p =>
            {
                return DeviceFactory
                    .CreateDefaultAsciiClient("COM2")
                    .AddSlave(p =>
                    {
                        p.SlaveId = 1;
                        p.IsStartZero = true;
                        p.IsCheckSlave = true;
                        p.DataFormat = ModbusRtuLib.Models.Enums.DataFormat.DCBA;
                        p.StringEncoding = Encoding.ASCII;
                        p.ReverseString = false;
                    })
                    .SetupStart();
            })
            .BuildServiceProvider();
        var client = Service.GetService<IModbusTcpClient>()!;
        //if ((client.Connect("127.0.0.1").Data))
        //{
        //    var result = client.GetSlave(1).ReadCoil(0x0000);
        //}
        client.Connect("127.0.0.1");
        while (true)
        {
            if (!client.IsConnected)
                continue;
            else
            {
                var value = client.GetSlave(1).ReadCoil(0000);
                if (value.IsOK)
                {
                    Console.WriteLine(value.Data);
                }
                else
                {
                    Console.WriteLine(value.Error);
                }
            }
        }
    }
}
