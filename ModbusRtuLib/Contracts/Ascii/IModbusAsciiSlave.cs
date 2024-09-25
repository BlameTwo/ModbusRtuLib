using System.Threading.Tasks;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;

namespace ModbusRtuLib.Contracts.Ascii
{
    public interface IModbusAsciiSlave
    {
        #region 同步

        /// <summary>
        /// 读取单线圈
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<bool> ReadCoilSignle(ushort start);

        /// <summary>
        /// 写入单线圈
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataResult<bool> WriteSingleCoil(ushort start, bool value);

        /// <summary>
        /// 读输入状态
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<bool> ReadDiscrete(ushort start);

        /// <summary>
        /// 读取Int16 一个modbus 寄存器地址的数据
        /// </summary>
        /// <param name="start">开始索引</param>
        /// <param name="readType">读取类型</param>
        /// <returns></returns>
        public DataResult<short> ReadInt16(
            ushort start,
            ReadType readType = ReadType.HoldingRegister
        );

        /// <summary>
        /// 读取Int32
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataResult<bool> WriteInt16(ushort start, short value);

        /// <summary>
        /// 读取单精度浮点
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<float> ReadFloat(ushort start);

        /// <summary>
        /// 写入单精度浮点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        public DataResult<bool> WriteFloat(ushort start, float value);

        public DataResult<bool> WriteInt32(ushort start, int value);

        public DataResult<int> ReadInt32(ushort start);
        #endregion

        #region 异步
        /// <summary>
        /// 异步写入线圈
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<DataResult<bool>> WriteSingleCoilAsync(ushort start, bool value);

        /// <summary>
        /// 异步读取线圈
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public Task<DataResult<bool>> ReadCoilSingleAsync(ushort start);

        /// <summary>
        /// 异步读取输入状态
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public Task<DataResult<bool>> ReadiscreteAsync(ushort start);

        /// <summary>
        /// 读取Int16 一个modbus 寄存器地址的数据
        /// </summary>
        /// <param name="start">开始索引</param>
        /// <param name="readType">读取类型</param>
        /// <returns></returns>
        public Task<DataResult<short>> ReadInt16Async(
            ushort start,
            ReadType readType = ReadType.HoldingRegister
        );

        public Task<DataResult<bool>> WriteInt16Async(ushort start, short value);
        #endregion

        #region 基础
        public byte[] ReadHoldingRegisters(ushort start, ushort length);
        #endregion
    }
}
