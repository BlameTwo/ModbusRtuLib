using System;
using System.Collections.Generic;
using System.IO.Ports;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Mitsubishi;

public partial class McNetSerialPort : IMcNetSerialPort
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

    public List<byte> GetHeader() => [0x50, 0x00, NetWorkId, 0xFF, .. DeviceCode, NetWorkSlave];
}
