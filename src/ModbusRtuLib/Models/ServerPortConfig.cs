namespace ModbusRtuLib.Models;

public class ServerPortConfig
{
    public ServerPortConfig(string ipAddress, int port = 502)
    {
        IpAddress = ipAddress;
        Port = port;
    }

    public string IpAddress { get; set; }

    /// <summary>
    /// 重连，连接过程中读取直接字节0
    /// </summary>
    public bool IsReconnect { get; set; }

    public int Port { get; set; }
}
