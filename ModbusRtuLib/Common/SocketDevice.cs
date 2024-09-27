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

    public SocketDevice(bool isReconnect)
    {
        IsReconnect = isReconnect;
    }

    public DataResult<bool> Connect(string ip, int port = 502)
    {
        try
        {
            if (TcpClient != null)
                TcpClient.Dispose();
            this.IP = ip;
            this.Port = port;
            TcpClient = new TcpClient();
            TcpClient.Connect(ip, port);
            IsConnected = TcpClient.Connected;
            StartListening();
            return DataResult<bool>.OK(TcpClient.Connected);
        }
        catch (Exception ex)
        {
            Console.WriteLine("连接失败: " + ex.Message);
            StartReconnect();
            return DataResult<bool>.OK(TcpClient.Connected);
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
            return DataResult<bool>.OK(TcpClient.Connected);
        }
        catch (Exception ex)
        {
            Console.WriteLine("连接失败: " + ex.Message);
            StartReconnect();
            return DataResult<bool>.OK(TcpClient.Connected);
        }
    }

    private void StartReconnect()
    {
        IsConnected = false;
        Console.WriteLine("尝试重连...");
        Timer timer = null;
        timer = new Timer(
            async _ =>
            {
                if (!IsConnected)
                {
                    try
                    {
                        await ConnectAsync(IP, Port);
                        timer?.Dispose();
                    }
                    catch
                    {
                        Console.WriteLine("重连失败，继续尝试...");
                    }
                }
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5)
        );
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
                    Thread.Sleep(100);
                    try
                    {
                        if (
                            TcpClient.Client.Poll(0, SelectMode.SelectRead)
                            && TcpClient.Available == 0
                        )
                        {
                            Console.WriteLine("连接已断开");
                            IsConnected = false;
                            if (IsReconnect)
                                StartReconnect();
                        }
                    }
                    catch (Exception)
                    {
                        IsConnected = false;
                        if (IsReconnect)
                            StartReconnect();
                    }
                }
            },
            token
        );
    }

    public void SendData(string message)
    {
        if (IsConnected)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            NetworkStream stream = TcpClient.GetStream();
            stream.Write(data, 0, data.Length);
        }
        else
        {
            Console.WriteLine("未连接，无法发送数据");
        }
    }

    public (byte[], int) SendData(byte[] message)
    {
        if (IsConnected)
        {
            NetworkStream stream = TcpClient.GetStream();
            stream.Write(message, 0, message.Length);
            Thread.Sleep(200);
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            byte[] resultByte = new byte[bytesRead];
            Array.Copy(buffer, 0, resultByte, 0, bytesRead);
            return (resultByte, bytesRead);
        }
        else
        {
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
