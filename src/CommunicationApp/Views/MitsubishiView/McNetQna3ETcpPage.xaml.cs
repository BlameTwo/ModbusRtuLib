using CommunicationApp.ViewModels.MitsubishiViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace CommunicationApp.Views.MitsubishiView;

public sealed partial class McNetQna3ETcpPage : Page
{
    public McNetQna3ETcpPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.ServiceProvider.GetService<McNetQna3ETcpViewModel>();
    }

    public McNetQna3ETcpViewModel ViewModel { get; }
}
