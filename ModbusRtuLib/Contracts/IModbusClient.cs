using System;
using ModbusRtuLib.Models.Handlers;

namespace ModbusRtuLib.Contracts
{
    public interface IModbusClient : IDisposable
    {
        public event ModbusDataReceived DataRecived;

        public event ModbusConnectChanged ConnectChanged;
    }
}
