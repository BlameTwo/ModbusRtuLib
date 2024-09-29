using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Common;

public class SocketDevice : ISocketDevice
{
    public TcpClient TcpClient { get; private set; }
    public string IP { get; private set; }
    public int Port { get; private set; }
    public bool IsReconnect { get; private set; } = false;

    public bool IsConnected { get; private set; }
    private CancellationTokenSource _cancellationTokenSource;
    private bool isConnecting;

    public SocketDevice(bool isReconnect)
    {
        IsReconnect = isReconnect;
    }

    public DataResult<bool> Connect(string ip, int port = 502)
    {
        try
        {
            if (TcpClient != null)
            {
                TcpClient.Close();
                TcpClient = null;
            }
            this.IP = ip;
            this.Port = port;
            TcpClient = new TcpClient();
            TcpClient.Connect(ip, port);
            IsConnected = TcpClient.Connected;
            StartListening();
            return DataResult<bool>.OK(TcpClient.Connected, null, null);
        }
        catch (Exception ex)
        {
            TcpClient.Close();
            TcpClient.Dispose();
            Console.WriteLine("连接失败: " + ex.Message);
            return DataResult<bool>.OK(TcpClient.Connected, null, null);
        }
    }

    public async Task<DataResult<bool>> ConnectAsync(string ip, int port = 502)
    {
        try
        {
            if (TcpClient != null)
                TcpClient.Dispose();
            this.IP = ip;
            this.Port = port;
            TcpClient = new TcpClient();
            await TcpClient.ConnectAsync(IP, Port);
            IsConnected = true;
            StartListening();
            return DataResult<bool>.OK(TcpClient.Connected, null, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine("连接失败: " + ex.Message);
            return DataResult<bool>.OK(TcpClient.Connected, null, null);
        }
    }

    private void StartListening()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        Task.Run(
            async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);
                    try
                    {
                        if (IsConnected == false)
                        {
                            if (IsReconnect)
                            {
                                isConnecting = true;
                                IsConnected = false;
                                Console.WriteLine("尝试重连...");
                                TcpClient = new TcpClient();
                                await TcpClient.ConnectAsync(IP, Port);
                                IsConnected = TcpClient.Connected;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        if (IsReconnect)
                        {
                            isConnecting = true;
                            IsConnected = false;
                            Console.WriteLine("尝试重连...");
                            TcpClient = new TcpClient();
                            await TcpClient.ConnectAsync(IP, Port);
                            IsConnected = TcpClient.Connected;
                        }
                    }
                }
            },
            token
        );
    }

    public void SendData(string message)
    {
        try
        {
            if (this.IsConnected == false)
                return;
            byte[] data = Encoding.ASCII.GetBytes(message);
            NetworkStream stream = TcpClient.GetStream();
            stream.Write(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            this.IsConnected = false;
        }
    }

    public (byte[], int) SendData(byte[] message)
    {
        try
        {
            if (IsConnected == false)
                return (null, 0);
            NetworkStream stream = TcpClient.GetStream();
            stream.Write(message, 0, message.Length);
            Thread.Sleep(200);
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            byte[] resultByte = new byte[bytesRead];
            Array.Copy(buffer, 0, resultByte, 0, bytesRead);
            return (resultByte, bytesRead);
        }
        catch (Exception)
        {
            this.IsConnected = false;
            return (null, 0);
        }
    }

    public async Task SendDataAsync(string message)
    {
        if (IsConnected)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkStream stream = TcpClient.GetStream();
            await stream.WriteAsync(data, 0, data.Length);
        }
        else
        {
            Console.WriteLine("未连接，无法发送数据");
        }
    }

    public void Disconnect()
    {
        _cancellationTokenSource?.Cancel();
        TcpClient?.Close();
        IsConnected = false;
        Console.WriteLine("连接已关闭");
    }
}
