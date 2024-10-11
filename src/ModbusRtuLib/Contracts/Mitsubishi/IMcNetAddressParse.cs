using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Contracts.Mitsubishi;

public interface IMcNetAddressParse
{
    public MitsubishiMCType GetMcType(string address);

    public bool? IsWordType(string address);
    public bool? IsWordType(MitsubishiMCType type);

    public byte[] GetStart(string address, int startSplit);

    public byte[] GetLength(MitsubishiMCType type, ushort length);
    public byte[] GetTimeSpan(int value);
}
