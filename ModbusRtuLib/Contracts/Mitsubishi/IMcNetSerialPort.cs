using System.IO.Ports;
using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Mitsubishi;

/// <summary>
/// MC Qna3E协议-串口
/// </summary>
public interface IMcNetSerialPort
{
    byte NetWorkId { get; set; }
    byte NetWorkSlave { get; set; }
    IMcNetAddressParse Parse { get; }
    SerialPort Port { get; }
    int TimeSpan { get; set; }
    DataResult<bool> OpenSerial(SerialPortConfig serialPortConfig);
    DataResult<bool> Write(double value, string address);
    Task<DataResult<bool>> WriteAsync(double value, string address);
}
