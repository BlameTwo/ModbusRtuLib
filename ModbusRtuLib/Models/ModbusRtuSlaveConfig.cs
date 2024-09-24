using System.Text;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Models;

public class ModbusRtuSlaveConfig
{
    /// <summary>
    /// 是否从0开始,纯使用modbus不用管，默认为true
    /// </summary>
    public bool IsStartZero { get; set; } = true;

    /// <summary>
    /// 是否校验站号
    /// </summary>
    public bool IsCheckSlave { get; set; } = true;

    /// <summary>
    /// 字节序排列模式
    /// </summary>
    public DataFormat DataFormat { get; set; } = DataFormat.CDAB;

    /// <summary>
    /// 字符串编码格式
    /// </summary>
    public Encoding StringEncoding { get; set; }

    /// <summary>
    /// 站号
    /// </summary>
    public byte SlaveId { get; set; }

    /// <summary>
    /// 读取延时
    /// </summary>
    public int ReadTimeSpan { get; set; } = 200;

    /// <summary>
    /// 写入延时
    /// </summary>
    public int WriteTimepan { get; set; } = 200;
}
