using System;
using System.Text;
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

    public DataResult<bool> WriteInt64(ushort start, long value)
    {
        var bytes = this.WriteHoldingRegisters(
            start,
            2,
            ByteConvert.ToLong([value], Config.DataFormat)
        );
        if (bytes[0] == this.Config.SlaveId && bytes[1] == 0x10)
        {
            return DataResult<bool>.OK(true);
        }
        return DataResult<bool>.NG("写入功能码与站号不同！");
    }

    public DataResult<long> ReadInt64(ushort start)
    {
        var bytes = this.ReadHoldingRegisters(start, 4);
        var floatByte = ByteConvert.BackLong(bytes, Config.DataFormat);
        var f = BitConverter.ToInt64(floatByte, 0);
        return DataResult<long>.OK(f);
    }

    public DataResult<bool> WriteString(ushort start, string value, ushort length)
    {
        byte[] bytes = null;
        if (Config.ReverseString == false)
        {
            bytes = this.WriteHoldingRegisters(start, length, Encoding.ASCII.GetBytes(value));
        }
        else
        {
            bytes = this.WriteHoldingRegisters(
                start,
                length,
                ByteConvert.ToStringWorld(Encoding.ASCII.GetBytes(value), Config.DataFormat)
            );
        }
        if (bytes[0] == this.Config.SlaveId && bytes[1] == 0x10)
        {
            return DataResult<bool>.OK(true);
        }
        return DataResult<bool>.NG("写入功能码与站号不同！");
    }

    public DataResult<string> ReadString(ushort start, ushort length)
    {
        var bytes = this.ReadHoldingRegisters(start, length);
        if (Config.ReverseString == false)
        {
            var f = Encoding.ASCII.GetString(bytes);
            return DataResult<string>.OK(f);
        }
        else
        {
            var word = Encoding.ASCII.GetString(
                ByteConvert.ToStringWorld(bytes, this.Config.DataFormat)
            );
            return DataResult<string>.OK(word);
        }
    }

    public DataResult<bool> WriteDouble(ushort start, double value)
    {
        var bytes = this.WriteHoldingRegisters(
            start,
            4,
            ByteConvert.ToDouble([value], Config.DataFormat)
        );
        if (bytes[0] == this.Config.SlaveId && bytes[1] == 0x10)
        {
            return DataResult<bool>.OK(true);
        }
        return DataResult<bool>.NG("写入功能码与站号不同！");
    }
}
