using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace CommunicationApp.ViewModels.MitsubishiViewModels;

partial class McNetQna3EViewModel
{
    [RelayCommand]
    async Task ReadBit()
    {
        if (this.mcNetSerialPort == null || !this.mcNetSerialPort.IsConnected)
        {
            return;
        }
        var result = await mcNetSerialPort.ReadBitAsync(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadInt32()
    {
        if (this.mcNetSerialPort == null || !this.mcNetSerialPort.IsConnected)
        {
            return;
        }
        var result = await mcNetSerialPort.ReadInt32Async(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadInt16()
    {
        if (this.mcNetSerialPort == null || !this.mcNetSerialPort.IsConnected)
        {
            return;
        }
        var result = await mcNetSerialPort.ReadInt16Async(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadInt64()
    {
        if (this.mcNetSerialPort == null || !this.mcNetSerialPort.IsConnected)
        {
            return;
        }
        var result = await mcNetSerialPort.ReadInt64Async(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadFloat()
    {
        if (this.mcNetSerialPort == null || !this.mcNetSerialPort.IsConnected)
        {
            return;
        }
        var result = await mcNetSerialPort.ReadFloatAsync(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadDouble()
    {
        if (this.mcNetSerialPort == null || !this.mcNetSerialPort.IsConnected)
        {
            return;
        }
        var result = await mcNetSerialPort.ReadDoubleAsync(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }
}
