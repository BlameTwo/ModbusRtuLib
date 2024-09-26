using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using ModbusRtuLib.Common;
using ModbusRtuLib.Contracts;
using ModbusRtuLib.Contracts.Rtu;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Rtu;

public partial class ModbusRtuSlave : IModbusRtuSlave
{
    public ModbusRtuSlave(SerialPort serialPort, ModbusSlaveConfig config)
    {
        SerialPort = serialPort;
        Config = config;
    }

    IByteConvert ByteConvert = new ByteConvert();

    public SerialPort SerialPort { get; }

    public ModbusSlaveConfig Config { get; }
}
