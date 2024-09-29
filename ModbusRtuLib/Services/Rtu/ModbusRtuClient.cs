using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using ModbusRtuLib.Contracts.Rtu;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Handlers;

namespace ModbusRtuLib.Services.Rtu
{
    public sealed class ModbusRtuClient : IModbusRtuClient
    {
        public SerialPort Port { get; set; }
        public SerialPortConfig Config { get; set; }

        public bool IsConnected => Port.IsOpen;

        readonly Dictionary<int, ModbusSlaveConfig> slaves = new();

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

        private ModbusDataReceived dataRevicedHandler;

        private ModbusConnectChanged connecthandler;

        private bool disposedValue;

        public void AddSlave(ModbusSlaveConfig modbusRtuSlaveConfig)
        {
            if (slaves.ContainsKey(modbusRtuSlaveConfig.SlaveId))
                return;
            slaves.Add(modbusRtuSlaveConfig.SlaveId, modbusRtuSlaveConfig);
        }

        internal void Setup()
        {
            if (Port != null)
            {
                Port.DataReceived -= Port_DataReceived;
            }
            Port = new SerialPort();
            Port.BaudRate = Config.BaudRate;
            Port.DataBits = Config.DataBit;
            Port.StopBits = Config.StopBit;
            Port.PortName = Config.SerialPortName;
            Port.Parity = Config.Parity;
            Port.Handshake = Config.Handshake;
            Port.DataReceived += Port_DataReceived;
            Port.Open();
            this.connecthandler?.Invoke(this, Port.IsOpen);
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.dataRevicedHandler?.Invoke(
                this,
                new ModbusDataModel() { Type = Models.Enums.ModbusMessageDataType.Bytes }
            );
        }

        public IModbusRtuSlave GetSlave(byte slaveId)
        {
            if (slaves.ContainsKey(slaveId))
            {
                if (slaves.TryGetValue(slaveId, out var config))
                {
                    return new ModbusRtuSlave(Port, config) { };
                }
            }
            return new ModbusRtuSlave(
                Port,
                new ModbusSlaveConfig()
                {
                    IsStartZero = true,
                    SlaveId = slaveId,
                    ReadTimeSpan = Config.ReadTimeSpan,
                    WriteTimepan = Config.WriteTimeSpan,
                    DataFormat = Models.Enums.DataFormat.CDAB,
                    IsCheckSlave = true,
                    StringEncoding = Encoding.ASCII,
                }
            );
        }

        public DataResult<ushort> ReadHoldingRegisterSingle(byte slaveId, ushort start)
        {
            var slave = GetSlave(slaveId);
            return slave.ReadHoldingRegisterSingle(start);
        }

        public DataResult<bool> ReadCoilSingle(byte slaveId, ushort start)
        {
            var slave = GetSlave(slaveId);
            return slave.ReadCoilSingle(start);
        }

        public DataResult<bool> ReadDiscreteSingle(byte slaveId, ushort start)
        {
            var slave = GetSlave(slaveId);
            return slave.ReadDiscreteSingle(start);
        }

        public void Close()
        {
            this.Port.Close();
            this.connecthandler?.Invoke(this, Port.IsOpen);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Port.Close();
                    this.Port.Dispose();
                }
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ModbusRtuClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
