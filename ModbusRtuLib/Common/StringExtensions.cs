using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusRtuLib.Common
{
    public static class StringExtensions
    {
        internal static byte[] SplitIntoGroupsOfFour(this Encoding Config, string value)
        {
            byte[] bytes = Config.GetBytes(value);
            int paddingLength = (4 - bytes.Length % 4) % 4;
            byte[] paddedBytes = new byte[bytes.Length + paddingLength];
            Array.Copy(bytes, 0, paddedBytes, 0, bytes.Length);
            for (int i = bytes.Length; i < paddedBytes.Length; i++)
            {
                paddedBytes[i] = 0;
            }

            return paddedBytes;
        }

        /// <summary>
        /// 补充字节到目标字节数量，不足向右补0
        /// </summary>
        /// <param name="Config"></param>
        /// <param name="value"></param>
        /// <param name="targetLength"></param>
        /// <returns></returns>
        internal static byte[] PadToLength(this Encoding Config, string value, int targetLength)
        {
            byte[] bytes = Config.GetBytes(value);
            byte[] paddedBytes = new byte[targetLength];
            Array.Copy(bytes, paddedBytes, Math.Min(bytes.Length, targetLength));
            if (bytes.Length < targetLength)
            {
                for (int i = bytes.Length; i < targetLength; i++)
                {
                    paddedBytes[i] = 0;
                }
            }
            return paddedBytes;
        }
    }
}
