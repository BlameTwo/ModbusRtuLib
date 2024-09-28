using CommunicationApp.ViewModels.ModbusViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace CommunicationApp.Views.ModbusView;

public sealed partial class ModbusSerialPortViewPage : Page
{
    public ModbusSerialPortViewPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.ServiceProvider.GetService<ModbusSerialPortViewModel>();
    }

    internal ModbusSerialPortViewModel ViewModel { get; private set; }
}
