using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusRtuLib.Contracts.Modbus.Ascii;
using ModbusRtuLib.Contracts.Modbus.Rtu;
using ModbusRtuLib.Contracts.Modbus.Tcp;
using ModbusRtuLib.Services.Ascii;
using ModbusRtuLib.Services.Modbus.Ascii;
using ModbusRtuLib.Services.Modbus.Rtu;
using ModbusRtuLib.Services.Modbus.Tcp;
using ModbusRtuLib.Services.Rtu;
using ModbusRtuLib.Services.Tcp;

namespace ModbusRtuLib.Common.Factorys;

public static class DeviceFactory
{
    /// <summary>
    /// 默认串口
    /// </summary>
    /// <returns></returns>
    public static IModbusRtuClient CreateDefaultRtuClient()
    {
        return new ModbusRtuClient().InitSerialPort(p =>
        {
            p.SerialPortName = "COM2";
            p.BaudRate = 9600;
            p.DataBit = 8;
            p.StopBit = System.IO.Ports.StopBits.One;
            p.Parity = System.IO.Ports.Parity.None;
            p.ReadTimeSpan = 200;
            p.WriteTimeSpan = 200;
            p.DataFormat = Models.Enums.DataFormat.CDAB;
        });
    }

    public static IModbusAsciiClient CreateDefaultAsciiClient(string COMName)
    {
        return new ModbusAsciiClient().InitSerialPort(p =>
        {
            p.SerialPortName = COMName;
            p.BaudRate = 9600;
            p.DataBit = 8;
            p.StopBit = System.IO.Ports.StopBits.One;
            p.Parity = System.IO.Ports.Parity.None;
            p.ReadTimeSpan = 200;
            p.WriteTimeSpan = 200;
            p.DataFormat = Models.Enums.DataFormat.CDAB;
        });
    }

    public static IModbusTcpClient CreateDefaultTcpClient()
    {
        return new ModbusTcpClient().InitServerPort(p =>
        {
            p.IpAddress = "127.0.0.1";
            p.Port = 502;
            p.IsReconnect = true;
        });
    }
}
