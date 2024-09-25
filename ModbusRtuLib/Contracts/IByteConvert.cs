using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Contracts;

/// <summary>
/// 字节序转换
/// </summary>
public interface IByteConvert
{
    public byte[] ToInt16(short[] values, DataFormat dataFormat);

    public byte[] ToFloat(float[] values, DataFormat dataFormat);

    public byte[] ToLong(long[] values, DataFormat dataFormat);

    public byte[] ToDouble(double[] values, DataFormat dataFormat);

    public byte[] ToWord(byte[] byteValues, DataFormat dataFormat);

    public byte[] ToStringWorld(byte[] byteValues, DataFormat dataFormat);

    public byte[] BackLong(byte[] values, DataFormat dataFormat);

    public byte[] BackFloat(byte[] bytes, DataFormat dataFormat);

    public byte[] BackInt16(byte[] bytes, DataFormat dataFormat);

    public byte[] BackDouble(byte[] bytes, DataFormat dataFormat);

    public byte[] GetStart(ushort value);

    public byte[] GetLength(ushort value);
}
