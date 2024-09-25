using System;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Ascii;

partial class ModbusAsciiSlave
{
    public DataResult<float> ReadFloat(ushort start)
    {
        var bytes = this.ReadHoldingRegisters(start, 2);
        var floatByte = ByteConvert.BackFloat(bytes, Config.DataFormat);
        var f = BitConverter.ToSingle(floatByte, 0);
        return DataResult<float>.OK(f);
    }

    public DataResult<bool> WriteFloat(ushort start, float value)
    {
        var write = this.WriteHoldingRegisters(
            start,
            2,
            ByteConvert.ToFloat([value], Config.DataFormat)
        );
        if (write[0] == this.Config.SlaveId && write[1] == 0x10)
        {
            return DataResult<bool>.OK(true);
        }
        return DataResult<bool>.NG("写入功能码与站号不同！");
    }

    public DataResult<bool> WriteInt32(ushort start, int value)
    {
        var bytes = this.WriteHoldingRegisters(
            start,
            2,
            ByteConvert.ToInt32([value], Config.DataFormat)
        );
        if (bytes[0] == this.Config.SlaveId && bytes[1] == 0x10)
        {
            return DataResult<bool>.OK(true);
        }
        return DataResult<bool>.NG("写入功能码与站号不同！");
    }

    public DataResult<int> ReadInt32(ushort start)
    {
        var bytes = this.ReadHoldingRegisters(start, 2);
        var floatByte = ByteConvert.BackInt32(bytes, Config.DataFormat);
        var f = BitConverter.ToInt32(floatByte, 0);
        return DataResult<int>.OK(f);
    }
}
