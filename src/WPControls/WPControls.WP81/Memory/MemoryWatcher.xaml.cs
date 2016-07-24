using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WPControls.Memory
{
    public sealed partial class MemoryWatcher : UserControl
    {
        public MemoryWatcher()
        {
            this.InitializeComponent();
            this.Loaded += MemoryWatcher_Loaded;
        }

        private void MemoryWatcher_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
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
