using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Common;

public class UdpSocketDevice : ISocketDevice
{
    public UdpClient Client { get; private set; }

    private NetworkStream streamBase;

    public bool IsReconnect { get; set; }

    public bool IsConnected => throw new NotImplementedException("udp connect error");

    public string Ip { get; private set; }
    public int Port { get; private set; }

    public DataResult<bool> Connect(string ip, int port = 502)
    {
        this.Ip = ip;
        this.Port = port;
        Client = new UdpClient();
        Client.Connect(ip, port);
        return DataResult<bool>.OK(true);
    }

    public async Task<DataResult<bool>> ConnectAsync(string ip, int port = 502)
    {
        return await Task.Run(() =>
        {
            this.Ip = ip;
            this.Port = port;
            Client = new UdpClient();
            Client.Connect(ip, port);
            return DataResult<bool>.OK(true);
        });
    }

    public void Disconnect()
    {
        this.Client.Close();
        this.Client.Dispose();
    }

    public void SendData(string message)
    {
        throw new NotImplementedException();
    }

    public byte[] SendData(byte[] message)
    {
        var stream = Client.Client.Send(message);
        Thread.Sleep(200);
        var buffer = new byte[512];
        var length = Client.Client.Receive(buffer);
        byte[] result = new byte[length];
        Array.Copy(buffer, 0, result, 0, length);
        return result;
    }

    public async Task<byte[]> SendDataAsync(byte[] message)
    {
        var stream = await Client.Client.SendAsync(message);
        Thread.Sleep(200);
        var buffer = new byte[512];
        var length = await Client.Client.ReceiveAsync(buffer);
        byte[] result = new byte[length];
        Array.Copy(buffer, 0, result, 0, length);
        return result;
    }

    public Task SendDataAsync(string message)
    {
        throw new NotImplementedException();
    }
}
