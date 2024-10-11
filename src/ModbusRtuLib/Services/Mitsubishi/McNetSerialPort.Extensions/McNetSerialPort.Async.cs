using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Mitsubishi;

public partial class McNetSerialPort
{
    public async Task<DataResult<bool>> WriteAsync(string address, byte[] data, ushort length)
    {
        List<byte> Resultbytes =
        [
            //固定
            0x50,
            0x00,
            //网络号
            NetWorkId,
            //PLC编号
            0xFF,
            //CPU编号
            .. DeviceCode,
            //网络站号
            NetWorkSlave,
        ];
        //后续数据长度
        var dataBytes = new List<byte>();
        dataBytes.AddRange(Parse.GetTimeSpan(this.TimeSpan));
        dataBytes.AddRange(new byte[] { 0x00, 0x01 });
        dataBytes.AddRange(new byte[] { 0x14, 0x00 });
        dataBytes.AddRange(new byte[] { 0x00 });
        var result = Parse.GetStart(address, 1);
        dataBytes.AddRange(result);
        dataBytes.AddRange(new byte[] { 0x00 });
        var method = Parse.GetMcType(address);
        dataBytes.Add((byte)method);
        dataBytes.AddRange(Parse.GetLength(method, length));
        dataBytes.AddRange(data);
        Resultbytes.Add((byte)dataBytes.Count);
        Resultbytes.AddRange(dataBytes);
        await this.Port.BaseStream.WriteAsync(Resultbytes.ToArray(), 0, Resultbytes.Count);
        await Task.Delay(TimeSpan);
        var count = Port.BytesToRead;
        var resultByte = new byte[count];
        await Port.BaseStream.ReadAsync(resultByte, 0, count);
        if (
            resultByte[0] == 0xD0
            && resultByte[2] == this.NetWorkId
            && resultByte[3] == 0xFF
            && resultByte[5] == 0x03
        )
        {
            return DataResult<bool>.OK(true, Resultbytes.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("写入失败！");
    }

    public async Task<DataResult<bool>> WriteAsync(double value, string address)
    {
        return await this.WriteAsync(address, BitConverter.GetBytes(value), 0x04);
    }
}
