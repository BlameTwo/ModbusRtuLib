using System;
using System.Collections;
using System.Collections.Generic;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Services;

/// <summary>
/// 字节序转换
/// </summary>
public sealed class ByteConvert : IByteConvert
{
    public byte[] ToFloat(float[] values, DataFormat dataFormat)
    {
        List<byte> byteResult = new List<byte>();
        for (int i = 0; i < values.Length; i++)
        {
            var byteValues = BitConverter.GetBytes(values[i]);
            byteResult.AddRange(AdjustByteOrder(byteValues, dataFormat));
        }
        return byteResult.ToArray();
    }

    public byte[] ToInt16(short[] values, DataFormat dataFormat)
    {
        List<byte> byteResult = new List<byte>();
        for (int i = 0; i < values.Length; i++)
        {
            var byteValues = BitConverter.GetBytes(values[i]);
            byteResult.AddRange(ToWord(byteValues, dataFormat));
        }
        return byteResult.ToArray();
    }

    public byte[] ToDouble(double[] values, DataFormat dataFormat)
    {
        List<byte> bytesResult = new List<byte>();
        for (int i = 0; i < values.Length; i++)
        {
            var byteValues = BitConverter.GetBytes(values[i]);
            var bytes = AdjustByteOrder(byteValues, dataFormat);
            bytesResult.AddRange(bytes);
        }
        return bytesResult.ToArray();
    }

    /// <summary>
    /// 转字节数据
    /// </summary>
    /// <param name="byteValues">原始byte数据</param>
    /// <param name="dataFormat">格式化方式</param>
    /// <returns></returns>
    public byte[] ToWord(byte[] byteValues, DataFormat dataFormat)
    {
        List<byte> byteResult = new();
        if (byteValues.Length == 4)
        {
            switch (dataFormat)
            {
                case DataFormat.ABCD:
                    byteResult.AddRange(
                        [byteValues[3], byteValues[2], byteValues[1], byteValues[0]]
                    );
                    break;
                case DataFormat.CDAB:
                    byteResult.AddRange(
                        [byteValues[1], byteValues[0], byteValues[3], byteValues[2]]
                    );
                    break;
                case DataFormat.BADC:
                    byteResult.AddRange(
                        [byteValues[2], byteValues[3], byteValues[0], byteValues[1]]
                    );
                    break;
                case DataFormat.DCBA:
                    byteResult.AddRange(
                        [byteValues[3], byteValues[2], byteValues[1], byteValues[0]]
                    );
                    break;
            }
        }
        else if (byteValues.Length == 2)
        {
            byteResult.AddRange([byteValues[1], byteValues[0]]);
        }
        else if (byteValues.Length == 8)
        {
            if (
                dataFormat == DataFormat.ABCD
                || dataFormat == DataFormat.BADC
                || dataFormat == DataFormat.DCBA
            )
            {
                byte[] end = new byte[4];
                Array.Copy(byteValues, 4, end, 0, 4);
                byteResult.AddRange(ToWord(end, dataFormat));
                byte[] start = new byte[4];
                Array.Copy(byteValues, 0, start, 0, 4);
                byteResult.AddRange(ToWord(start, dataFormat));
            }
            if (dataFormat == DataFormat.CDAB)
            {
                byte[] start = new byte[4];
                Array.Copy(byteValues, 0, start, 0, 4);
                byteResult.AddRange(ToWord(start, dataFormat));
                byte[] end = new byte[4];
                Array.Copy(byteValues, 4, end, 0, 4);
                byteResult.AddRange(ToWord(end, dataFormat));
            }
        }
        return byteResult.ToArray();
    }

    /// <summary>
    /// 转换String字
    /// </summary>
    /// <param name="byteValues"></param>
    /// <param name="dataFormat"></param>
    /// <returns></returns>
    public byte[] ToStringWorld(byte[] byteValues, DataFormat dataFormat)
    {
        //补全偶数，只是为了凑够一个字（2byte）
        byte[] length =
            byteValues.Length % 2 == 0
                ? new byte[byteValues.Length]
                : new byte[byteValues.Length + 1];
        Array.Copy(byteValues, 0, length, 0, byteValues.Length);
        List<byte> bytes = new List<byte>();
        for (int i = 0; i < length.Length; i += 2)
        {
            switch (dataFormat)
            {
                case DataFormat.ABCD:
                    bytes.AddRange([length[i + 1], length[i]]);
                    break;
                case DataFormat.CDAB:
                    bytes.AddRange([length[i + 1], length[i]]);
                    break;
                case DataFormat.BADC:
                    bytes.AddRange([length[i], length[i + 1]]);
                    break;
                case DataFormat.DCBA:
                    bytes.AddRange([length[i], length[i + 1]]);
                    break;
            }
        }
        var resultByte = new byte[byteValues.Length];
        Array.Copy(bytes.ToArray(), 0, resultByte, 0, byteValues.Length);
        return resultByte;
    }

    public byte[] ToLong(long[] values, DataFormat dataFormat)
    {
        List<byte> bytesResult = new List<byte>();
        for (int i = 0; i < values.Length; i++)
        {
            var byteValues = BitConverter.GetBytes(values[i]);
            bytesResult.AddRange(AdjustByteOrder(byteValues, dataFormat));
        }
        return bytesResult.ToArray();
    }

    /// <summary>
    /// 字节序列转回，区别方法名
    /// </summary>
    /// <param name="byteValues"></param>
    /// <param name="dataFormat"></param>
    /// <returns></returns>
    byte[] BackWord(byte[] byteValues, DataFormat dataFormat)
    {
        return ToWord(byteValues, dataFormat);
    }

    public byte[] BackLong(byte[] bytes, DataFormat dataFormat)
    {
        List<byte> byteResult = [.. ReverseByteOrder(bytes, dataFormat)];
        return byteResult.ToArray();
    }

    public byte[] BackFloat(byte[] bytes, DataFormat dataFormat)
    {
        List<byte> byteResult = [.. ReverseByteOrder(bytes, dataFormat)];
        return byteResult.ToArray();
    }

    public byte[] BackInt16(byte[] bytes, DataFormat dataFormat)
    {
        List<byte> bytesResult = new List<byte>();
        for (int i = 0; i < bytes.Length; i++)
        {
            bytesResult.AddRange(ToWord(bytes, dataFormat));
        }
        return bytesResult.ToArray();
    }

    public byte[] BackInt32(byte[] bytes, DataFormat dataFormat)
    {
        List<byte> bytesResult = new List<byte>();
        for (int i = 0; i < bytes.Length; i += 2)
        {
            bytesResult.AddRange(ToWord(bytes, dataFormat));
        }
        return bytesResult.ToArray();
    }

    public byte[] ToInt32(int[] ints, DataFormat dataFormat)
    {
        List<byte> bytesResult = new List<byte>();
        for (int i = 0; i < ints.Length; i++)
        {
            var byteValues = BitConverter.GetBytes(ints[i]);
            var bytes = ToWord(byteValues, dataFormat);
            bytesResult.AddRange(bytes);
        }
        return bytesResult.ToArray();
    }

    public byte[] BackDouble(byte[] bytes, DataFormat dataFormat)
    {
        List<byte> byteResult = [.. ReverseByteOrder(bytes, dataFormat)];
        return byteResult.ToArray();
    }

    public byte[] GetStart(ushort value)
    {
        var start = new byte[2];
        start[0] = (byte)(value / 256);
        start[1] = (byte)(value % 256);
        return start;
    }

    public byte[] GetLength(ushort value)
    {
        return GetStart(value);
    }

    public static byte[] AdjustByteOrder(byte[] input, DataFormat format)
    {
        if (input == null || input.Length == 0)
        {
            throw new ArgumentException("输入数据不能为空");
        }

        int length = input.Length;
        List<byte> bytesResult = new List<byte>();

        switch (format)
        {
            case DataFormat.ABCD:
                Array.Reverse(input);
                bytesResult.AddRange(input);
                break;

            case DataFormat.CDAB:
                for (int i = 0; i < length; i += 2)
                {
                    if (i + 1 < length)
                    {
                        bytesResult.Add(input[i + 1]);
                        bytesResult.Add(input[i]);
                    }
                    else
                    {
                        bytesResult.Add(input[i]);
                    }
                }
                break;

            case DataFormat.BADC:
                for (int i = 0; i < length; i += 8)
                {
                    if (i + 7 < length)
                    {
                        bytesResult.Add(input[i + 6]);
                        bytesResult.Add(input[i + 7]);
                        bytesResult.Add(input[i + 4]);
                        bytesResult.Add(input[i + 5]);
                        bytesResult.Add(input[i + 2]);
                        bytesResult.Add(input[i + 3]);
                        bytesResult.Add(input[i]);
                        bytesResult.Add(input[i + 1]);
                    }
                    else
                    {
                        for (int j = i; j < length; j++)
                        {
                            bytesResult.Add(input[j]);
                        }
                    }
                }
                break;

            case DataFormat.DCBA:
                bytesResult.AddRange(input);
                break;
        }

        return bytesResult.ToArray();
    }

    public static byte[] ReverseByteOrder(byte[] input, DataFormat format)
    {
        if (input == null || input.Length == 0)
        {
            throw new ArgumentException("输入数据不能为空");
        }

        int length = input.Length;
        List<byte> bytesResult = new List<byte>();

        switch (format)
        {
            case DataFormat.ABCD:
                Array.Reverse(input);
                bytesResult.AddRange(input);
                break;

            case DataFormat.CDAB:
                for (int i = 0; i < length; i += 2)
                {
                    if (i + 1 < length)
                    {
                        bytesResult.Add(input[i + 1]);
                        bytesResult.Add(input[i]);
                    }
                    else
                    {
                        bytesResult.Add(input[i]);
                    }
                }
                break;

            case DataFormat.BADC:
                for (int i = 0; i < length; i += 8)
                {
                    if (i + 7 < length)
                    {
                        bytesResult.Add(input[i + 6]);
                        bytesResult.Add(input[i + 7]);
                        bytesResult.Add(input[i + 4]);
                        bytesResult.Add(input[i + 5]);
                        bytesResult.Add(input[i + 2]);
                        bytesResult.Add(input[i + 3]);
                        bytesResult.Add(input[i]);
                        bytesResult.Add(input[i + 1]);
                    }
                    else
                    {
                        for (int j = i; j < length; j++)
                        {
                            bytesResult.Add(input[j]);
                        }
                    }
                }
                break;

            case DataFormat.DCBA:
                bytesResult.AddRange(input);
                break;
        }

        return bytesResult.ToArray();
    }
}
