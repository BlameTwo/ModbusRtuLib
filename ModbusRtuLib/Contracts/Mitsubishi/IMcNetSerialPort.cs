using System.IO.Ports;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Mitsubishi;

public interface IMcNetSerialPort
{
    byte NetWorkId { get; set; }
    byte NetWorkSlave { get; set; }
    IMcNetAddressParse Parse { get; }
    SerialPort Port { get; }
    int TimeSpan { get; set; }
    DataResult<bool> OpenSerial(SerialPortConfig serialPortConfig);
    DataResult<bool> Write(double value, string address);
}
