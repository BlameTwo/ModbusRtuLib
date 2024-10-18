using System.Net.Sockets;
using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts
{
    /// <summary>
    /// Tcp Socket
    /// </summary>
    public interface ISocketDevice
    {
        public DataResult<bool> Connect(string ip, int port = 502);

        public Task<DataResult<bool>> ConnectAsync(string ip, int port = 502);

        public bool IsReconnect { get; set; }

        public bool IsConnected { get; }
        public void Disconnect();

        public void SendData(string message);

        public byte[] SendData(byte[] message);

        public Task SendDataAsync(string message);

        public Task<byte[]> SendDataAsync(byte[] message);
        public TcpClient Client { get; }
    }
}
