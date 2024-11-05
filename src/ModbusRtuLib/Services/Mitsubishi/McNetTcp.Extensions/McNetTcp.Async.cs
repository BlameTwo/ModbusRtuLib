using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Mitsubishi;

partial class McNetTcp
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
        resultbytes.Add((byte)(dataBytes.Count - 1));
        resultbytes.AddRange(dataBytes);
        var resultByte = await this.Device.SendDataAsync(resultbytes.ToArray());

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
        resultbytes.Add((byte)(dataBytes.Count - 1));
        resultbytes.AddRange(dataBytes);
        var resultByte = await this.Device.SendDataAsync(resultbytes.ToArray());
        Thread.Sleep(TimeSpan);
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

    public async Task<DataResult<bool>> ReadBitAsync(string address)
    {
        if (address.IndexOf(".", StringComparison.Ordinal) == -1)
        {
            var result = await this.ReadAsync(address, 0x01);
            byte[] data = new byte[result.ReceivedData.Length];
            result.ReceivedData.CopyTo(data, 0);
            if (CheckData(data, out var formatData, out var message))
            {
                return DataResult<bool>.OK(
                    BitConverter.ToBoolean(formatData),
                    result.OrginSend,
                    data
                );
            }
            return DataResult<bool>.NG(message);
        }
        else
        {
            return await ReadSingleBitAsync(address);
        }
    }

    public async Task<DataResult<bool>> WriteSignleBitAsync(string address, bool v)
    {
        var split = address.Split('.');
        if (split.Length != 2)
        {
            return DataResult<bool>.NG("地址无法解析：.过多");
        }
        if (int.Parse(split[1]) >= 16)
        {
            return DataResult<bool>.NG("超出单个读取的16位限制！");
        }
        var result = await this.ReadInt16Async(split[0]);
        var value = SetBitAtPosition(result.Data, int.Parse(split[1]), v);
        this.Write(value, split[0]);
        return DataResult<bool>.OK(true);
    }

    public async Task<DataResult<bool>> ReadSingleBitAsync(string address)
    {
        var split = address.Split('.');
        if (split.Length != 2)
        {
            return DataResult<bool>.NG("地址无法解析：.过多");
        }
        if (int.Parse(split[1]) >= 16)
        {
            return DataResult<bool>.NG("超出单个读取的16位限制！");
        }
        var result = await this.ReadInt16Async(split[0]);
        var value = GetBitAtPosition(result.Data, int.Parse(split[1]));
        return DataResult<bool>.OK(value);
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

    public async Task<DataResult<bool>> WriteAsync(double value, string address)
    {
        return await this.WriteAsync(address, BitConverter.GetBytes(value), sizeof(double) / 2);
    }

    public Task<DataResult<bool>> WriteBitAsync(string address, bool value)
    {
        if (address.IndexOf(".", StringComparison.Ordinal) == -1)
        {
            byte byteVal = (byte)(value == true ? 0x10 : 0x00);
            return this.WriteAsync(address, [byteVal], 0x01, true);
        }
        else
        {
            return this.WriteSignleBitAsync(address, value);
        }
    }
}
