using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CommunicationApp.Contracts;

public interface INavigationService
{
    public void RegisterView(Frame frame);

    public void UnRegisterView();

    public bool CanGoBack { get; }

    public bool CanGoForward { get; }

    public bool GoBack();

    public bool GoForward();

    public bool NavigationTo(string key, object args);

    public bool NavigationTo<ViewModel>(object args)
        where ViewModel : ObservableObject;

    public event NavigatedEventHandler Navigated;

    public event NavigationFailedEventHandler NavigationFailed;
}
