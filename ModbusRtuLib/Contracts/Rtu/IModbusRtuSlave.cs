using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Contracts.Rtu
{
    public interface IModbusRtuSlave
    {
        public ModbusSlaveConfig Config { get; }

        /// <summary>
        /// 读取单个线圈
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<bool> ReadCoilSingle(ushort start);

        public Task<DataResult<bool>> ReadCoilSingleAsync(ushort start);

        /// <summary>
        /// 读取单个保持寄存器
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<ushort> ReadHoldingRegisterSingle(ushort start);

        public Task<DataResult<ushort>> ReadHoldingRegisterSingleAsync(ushort start);

        /// <summary>
        /// 读取单个离散寄存器
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<bool> ReadDiscreteSingle(ushort start);

        public Task<DataResult<bool>> ReadDiscreteSingleAsync(ushort start);

        /// <summary>
        /// 写入单个线圈
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataResult<bool> WriteCoil(ushort start, bool value);

        public Task<DataResult<bool>> WriteCoilAsync(ushort start, bool value);

        /// <summary>
        /// 写入16位地址
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataResult<bool> WriteInt16(short start, short value);

        public Task<DataResult<bool>> WriteInt16Async(short start, short value);

        /// <summary>
        /// 写入一个float，占用4个字节，32位单精度数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteFloat(short start, float value);

        /// <summary>
        /// 写入双浮点double，占用8个字节
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteDouble(ushort start, double value);

        /// <summary>
        /// 写入long，占用8个字节，对应4个寄存器
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        public void WriteLong(short start, long value);

        public DataResult<long> ReadLong(ushort start);

        /// <summary>
        /// 读取单精度浮点数，对应保持寄存器2个
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<float> ReadFloat(ushort start);

        /// <summary>
        /// 读取16位数字，对应保持寄存器1个
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<short> ReadInt16(ushort start);

        public Task<DataResult<short>> ReadInt16Async(ushort start);

        /// <summary>
        /// 写入Int32
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataResult<bool> WriteInt32(ushort start, int value);

        /// <summary>
        /// 读取Int32
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<int> ReadInt32(ushort start);

        /// <summary>
        /// 读取双精度浮点数，对应保持寄存器4个
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DataResult<double> ReadDouble(ushort start);

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="start">起始寄存器</param>
        /// <param name="value">结束</param>
        /// <param name="length">必须指定长度，为了提醒你</param>
        public DataResult<bool> WriteString(ushort start, string value, int length);

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public DataResult<string> ReadString(ushort start, ushort length);
    }
}
