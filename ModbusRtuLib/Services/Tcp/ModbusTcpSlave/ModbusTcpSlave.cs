using System;
using System.Collections.Generic;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Contracts.Tcp;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Tcp;

public partial class ModbusTcpSlave : IModbusTcpSlave
{
    IByteConvert ByteConvert = new ByteConvert();

    public ModbusTcpSlave(ISocketDevice device, byte id)
    {
        Device = device;
        Id = id;
    }

    public ModbusSlaveConfig Config { get; }

    public ISocketDevice Device { get; }
    public byte Id { get; }
}
