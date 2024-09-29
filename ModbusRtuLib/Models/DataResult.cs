namespace ModbusRtuLib.Models;

public class DataResult<T>
{
    public T Data { get; set; }

    public bool IsOK { get; set; }

    public byte[] OrginSend { get; set; }

    public byte[] ReceivedData { get; set; }

    public string Error { get; set; }

    public static DataResult<T> OK(T data, byte[] orginSend = null, byte[] receivedData = null)
    {
        return new DataResult<T>()
        {
            Data = data,
            IsOK = true,
            OrginSend = orginSend,
            ReceivedData = receivedData,
        };
    }

    public static DataResult<T> NG(string message)
    {
        return new DataResult<T>() { IsOK = false, Error = message };
    }
}
