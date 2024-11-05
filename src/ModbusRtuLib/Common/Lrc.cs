using System;
using System.Linq;

namespace ModbusRtuLib.Common;

public static class Lrc
{
    /// <summary>
    /// 纵向冗余校验LRC
    /// </summary>
    /// <param name="data">原数组</param>
    /// <param name="offset">从头开始偏移几个byte</param>
    /// <param name="length">偏移后取几个字节byte</param>
    /// <returns>8位LRC</returns>
    public static byte LrcCalc(byte[] data, int offset, int length)
    {
        byte lrc = 0;
        for (int i = 0; i < length; i++)
            lrc += data[i + offset];
        return (byte)(-(sbyte)lrc);
    }

    /// <summary>
    /// 纵向冗余校验LRC
    /// </summary>
    /// <param name="data">原数组（有效字符串，不能含帧头和帧尾）</param>
    /// <param name="offset">从头开始偏移几个byte</param>
    /// <param name="length">偏移后取几个字节byte</param>
    /// <returns>8位LRC</returns>
    public static byte LrcCalc(string data, int offset, int length)
    {
        try
        {
            data = new string(
                data.Replace(" ", string.Empty)
                    .ToCharArray()
                    .Skip(offset)
                    .Take(length)
                    .ToArray()
            );

            if (data.Length % 2 == 1)
                throw new ArgumentException();

            byte[] buffer = new byte[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
                buffer[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);

            return LrcCalc(buffer, 0, buffer.Length);
        }
        catch
        {
            return 0;
        }
    }

    public static bool CheckLrc(byte[] data)
    {
        if (data == null)
            return false;
        if (data.Length < 2)
            return false;

        var crc = LrcCalc(data, 0, data.Length - 1);
        if (crc == data.Last())
        {
            return true;
        }
        return true;
    }
}