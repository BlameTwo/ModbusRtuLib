using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Ascii;

public interface IModbusAsciiClient
{
    System.IO.Ports.SerialPort Port { get; set; }

    SerialPortConfig Config { get; set; }
    public void AddSlave(ModbusRtuSlaveConfig modbusRtuSlaveConfig);
    bool IsConnected { get; }

    public IModbusAsciiSlave GetSlave(byte slaveId);
}
