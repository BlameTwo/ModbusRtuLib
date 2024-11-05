using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Mitsubishi;

public partial class McNetSerialPort
{
    private async Task<DataResult<bool>> WriteAsync(
        string address,
        byte[] data,
        ushort length,
        bool isBit = false
    )
    {
        List<byte> resultbytes = GetHeader();
        var dataBytes = new List<byte>();
        dataBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        var method = Parse.GetMcType(address);
        var isWord = Parse.IsWordType(method);
        if (isBit)
        {
            dataBytes.AddRange([0x00, 0x01]);
            dataBytes.AddRange([0x14, 0x01]);
        }
        else if (length >= 1)
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
        if (length > 1 && isWord == false)
        {
            return DataResult<bool>.NG("该寄存器无法写入当前字节大小");
        }
        dataBytes.Add((byte)method);
        dataBytes.AddRange(Parse.GetLength(method, length));
        dataBytes.AddRange(data);
        resultbytes.Add((byte)dataBytes.Count);
        resultbytes.AddRange(dataBytes);
        await this.Port.BaseStream.WriteAsync(resultbytes.ToArray(), 0, resultbytes.Count);
        Thread.Sleep(TimeSpan);
        var count = Port.BytesToRead;
        var resultByte = new byte[count];
        _ = await Port.BaseStream.ReadAsync(resultByte, 0, count);
        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[4] == DeviceCode[0]
            && resultByte[5] == DeviceCode[1]
        )
        {
            return DataResult<bool>.OK(true, resultbytes.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("写入失败！");
    }

    public async Task<DataResult<byte[]>> ReadAsync(string address, ushort length)
    {
        List<byte> resultbytes = GetHeader();
        var dataBytes = new List<byte>();
        dataBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        var method = Parse.GetMcType(address);

        dataBytes.AddRange([0x00, 0x01, 0x04, 0x00]);
        dataBytes.AddRange([0x00]);
        dataBytes.AddRange(Parse.GetStart(address, 1));
        dataBytes.Add((byte)method);
        dataBytes.AddRange(Parse.GetLength(method, length));
        resultbytes.Add((byte)dataBytes.Count);
        resultbytes.AddRange(dataBytes);
        await this.Port.BaseStream.WriteAsync(resultbytes.ToArray(), 0, resultbytes.Count);
        Thread.Sleep(TimeSpan);
        var count = Port.BytesToRead;
        var resultByte = new byte[count];
        await Port.BaseStream.ReadAsync(resultByte, 0, count);
        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[4] == DeviceCode[0]
            && resultByte[5] == DeviceCode[1]
        )
        {
            return DataResult<byte[]>.OK(resultByte, resultbytes.ToArray(), resultByte);
        }
        return DataResult<byte[]>.NG("写入失败！");
    }

    public async Task<DataResult<bool>> WriteAsync(double value, string address)
    {
        return await this.WriteAsync(address, BitConverter.GetBytes(value), 0x04);
    }

    public async Task<DataResult<bool>> WriteAsync(float value, string address)
    {
        return await this.WriteAsync(address, BitConverter.GetBytes(value), sizeof(float) / 2);
    }

    public async Task<DataResult<bool>> WriteAsync(int value, string address)
    {
        return await this.WriteAsync(address, BitConverter.GetBytes(value), sizeof(int) / 2);
    }

    public async Task<DataResult<bool>> WriteAsync(long value, string address)
    {
        return await this.WriteAsync(address, BitConverter.GetBytes(value), sizeof(long) / 2);
    }

    public async Task<DataResult<bool>> WriteAsync(short value, string address)
    {
        return await this.WriteAsync(address, BitConverter.GetBytes(value), 1);
    }

    public async Task<DataResult<int>> ReadInt32Async(string address)
    {
        var result = await this.ReadAsync(address, 0x02);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<int>.OK(BitConverter.ToInt32(formatData), result.OrginSend, data);
        }
        return DataResult<int>.NG(message);
    }

    public async Task<DataResult<bool>> ReadBitAsync(string address)
    {
        List<byte> resultbytes = GetHeader();
        List<byte> addBytes = new List<byte>();
        addBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        addBytes.AddRange([0x00, 0x01, 0x04, 0x01]);
        addBytes.Add(0x00);
        addBytes.AddRange(Parse.GetStart(address, 1));
        var method = Parse.GetMcType(address);
        addBytes.Add((byte)method);
        addBytes.AddRange(Parse.GetLength(method, 1));
        resultbytes.Add((byte)addBytes.Count);
        resultbytes.AddRange(addBytes);
        await this.Port.BaseStream.WriteAsync(resultbytes.ToArray(), 0, resultbytes.Count);
        Thread.Sleep(TimeSpan);
        var count = Port.BytesToRead;
        var resultByte = new byte[count];
        _ = await Port.BaseStream.ReadAsync(resultByte, 0, count);
        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[4] == 0xFF
            && resultByte[5] == 0x03
        )
        {
            var result = BitConverter.ToBoolean(resultByte, resultByte.Length - 1);
            return DataResult<bool>.OK(result, resultbytes.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("读取校验失败！");
    }

    public async Task<DataResult<bool>> WriteBitAsync(string address, bool value)
    {
        byte byteVal = (byte)(value == true ? 0x10 : 0x00);
        return await this.WriteAsync(address, [byteVal], 0x01, true);
    }

    public async Task<DataResult<float>> ReadFloatAsync(string address)
    {
        var result = await this.ReadAsync(address, 0x02);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<float>.OK(BitConverter.ToSingle(formatData), result.OrginSend, data);
        }
        return DataResult<float>.NG(message);
    }

    public async Task<DataResult<double>> ReadDoubleAsync(string address)
    {
        var result = await this.ReadAsync(address, 0x04);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<double>.OK(BitConverter.ToDouble(formatData), result.OrginSend, data);
        }
        return DataResult<double>.NG(message);
    }

    public async Task<DataResult<long>> ReadInt64Async(string address)
    {
        var result = await this.ReadAsync(address, 0x04);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<long>.OK(BitConverter.ToInt64(formatData), result.OrginSend, data);
        }
        return DataResult<long>.NG(message);
    }

    public async Task<DataResult<short>> ReadInt16Async(string address)
    {
        var result = await this.ReadAsync(address, 0x01);
        byte[] data = new byte[result.ReceivedData.Length];
        result.ReceivedData.CopyTo(data, 0);
        if (CheckData(data, out var formatData, out var message))
        {
            return DataResult<short>.OK(BitConverter.ToInt16(formatData), result.OrginSend, data);
        }
        return DataResult<short>.NG(message);
    }
}
