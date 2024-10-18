using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media;
using WinRT;

namespace CommunicationApp.Models.UI;

[Windows.UI.Xaml.Data.Bindable]
[GeneratedBindableCustomPropertyAttribute()]
public partial class HomeItem : ObservableObject
{
    [ObservableProperty]
    public string icon;

    [ObservableProperty]
    FontFamily fontFamily = new FontFamily("Segoe Fluent Icons");

    public HomeItem(string icon, string title, string description, Type pageKey)
    {
        Icon = icon;
        Title = title;
        Description = description;
        PageKey = pageKey;
    }

    [RelayCommand]
    void NavigationTo()
    {
        WeakReferenceMessenger.Default.Send(new HomeItemInvoke(this.PageKey));
    }

    public Type PageKey { get; set; }

    [ObservableProperty]
    public string title;

    [ObservableProperty]
    public string description;
}

public record HomeItemInvoke(Type key);
