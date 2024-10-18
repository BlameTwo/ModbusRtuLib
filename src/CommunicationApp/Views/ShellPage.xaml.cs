using System;
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

        private void AutoSuggestBox_TextChanged(
            AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args
        )
        {
            string userInput = sender.Text;

            ViewModel.SaveItems.Clear();
            foreach (var item in ViewModel.ModBusItems)
            {
                if (item.Title.Contains(userInput, StringComparison.OrdinalIgnoreCase)) // 基于 Title 属性的筛选
                {
                    ViewModel.SaveItems.Add(item);
                }
            }
        }
    }
}
