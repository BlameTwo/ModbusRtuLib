using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusRtuLib.Common
{
    internal class LRC
    {
        /// <summary>
        /// 纵向冗余校验LRC
        /// </summary>
        /// <param name="data">原数组</param>
        /// <param name="offset">从头开始偏移几个byte</param>
        /// <param name="length">偏移后取几个字节byte</param>
        /// <returns>8位LRC</returns>
        public static byte LRCCalc(byte[] data, int offset, int length)
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
        public static byte LRCCalc(string data, int offset, int length)
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

                return LRCCalc(buffer, 0, buffer.Length);
            }
            catch
            {
                return 0;
            }
        }
    }
}
