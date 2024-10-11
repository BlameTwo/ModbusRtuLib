using System;

namespace CommunicationApp.Models;

public enum ModbusMessageType
{
    /// <summary>
    /// 发送
    /// </summary>
    Rx,

    /// <summary>
    /// 接收
    /// </summary>
    Tx,

    Data,
}

public class ModbusMessage
{
    public ModbusMessageType Type { get; set; }

    public ModbusMessage()
    {
        this.Time = DateTime.Now;
    }

    public string Data { get; set; }

    public DateTime Time { get; set; }

    public byte[] Datas
    {
        set
        {
            if (value == null)
                return;
            foreach (var item in value)
            {
                this.Value += Convert.ToString(item, 16).ToUpper() + " ";
            }
        }
    }

    public string Value { get; set; } = "";
}
