using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WPControls.Memory;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace ME.Controls.Test.WP81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            MemoryTracker memoryTracker = new MemoryTracker();
            memoryTracker.Run(true);
            memoryTracker.VerticalAlignment = VerticalAlignment.Bottom;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            
        }

        private void prompt_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PromptView));
        }

        private void input_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InputView));
        }

        private void message_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MessagePromptView));
        }
    }
}
