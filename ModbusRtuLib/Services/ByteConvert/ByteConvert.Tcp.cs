using System;

namespace ModbusRtuLib.Services;

partial class ByteConvert
{
    public ushort GetStart(params byte[] data) => Getushort(data);

    ushort Getushort(params byte[] data)
    {
        Array.Reverse(data);
        return BitConverter.ToUInt16(data, 0);
    }
}
