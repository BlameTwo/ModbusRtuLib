using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Tcp
{
    public interface IModbusTcpClient
    {
        public DataResult<bool> Connect(string host, int port = 502);

        public ISocketDevice Device { get; }

        public ServerPortConfig Config { get; set; }

        public bool IsConnected { get; }

        public void Disconnect();

        public IModbusTcpSlave GetSlave(byte id);
    }
}
