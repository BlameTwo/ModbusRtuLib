using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Ascii
{
    public interface IModbusAsciiSlave
    {
        public DataResult<bool> ReadCoilSignle(ushort start);
    }
}
