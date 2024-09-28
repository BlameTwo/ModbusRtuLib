using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationApp.Common;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CommunicationApp.ViewModels.ModbusViewModels;

public sealed partial class ModbusSerialPortViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<string> ports = SerialPort.GetPortNames().ToObservable();

    [ObservableProperty]
    ObservableCollection<string> contracts = new ObservableCollection<string>() { "Rtu", "Ascii" };
}
