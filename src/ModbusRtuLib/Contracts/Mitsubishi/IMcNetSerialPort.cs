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
    DataResult<bool> ReadBit(string address);
    DataResult<bool> WriteBit(string address, bool value);
    DataResult<bool> Write(int value, string address);
    DataResult<bool> Write(long value, string address);
    DataResult<bool> Write(short value, string address);
    DataResult<int> ReadInt32(string address);
    DataResult<float> ReadFloat(string address);
    DataResult<double> ReadDouble(string address);
    DataResult<long> ReadInt64(string address);
    DataResult<short> ReadInt16(string address);

    #endregion

    #region Async
    Task<DataResult<bool>> WriteAsync(float value, string address);
    Task<DataResult<bool>> WriteAsync(int value, string address);
    Task<DataResult<bool>> WriteAsync(long value, string address);
    Task<DataResult<bool>> WriteAsync(short value, string address);
    Task<DataResult<byte[]>> ReadAsync(string address, ushort length);
    Task<DataResult<int>> ReadInt32Async(string address);
    Task<DataResult<float>> ReadFloatAsync(string address);
    Task<DataResult<double>> ReadDoubleAsync(string address);
    Task<DataResult<long>> ReadInt64Async(string address);
    Task<DataResult<short>> ReadInt16Async(string address);
    Task<DataResult<bool>> ReadBitAsync(string address);
    Task<DataResult<bool>> WriteBitAsync(string address, bool value);
    #endregion
}
