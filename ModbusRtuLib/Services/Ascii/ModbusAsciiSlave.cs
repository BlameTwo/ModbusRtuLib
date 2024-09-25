﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Common;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Contracts.Ascii;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Ascii
{
    public class ModbusAsciiSlave : IModbusAsciiSlave
    {
        public ModbusAsciiSlave(SerialPort serialPort, ModbusRtuSlaveConfig config)
        {
            SerialPort = serialPort;
            Config = config;
        }

        IByteConvert ByteConvert = new ByteConvert();

        public SerialPort SerialPort { get; }

        public ModbusRtuSlaveConfig Config { get; }

        public DataResult<bool> ReadCoilSignle(ushort start)
        {
            var resultString = ReadData(start, 0x01, 0x0001);

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

        private byte[] ReadData(ushort start, byte method, ushort length)
        {
            List<byte> strings = new List<byte>();
            strings.Add(0x3A);
            strings.Add(this.Config.SlaveId);
            strings.Add(method);
            strings.AddRange(ByteConvert.GetStart(start));

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

        private Task<byte[]> ReadDataAsync(ushort start, byte method, ushort length)
        {
            return Task.Factory.StartNew(() => ReadData(start, method, length));
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

        public byte[] WriteData(byte method, ushort start, params byte[] bytes)
        {
            List<byte> writeByte = new();
            writeByte.Add(0x3A);
            writeByte.Add(this.Config.SlaveId);
            writeByte.Add(method);
            var bitStart = BitConverter.GetBytes(start);
            Array.Reverse(bitStart);
            writeByte.AddRange(bitStart);
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

        public Task<byte[]> WriteDataAsync(byte method, ushort start, params byte[] bytes)
        {
            return Task.Factory.StartNew(() => WriteData(method, start, bytes));
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

        public async Task<DataResult<bool>> WriteSingleCoilAsync(ushort start, bool value)
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
                resultString = await WriteDataAsync(0x05, start, 0xFF, 0x00);
            }
            else
            {
                writeByte.Add(0x00);
                writeByte.Add(0x00);
                resultString = await WriteDataAsync(0x05, start, 0x00, 0x00);
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

        public async Task<DataResult<bool>> ReadCoilSingleAsync(ushort start)
        {
            var resultString = await ReadDataAsync(start, 0x01, start);

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

        public DataResult<bool> ReadDiscrete(ushort start)
        {
            var resultString = ReadData(start, 0x02, start);
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

        public async Task<DataResult<bool>> ReadiscreteAsync(ushort start)
        {
            var resultString = await ReadDataAsync(start, 0x02, start);
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
}
