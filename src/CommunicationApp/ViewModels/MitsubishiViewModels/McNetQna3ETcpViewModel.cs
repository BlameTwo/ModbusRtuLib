using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunicationApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Models;
using ModbusRtuLib.Services.Mitsubishi;

namespace CommunicationApp.ViewModels.MitsubishiViewModels;

public sealed partial class McNetQna3ETcpViewModel : ObservableRecipient
{
    [ObservableProperty]
    string ipAddress;

    [ObservableProperty]
    string port = "6000";

    [ObservableProperty]
    bool _EnableStart = true;

    [ObservableProperty]
    string _NetworkId = "0";

    [ObservableProperty]
    SolidColorBrush connectStatus = new SolidColorBrush(Colors.Gray);

    [ObservableProperty]
    string _NetworkSlave = "0";

    [ObservableProperty]
    ObservableCollection<ModbusMessage> modbusMessages = new ObservableCollection<ModbusMessage>();

    IMcNetTcp netTcp = null;

    [RelayCommand]
    async Task Start()
    {
        if (netTcp != null)
            netTcp.Close();
        netTcp = new McNetTcp();
        netTcp.ConnectChanged += NetTcp_ConnectChanged;
        netTcp.NetWorkId = (byte)int.Parse(this.NetworkId);
        netTcp.NetWorkSlave = (byte)int.Parse(this.NetworkSlave);
        var result = await netTcp.OpenDeviceAsync(IpAddress, int.Parse(this.Port));
        if (result.Data)
        {
            this.EnableStart = false;
        }
    }

    private void NetTcp_ConnectChanged(object port, bool connect)
    {
        if (connect)
        {
            this.ConnectStatus = new SolidColorBrush(Colors.Green);
        }
        else
        {
            this.ConnectStatus = new SolidColorBrush(Colors.Gray);
        }
    }

    public void AddMessage<T>(DataResult<T> result)
    {
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
    }

    [RelayCommand]
    void Stop()
    {
        if (netTcp == null)
            return;
        netTcp.Close();
        netTcp = null;
        this.EnableStart = true;
    }
}
