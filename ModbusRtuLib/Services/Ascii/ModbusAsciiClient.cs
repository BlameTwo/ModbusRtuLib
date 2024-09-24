using System;
using System.Collections.Generic;
using System.IO.Ports;
using ModbusRtuLib.Contracts.Ascii;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Ascii
{
    internal class ModbusAsciiClient : IModbusAsciiClient
    {
        public SerialPort Port { get; set; }
        public SerialPortConfig Config { get; set; }

        public bool IsConnected => Port.IsOpen;

        readonly Dictionary<int, ModbusRtuSlaveConfig> slaves = new();

        public void AddSlave(ModbusRtuSlaveConfig modbusRtuSlaveConfig)
        {
            if (slaves.ContainsKey(modbusRtuSlaveConfig.SlaveId))
                return;
            slaves.Add(modbusRtuSlaveConfig.SlaveId, modbusRtuSlaveConfig);
        }

        internal void Setup()
        {
            Port = new SerialPort();
            Port.BaudRate = Config.BaudRate;
            Port.DataBits = Config.DataBit;
            Port.StopBits = Config.StopBit;
            Port.PortName = Config.SerialPortName;
            Port.Parity = Config.Parity;
            Port.Handshake = Config.Handshake;
            Port.Open();
        }
    }
}
