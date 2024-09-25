using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Ascii
{
    public interface IModbusAsciiSlave
    {
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
        #endregion
    }
}
