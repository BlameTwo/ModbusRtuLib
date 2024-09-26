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

        readonly Dictionary<int, ModbusSlaveConfig> slaves = new();

        public void AddSlave(ModbusSlaveConfig modbusRtuSlaveConfig)
        {
            if (slaves.ContainsKey(modbusRtuSlaveConfig.SlaveId))
                return;
            slaves.Add(modbusRtuSlaveConfig.SlaveId, modbusRtuSlaveConfig);
        }

        public IModbusAsciiSlave GetSlave(byte slaveId)
        {
            if (slaves.ContainsKey(slaveId))
            {
                if (slaves.TryGetValue(slaveId, out var config))
                {
                    return new ModbusAsciiSlave(Port, config) { };
                }
            }
            return new ModbusAsciiSlave(
                Port,
                new ModbusSlaveConfig()
                {
                    IsStartZero = true,
                    SlaveId = slaveId,
                    ReadTimeSpan = Config.ReadTimeSpan,
                    WriteTimepan = Config.WriteTimeSpan,
                }
            );
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
