﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Common;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Services.Ascii
{
    partial class ModbusAsciiSlave
    {
        public Task<byte[]> WriteDataAsync(byte method, ushort start, params byte[] bytes)
        {
            return Task.Factory.StartNew(() => WriteData(method, start, bytes));
        }

        private Task<byte[]> ReadDatasAsync(ushort start, byte method, ushort length)
        {
            return Task.Factory.StartNew(() => ReadDatas(start, method, length));
        }

        public async Task<DataResult<bool>> ReadDiscreteAsync(ushort start)
        {
            return await Task.Factory.StartNew(() => ReadDiscrete(start));
        }

        public async Task<DataResult<long>> ReadInt64Async(ushort start)
        {
            return await Task.Factory.StartNew(() => ReadInt64(start));
        }

        public async Task<DataResult<float>> ReadFloatAsync(ushort start)
        {
            return await Task.Factory.StartNew(() => ReadFloat(start));
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
            if (Lrc.CheckLrc(resultString))
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
            var resultString = await ReadDatasAsync(start, 0x01, start);

            if (Lrc.CheckLrc(resultString))
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

        public async Task<DataResult<bool>> ReadiscreteAsync(ushort start)
        {
            var resultString = await ReadDatasAsync(start, 0x02, start);
            if (Lrc.CheckLrc(resultString))
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

        public async Task<DataResult<short>> ReadInt16Async(
            ushort start,
            ReadType readType = ReadType.HoldingRegister
        )
        {
            byte[] inputbytes = null;
            if (readType == ReadType.HoldingRegister)
            {
                inputbytes = await ReadDatasAsync(start, 0x03, 0x0001);
            }
            else
            {
                inputbytes = await ReadDatasAsync(start, 0x04, 0x0001);
            }
            var inputbyteResult = new byte[2];
            Array.Copy(inputbytes, 3, inputbyteResult, 0, 2);
            var inputresult = BitConverter.ToInt16(
                ByteConvert.BackInt16(inputbyteResult, this.Config.DataFormat),
                0
            );
            return DataResult<short>.OK(inputresult);
        }

        public async Task<DataResult<bool>> WriteInt16Async(ushort start, short value)
        {
            var bytes = await WriteDataAsync(
                0x06,
                start,
                ByteConvert.ToInt16(new short[] { value }, this.Config.DataFormat)
            );
            return DataResult<bool>.OK(true);
        }

        public async Task<DataResult<bool>> WriteInt32Async(ushort start, int value)
        {
            return await Task.Factory.StartNew(() => WriteInt32(start, value));
        }

        public async Task<DataResult<int>> ReadInt32Async(ushort start)
        {
            return await Task.Factory.StartNew(() => ReadInt32(start));
        }

        public async Task<DataResult<bool>> WriteDoubleAsync(ushort start, double value)
        {
            return await Task.Factory.StartNew(() => this.WriteDouble(start, value));
        }

        public async Task<DataResult<double>> ReadDoubleAsync(ushort start)
        {
            return await Task.Factory.StartNew(() => this.ReadDouble(start));
        }

        public async Task<DataResult<bool>> WriteInt64Async(ushort start, long value)
        {
            var bytes = await this.WriteHoldingRegistersAsync(
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

        public async Task<byte[]> WriteHoldingRegistersAsync(
            ushort start,
            ushort length,
            params byte[] value
        )
        {
            var bytes = await WriteDatasAsync(start, value);
            return bytes;
        }

        public async Task<byte[]> WriteDatasAsync(ushort start, params byte[] data)
        {
            List<byte> writeByte = new();
            writeByte.Add(0x3A);
            writeByte.Add(this.Config.SlaveId);
            writeByte.Add(0x10);
            writeByte.AddRange(ByteConvert.GetStartBytes(start));
            writeByte.AddRange(ByteConvert.GetLength((ushort)(data.Length / 2)));
            writeByte.Add((byte)data.Length);
            writeByte.AddRange(data);
            var lrc = Lrc.LrcCalc(writeByte.ToArray(), 1, writeByte.Count - 1);
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
            await SerialPort.BaseStream.WriteAsync(finalBytes.ToArray(), 0, finalBytes.Count);
            Thread.Sleep(Config.ReadTimeSpan);
            var count = SerialPort.BytesToRead;
            var resultByte = new byte[count];
            await SerialPort.BaseStream.ReadAsync(resultByte, 0, count);
            var resultString = GetAsciiByte(Encoding.ASCII.GetString(resultByte));
            return resultString;
        }

        public async Task<DataResult<bool>> WriteFloatAsync(ushort start, float value)
        {
            var write = await this.WriteHoldingRegistersAsync(
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
    }
}
