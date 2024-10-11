using System;
using System.Collections.Generic;
using System.Threading;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Mitsubishi;

public partial class McNetSerialPort
{
    public DataResult<bool> Write(string address, byte[] data, ushort length)
    {
        List<byte> Resultbytes = GetHeader();
        //后续数据长度
        var dataBytes = new List<byte>();
        dataBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        if (length > 1)
        {
            dataBytes.AddRange(new byte[] { 0x00, 0x01 });
            dataBytes.AddRange(new byte[] { 0x14, 0x00 });
        }
        else
        {
            dataBytes.AddRange(new byte[] { 0x00, 0x01 });
            dataBytes.AddRange(new byte[] { 0x14, 0x01 });
        }

        dataBytes.AddRange(new byte[] { 0x00 });
        dataBytes.AddRange(Parse.GetStart(address, 1));
        var method = Parse.GetMcType(address);
        dataBytes.Add((byte)method);
        dataBytes.AddRange(Parse.GetLength(method, length));
        dataBytes.AddRange(data);
        Resultbytes.Add((byte)dataBytes.Count);
        Resultbytes.AddRange(dataBytes);
        this.Port.Write(Resultbytes.ToArray(), 0, Resultbytes.Count);
        Thread.Sleep(TimeSpan);
        var count = Port.BytesToRead;
        var resultByte = new byte[count];
        Port.Read(resultByte, 0, count);
        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[4] == 0xFF
            && resultByte[5] == 0x03
        )
        {
            return DataResult<bool>.OK(true, Resultbytes.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("写入失败！");
    }

    public DataResult<bool> ReadBit(string address)
    {
        List<byte> Resultbytes = GetHeader();
        List<byte> addBytes = new List<byte>();
        addBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        addBytes.AddRange([0x00, 0x01, 0x04, 0x01]);
        addBytes.Add(0x00);
        addBytes.AddRange(Parse.GetStart(address, 1));
        var method = Parse.GetMcType(address);
        addBytes.Add((byte)method);
        addBytes.AddRange(Parse.GetLength(method, 1));
        Resultbytes.Add((byte)addBytes.Count);
        Resultbytes.AddRange(addBytes);
        this.Port.Write(Resultbytes.ToArray(), 0, Resultbytes.Count);
        Thread.Sleep(TimeSpan);
        var count = Port.BytesToRead;
        var resultByte = new byte[count];
        Port.Read(resultByte, 0, count);
        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[4] == 0xFF
            && resultByte[5] == 0x03
        )
        {
            var result = BitConverter.ToBoolean(resultByte, resultByte.Length - 1);
            return DataResult<bool>.OK(result, Resultbytes.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("读取校验失败！");
    }

    public DataResult<bool> WriteBit(string address, bool value)
    {
        byte byteVal = (byte)(value == true ? 0x10 : 0x00);
        return this.Write(address, [byteVal], 0x01);
    }

    public DataResult<bool> Write(double value, string address)
    {
        return this.Write(address, BitConverter.GetBytes(value), 0x04);
    }
}
