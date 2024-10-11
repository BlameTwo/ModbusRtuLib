using System;
using System.Collections.Generic;
using System.IO.Ports;
using ModbusRtuLib.Contracts.Modbus.Ascii;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Handlers;
using ModbusRtuLib.Services.Ascii;

namespace ModbusRtuLib.Services.Modbus.Ascii
{
    internal class ModbusAsciiClient : IModbusAsciiClient
    {
        public SerialPort Port { get; set; }
        public SerialPortConfig Config { get; set; }

        public bool IsConnected => Port.IsOpen;

        readonly Dictionary<int, ModbusSlaveConfig> slaves = new();
        private bool disposedValue;

        private ModbusDataReceived dataRevicedHandler;

        private ModbusConnectChanged connecthandler;

        public event ModbusDataReceived DataRecived
        {
            add => dataRevicedHandler += value;
            remove => dataRevicedHandler -= value;
        }
        public event ModbusConnectChanged ConnectChanged
        {
            add => connecthandler += value;
            remove => connecthandler -= value;
        }

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
            connecthandler?.Invoke(this, Port.IsOpen);
        }

        public void Close()
        {
            Port.Close();
            connecthandler?.Invoke(this, Port.IsOpen);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Port.Close();
                    Port.Dispose();
                }
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ModbusAsciiClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
