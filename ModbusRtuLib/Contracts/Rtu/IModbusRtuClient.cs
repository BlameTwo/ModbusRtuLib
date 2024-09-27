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

        /// <summary>
        /// 读取保持寄存器
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<ushort> ReadHoldingRegisterSingle(byte slaveId, ushort start);

        /// <summary>
        /// 读取线圈
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<bool> ReadCoilSingle(byte slaveId, ushort start);

        /// <summary>
        /// 读取输入寄存器
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<bool> ReadDiscreteSingle(byte slaveId, ushort start);
    }
}
