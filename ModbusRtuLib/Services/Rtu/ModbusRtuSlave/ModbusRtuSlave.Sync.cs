using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Common;
using ModbusRtuLib.Contracts.Rtu;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Rtu;

public partial class ModbusRtuSlave : IModbusRtuSlave
{
    public DataResult<bool> ReadCoilSingle(ushort start)
    {
        if (!SerialPort.IsOpen)
        {
            return DataResult<bool>.NG("端口未打开");
        }

        var single = ReadCoil(Config.SlaveId, start, 0001);
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

    public async Task<DataResult<bool>> ReadCoilSingleAsync(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadCoilSingle(start));
    }

    public DataResult<ushort> ReadHoldingRegisterSingle(ushort start)
    {
        if (!SerialPort.IsOpen)
        {
            return DataResult<ushort>.NG("端口未打开");
        }

        var single = ReadHoldingRegister(Config.SlaveId, start, 0001);
        if (single.Length == 2)
        {
            Array.Reverse(single);
            return DataResult<ushort>.OK(BitConverter.ToUInt16(single, 0));
        }
        return DataResult<ushort>.NG("写入错误！");
    }

    public DataResult<bool> ReadDiscreteSingle(ushort start)
    {
        if (!SerialPort.IsOpen)
        {
            return DataResult<bool>.NG("端口未打开");
        }

        var single = ReadDiscrete(Config.SlaveId, start, 0001);
        if (single.Length == 1)
        {
            return DataResult<bool>.OK(Convert.ToBoolean(single[0]));
        }
        return DataResult<bool>.NG("写入错误！");
    }

    public byte[] ReadHoldingRegister(byte id, ushort start, ushort length)
    {
        var value = ReadData(id, 0x03, start, length);
        byte[] result = new byte[length * 2];
        Array.Copy(value.Item2, 3, result, 0, length * 2);
        return result;
    }

    (byte[], byte[]) ReadCoil(byte id, ushort start, ushort length)
    {
        var value = ReadData(id, 0x01, start, length);
        if (value.Item2 != null)
        {
            byte[] result = new byte[length];
            Array.Copy(value.Item2, 3, result, 0, length);
            return new(value.Item1, result);
        }
        return new(null, null);
    }

    byte[] ReadDiscrete(byte id, ushort start, ushort length)
    {
        var value = ReadData(id, 0x02, start, length);
        if (value.Item2 != null)
        {
            byte[] result = new byte[length];
            Array.Copy(value.Item2, 3, result, 0, length);
            return result;
        }
        return new byte[length];
    }

    (byte[], byte[]) ReadData(byte id, byte method, ushort start, ushort length)
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
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(Config.ReadTimeSpan);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        SerialPort.Read(resultByte, 0, count);
        var a = CRC.CheckCRC(resultByte, method, id, Config.IsCheckSlave);
        if (a)
        {
            return new(data.ToArray(), resultByte);
        }
        return (null, null);
    }

    /// <summary>
    /// 写入线圈
    /// </summary>
    /// <param name="id"></param>
    /// <param name="start"></param>
    /// <param name="value"></param>
    public bool WriteCoil(ushort start, bool value)
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
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        SerialPort.Read(resultByte, 0, count);
        var a = CRC.CheckCRC(resultByte, 0x05, Config.SlaveId, Config.IsCheckSlave);
        if (a)
        {
            return true;
        }
        return false;
    }

    public bool WriteDouble(ushort start, double value)
    {
        var byteValue = BitConverter.GetBytes(value);
        List<byte> data = new List<byte>();
        data.Add(Config.SlaveId);
        data.Add(0x10);
        data.Add((byte)(start / 256));
        data.Add((byte)(start % 256));
        data.Add(0x00);
        data.Add(0x04);
        data.Add(8);
        data.AddRange(ByteConvert.ToDouble([value], Config.DataFormat));
        byte[] crc = CRC.Crc16(data.ToArray(), 15);
        data.AddRange(crc);
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        return true;
    }

    public bool WriteInt16(short start, short value)
    {
        List<byte> data = new List<byte>();
        data.Add(Config.SlaveId);
        data.Add(0x06);
        data.Add((byte)(start / 256));
        data.Add((byte)(start % 256));
        data.AddRange(ByteConvert.ToInt16([value], Config.DataFormat));
        byte[] crc = CRC.Crc16(data.ToArray(), 6);
        data.AddRange(crc);
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        SerialPort.Read(resultByte, 0, count);
        var a = CRC.CheckCRC(resultByte, 0x06, Config.SlaveId, Config.IsCheckSlave);
        if (a)
        {
            return true;
        }
        return false;
    }

    public void WriteFloat(short start, float value)
    {
        var byteValue = BitConverter.GetBytes(value);
        List<byte> data = new List<byte>();
        data.Add(Config.SlaveId);
        data.Add(0x10);
        data.Add((byte)(start / 256));
        data.Add((byte)(start % 256));
        data.Add(0x00);
        data.Add(0x02);
        data.Add(4);
        data.AddRange(ByteConvert.ToFloat([value], Config.DataFormat));
        byte[] crc = CRC.Crc16(data.ToArray(), 11);
        data.AddRange(crc);
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
    }

    public void WriteLong(short start, long value)
    {
        var byteValue = BitConverter.GetBytes(value);
        List<byte> data = new List<byte>();
        data.Add(Config.SlaveId);
        data.Add(0x10);
        data.Add((byte)(start / 256));
        data.Add((byte)(start % 256));
        data.Add(0x00);
        data.Add(0x04);
        data.Add(8);
        data.AddRange(ByteConvert.ToLong([value], Config.DataFormat));
        byte[] crc = CRC.Crc16(data.ToArray(), 15);
        data.AddRange(crc);
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
    }

    public DataResult<long> ReadLong(ushort start)
    {
        var result = ReadHoldingRegister(Config.SlaveId, start, 4);
        var l = ByteConvert.BackLong(result, Config.DataFormat);
        var result2 = BitConverter.ToInt64(l, 0);
        return DataResult<long>.OK(result2);
    }

    public DataResult<float> ReadFloat(ushort start)
    {
        var result = ReadHoldingRegister(Config.SlaveId, start, 2);
        var l = ByteConvert.BackFloat(result, Config.DataFormat);
        var result2 = BitConverter.ToSingle(l, 0);
        return DataResult<float>.OK(result2);
    }

    public DataResult<short> ReadInt16(ushort start)
    {
        var result = ReadHoldingRegister(Config.SlaveId, start, 1);
        var l = ByteConvert.BackInt16(result, Config.DataFormat);
        var result2 = BitConverter.ToInt16(l, 0);
        return DataResult<short>.OK(result2);
    }

    public DataResult<double> ReadDouble(ushort start)
    {
        var result = ReadHoldingRegister(Config.SlaveId, start, 4);
        var l = ByteConvert.BackDouble(result, Config.DataFormat);
        var result2 = BitConverter.ToDouble(l, 0);
        return DataResult<double>.OK(result2);
    }

    public void WriteString(ushort start, string value, int Bytelength)
    {
        var stringBytes = Config.StringEncoding.GetBytes(value);
        if (stringBytes.Length > Bytelength)
        {
            return;
        }
        List<byte> data = new List<byte>();
        data.Add(Config.SlaveId);
        data.Add(0x10);
        data.Add((byte)(start / 256));
        data.Add((byte)(start % 256));
        var bytes = ByteConvert.ToStringWorld(
            Config.StringEncoding.PadToLength(value, Bytelength),
            Config.DataFormat
        );
        ushort Ulength = (ushort)bytes.Length;
        data.Add((byte)(Ulength / 256));
        data.Add((byte)(Ulength % 256));
        data.Add((byte)bytes.Length);
        data.AddRange(bytes);
        byte[] crc = CRC.Crc16(data.ToArray(), data.Count);
        data.AddRange(crc);
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        SerialPort.Read(resultByte, 0, count);
        var a = CRC.CheckCRC(resultByte, 0x10, Config.SlaveId, Config.IsCheckSlave);
    }

    public DataResult<string> ReadString(ushort start, ushort length)
    {
        var result = ReadHoldingRegister(Config.SlaveId, start, length);
        var l = ByteConvert.ToStringWorld(result, Config.DataFormat);
        var str = Config.StringEncoding.GetString(l);
        return DataResult<string>.OK(str);
    }

    public DataResult<bool> WriteInt32(ushort start, int value)
    {
        var byteValue = BitConverter.GetBytes(value);
        List<byte> data = new List<byte>();
        data.Add(Config.SlaveId);
        data.Add(0x10);
        data.AddRange(ByteConvert.GetStartBytes(start));
        data.AddRange(ByteConvert.GetLength(0x0002));
        data.Add(4);
        data.AddRange(ByteConvert.ToInt32([value], Config.DataFormat));
        byte[] crc = CRC.Crc16(data.ToArray(), 11);
        data.AddRange(crc);
        SerialPort.Write(data.ToArray(), 0, data.Count);
        Thread.Sleep(200);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        return DataResult<bool>.OK(true);
    }

    public DataResult<int> ReadInt32(ushort start)
    {
        var bytes = this.ReadHoldingRegister(this.Config.SlaveId, start, 0002);
        var floatByte = ByteConvert.BackInt32(bytes, Config.DataFormat);
        var f = BitConverter.ToInt32(floatByte, 0);
        return DataResult<int>.OK(f);
    }
}
