using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Rtu
{
    public interface IModbusRtuClient
    {
        System.IO.Ports.SerialPort Port { get; set; }

        SerialPortConfig Config { get; set; }

        bool IsConnected { get; }

        void AddSlave(ModbusSlaveConfig modbusRtuSlaveConfig);

        /// <summary>
        /// 根据注册站号或传入站号创建一个从站信息，传入站号时内部不存储站号信息
        /// </summary>
        /// <param name="slaveId"></param>
        /// <returns></returns>
        IModbusRtuSlave GetSlave(byte slaveId);

        public DataResult<ushort> ReadHoldingRegisterSingle(byte slaveId, ushort start);

        public DataResult<bool> ReadCoilSingle(byte slaveId, ushort start);

        public DataResult<bool> ReadDiscreteSingle(byte slaveId, ushort start);
    }
}
