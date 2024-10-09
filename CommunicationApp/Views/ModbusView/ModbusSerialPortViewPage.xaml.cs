using CommunicationApp.ViewModels.ModbusViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CommunicationApp.Views.ModbusView;

public sealed partial class ModbusSerialPortViewPage : Page
{
    public ModbusSerialPortViewPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.ServiceProvider.GetService<ModbusSerialPortViewModel>();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        this.ViewModel.StopPortCommand.Execute(null);
    }

    internal ModbusSerialPortViewModel ViewModel { get; private set; }
}
