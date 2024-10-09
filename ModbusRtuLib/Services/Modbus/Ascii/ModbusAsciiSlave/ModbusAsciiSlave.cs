using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModbusRtuLib.Common;
using ModbusRtuLib.Contracts.Modbus;
using ModbusRtuLib.Contracts.Modbus.Ascii;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Services.Ascii
{
    public sealed partial class ModbusAsciiSlave : IModbusAsciiSlave
    {
        public ModbusAsciiSlave(SerialPort serialPort, ModbusSlaveConfig config)
        {
            SerialPort = serialPort;
            Config = config;
        }

        IByteConvert ByteConvert = new ByteConvert();

        public SerialPort SerialPort { get; }

        public ModbusSlaveConfig Config { get; }
    }
}
