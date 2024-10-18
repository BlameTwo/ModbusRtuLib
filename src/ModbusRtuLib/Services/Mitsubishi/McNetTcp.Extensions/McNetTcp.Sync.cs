using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Mitsubishi;

partial class McNetTcp
{
    public List<byte> GetHeader() => [0x50, 0x00, NetWorkId, 0xFF, .. DeviceCode, NetWorkSlave];

    public DataResult<bool> Write(string address, byte[] data, ushort length, bool isBit = false)
    {
        List<byte> Resultbytes = GetHeader();
        var dataBytes = new List<byte>();
        dataBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        if (!isBit)
        {
            dataBytes.AddRange([0x00, 0x01]);
            dataBytes.AddRange([0x14, 0x00]);
        }
        else
        {
            dataBytes.AddRange([0x00, 0x01]);
            dataBytes.AddRange([0x14, 0x01]);
        }
        dataBytes.AddRange([0x00]);
        dataBytes.AddRange(Parse.GetStart(address, 1));
        var method = Parse.GetMcType(address);
        var isWord = Parse.IsWordType(method);
        if (length > 1 && isWord == false)
        {
            return DataResult<bool>.NG("该寄存器无法写入当前字节大小");
        }
        dataBytes.Add((byte)method);
        dataBytes.AddRange(Parse.GetLength(method, length));
        dataBytes.AddRange(data);
        Resultbytes.Add((byte)(dataBytes.Count - 1));
        Resultbytes.AddRange(dataBytes);
        var resultByte = this.Device.SendData(Resultbytes.ToArray());

        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[4] == DeviceCode[0]
            && resultByte[5] == DeviceCode[1]
        )
        {
            return DataResult<bool>.OK(true, Resultbytes.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("写入失败！");
    }

    public DataResult<byte[]> Read(string address, ushort length)
    {
        List<byte> Resultbytes = GetHeader();
        var dataBytes = new List<byte>();
        dataBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        var method = Parse.GetMcType(address);

        dataBytes.AddRange([0x00, 0x01, 0x04, 0x00]);
        dataBytes.AddRange([0x00]);
        dataBytes.AddRange(Parse.GetStart(address, 1));
        dataBytes.Add((byte)method);
        dataBytes.AddRange(Parse.GetLength(method, length));
        Resultbytes.Add((byte)(dataBytes.Count - 1));
        Resultbytes.AddRange(dataBytes);
        var resultByte = this.Device.SendData(Resultbytes.ToArray());
        Thread.Sleep(TimeSpan);
        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[4] == DeviceCode[0]
            && resultByte[5] == DeviceCode[1]
        )
        {
            return DataResult<byte[]>.OK(resultByte, Resultbytes.ToArray(), resultByte);
        }
        return DataResult<byte[]>.NG("写入失败！");
    }

    public DataResult<bool> WriteBit(string address, bool value)
    {
        byte byteVal = (byte)(value == true ? 0x10 : 0x00);
        return this.Write(address, [byteVal], 0x01, true);
    }

    public DataResult<bool> ReadBit(string address)
    {
        var result = this.Read(address, 0x01);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<bool>.OK(BitConverter.ToBoolean(formatData), result.OrginSend, data);
        }
        return DataResult<bool>.NG(message);
    }

    public DataResult<int> ReadInt32(string address)
    {
        var result = this.Read(address, 0x02);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<int>.OK(BitConverter.ToInt32(formatData), result.OrginSend, data);
        }
        return DataResult<int>.NG(message);
    }

    public DataResult<float> ReadFloat(string address)
    {
        var result = this.Read(address, 0x02);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<float>.OK(BitConverter.ToSingle(formatData), result.OrginSend, data);
        }
        return DataResult<float>.NG(message);
    }

    public DataResult<double> ReadDouble(string address)
    {
        var result = this.Read(address, 0x04);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<double>.OK(BitConverter.ToDouble(formatData), result.OrginSend, data);
        }
        return DataResult<double>.NG(message);
    }

    public DataResult<long> ReadInt64(string address)
    {
        var result = this.Read(address, 0x04);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<long>.OK(BitConverter.ToInt64(formatData), result.OrginSend, data);
        }
        return DataResult<long>.NG(message);
    }

    public DataResult<short> ReadInt16(string address)
    {
        var result = this.Read(address, 0x01);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<short>.OK(BitConverter.ToInt16(formatData), result.OrginSend, data);
        }
        return DataResult<short>.NG(message);
    }

    private bool CheckData(byte[] data, out byte[] dataResult, out string messageData)
    {
        if (!(data[0] == 0xD0 && data[1] == 0x00))
        {
            messageData = "数据返回头出错";
            dataResult = null;
            return false;
        }
        if (!(data[2] == this.NetWorkId && data[4] == 0xff && data[5] == 0x03))
        {
            messageData = "数据返回头出错";
            dataResult = null;
            return false;
        }
        byte[] length = new byte[2];
        Array.Copy(data, 6, length, 0, 2);
        Array.Reverse(length);
        var Spiltlength = BitConverter.ToInt16(length, 0);
        byte[] resultData = new byte[Spiltlength];
        Array.Copy(data, 7 + 2, resultData, 0, Spiltlength);
        var d = resultData.Skip(2).ToArray();
        dataResult = d;
        messageData = "无错误";
        return true;
    }

    public DataResult<bool> Write(float value, string address)
    {
        return this.Write(address, BitConverter.GetBytes(value), sizeof(float) / 2);
    }

    public DataResult<bool> Write(int value, string address)
    {
        return this.Write(address, BitConverter.GetBytes(value), sizeof(int) / 2);
    }

    public DataResult<bool> Write(long value, string address)
    {
        return this.Write(address, BitConverter.GetBytes(value), sizeof(long) / 2);
    }

    public DataResult<bool> Write(short value, string address)
    {
        return this.Write(address, BitConverter.GetBytes(value), 1);
    }

    public DataResult<bool> Write(double value, string address)
    {
        return this.Write(address, BitConverter.GetBytes(value), sizeof(double) / 2);
    }
}
