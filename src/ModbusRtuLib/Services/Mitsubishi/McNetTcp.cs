using System.Threading.Tasks;
using ModbusRtuLib.Common;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Handlers;
using ModbusRtuLib.Services.Mitsubishi.Parse;

namespace ModbusRtuLib.Services.Mitsubishi;

public partial class McNetTcp : IMcNetTcp
{
    public ISocketDevice Device { get; private set; }

    public byte NetWorkId { get; set; }
    public byte NetWorkSlave { get; set; }

    public byte[] DeviceCode { get; set; } = new byte[] { 0xFF, 0x03 };
    public IMcNetAddressParse Parse => new McNetSerialPortAddressParse();

    public bool IsConnected { get; private set; }

    public int TimeSpan { get; set; } = 100;

    public event MitsubishiQna3EConnectChanged ConnectChanged
    {
        add => mitsubishiQna3EConnectChanged += value;
        remove => mitsubishiQna3EConnectChanged -= value;
    }

    public MitsubishiQna3EConnectChanged mitsubishiQna3EConnectChanged;

    public void Close()
    {
        if (Device != null)
        {
            Device.IsReconnect = false;
            this.mitsubishiQna3EConnectChanged?.Invoke(this, IsConnected);
            Device.Disconnect();
        }
    }

    public DataResult<bool> OpenDevice(string address, int port = 6000)
    {
        Device = new SocketDevice();
        var connnect = Device.Connect(address, port);
        this.IsConnected = connnect.Data;
        this.mitsubishiQna3EConnectChanged?.Invoke(this, IsConnected);
        return DataResult<bool>.OK(this.IsConnected);
    }

    public async Task<DataResult<bool>> OpenDeviceAsync(string address, int port = 6000)
    {
        Device = new SocketDevice();
        var connnect = await Device.ConnectAsync(address, port);
        this.IsConnected = connnect.Data;
        this.mitsubishiQna3EConnectChanged?.Invoke(this, IsConnected);
        return DataResult<bool>.OK(true);
    }
}
