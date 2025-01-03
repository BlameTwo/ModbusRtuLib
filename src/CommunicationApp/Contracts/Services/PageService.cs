﻿using System;
using System.Collections.Generic;
using CommunicationApp.ViewModels;
using CommunicationApp.ViewModels.MitsubishiViewModels;
using CommunicationApp.ViewModels.ModbusViewModels;
using CommunicationApp.Views;
using CommunicationApp.Views.MitsubishiView;
using CommunicationApp.Views.ModbusView;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace CommunicationApp.Contracts.Services;

public sealed partial class PageService : IPageService
{
    private readonly Dictionary<string, Tuple<Type, Type>> _pages;

    public PageService()
    {
        _pages = new();
        this.RegisterView<ModbusSerialPortViewPage, ModbusSerialPortViewModel>();
        this.RegisterView<McNetQna3EPage, McNetQna3EViewModel>();
        this.RegisterView<McNetQna3ETcpPage, McNetQna3ETcpViewModel>();
        this.RegisterView<HomePage, HomeViewModel>();
    }

    public Type GetPage(string key)
    {
        _pages.TryGetValue(key, out var page);
        if (page is null)
        {
            return null;
        }
        return page.Item1;
    }

    public void RegisterView<View, ViewModel>()
        where View : Page
        where ViewModel : ObservableObject =>
        _pages.Add(typeof(ViewModel).FullName, new(typeof(View), typeof(ViewModel)));
}
