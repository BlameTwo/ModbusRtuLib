using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ModbusRtuLib.Models;

namespace ModbusRtuLib.Services.Rtu;

partial class ModbusRtuSlave
{
    public async Task<DataResult<bool>> ReadCoilSingleAsync(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadCoilSingle(start));
    }

    public async Task<DataResult<ushort>> ReadHoldingRegisterSingleAsync(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadHoldingRegisterSingle(start));
    }

    public async Task<DataResult<bool>> ReadDiscreteSingleAsync(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadDiscreteSingle(start));
    }

    public async Task<DataResult<bool>> WriteCoilAsync(ushort start, bool value)
    {
        return await Task.Factory.StartNew(() => WriteCoil(start, value));
    }

    public async Task<DataResult<bool>> WriteInt16Async(ushort start, short value)
    {
        return await Task.Factory.StartNew(() => WriteInt16(start, value));
    }

    public async Task<DataResult<short>> ReadInt16Async(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadInt16(start));
    }

    public async Task<DataResult<bool>> WriteInt32Async(ushort start, int value)
    {
        return await Task.Factory.StartNew(() => WriteInt32(start, value));
    }

    public async Task<DataResult<int>> ReadInt32Async(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadInt32(start));
    }

    public async Task<DataResult<bool>> WriteDoubleAsync(ushort start, double value)
    {
        return await Task.Factory.StartNew(() => WriteDouble(start, value));
    }

    public async Task<DataResult<double>> ReadDoubleAsync(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadDouble(start));
    }

    public async Task<DataResult<bool>> WriteFloatAsync(ushort start, float value)
    {
        return await Task.Factory.StartNew(() => WriteFloat(start, value));
    }

    public async Task<DataResult<float>> ReadFloatAsync(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadFloat(start));
    }

    public async Task<DataResult<bool>> WriteStringAsync(ushort start, string value, int Bytelength)
    {
        return await Task.Factory.StartNew(() => WriteString(start, value, Bytelength));
    }

    public async Task<DataResult<string>> ReadStringAsync(ushort start, ushort bytelength)
    {
        return await Task.Factory.StartNew(() => ReadString(start, bytelength));
    }

    public async Task<DataResult<bool>> WriteInt64Async(ushort start, long value)
    {
        return await Task.Factory.StartNew(() => WriteInt64(start, value));
    }

    public async Task<DataResult<long>> ReadInt64Async(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadInt64(start));
    }
}
