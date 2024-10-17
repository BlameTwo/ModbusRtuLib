using System;
using System.IO.Ports;
using System.Threading.Tasks;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Handlers;

namespace ModbusRtuLib.Contracts.Mitsubishi;

/// <summary>
/// MC Qna3E协议-串口
/// </summary>
public interface IMcNetSerialPort : IDisposable
{
    byte NetWorkId { get; set; }
    byte NetWorkSlave { get; set; }
    IMcNetAddressParse Parse { get; }

    event MitsubishiQna3EConnectChanged ConnectChanged;

    bool IsConnected { get; }

    void Close();

    SerialPort Port { get; }
    int TimeSpan { get; set; }
    DataResult<bool> OpenSerial(SerialPortConfig serialPortConfig);
    DataResult<bool> Write(double value, string address);
    Task<DataResult<bool>> WriteAsync(double value, string address);

    #region Sync
    public DataResult<bool> Write(float value, string address);
    #endregion
}
