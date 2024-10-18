using System;
using System.Collections.Generic;
using System.IO.Ports;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Handlers;
using ModbusRtuLib.Services.Mitsubishi.Parse;

namespace ModbusRtuLib.Services.Mitsubishi;

public partial class McNetSerialPort : IMcNetSerialPort
{
    private bool disposedValue;

    public event MitsubishiQna3EConnectChanged ConnectChanged
    {
        add => mitsubishiQna3EHandler += value;
        remove => mitsubishiQna3EHandler -= value;
    }

    public MitsubishiQna3EConnectChanged mitsubishiQna3EHandler;

    public IMcNetAddressParse Parse => new McNetSerialPortAddressParse();

    public byte NetWorkId { get; set; } = 0x00;

    public byte NetWorkSlave { get; set; } = 0x00;

    public bool IsConnected { get; private set; }

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
            {
                this.IsConnected = Port.IsOpen;
                this.mitsubishiQna3EHandler?.Invoke(this, this.Port.IsOpen);
                return DataResult<bool>.OK(true);
            }
            else
            {
                this.IsConnected = Port.IsOpen;
                this.mitsubishiQna3EHandler?.Invoke(this, this.Port.IsOpen);
                return DataResult<bool>.NG("open error");
            }
        }
        catch (Exception ex)
        {
            this.IsConnected = Port.IsOpen;
            this.mitsubishiQna3EHandler?.Invoke(this, this.Port.IsOpen);
            return DataResult<bool>.NG(ex.Message);
        }
    }

    public List<byte> GetHeader() => [0x50, 0x00, NetWorkId, 0xFF, .. DeviceCode, NetWorkSlave];

    public void Close()
    {
        this.Port.Close();
        this.mitsubishiQna3EHandler?.Invoke(this, false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (this.Port != null)
                {
                    this.Close();
                    this.Port.Dispose();
                }
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
