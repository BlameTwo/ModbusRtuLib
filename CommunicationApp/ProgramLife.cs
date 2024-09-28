﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationApp.Contracts;
using CommunicationApp.Contracts.Services;
using CommunicationApp.ViewModels;
using CommunicationApp.ViewModels.ModbusViewModels;
using CommunicationApp.Views;
using CommunicationApp.Views.ModbusView;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationApp
{
    public static class ProgramLife
    {
        public const string ShellNavigationKey = "ShellNavigationKey";
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void InitService()
        {
            ServiceProvider = new ServiceCollection()
                #region Navigation
                .AddKeyedSingleton<INavigationViewService, ShellNavigaionViewService>(
                    ProgramLife.ShellNavigationKey
                )
                .AddKeyedSingleton<INavigationService, ShellNavigationService>(
                    ProgramLife.ShellNavigationKey
                )
                .AddTransient<IPageService, PageService>()
                #endregion
                #region Viwe And ViewModel
                .AddTransient<ShellPage>()
                .AddTransient<ShellViewModel>()
                .AddTransient<ModbusSerialPortViewModel>()
                #endregion
                .BuildServiceProvider();
        }
    }
}