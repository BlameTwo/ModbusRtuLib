using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace CommunicationApp.ViewModels.ModbusViewModels;

partial class ModbusSerialPortViewModel
{
    [RelayCommand]
    async Task ReadSingileCoilAsync()
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
                    .ReadCoilSingleAsync(Convert.ToUInt16(StartPostion));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                if (asciiClient == null)
                    return;
                var asciiResult = await asciiClient
                    .GetSlave((byte)SlaveDevice)
                    .ReadCoilSingleAsync(Convert.ToUInt16(StartPostion));
                AddMessage(asciiResult);
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task ReadSingleDiscreteAsync()
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
                    .ReadDiscreteSingleAsync(Convert.ToUInt16(StartPostion));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                if (asciiClient == null)
                    return;
                var asciiResult = await asciiClient
                    .GetSlave((byte)SlaveDevice)
                    .ReadDiscreteAsync(Convert.ToUInt16(StartPostion));
                AddMessage(asciiResult);
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task ReadInt16Async()
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
                    .ReadInt16Async(Convert.ToUInt16(StartPostion));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                if (asciiClient == null)
                    return;
                var asciiResult = await asciiClient
                    .GetSlave((byte)SlaveDevice)
                    .ReadInt16Async(Convert.ToUInt16(StartPostion));
                AddMessage(asciiResult);
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task ReadInt32Async()
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
                    .ReadInt32Async(Convert.ToUInt16(StartPostion));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                if (asciiClient == null)
                    return;
                var asciiResult = await asciiClient
                    .GetSlave((byte)SlaveDevice)
                    .ReadInt32Async(Convert.ToUInt16(StartPostion));
                AddMessage(asciiResult);
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task ReadInt64Async()
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
                    .ReadInt64Async(Convert.ToUInt16(StartPostion));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                if (asciiClient == null)
                    return;
                var asciiResult = await asciiClient
                    .GetSlave((byte)SlaveDevice)
                    .ReadInt64Async(Convert.ToUInt16(StartPostion));
                AddMessage(asciiResult);
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task ReadDoubleAsync()
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
                    .ReadDoubleAsync(Convert.ToUInt16(StartPostion));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                if (asciiClient == null)
                    return;
                var asciiResult = await asciiClient
                    .GetSlave((byte)SlaveDevice)
                    .ReadDoubleAsync(Convert.ToUInt16(StartPostion));
                AddMessage(asciiResult);
                break;
            default:
                break;
        }
    }

    [RelayCommand]
    async Task ReadFloatAsync()
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
                    .ReadFloatAsync(Convert.ToUInt16(StartPostion));
                AddMessage(result);
                break;
            case Models.ModbusRunType.Ascii:
                if (asciiClient == null)
                    return;
                var asciiResult = await asciiClient
                    .GetSlave((byte)SlaveDevice)
                    .ReadFloatAsync(Convert.ToUInt16(StartPostion));
                AddMessage(asciiResult);
                break;
            default:
                break;
        }
    }
}
