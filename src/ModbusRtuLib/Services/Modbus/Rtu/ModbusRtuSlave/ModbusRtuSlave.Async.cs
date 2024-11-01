using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Common;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Services.Rtu;

partial class ModbusRtuSlave
{
    public async Task<DataResult<bool>> ReadCoilSingleAsync(ushort start)
    {
        if (!SerialPort.IsOpen)
        {
            return DataResult<bool>.NG("端口未打开");
        }

        var single = await ReadCoilAsync(Config.SlaveId, start, 0001);
        if (single.Item2.Length == 1)
        {
            return DataResult<bool>.OK(
                Convert.ToBoolean(single.Item2[0]),
                single.Item1,
                single.Item2
            );
        }
        return DataResult<bool>.NG("写入错误！");
    }

    async Task<(byte[], byte[])> ReadCoilAsync(byte id, ushort start, ushort length)
    {
        var value = await ReadDataAsync(id, 0x01, start, length);
        if (value.Item2 != null)
        {
            byte[] result = new byte[length];
            Array.Copy(value.Item2, 3, result, 0, length);
            return new(value.Item1, result);
        }
        return new(null, null);
    }

    async Task<(byte[], byte[])> ReadDataAsync(byte id, byte method, ushort start, ushort length)
    {
        if (!Config.IsStartZero)
        {
            start++;
        }
        List<byte> data = new List<byte>();
        data.Add(id);
        data.Add(method);
        data.AddRange(ByteConvert.GetStartBytes(start));
        data.AddRange(ByteConvert.GetStartBytes(length));
        byte[] crc = CRC.Crc16(data.ToArray(), 6);
        data.AddRange(crc);
        await SerialPort.BaseStream.WriteAsync(data.ToArray(), 0, data.Count);
        await Task.Delay(Config.ReadTimeSpan);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        await SerialPort.BaseStream.ReadAsync(resultByte, 0, count);
        var a = CRC.CheckCRC(resultByte, method, id, Config.IsCheckSlave);
        if (a)
        {
            return new(data.ToArray(), resultByte);
        }
        return (null, null);
    }

    public async Task<DataResult<ushort>> ReadHoldingRegisterSingleAsync(ushort start)
    {
        if (!SerialPort.IsOpen)
        {
            return DataResult<ushort>.NG("端口未打开");
        }

        var single = await ReadHoldingRegisterAsync(Config.SlaveId, start, 0001);
        if (single.Item2.Length == 2)
        {
            Array.Reverse(single.Item2);
            return DataResult<ushort>.OK(
                BitConverter.ToUInt16(single.Item2, 0),
                single.Item1,
                single.Item2
            );
        }
        return DataResult<ushort>.NG("写入错误！");
    }

    public async Task<(byte[], byte[])> ReadHoldingRegisterAsync(
        byte id,
        ushort start,
        ushort length
    )
    {
        var value = await ReadDataAsync(id, 0x03, start, length);
        byte[] result = new byte[length * 2];
        Array.Copy(value.Item2, 3, result, 0, length * 2);
        return (value.Item1, result);
    }

    async Task<(byte[], byte[])> ReadDiscreteAsync(byte id, ushort start, ushort length)
    {
        var value = await ReadDataAsync(id, 0x02, start, length);
        if (value.Item2 != null)
        {
            byte[] result = new byte[length];
            Array.Copy(value.Item2, 3, result, 0, length);
            return new(value.Item1, result);
        }
        return new(null, null);
    }

    public async Task<DataResult<bool>> ReadDiscreteSingleAsync(ushort start)
    {
        if (!SerialPort.IsOpen)
        {
            return DataResult<bool>.NG("端口未打开");
        }

        var single = await ReadDiscreteAsync(Config.SlaveId, start, 0001);
        if (single.Item1 == null)
            return DataResult<bool>.NG("错误数据");
        if (single.Item2.Length == 1)
        {
            return DataResult<bool>.OK(
                Convert.ToBoolean(single.Item2[0]),
                single.Item1,
                single.Item2
            );
        }
        return DataResult<bool>.NG("读取错误！");
    }

    public async Task<DataResult<bool>> WriteCoilAsync(ushort start, bool value)
    {
        List<byte> data = new List<byte>();
        data.Add(Config.SlaveId);
        data.Add(0x05);
        data.Add((byte)(start / 256));
        data.Add((byte)(start % 256));
        data.Add((byte)(value ? 0xFF : 0x00));
        data.Add(0);
        byte[] crc = CRC.Crc16(data.ToArray(), 6);
        data.AddRange(crc);
        await SerialPort.BaseStream.WriteAsync(data.ToArray(), 0, data.Count);
        await Task.Delay(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        await SerialPort.BaseStream.ReadAsync(resultByte, 0, count);
        var a = CRC.CheckCRC(resultByte, 0x05, Config.SlaveId, Config.IsCheckSlave);
        if (a)
        {
            return DataResult<bool>.OK(true, data.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("写入错误");
    }

    public async Task<DataResult<bool>> WriteInt16Async(ushort start, short value)
    {
        return await Task.Factory.StartNew(() => WriteInt16(start, value));
    }

    public async Task<DataResult<short>> ReadInt16Async(
        ushort start,
        ReadType readType = ReadType.HoldingRegister
    )
    {
        return await Task.Factory.StartNew(() => ReadInt16(start));
    }

    public async Task<DataResult<bool>> WriteInt32Async(ushort start, int value)
    {
        return await Task.Factory.StartNew(() => WriteInt32(start, value));
    }

    public async Task<DataResult<int>> ReadInt32Async(
        ushort start,
        ReadType readType = ReadType.HoldingRegister
    )
    {
        return await Task.Factory.StartNew(() => ReadInt32(start));
    }

    public async Task<DataResult<bool>> WriteDoubleAsync(ushort start, double value)
    {
        return await Task.Factory.StartNew(() => WriteDouble(start, value));
    }

    public async Task<DataResult<double>> ReadDoubleAsync(
        ushort start,
        ReadType readType = ReadType.HoldingRegister
    )
    {
        return await Task.Factory.StartNew(() => ReadDouble(start));
    }

    public async Task<DataResult<bool>> WriteFloatAsync(ushort start, float value)
    {
        return await Task.Factory.StartNew(() => WriteFloat(start, value));
    }

    public async Task<DataResult<float>> ReadFloatAsync(
        ushort start,
        ReadType readType = ReadType.HoldingRegister
    )
    {
        return await Task.Factory.StartNew(() => ReadFloat(start));
    }

    public async Task<DataResult<bool>> WriteStringAsync(ushort start, string value, int Bytelength)
    {
        return await Task.Factory.StartNew(() => WriteString(start, value, Bytelength));
    }

    public async Task<DataResult<string>> ReadStringAsync(
        ushort start,
        ushort bytelength,
        ReadType readType = ReadType.HoldingRegister
    )
    {
        return await Task.Factory.StartNew(() => ReadString(start, bytelength, readType));
    }

    public async Task<DataResult<bool>> WriteInt64Async(ushort start, long value)
    {
        return await Task.Factory.StartNew(() => WriteInt64(start, value));
    }

    public async Task<DataResult<long>> ReadInt64Async(
        ushort start,
        ReadType readType = ReadType.HoldingRegister
    )
    {
        return await Task.Factory.StartNew(() => ReadInt64(start));
    }
}
