using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using ModbusRtuLib.Contracts.Rtu;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Rtu
{
    public sealed class ModbusRtuClient : IModbusRtuClient
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
                new ModbusRtuSlaveConfig()
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
    }
}
