using ModbusRtuLib.Contracts;
using ModbusRtuLib.Contracts.Ascii;
using ModbusRtuLib.Contracts.Rtu;
using ModbusRtuLib.Services.Ascii;
using ModbusRtuLib.Services.Rtu;

namespace ModbusRtuLib.Common
{
    public static class ModBusFactory
    {
        /// <summary>
        /// 默认串口
        /// </summary>
        /// <returns></returns>
        public static IModbusRtuClient CreateDefaultRtuClient(string COMName)
        {
            return new ModbusRtuClient().InitSerialPort(p =>
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
    }
}
