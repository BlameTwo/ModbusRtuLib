﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace CommunicationApp.Controls
{
    public sealed partial class HeaderTile : Control
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            Button = (HyperlinkButton)GetTemplateChild("button");
            base.OnApplyTemplate();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(HeaderTile),
            new PropertyMetadata(null)
        );

        public ICommand InvokeCommand
        {
            get { return (ICommand)GetValue(InvokeCommandProperty); }
            set { SetValue(InvokeCommandProperty, value); }
        }
        public static readonly DependencyProperty InvokeCommandProperty =
            DependencyProperty.Register(
                "InvokeCommand",
                typeof(ICommand),
                typeof(HeaderTile),
                new PropertyMetadata(null)
            );

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description",
            typeof(string),
            typeof(HeaderTile),
            new PropertyMetadata(null)
        );

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(string),
            typeof(HeaderTile),
            new PropertyMetadata("")
        );

        public FontFamily IconFontFamily
        {
            get { return (FontFamily)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        public HyperlinkButton Button { get; private set; }

        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register(
                "IconFontFamily",
                typeof(FontFamily),
                typeof(HeaderTile),
                new PropertyMetadata(new FontFamily("Segoe Fluent Icons"))
            );
    }
}
