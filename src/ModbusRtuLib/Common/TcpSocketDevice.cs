using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Common;

public class TcpSocketDevice : ISocketDevice
{
    private NetworkStream streamBase = null;

    public bool IsReconnect { get; set; }

    public bool IsConnected => Client.Connected;

    public string Ip { get; private set; }
    public int Port { get; private set; }
    public TcpClient Client { get; private set; }

    public DataResult<bool> Connect(string ip, int port = 502)
    {
        this.Ip = ip;
        this.Port = port;
        Client = new TcpClient();
        Client.SendTimeout = 1000;
        Client.ReceiveTimeout = 1000;
        Client.Connect(ip, port);
        return DataResult<bool>.OK(true);
    }

    public async Task<DataResult<bool>> ConnectAsync(string ip, int port = 502)
    {
        this.Ip = ip;
        this.Port = port;
        Client = new TcpClient();
        Client.SendTimeout = 1000;
        Client.ReceiveTimeout = 1000;
        await Client.ConnectAsync(ip, port);
        return DataResult<bool>.OK(true);
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
        var stream = Client.GetStream();
        stream.Write(message);
        Thread.Sleep(200);
        var buffer = new byte[512];
        var length = stream.Read(buffer);
        byte[] result = new byte[length];
        Array.Copy(buffer, 0, result, 0, length);
        return result;
    }

    public async Task<byte[]> SendDataAsync(byte[] message)
    {
        var stream = Client.GetStream();
        await stream.WriteAsync(message);
        await Task.Delay(200);
        var buffer = new byte[512];
        var length = await stream.ReadAsync(buffer);
        byte[] result = new byte[length];
        Array.Copy(buffer, 0, result, 0, length);
        return result;
    }

    public Task SendDataAsync(string message)
    {
        throw new NotImplementedException();
    }
}
