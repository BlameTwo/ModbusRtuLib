using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using CommunicationApp.Common;
using CommunicationApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using ModbusRtuLib.Contracts.Mitsubishi;
using ModbusRtuLib.Models;
using ModbusRtuLib.Services.Mitsubishi;

namespace CommunicationApp.ViewModels.MitsubishiViewModels;

public sealed partial class McNetQna3EViewModel : ObservableObject
{
    IMcNetSerialPort mcNetSerialPort;

    [ObservableProperty]
    ObservableCollection<string> ports = SerialPort.GetPortNames().ToObservable();

    [ObservableProperty]
    string _selectPort;

    [ObservableProperty]
    ObservableCollection<string> contracts = new ObservableCollection<string>()
    {
        "Binary",
        "Ascii",
    };

    [ObservableProperty]
    string _selectContract;

    [ObservableProperty]
    ObservableCollection<StopBits> stopBits =
        new()
        {
            System.IO.Ports.StopBits.None,
            System.IO.Ports.StopBits.One,
            System.IO.Ports.StopBits.Two,
        };

    [ObservableProperty]
    StopBits _selectStopBit = System.IO.Ports.StopBits.One;

    [ObservableProperty]
    ObservableCollection<int> dataBit = new() { 5, 6, 7, 8 };

    [ObservableProperty]
    bool enableStart = true;

    [ObservableProperty]
    int _selectBit = 8;

    [ObservableProperty]
    SolidColorBrush _connectStatus = new SolidColorBrush(Colors.Gray);

    [ObservableProperty]
    double _baudRate = 9600;

    [ObservableProperty]
    ObservableCollection<Parity> paritys =
        new() { Parity.None, Parity.Odd, Parity.Mark, Parity.Space, Parity.Even };

    [ObservableProperty]
    int _slaveDevice;

    [ObservableProperty]
    string _networkId = "0";

    [ObservableProperty]
    string _networkSlave = "0";

    [ObservableProperty]
    Parity _selectParity = Parity.None;

    [ObservableProperty]
    ObservableCollection<ModbusMessage> modbusMessages = new ObservableCollection<ModbusMessage>();

    [RelayCommand]
    void Start()
    {
        if (SelectContract == "Binary")
        {
            mcNetSerialPort = new McNetSerialPort();
            mcNetSerialPort.ConnectChanged += McNetSerialPort_ConnectChanged;
            mcNetSerialPort.OpenSerial(
                new ModbusRtuLib.Models.SerialPortConfig()
                {
                    DataBit = this.SelectBit,
                    Parity = this.SelectParity,
                    SerialPortName = this.SelectPort,
                    StopBit = this.SelectStopBit,
                    BaudRate = (int)this.BaudRate,
                }
            );
            if (mcNetSerialPort.IsConnected)
            {
                this.mcNetSerialPort.NetWorkId = (byte)Convert.ToInt32(this.NetworkId);
                this.mcNetSerialPort.NetWorkSlave = (byte)Convert.ToInt32(this.NetworkSlave);
            }
            EnableStart = false;
        }
    }

    private void McNetSerialPort_ConnectChanged(IMcNetSerialPort port, bool connect)
    {
        if (connect == true)
        {
            this.ConnectStatus = new SolidColorBrush(Colors.Green);
        }
        else
        {
            this.ConnectStatus = new SolidColorBrush(Colors.Gray);
        }
    }

    [RelayCommand]
    void Stop()
    {
        if (mcNetSerialPort != null)
            mcNetSerialPort.Dispose();
        EnableStart = true;
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
}
