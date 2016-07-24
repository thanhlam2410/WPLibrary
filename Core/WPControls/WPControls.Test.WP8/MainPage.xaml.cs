using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WPControls.Memory;

namespace ME.Controls.Test.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.IsVisible = false;

            MemoryTracker memoryTracker = new MemoryTracker();
            memoryTracker.Run(true);
        }

        private void prompt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PromptView.xaml", UriKind.Relative));
        }

        private void input_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InputView.xaml", UriKind.Relative));
        }

        private void message_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MessagePrompt.xaml", UriKind.Relative));
        }
    }
}