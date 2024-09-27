using System;
using System.Collections.Generic;
using System.Linq;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Tcp;

public partial class ModbusTcpSlave
{
    public DataResult<bool> ReadCoil(ushort start)
    {
        List<byte> bytes = new List<byte>();
        bytes.AddRange(ByteConvert.GetStartBytes(0x0002));
        bytes.Add(0x00);
        bytes.Add(0x00);
        List<byte> be = new List<byte>();
        be.Add(Id);
        be.Add(0x01);
        be.AddRange(ByteConvert.GetStartBytes(start));
        be.AddRange(ByteConvert.GetLength(0x0001));
        bytes.AddRange(ByteConvert.GetStartBytes(Convert.ToUInt16(be.Count)));
        bytes.AddRange(be);
        var result = Device.SendData(bytes.ToArray());
        if (ByteConvert.GetStart(result.Item1[0], result.Item1[1]) == 0x0002)
        {
            var getLength = ByteConvert.GetStart(result.Item1[4], result.Item1[5]);
            var data = result
                .Item1.Skip(Math.Max(0, result.Item1.Length - getLength))
                .Take(getLength)
                .ToArray();
            if (data[0] == this.Id && data[1] == 0x01 && data[1] == 0x01)
            {
                return DataResult<bool>.OK(Convert.ToBoolean(data.Last()));
            }
            else
            {
                return DataResult<bool>.NG("站号，数据长度，功能码有一不同！");
            }
        }
        return DataResult<bool>.NG("请求码不同！");
    }
}
