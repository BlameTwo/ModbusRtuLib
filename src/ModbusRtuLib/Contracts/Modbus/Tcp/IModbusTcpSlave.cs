using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Modbus.Tcp;

public interface IModbusTcpSlave
{
    public ModbusSlaveConfig Config { get; }

    public DataResult<bool> ReadCoil(ushort start);
}
