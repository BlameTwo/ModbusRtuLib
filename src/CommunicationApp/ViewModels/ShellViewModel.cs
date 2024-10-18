using System.Collections.ObjectModel;
using CommunicationApp.Contracts;
using CommunicationApp.Models.UI;
using CommunicationApp.ViewModels.MitsubishiViewModels;
using CommunicationApp.ViewModels.ModbusViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationApp.ViewModels;

public sealed partial class ShellViewModel : ObservableRecipient, IRecipient<HomeItemInvoke>
{
    public ShellViewModel(
        [FromKeyedServices(ProgramLife.ShellNavigationKey)]
            INavigationService shellNavigationService,
        [FromKeyedServices(ProgramLife.ShellNavigationKey)]
            INavigationViewService shellNavigationViewService
    )
    {
        ShellNavigationService = shellNavigationService;
        ShellNavigationViewService = shellNavigationViewService;
        ShellNavigationService.Navigated += ShellNavigationService_Navigated;
        this.IsActive = true;
    }

    private void ShellNavigationService_Navigated(
        object sender,
        Microsoft.UI.Xaml.Navigation.NavigationEventArgs e
    )
    {
        this.IsBack = ShellNavigationService.CanGoBack;
        this.SelectItem = ShellNavigationViewService.GetSelectItem(e.SourcePageType);
    }

    [ObservableProperty]
    object selectItem;

    public INavigationService ShellNavigationService { get; }
    public INavigationViewService ShellNavigationViewService { get; }

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
            new("\uEB7B", "Qna3E SerialPort", "三菱Qna3E串口通信示例", typeof(McNetQna3EViewModel))
            {
                FontFamily = new("FluentSystemIcons-Filled"),
            },
            new("\uEC05", "Qna3E Tcp", "三菱Qna3E Tcp 通信示例", typeof(McNetQna3ETcpViewModel)),
        };

    [ObservableProperty]
    ObservableCollection<HomeItem> saveItems = new();

    [ObservableProperty]
    bool isBack;

    [RelayCommand]
    void Loaded()
    {
        this.ShellNavigationService.NavigationTo<HomeViewModel>(null);
    }

    [RelayCommand]
    void GoBack()
    {
        this.ShellNavigationService.GoBack();
    }

    public void Receive(HomeItemInvoke message)
    {
        this.ShellNavigationService.NavigationTo(message.key.FullName, null);
    }
}
