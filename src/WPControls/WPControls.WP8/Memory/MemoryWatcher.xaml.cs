using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.UI.Core;

namespace WPControls.Memory
{
    public partial class MemoryWatcher : UserControl
    {
        public MemoryWatcher()
        {
            InitializeComponent();
            this.Loaded += MemoryWatcher_Loaded;
        }

        private void MemoryWatcher_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = Application.Current.Host.Content.ActualWidth;
            this.Height = Application.Current.Host.Content.ActualHeight;
        }

        public void SetValue(string value)
        {
            MemoryCount.Text = value;
        }

        public void SetVerticalAlignment(VerticalAlignment value)
        {
            LayoutRoot.VerticalAlignment = value;
        }

        public void SetMargin(Thickness value)
        {
            LayoutRoot.Margin = value;
        }
    }
}
