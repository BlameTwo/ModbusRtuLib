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
        var a = result.ToArray();
        Array.Reverse(a);
        string value = "";
        foreach (var item in a)
        {
            value += item;
        }
        var resultValue = BitConverter.GetBytes(short.Parse(value));
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(resultValue);
        }
        return resultValue;
    }

    public byte[] GetLength(short length)
    {
        var lengthValue = BitConverter.GetBytes(length);
        return lengthValue;
    }
}
