﻿using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WPControls.Toast;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ME.Controls.Test.WP81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PromptView : Page
    {
        public PromptView()
        {
            this.InitializeComponent();
        }

        private readonly SolidColorBrush _aliceBlueSolidColorBrush = new SolidColorBrush(Color.FromArgb(255, 240, 248, 255));
        private readonly SolidColorBrush _cornFlowerBlueSolidColorBrush = new SolidColorBrush(Color.FromArgb(200, 100, 149, 237));
        private static readonly string _testImg = "ms-appx:///Assets/iconapp_wp.png";

        const string LongText = "Testing text body wrapping with a bit of Lorem Ipsum.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at orci felis, in imperdiet tortor.";

        private ToastPrompt _prompt;

        private void InitializePrompt()
        {
            //var reuseObject = ReuseObject.IsChecked.GetValueOrDefault(false);

            if (_prompt != null)
            {
                _prompt.Completed -= PromptCompleted;
            }

            //if (!reuseObject || _prompt == null)
            {
                _prompt = new ToastPrompt();
            }

            _prompt.Completed += PromptCompleted;
            _prompt.Background = _cornFlowerBlueSolidColorBrush;
        }

        #region toast
        #region basic toast
        private void ToastBasicClick(object sender, RoutedEventArgs e)
        {
            InitializeBasicToast();

            _prompt.Show();
        }

        private void ToastWrapBasicClick(object sender, RoutedEventArgs e)
        {
            InitializeBasicToast(wrap: TextWrapping.Wrap);

            _prompt.Show();
        }

        private void InitializeBasicToast(string title = "Basic", TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.Title = title;
            _prompt.Message = LongText;
            _prompt.TextWrapping = wrap;
        }
        #endregion
        #region toast img and no title
        private void ToastWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
        {
            InitializeToastWithImgAndNoTitle();

            _prompt.Show();
        }

        private void ToastWrapWithImgAndNoTitleClick(object sender, RoutedEventArgs e)
        {
            InitializeToastWithImgAndNoTitle(TextWrapping.Wrap);

            _prompt.Show();
        }

        private void InitializeToastWithImgAndNoTitle(TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.Message = LongText;
            _prompt.ImageSource = new BitmapImage(new Uri(_testImg, UriKind.RelativeOrAbsolute));
            _prompt.ImageHeight = 30;
            _prompt.Stretch = Stretch.Uniform;
            _prompt.TextWrapping = TextWrapping.Wrap;
            _prompt.TextOrientation = Orientation.Horizontal;
            _prompt.FontSize = 15;
        }
        #endregion
        #region toast img and title
        private void ToastWithImgAndTitleClick(object sender, RoutedEventArgs e)
        {
            var toast = GetToastWithImgAndTitle();

            toast.Show();
        }

        private void ToastWrapWithImgAndTitleClick(object sender, RoutedEventArgs e)
        {
            var toast = GetToastWithImgAndTitle();
            toast.TextWrapping = TextWrapping.Wrap;

            toast.Show();
        }

        private static ToastPrompt GetToastWithImgAndTitle()
        {
            return new ToastPrompt
            {
                Title = "With Image",
                TextOrientation = Orientation.Vertical,
                Message = LongText,
                ImageSource = new BitmapImage(new Uri(_testImg, UriKind.RelativeOrAbsolute))
            };
        }
        #endregion
        #region toast with custom everything and event
        private void ToastAdvancedClick(object sender, RoutedEventArgs e)
        {
            InitializeAdvancedToast();

            _prompt.Show();
        }

        private void ToastWrapAdvancedClick(object sender, RoutedEventArgs e)
        {
            InitializeAdvancedToast(TextWrapping.Wrap);

            _prompt.Show();
        }

        private void InitializeAdvancedToast(TextWrapping wrap = default(TextWrapping))
        {
            InitializePrompt();

            _prompt.IsAppBarVisible = false;
            _prompt.Title = "Advanced";
            _prompt.Message = "Custom Fontsize, img, and orientation";
            _prompt.ImageSource = new BitmapImage(new Uri(_testImg, UriKind.RelativeOrAbsolute));
            _prompt.FontSize = 50;
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Background = _aliceBlueSolidColorBrush;
            _prompt.Foreground = _cornFlowerBlueSolidColorBrush;

            _prompt.TextWrapping = wrap;
        }
        #endregion
        #region stress test
        private void ToastSysTrayVisClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray();
            InitializeBasicToast("Test Vis");

            _prompt.Show();
        }

        private void ToastSysTrayNotVisClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray(false);
            InitializeBasicToast("Test not Vis");

            _prompt.Show();
        }

        private void ToastSysTrayVisWithOpacityClick(object sender, RoutedEventArgs e)
        {
            AdjustSystemTray(true, .8);
            InitializeBasicToast("Test with Opacity");

            _prompt.Show();
        }
        #endregion

        #region large image
        private void LargeImageClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "With Image";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageSource = new BitmapImage(new Uri(_testImg, UriKind.RelativeOrAbsolute));

            _prompt.Show();
        }

        private void LargeImageWidthHeightClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Width + Height";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageHeight = 50;
            _prompt.ImageWidth = 100;
            _prompt.ImageSource = new BitmapImage(new Uri(_testImg, UriKind.RelativeOrAbsolute));

            _prompt.Show();
        }

        private void LargeImageStretchClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Stretch";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageHeight = 100;
            _prompt.Stretch = Stretch.Fill;
            _prompt.TextWrapping = TextWrapping.WrapWholeWords;
            _prompt.ImageSource = new BitmapImage(new Uri(_testImg, UriKind.RelativeOrAbsolute));

            _prompt.Show();
        }

        private void LargeImageStretchWidthHeightClick(object sender, RoutedEventArgs e)
        {
            InitializePrompt();

            _prompt.Title = "Stretch + Width + Height";
            _prompt.TextOrientation = Orientation.Vertical;
            _prompt.Message = LongText;
            _prompt.ImageHeight = 50;
            _prompt.ImageWidth = 100;
            _prompt.Stretch = Stretch.Uniform;
            _prompt.ImageSource = new BitmapImage(new Uri(_testImg, UriKind.RelativeOrAbsolute));

            _prompt.Show();
        }
        #endregion
        #endregion

        void PromptCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            Results.Text = string.Format("{0}::{1}", e.PopUpResult, e.Result);
        }

        private static void AdjustSystemTray(bool isVisible = true, double opacity = 1)
        {
            //SystemTray.IsVisible = isVisible;
            //SystemTray.Opacity = opacity;
        }
    }
}
