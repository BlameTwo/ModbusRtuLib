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

    public async Task<DataResult<bool>> WriteInt16Async(short start, short value)
    {
        return await Task.Factory.StartNew(() => WriteInt16(start, value));
    }

    public async Task<DataResult<short>> ReadInt16Async(ushort start)
    {
        return await Task.Factory.StartNew(() => ReadInt16(start));
    }
}
