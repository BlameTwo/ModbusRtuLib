using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Contracts.Mitsubishi;

public interface IMcNetAddressParse
{
    public MitsubishiMCType GetMcType(string address);

    public byte[] GetStart(string address, int startSplit);

    public byte[] GetLength(short length);
    public byte[] GetTimeSpan(int value);
}
