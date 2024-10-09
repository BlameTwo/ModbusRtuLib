using CommunicationApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CommunicationApp.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage(ShellViewModel shellViewModel)
        {
            this.InitializeComponent();
            this.ViewModel = shellViewModel;
            this.ViewModel.ShellNavigationViewService.Register(this.view);
            this.ViewModel.ShellNavigationService.RegisterView(this.frame);
        }

        public ShellViewModel ViewModel { get; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            view.IsPaneOpen = !view.IsPaneOpen;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}
