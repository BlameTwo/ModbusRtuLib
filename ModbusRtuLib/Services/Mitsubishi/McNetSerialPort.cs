using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Services.Mitsubishi;

public class McNetSerialPort : IMcNetSerialPort
{
    public IMcNetAddressParse Parse => new McNetAddressParse();

    public byte NetWorkId { get; set; } = 0x00;

    public byte NetWorkSlave { get; set; } = 0x00;

    public int TimeSpan { get; set; } = 100;

    public SerialPort Port { get; private set; }
    public byte[] DeviceCode { get; set; } = new byte[] { 0xFF, 0x03 };

    public DataResult<bool> OpenSerial(SerialPortConfig serialPortConfig)
    {
        try
        {
            this.Port = new SerialPort();
            Port.PortName = serialPortConfig.SerialPortName;
            Port.BaudRate = serialPortConfig.BaudRate;
            Port.DataBits = serialPortConfig.DataBit;
            Port.StopBits = serialPortConfig.StopBit;
            Port.Parity = serialPortConfig.Parity;
            Port.Open();
            if (Port.IsOpen)
                return DataResult<bool>.OK(true);
        }
        catch (Exception ex)
        {
            return DataResult<bool>.NG(ex.Message);
        }
        return DataResult<bool>.NG("打开失败！");
    }

    public DataResult<bool> Write(double value, string address)
    {
        List<byte> Resultbytes = new List<byte>();
        //固定
        Resultbytes.Add(0x50);
        Resultbytes.Add(0x00);
        //网络号
        Resultbytes.Add(NetWorkId);
        //PLC编号
        Resultbytes.Add(0xFF);
        //CPU编号
        Resultbytes.AddRange(DeviceCode);
        //网络站号
        Resultbytes.Add(NetWorkSlave);
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
        dataBytes.AddRange(Parse.GetLength(sizeof(double) / 2));
        var valueBytes = BitConverter.GetBytes(value);
        dataBytes.AddRange(valueBytes);
        Resultbytes.Add((byte)dataBytes.Count);
        Resultbytes.AddRange(dataBytes);
        this.Port.Write(Resultbytes.ToArray(), 0, Resultbytes.Count);
        Thread.Sleep(TimeSpan);
        var count = Port.BytesToRead;
        var resultByte = new byte[count];
        Port.Read(resultByte, 0, count);
        if (
            Resultbytes[0] == 0xD0
            && Resultbytes[2] == this.NetWorkId
            && Resultbytes[3] == 0xFF
            && Resultbytes[3] == 0x03
        )
        {
            return DataResult<bool>.OK(true, Resultbytes.ToArray(), resultByte);
        }
        return DataResult<bool>.NG("写入失败！");
    }
}
