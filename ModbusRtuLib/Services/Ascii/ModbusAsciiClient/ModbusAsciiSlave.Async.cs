using System;
using System.Collections.Generic;
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
            var resultString = await ReadDatasAsync(start, 0x01, start);

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

        public async Task<DataResult<bool>> ReadiscreteAsync(ushort start)
        {
            var resultString = await ReadDatasAsync(start, 0x02, start);
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
    }
}
