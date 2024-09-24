namespace ModbusRtuLib.Models;

public class DataResult<T>
{
    public T Data { get; set; }

    public bool IsOK { get; set; }

    public string Error { get; set; }

    public static DataResult<T> OK(T data)
    {
        return new DataResult<T>() { Data = data, IsOK = true };
    }

    public static DataResult<T> NG(string message)
    {
        return new DataResult<T>() { IsOK = false, Error = message };
    }
}
