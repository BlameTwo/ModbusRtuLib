using CommunicationApp.ViewModels.MitsubishiViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace CommunicationApp.Views.MitsubishiView;

public sealed partial class McNetQna3EPage : Page
{
    public McNetQna3EPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.ServiceProvider.GetService<McNetQna3EViewModel>();
    }

    public McNetQna3EViewModel ViewModel { get; }
}
