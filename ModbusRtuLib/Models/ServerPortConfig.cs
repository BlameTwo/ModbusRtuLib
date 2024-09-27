namespace ModbusRtuLib.Models;

public class ServerPortConfig
{
    public ServerPortConfig(string ipAddress, int port = 502)
    {
        IpAddress = ipAddress;
        Port = port;
    }

    public string IpAddress { get; set; }

    public bool IsReconnect { get; set; }

    public int Port { get; set; }
}
