using System;
using ModbusRtuLib.Models.Handlers;

namespace ModbusRtuLib.Contracts.Modbus
{
    public interface IModbusClient : IDisposable
    {
        public event ModbusDataReceived DataRecived;

        public event ModbusConnectChanged ConnectChanged;
    }
}
