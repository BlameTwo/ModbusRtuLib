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
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Type = Models.ModbusMessageType.Rx,
                        Datas = result.OrginSend,
                    }
                );
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Type = Models.ModbusMessageType.Tx,
                        Datas = result.ReceivedData,
                    }
                );
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Value = result.Data.ToString(),
                        Type = Models.ModbusMessageType.Data,
                    }
                );
                break;
            case Models.ModbusRunType.Ascii:
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
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Type = Models.ModbusMessageType.Rx,
                        Datas = result.OrginSend,
                    }
                );
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Type = Models.ModbusMessageType.Tx,
                        Datas = result.ReceivedData,
                    }
                );
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Value = result.Data.ToString(),
                        Type = Models.ModbusMessageType.Data,
                    }
                );
                break;
            case Models.ModbusRunType.Ascii:
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
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Type = Models.ModbusMessageType.Rx,
                        Datas = result.OrginSend,
                    }
                );
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Type = Models.ModbusMessageType.Tx,
                        Datas = result.ReceivedData,
                    }
                );
                ModbusMessages.Add(
                    new Models.ModbusMessage()
                    {
                        Value = result.Data.ToString(),
                        Type = Models.ModbusMessageType.Data,
                    }
                );
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }
}
