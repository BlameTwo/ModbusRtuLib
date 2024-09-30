using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommunicationApp.ViewModels.ModbusViewModels;

partial class ModbusSerialPortViewModel
{
    [ObservableProperty]
    string writeData;

    [RelayCommand]
    async Task WriteSingileCoilAsync()
    {
        switch (this._runType)
        {
            case Models.ModbusRunType.Rtu:
                if (rtuClient == null)
                {
                    return;
                }
                var result = await rtuClient
                    .GetSlave((byte)SlaveDevice)
                    .WriteCoilAsync(Convert.ToUInt16(StartPostion), Convert.ToBoolean(WriteData));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task WriteInt16Async()
    {
        switch (this._runType)
        {
            case Models.ModbusRunType.Rtu:
                if (rtuClient == null)
                {
                    return;
                }
                var result = await rtuClient
                    .GetSlave((byte)SlaveDevice)
                    .WriteInt16Async(Convert.ToUInt16(StartPostion), Convert.ToInt16(WriteData));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task WriteInt32Async()
    {
        switch (this._runType)
        {
            case Models.ModbusRunType.Rtu:
                if (rtuClient == null)
                {
                    return;
                }
                var result = await rtuClient
                    .GetSlave((byte)SlaveDevice)
                    .WriteInt32Async(Convert.ToUInt16(StartPostion), Convert.ToInt32(WriteData));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task WriteInt64Async()
    {
        switch (this._runType)
        {
            case Models.ModbusRunType.Rtu:
                if (rtuClient == null)
                {
                    return;
                }
                var result = await rtuClient
                    .GetSlave((byte)SlaveDevice)
                    .WriteInt64Async(Convert.ToUInt16(StartPostion), Convert.ToInt64(WriteData));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task WriteFloatAsync()
    {
        switch (this._runType)
        {
            case Models.ModbusRunType.Rtu:
                if (rtuClient == null)
                {
                    return;
                }
                var result = await rtuClient
                    .GetSlave((byte)SlaveDevice)
                    .WriteFloatAsync(Convert.ToUInt16(StartPostion), Convert.ToSingle(WriteData));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task WriteDoubleAsync()
    {
        switch (this._runType)
        {
            case Models.ModbusRunType.Rtu:
                if (rtuClient == null)
                {
                    return;
                }
                var result = await rtuClient
                    .GetSlave((byte)SlaveDevice)
                    .WriteDoubleAsync(Convert.ToUInt16(StartPostion), Convert.ToDouble(WriteData));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }
}
