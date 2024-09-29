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
    }

    public INavigationService ShellNavigationService { get; }
    public INavigationViewService ShellNavigationViewService { get; }

    [RelayCommand]
    void Loaded()
    {
        this.ShellNavigationService.NavigationTo<HomeViewModel>(null);
    }
}
