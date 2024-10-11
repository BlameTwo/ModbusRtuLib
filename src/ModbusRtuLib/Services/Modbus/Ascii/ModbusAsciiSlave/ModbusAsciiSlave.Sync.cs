using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ModbusRtuLib.Common;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;

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

    public DataResult<double> ReadDouble(ushort start)
    {
        byte[] inputbytes = null;
        inputbytes = ReadHoldingRegisters(start, 0x0004);
        var inputresult = BitConverter.ToDouble(
            ByteConvert.BackDouble(inputbytes, this.Config.DataFormat),
            0
        );
        return DataResult<double>.OK(inputresult);
    }

    public DataResult<short> ReadInt16(ushort start, ReadType readType = ReadType.HoldingRegister)
    {
        byte[] inputbytes = null;
        if (readType == ReadType.HoldingRegister)
        {
            inputbytes = ReadDatas(start, 0x03, 0x0001);
        }
        else
        {
            inputbytes = ReadDatas(start, 0x04, 0x0001);
        }
        var inputbyteResult = new byte[2];
        Array.Copy(inputbytes, 3, inputbyteResult, 0, 2);
        var inputresult = BitConverter.ToInt16(
            ByteConvert.BackInt16(inputbyteResult, this.Config.DataFormat),
            0
        );
        return DataResult<short>.OK(inputresult);
    }

    public DataResult<bool> WriteInt16(ushort start, short value)
    {
        var bytes = WriteData(
            0x06,
            start,
            ByteConvert.ToInt16(new short[] { value }, this.Config.DataFormat)
        );
        return DataResult<bool>.OK(true);
    }

    public DataResult<bool> ReadCoilSignle(ushort start)
    {
        var resultString = ReadDatas(start, 0x01, 0x0001);

        if (LRC.CheckLRC(resultString))
        {
            if (resultString[0] == 0x01)
            {
                if (resultString.Length != 4)
                {
                    return DataResult<bool>.OK(Convert.ToBoolean(resultString[3]));
                }
                else
                {
                    return DataResult<bool>.NG($"错误代码：{resultString[2]}");
                }
            }
        }

        return DataResult<bool>.NG("LRC 校验错误");
    }

    private byte[] ReadDatas(ushort start, byte method, ushort length)
    {
        List<byte> strings = new List<byte>();
        strings.Add(0x3A);
        strings.Add(this.Config.SlaveId);
        strings.Add(method);
        strings.AddRange(ByteConvert.GetStartBytes(start));
        strings.AddRange(ByteConvert.GetLength(length));
        var lrc = LRC.LRCCalc(strings.ToArray(), 1, 6);
        strings.Add(lrc);
        List<string> sendValue = new List<string>();
        foreach (var item in strings)
        {
            if (item == 0x3A)
            {
                sendValue.Add(":");
                continue;
            }
            sendValue.Add(item.ToString("X2"));
        }
        var strResult = string.Join("", sendValue);
        var aa = Encoding.ASCII.GetBytes(strResult);
        List<byte> finalBytes = new List<byte>(aa);
        finalBytes.Add(0x0D);
        finalBytes.Add(0x0A);
        SerialPort.Write(finalBytes.ToArray(), 0, finalBytes.Count);
        Thread.Sleep(Config.ReadTimeSpan);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        SerialPort.Read(resultByte, 0, count);
        var resultString = GetAsciiByte(Encoding.ASCII.GetString(resultByte));
        return resultString;
    }

    byte[] GetAsciiByte(string orginText)
    {
        string dataPart = orginText.Substring(1).Replace("\r\n", "");

        byte[] bytesSequence = new byte[dataPart.Length / 2];
        for (int i = 0; i < dataPart.Length; i += 2)
        {
            bytesSequence[i / 2] = Convert.ToByte(dataPart.Substring(i, 2), 16);
        }
        return bytesSequence;
    }

    public byte[] ReadHoldingRegisters(ushort start, ushort length)
    {
        var bytes = ReadDatas(start, 0x03, length);
        byte[] result = new byte[length * 2];
        Array.Copy(bytes, 3, result, 0, length * 2);
        return result;
    }

    public byte[] WriteHoldingRegisters(ushort start, ushort length, params byte[] value)
    {
        var bytes = WriteDatas(start, value);
        return bytes;
    }

    public DataResult<bool> WriteSingleCoil(ushort start, bool value)
    {
        List<byte> writeByte = new();
        writeByte.Add(0x3A);
        writeByte.Add(this.Config.SlaveId);
        writeByte.Add(0x05);
        var bitStart = BitConverter.GetBytes(start);
        Array.Reverse(bitStart);
        writeByte.AddRange(bitStart);
        byte[] resultString = null;
        if (value)
        {
            writeByte.Add(0xFF);
            writeByte.Add(0x00);
            resultString = WriteData(0x05, start, 0xFF, 0x00);
        }
        else
        {
            writeByte.Add(0x00);
            writeByte.Add(0x00);
            resultString = WriteData(0x05, start, 0x00, 0x00);
        }
        if (LRC.CheckLRC(resultString))
        {
            if (resultString[1] == 0x05 && resultString[0] == Config.SlaveId)
            {
                if (resultString.Length != 4)
                {
                    return DataResult<bool>.OK(Convert.ToBoolean(true));
                }
                else
                {
                    return DataResult<bool>.NG($"错误代码：{resultString[2]}");
                }
            }
        }
        return DataResult<bool>.NG("LRC校验错误");
    }

    public byte[] WriteData(byte method, ushort start, params byte[] bytes)
    {
        List<byte> writeByte = new();
        writeByte.Add(0x3A);
        writeByte.Add(this.Config.SlaveId);
        writeByte.Add(method);
        writeByte.AddRange(ByteConvert.GetStartBytes(start));
        foreach (var item in bytes)
        {
            writeByte.Add(item);
        }
        var lrc = LRC.LRCCalc(writeByte.ToArray(), 1, writeByte.Count - 1);
        writeByte.Add(lrc);
        List<string> sendValue = new List<string>();
        foreach (var item in writeByte)
        {
            if (item == 0x3A)
            {
                sendValue.Add(":");
                continue;
            }
            sendValue.Add(item.ToString("X2"));
        }
        var strResult = string.Join("", sendValue);
        var aa = Encoding.ASCII.GetBytes(strResult);
        List<byte> finalBytes = new List<byte>(aa);
        finalBytes.Add(0x0D);
        finalBytes.Add(0x0A);
        SerialPort.Write(finalBytes.ToArray(), 0, finalBytes.Count);
        Thread.Sleep(Config.ReadTimeSpan);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        SerialPort.Read(resultByte, 0, count);
        var resultString = GetAsciiByte(Encoding.ASCII.GetString(resultByte));
        return resultString;
    }

    public byte[] WriteDatas(ushort start, params byte[] data)
    {
        List<byte> writeByte = new();
        writeByte.Add(0x3A);
        writeByte.Add(this.Config.SlaveId);
        writeByte.Add(0x10);
        writeByte.AddRange(ByteConvert.GetStartBytes(start));
        writeByte.AddRange(ByteConvert.GetLength((ushort)(data.Length / 2)));
        writeByte.Add((byte)data.Length);
        writeByte.AddRange(data);
        var lrc = LRC.LRCCalc(writeByte.ToArray(), 1, writeByte.Count - 1);
        writeByte.Add(lrc);
        List<string> sendValue = new List<string>();
        foreach (var item in writeByte)
        {
            if (item == 0x3A)
            {
                sendValue.Add(":");
                continue;
            }
            sendValue.Add(item.ToString("X2"));
        }
        var strResult = string.Join("", sendValue);
        var aa = Encoding.ASCII.GetBytes(strResult);
        List<byte> finalBytes = new List<byte>(aa);
        finalBytes.Add(0x0D);
        finalBytes.Add(0x0A);
        SerialPort.Write(finalBytes.ToArray(), 0, finalBytes.Count);
        Thread.Sleep(Config.ReadTimeSpan);
        var count = SerialPort.BytesToRead;
        var resultByte = new byte[count];
        SerialPort.Read(resultByte, 0, count);
        var resultString = GetAsciiByte(Encoding.ASCII.GetString(resultByte));
        return resultString;
    }

    public DataResult<bool> ReadDiscrete(ushort start)
    {
        var resultString = ReadDatas(start, 0x02, start);
        if (LRC.CheckLRC(resultString))
        {
            if (resultString.Length != 4)
            {
                return DataResult<bool>.OK(Convert.ToBoolean(resultString[3]));
            }
            else
            {
                return DataResult<bool>.NG($"错误代码：{resultString[2]}");
            }
        }

        return DataResult<bool>.NG("LRC校验失败");
    }
}
