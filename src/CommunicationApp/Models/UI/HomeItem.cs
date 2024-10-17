using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;

namespace CommunicationApp.Models.UI;

public partial class HomeItem : ObservableObject
{
    [ObservableProperty]
    public string icon;

    [ObservableProperty]
    FontFamily fontFamily = new FontFamily("Segoe Fluent Icons");

    public HomeItem(string icon, string title, string description = null)
    {
        Icon = icon;
        Title = title;
        Description = description;
    }

    [ObservableProperty]
    public string title;

    [ObservableProperty]
    public string description;
}
