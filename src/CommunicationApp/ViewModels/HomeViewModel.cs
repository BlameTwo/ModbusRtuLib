﻿using System.Collections.ObjectModel;
using CommunicationApp.Models.UI;
using CommunicationApp.ViewModels.MitsubishiViewModels;
using CommunicationApp.ViewModels.ModbusViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CommunicationApp.ViewModels
{
    public sealed partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<HomeItem> modBusItems =
            new()
            {
                new(
                    "\uEB7B",
                    "Modbus SerialPort",
                    "Modbus串口通信示例",
                    typeof(ModbusSerialPortViewModel)
                )
                {
                    FontFamily = new("FluentSystemIcons-Filled"),
                },
                //new("\uEC05", "Modbus Tcp", "Modbus网络通信示例"),
                new(
                    "\uEB7B",
                    "Qna3E SerialPort",
                    "三菱Qna3E串口通信示例",
                    typeof(McNetQna3EViewModel)
                )
                {
                    FontFamily = new("FluentSystemIcons-Filled"),
                },
                new("\uEC05", "Qna3E Tcp", "三菱Qna3ETcp通信示例", typeof(McNetQna3ETcpViewModel)),
                //new("\uEC05", "Qna3E Tcp", "三菱Qna3E网络通信示例"),
            };
    }
}
