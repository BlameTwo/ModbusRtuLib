using System;
using System.Linq;
using System.Reflection;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Services.Mitsubishi;

public class McNetAddressParse : IMcNetAddressParse
{
    public MitsubishiMCType GetMcType(string address)
    {
        if (address.Length == 0)
            return MitsubishiMCType.None;
        string type = address.Substring(0, 1);
        switch (type.ToUpper())
        {
            case "X":
                return MitsubishiMCType.X;
            case "Y":
                return MitsubishiMCType.Y;
            case "M":
                return MitsubishiMCType.M;
            case "L":
                return MitsubishiMCType.L;
            case "B":
                return MitsubishiMCType.B;
            case "S":
                return MitsubishiMCType.S;
            case "F":
                return MitsubishiMCType.F;
            case "T":
                type = address.Substring(0, 2);
                if (type == "TS")
                    return MitsubishiMCType.TS;
                else if (type == "TC")
                    return MitsubishiMCType.TC;
                else if (type == "TN")
                    return MitsubishiMCType.TN;
                return MitsubishiMCType.None;
            case "C":
                type = address.Substring(0, 2);
                if (type == "CS")
                    return MitsubishiMCType.CS;
                else if (type == "CC")
                    return MitsubishiMCType.CC;
                else if (type == "CN")
                    return MitsubishiMCType.CN;
                return MitsubishiMCType.None;
            case "D":
                return MitsubishiMCType.D;
            case "W":
                return MitsubishiMCType.W;
            case "R":
                return MitsubishiMCType.R;
        }
        return MitsubishiMCType.None;
    }

    public byte[] GetTimeSpan(int value)
    {
        var timeSpan = (byte)value / 10;
        var header = BitConverter.GetBytes((short)timeSpan);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(header);
        return header;
    }

    public byte[] GetStart(string address, int startSplit)
    {
        if (address.IndexOf(".") != -1)
        {
            var splitEnd = address.IndexOf(".");
            //var result = address.Substring(startSplit, splitEnd);
            return new byte[0];
        }
        var result = address.Substring(startSplit);
        var method = this.GetMcType(address);
        byte[] resultValue = null;
        if (this.IsHexType(method) == true)
        {
            resultValue = BitConverter.GetBytes(Convert.ToUInt32(result, 16));
        }
        else
        {
            resultValue = BitConverter.GetBytes(int.Parse(result));
        }
        byte[] threeBytes = new byte[3];
        Array.Copy(resultValue, 0, threeBytes, 0, 2); // 只取第1到第3个字节
        return threeBytes;
    }

    public byte[] GetLength(MitsubishiMCType type, ushort length)
    {
        var bytes = BitConverter.GetBytes(length);
        return bytes;
    }

    /// <summary>
    /// 获取标识是否是位操作
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public bool? IsWordType(string address)
    {
        if (address.Length == 0)
            return null;
        string type = address.Substring(0, 1);
        switch (type.ToUpper())
        {
            case "X":
            case "Y":
            case "M":
            case "L":
            case "B":
            case "S":
            case "F":
                return false;
            case "T":
                type = address.Substring(0, 2);
                if (type == "TS")
                    return false;
                else if (type == "TC")
                    return false;
                else if (type == "TN")
                    return true;
                else
                    return null;
            case "C":
                type = address.Substring(0, 2);
                if (type == "CS")
                    return false;
                else if (type == "CC")
                    return false;
                else if (type == "CN")
                    return true;
                return null;
            case "D":
            case "W":
            case "R":
                return true;
        }
        return null;
    }

    public bool? IsWordType(MitsubishiMCType type)
    {
        switch (type)
        {
            case MitsubishiMCType.None:
                return null;
            case MitsubishiMCType.X:
            case MitsubishiMCType.Y:
            case MitsubishiMCType.M:
            case MitsubishiMCType.L:
            case MitsubishiMCType.S:
            case MitsubishiMCType.B:
            case MitsubishiMCType.F:
            case MitsubishiMCType.TS:
            case MitsubishiMCType.TC:
                return false;
            case MitsubishiMCType.TN:
                return true;
            case MitsubishiMCType.CS:
            case MitsubishiMCType.CC:
                return false;
            case MitsubishiMCType.CN:
                return true;
            case MitsubishiMCType.D:
            case MitsubishiMCType.W:
            case MitsubishiMCType.R:
                return true;
            default:
                return false;
        }
    }

    public bool? IsHexType(MitsubishiMCType type)
    {
        switch (type)
        {
            case MitsubishiMCType.None:
                break;
            case MitsubishiMCType.X:
            case MitsubishiMCType.Y:
            case MitsubishiMCType.M:
            case MitsubishiMCType.L:
            case MitsubishiMCType.B:
            case MitsubishiMCType.W:
            case MitsubishiMCType.S:
            case MitsubishiMCType.F:
            case MitsubishiMCType.TS:
            case MitsubishiMCType.TC:
            case MitsubishiMCType.TN:
            case MitsubishiMCType.CS:
            case MitsubishiMCType.CC:
            case MitsubishiMCType.CN:
            case MitsubishiMCType.R:
                return true;
            case MitsubishiMCType.D:
                return false;
        }
        return null;
    }
}
