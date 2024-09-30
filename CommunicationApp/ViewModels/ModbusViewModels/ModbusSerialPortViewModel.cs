﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationApp.Common;
using CommunicationApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using ModbusRtuLib.Common;
using ModbusRtuLib.Common.Factorys;
using ModbusRtuLib.Contracts.Ascii;
using ModbusRtuLib.Contracts.Rtu;
using ModbusRtuLib.Models;
using ModbusRtuLib.Models.Enums;

namespace CommunicationApp.ViewModels.ModbusViewModels;

public sealed partial class ModbusSerialPortViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<string> ports = SerialPort.GetPortNames().ToObservable();

    [ObservableProperty]
    string _selectPort;

    [ObservableProperty]
    ObservableCollection<string> contracts = new ObservableCollection<string>() { "Rtu", "Ascii" };

    ModbusRunType _runType;

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
    StopBits _selectStopBit;

    [ObservableProperty]
    ObservableCollection<int> dataBit = new() { 5, 6, 7, 8 };

    [ObservableProperty]
    bool enableStart = true;

    [ObservableProperty]
    int _selectBit;

    [ObservableProperty]
    SolidColorBrush _connectStatus = new SolidColorBrush(Colors.Gray);

    [ObservableProperty]
    double _baudRate;

    [ObservableProperty]
    ObservableCollection<Parity> paritys =
        new() { Parity.None, Parity.Odd, Parity.Mark, Parity.Space, Parity.Even };

    [ObservableProperty]
    string _startPostion = "0001";

    [ObservableProperty]
    int _slaveDevice;

    [ObservableProperty]
    Parity _selectParity;

    [ObservableProperty]
    ObservableCollection<DataFormat> dataFormats =
        new() { DataFormat.BADC, DataFormat.DCBA, DataFormat.ABCD, DataFormat.CDAB };

    [ObservableProperty]
    ObservableCollection<ModbusMessage> modbusMessages = new ObservableCollection<ModbusMessage>();

    [ObservableProperty]
    DataFormat _selectFormat;

    IModbusRtuClient rtuClient = null;
    IModbusAsciiClient asciiClient = null;

    [RelayCommand]
    void BuildStart()
    {
        if (SelectContract == "Rtu")
        {
            if (rtuClient != null)
            {
                rtuClient.DataRecived -= Port_DataReceived;
                rtuClient.ConnectChanged -= RtuClient_ConnectChanged;
                rtuClient.Dispose();
                rtuClient = null;
            }
            rtuClient = DeviceFactory
                .CreateDefaultRtuClient()
                .InitSerialPort(p =>
                {
                    p.SerialPortName = this.SelectPort;
                    p.BaudRate = Convert.ToInt32(this.BaudRate);
                    p.DataFormat = SelectFormat;
                    p.Parity = SelectParity;
                    p.DataBit = SelectBit;
                    p.StopBit = SelectStopBit;
                    p.WriteTimeSpan = 200;
                    p.ReadTimeSpan = 200;
                });

            rtuClient.DataRecived += Port_DataReceived;
            rtuClient.ConnectChanged += RtuClient_ConnectChanged;
            rtuClient.SetupStart();
            _runType = ModbusRunType.Rtu;
        }
        else if (SelectContract == "Ascii") { }

        EnableStart = false;
    }

    [RelayCommand]
    void StopPort()
    {
        if (rtuClient != null)
        {
            this.rtuClient.Close();
            rtuClient.DataRecived -= Port_DataReceived;
            rtuClient.ConnectChanged -= RtuClient_ConnectChanged;
            this.rtuClient.Dispose();
            this.rtuClient = null;
            EnableStart = true;
        }
        GC.Collect();
    }

    private void RtuClient_ConnectChanged(object sender, bool connect)
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

    private void Port_DataReceived(object sender, ModbusDataModel message) { }
}
