using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommunicationApp.ViewModels.MitsubishiViewModels;

partial class McNetQna3ETcpViewModel
{
    [ObservableProperty]
    string valueData;

    [ObservableProperty]
    string postion;

    [RelayCommand]
    async Task WriteBit()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.WriteBitAsync(Postion, Convert.ToBoolean(ValueData));
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task WriteInt32()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.WriteAsync(Convert.ToInt32(ValueData), Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task WriteInt16()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.WriteAsync(Convert.ToInt16(ValueData), Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task WriteInt64()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.WriteAsync(Convert.ToInt64(ValueData), Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task WriteFloat()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.WriteAsync(Convert.ToSingle(ValueData), Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task WriteDouble()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.WriteAsync(Convert.ToDouble(ValueData), Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }
}
