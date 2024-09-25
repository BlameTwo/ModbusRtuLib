using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
                var value = resultString[3];
                return DataResult<bool>.OK(Convert.ToBoolean(value));
            }

            return DataResult<bool>.NG("LRC 校验错误");
        }

        private byte[] ReadData(ushort start, byte method, ushort length)
        {
            List<byte> strings = new List<byte>();
            strings.Add(0x3A);
            strings.Add(this.Config.SlaveId);
            strings.Add(method);
            var bitStart = BitConverter.GetBytes(start);
            Array.Reverse(bitStart);
            strings.AddRange(bitStart);
            var bitlength = BitConverter.GetBytes(start);
            Array.Reverse(bitlength);
            strings.AddRange(bitlength);
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

        private static byte CalculateLrc(List<string> messageParts)
        {
            byte[] messageBytes = new byte[messageParts.Count * 2 - 1];

            int index = 0;
            foreach (string part in messageParts)
            {
                if (index == 0 || index == messageParts.Count - 1) // 忽略起始字符和终止字符
                {
                    index++;
                    continue;
                }

                byte[] bytes = Enumerable
                    .Range(0, part.Length / 2)
                    .Select(x => Convert.ToByte(part.Substring(x * 2, 2), 16))
                    .ToArray();
                Array.Copy(bytes, 0, messageBytes, index, bytes.Length);
                index += bytes.Length;
            }

            byte lrc = 0;
            foreach (byte b in messageBytes)
            {
                lrc ^= b;
            }

            return lrc;
        }
    }
}
