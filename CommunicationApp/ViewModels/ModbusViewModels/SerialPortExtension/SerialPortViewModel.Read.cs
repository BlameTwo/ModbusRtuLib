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
                    .GetSlave(1)
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
                break;
            case Models.ModbusRunType.Ascii:
                break;
            default:
                break;
        }
    }
}
