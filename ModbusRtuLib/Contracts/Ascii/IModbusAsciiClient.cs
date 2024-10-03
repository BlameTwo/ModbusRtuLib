using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Ascii;

public interface IModbusAsciiClient : IModbusClient
{
    System.IO.Ports.SerialPort Port { get; set; }

    SerialPortConfig Config { get; set; }
    public void AddSlave(ModbusSlaveConfig modbusRtuSlaveConfig);
    bool IsConnected { get; }

    public void Close();

    public IModbusAsciiSlave GetSlave(byte slaveId);
}
