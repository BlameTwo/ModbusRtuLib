using ModbusRtuLib.Common;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Contracts.Tcp;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Tcp;

public class ModbusTcpClient : IModbusTcpClient
{
    public ISocketDevice Device { get; private set; }

    public ServerPortConfig Config { get; set; }

    public bool IsConnected
    {
        get
        {
            if (Device == null)
                return false;
            return Device.IsConnected;
        }
    }

    public DataResult<bool> Connect(string host, int port = 502)
    {
        Device = new SocketDevice(Config.IsReconnect);
        var connect = Device.Connect(host, port);
        return connect;
    }

    public void Disconnect() => Device.Disconnect();

    public IModbusTcpSlave GetSlave(byte id)
    {
        return new ModbusTcpSlave(this.Device, id);
    }
}
