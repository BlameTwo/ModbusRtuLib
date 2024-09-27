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
        public bool IsReconnect { get; }

        public bool IsConnected { get; }
        public void Disconnect();

        public void SendData(string message);

        public (byte[], int) SendData(byte[] message);

        public Task SendDataAsync(string message);

        public TcpClient TcpClient { get; }
    }
}
