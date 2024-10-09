using CommunicationApp.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationApp.ViewModels;

public sealed partial class ShellViewModel : ObservableRecipient
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
}
