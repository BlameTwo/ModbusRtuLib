using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace CommunicationApp.ViewModels.MitsubishiViewModels;

partial class McNetQna3ETcpViewModel
{
    [RelayCommand]
    async Task ReadBit()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.ReadBitAsync(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadInt32()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.ReadInt32Async(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadInt16()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.ReadInt16Async(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadInt64()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.ReadInt64Async(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadFloat()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.ReadFloatAsync(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }

    [RelayCommand]
    async Task ReadDouble()
    {
        if (this.netTcp == null || !this.netTcp.IsConnected)
        {
            return;
        }
        var result = await netTcp.ReadDoubleAsync(Postion);
        if (result.IsOK)
        {
            this.AddMessage(result);
        }
    }
}
