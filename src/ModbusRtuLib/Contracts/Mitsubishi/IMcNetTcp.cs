using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Handlers;

namespace ModbusRtuLib.Contracts.Mitsubishi
{
    public interface IMcNetTcp
    {
        ISocketDevice Device { get; }

        #region Sync
        public DataResult<bool> Write(float value, string address);
        DataResult<bool> ReadBit(string address);
        DataResult<bool> WriteBit(string address, bool value);
        DataResult<bool> Write(int value, string address);
        DataResult<bool> Write(long value, string address);
        DataResult<bool> Write(short value, string address);
        DataResult<int> ReadInt32(string address);
        DataResult<float> ReadFloat(string address);
        DataResult<double> ReadDouble(string address);
        DataResult<long> ReadInt64(string address);
        DataResult<short> ReadInt16(string address);

        #endregion

        #region Async
        Task<DataResult<bool>> WriteAsync(float value, string address);
        Task<DataResult<bool>> WriteAsync(int value, string address);
        Task<DataResult<bool>> WriteAsync(long value, string address);
        Task<DataResult<bool>> WriteAsync(short value, string address);
        Task<DataResult<byte[]>> ReadAsync(string address, ushort length);
        Task<DataResult<int>> ReadInt32Async(string address);
        Task<DataResult<float>> ReadFloatAsync(string address);
        Task<DataResult<double>> ReadDoubleAsync(string address);
        Task<DataResult<long>> ReadInt64Async(string address);
        Task<DataResult<short>> ReadInt16Async(string address);
        Task<DataResult<bool>> ReadBitAsync(string address);
        Task<DataResult<bool>> WriteBitAsync(string address, bool value);
        #endregion
        public byte[] DeviceCode { get; set; }
        byte NetWorkId { get; set; }
        byte NetWorkSlave { get; set; }
        IMcNetAddressParse Parse { get; }

        event MitsubishiQna3EConnectChanged ConnectChanged;

        bool IsConnected { get; }

        void Close();

        DataResult<bool> OpenDevice(string address, int port = 6000);

        Task<DataResult<bool>> OpenDeviceAsync(string address, int port = 6000);

        int TimeSpan { get; set; }

        DataResult<bool> Write(double value, string address);
        Task<DataResult<bool>> WriteAsync(double value, string address);
    }
}
