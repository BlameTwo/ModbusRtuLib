﻿using System;
using System.IO;
using Microsoft.UI.Xaml;

namespace CommunicationApp;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        this.UnhandledException += App_UnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
    }

    private void CurrentDomain_FirstChanceException(
        object sender,
        System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e
    )
    {
        if (!File.Exists("D:\\Log.txt"))
        {
            var fs = File.CreateText("D:\\Log.txt");
            fs.WriteLine(e.Exception.Message);
        }
        else
        {
            using (StreamWriter sw = new StreamWriter("D:\\Log.txt"))
            {
                sw.WriteLine(e.Exception.Message);
            }
        }
    }

    private void CurrentDomain_UnhandledException(
        object sender,
        System.UnhandledExceptionEventArgs e
    )
    {
        if (!File.Exists("D:\\Log.txt"))
        {
            var fs = File.CreateText("D:\\Log.txt");
            fs.WriteLine((e.ExceptionObject as Exception).Message);
        }
        else
        {
            using (StreamWriter sw = new StreamWriter("D:\\Log.txt"))
            {
                sw.WriteLine((e.ExceptionObject as Exception).Message);
            }
        }
    }

    private void App_UnhandledException(
        object sender,
        Microsoft.UI.Xaml.UnhandledExceptionEventArgs e
    )
    {
        if (!File.Exists("D:\\Log.txt"))
        {
            var fs = File.CreateText("D:\\Log.txt");
            fs.WriteLine(e.Message + e.Exception.Message);
        }
        else
        {
            using (StreamWriter sw = new StreamWriter("D:\\Log.txt"))
            {
                sw.WriteLine(e.Message + e.Exception.Message);
            }
        }
        e.Handled = true;
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = new Window();
        m_window.Activate();
    }

    private Window m_window;
}