using ModbusRtuLib.Contracts.Modbus;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Modbus.Ascii;

public interface IModbusAsciiClient : IModbusClient
{
    System.IO.Ports.SerialPort Port { get; set; }

    SerialPortConfig Config { get; set; }
    public void AddSlave(ModbusSlaveConfig modbusRtuSlaveConfig);
    bool IsConnected { get; }

    public void Close();

    public IModbusAsciiSlave GetSlave(byte slaveId);
}
