using CommunicationApp.ViewModels.ModbusViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace CommunicationApp.Views.ModbusView;

public sealed partial class ModbusAsciiView : Page
{
    public ModbusAsciiView()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.ServiceProvider.GetService<ModbusAsciiViewModel>();
    }

    public ModbusAsciiViewModel ViewModel { get; }
}
