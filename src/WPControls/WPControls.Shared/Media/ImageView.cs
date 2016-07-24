#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

using WPCore.Http;
using WPCore.Http.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System;

namespace WPControls.Media
{
    [TemplatePart(Name = PrimaryImage, Type = typeof(Image))]
    [TemplatePart(Name = PlaceholderBorder, Type = typeof(Border))]
    public class ImageView : Control
    {
        #region Constants
        public const string PrimaryImage = "PrimaryImage";

        public const string PlaceholderBorder = "PlaceholderBorder";
        #endregion

        #region Fields
        private ImageStates _imageState = ImageStates.ScreenSizeThumbnail;
        private BitmapImage _thumbnailBitmapImage = null;
        private ImageSource _thumbnailImageSource = null;
        private BitmapImage _fullResolutionBitmapImage = null;
        private ImageSource _fullResolutionImageSource = null;
        
        private Image _primaryImage;
        private Border _placeholderBorder;
        private bool _isPrimaryImageLoaded;

        private IPhotoSource _dataSource;
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(IPhotoSource),
            typeof(ImageView),
            new PropertyMetadata(default(IPhotoSource), OnSourceChanged));

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public IPhotoSource Source
        {
            get { return (IPhotoSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch",
            typeof(Stretch),
            typeof(ImageView),
            new PropertyMetadata(default(Stretch)));

        /// <summary>
        /// Gets or sets the stretch.
        /// </summary>
        /// <value>
        /// The stretch.
        /// </value>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderOpacityProperty =
            DependencyProperty.Register("PlaceholderOpacity", typeof(double), typeof(ImageView), new PropertyMetadata(1.0));

        public double PlaceholderOpacity
        {
            get { return (double)GetValue(PlaceholderOpacityProperty); }
            set { SetValue(PlaceholderOpacityProperty, value); }
        }
        
        public static readonly DependencyProperty PlaceholderBackgroundProperty = DependencyProperty.Register(
           "PlaceholderBackground",
           typeof(SolidColorBrush),
           typeof(ImageView),
           new PropertyMetadata(default(SolidColorBrush)));

        /// <summary>
        /// Gets or sets the placeholder background.
        /// </summary>
        /// <value>
        /// The placeholder background.
        /// </value>
        public SolidColorBrush PlaceholderBackground
        {
            get { return (SolidColorBrush)GetValue(PlaceholderBackgroundProperty); }
            set { SetValue(PlaceholderBackgroundProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the placeholder stretch.
        /// </summary>
        /// <value>
        /// The placeholder stretch.
        /// </value>
        public Stretch PlaceholderImageStretch
        {
            get { return (Stretch)GetValue(PlaceholderImageStretchProperty); }
            set { SetValue(PlaceholderImageStretchProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderImageStretchProperty = DependencyProperty.Register(
            "PlaceholderImageStretch",
            typeof(Stretch),
            typeof(ImageView),
            new PropertyMetadata(default(Stretch)));
        
        public DataTemplate PlaceHolderTemplate
        {
            get { return (DataTemplate)GetValue(PlaceHolderTemplateProperty); }
            set { SetValue(PlaceHolderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTemplateProperty =
            DependencyProperty.Register("PlaceHolderTemplate", typeof(DataTemplate), typeof(ImageView), new PropertyMetadata(null));
        #endregion

        #region Private Methods
        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // If the initial source and new values are null, do nothing
            if (obj == null || e.NewValue == null || e.NewValue == e.OldValue)
                return;

            var si = obj as ImageView;
            
            if (si == null || si._primaryImage == null)
                return;

            si.OnSourcePropertyChanged();
        }

        private void OnSourcePropertyChanged()
        {
            _isPrimaryImageLoaded = false;
            UpdatePlaceholderImageVisibility();

            BeginLoadingThumbnail();
        }

        private void OnPrimaryImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            _isPrimaryImageLoaded = true;
            UpdatePlaceholderImageVisibility();
        }

        private void OnPrimaryImageFailed(object sender, RoutedEventArgs routedEventArgs)
        {
            _isPrimaryImageLoaded = false;
            UpdatePlaceholderImageVisibility();
        }

        private void OnThumbnailOpened(object sender, RoutedEventArgs e)
        {
            _primaryImage.Source = null;
            _primaryImage.Source = _thumbnailImageSource;

            ClearImageSources();

            UpdatePlaceholderImageVisibility();
            InvalidateMeasure();
        }

        private void OnFullSizeImageOpened(object sender, RoutedEventArgs e)
        {
            _primaryImage.Source = null;
            _primaryImage.Source = _fullResolutionImageSource;

            ClearImageSources();

            UpdatePlaceholderImageVisibility();
            InvalidateMeasure();
        }

        private void UpdatePlaceholderImageVisibility()
        {
            // We hide the border not the Image as the border could be being used and as the 
            // PlaceholderImage is within the border, we get a twofer.
            if (_placeholderBorder != null)
            {
                _placeholderBorder.Visibility = _isPrimaryImageLoaded ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private bool CurrentImageSizeIsTooSmall(Size availableSize)
        {
            BitmapImage source = _primaryImage.Source as BitmapImage;

            if (source == null)
            {
                return true;
            }

            bool toReturn = ((source.PixelWidth < availableSize.Width) && (source.PixelHeight < availableSize.Height));
            return toReturn;
        }

        private async void BeginLoadingThumbnail()
        {
            if (Source is IPhotoSource == false)
            {
                return;
            }

            if (_thumbnailBitmapImage != null)
            {
                _thumbnailBitmapImage.ImageOpened -= OnThumbnailOpened;
            }

            _thumbnailBitmapImage = null;
            _thumbnailBitmapImage = new BitmapImage();
            _thumbnailBitmapImage.ImageOpened += OnThumbnailOpened;
            _thumbnailBitmapImage.CreateOptions = BitmapCreateOptions.None;
            _thumbnailBitmapImage.DecodePixelWidth = 150;

            var thumbnailImageStream = await GetImage(Source.ThumbnailUrl);

            if (_thumbnailBitmapImage != null && thumbnailImageStream != null)
            {
                _thumbnailBitmapImage.SetSource(thumbnailImageStream);
                _thumbnailImageSource = _thumbnailBitmapImage;
                _imageState = ImageStates.ScreenSizeThumbnail;
            }
            else
            {
                BeginLoadingFullResolutionImage();
            }
        }

        private async void BeginLoadingFullResolutionImage()
        {
            if (Source is IPhotoSource == false)
            {
                return;
            }

            if (_fullResolutionBitmapImage != null)
            {
                _fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
            }

            _fullResolutionBitmapImage = null;
            _fullResolutionBitmapImage = new BitmapImage();
            _fullResolutionBitmapImage.DecodePixelType = DecodePixelType.Logical;
            _fullResolutionBitmapImage.DecodePixelWidth = 350;
            _fullResolutionBitmapImage.CreateOptions = BitmapCreateOptions.None;
            _fullResolutionBitmapImage.ImageOpened += OnFullSizeImageOpened;

            var fullResolutionStream = await GetImage(Source.FullSizedUrl);

            if (_fullResolutionBitmapImage != null && fullResolutionStream != null)
            {
                _fullResolutionBitmapImage.SetSource(fullResolutionStream);
                _fullResolutionImageSource = _fullResolutionBitmapImage;
                _imageState = ImageStates.FullSizePhoto;
            }
        }

        private void ClearImageSources()
        {
            if (_thumbnailBitmapImage != null)
            {
                _thumbnailBitmapImage.ImageOpened -= OnThumbnailOpened;
            }
            _thumbnailBitmapImage = null;

            if (_fullResolutionBitmapImage != null)
            {
                _fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
                _fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
            }
            _fullResolutionBitmapImage = null;

            _thumbnailImageSource = null;
            _fullResolutionImageSource = null;
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        public async Task<IRandomAccessStream> GetImage(string url)
#else
        public async Task<Stream> GetImage(string url)
#endif
        {
            AdvancedHttpClient httpClient = new AdvancedHttpClient();
            ResourceGetModel requestModel = new ResourceGetModel()
            {
                Domain = url,
                Header = new Dictionary<string, string>(),
                Tag = "GetImage",
                Timeout = 30
            };
            
            byte[] image = await httpClient.GetBytes(requestModel);

            if (image != null && image.Length > 0)
            {
#if WINDOWS_STORE || WINDOWS_PHONE_APP
                var stream = new InMemoryRandomAccessStream();
                await stream.WriteAsync(image.AsBuffer());

                return stream;
#else
                var stream = new MemoryStream(image);
                return stream;
#endif
            }

            return null;
        }
#endregion

        #region Public
        public ImageView()
        {
            DefaultStyleKey = typeof(ImageView);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
        public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

            if (_primaryImage != null)
            {
                _primaryImage.ImageOpened -= OnPrimaryImageOpened;
                _primaryImage.ImageFailed -= OnPrimaryImageFailed;
            }

            // Get template parts
            _primaryImage = GetTemplateChild(PrimaryImage) as Image;
            _placeholderBorder = GetTemplateChild(PlaceholderBorder) as Border;

            // Reset whether the front image has loaded
            _isPrimaryImageLoaded = false;

            // Hook up to new elements
            if (_primaryImage != null)
            {
                _primaryImage.ImageOpened += OnPrimaryImageOpened;
                _primaryImage.ImageFailed += OnPrimaryImageFailed;
            }

            if (Source != null)
            {
                OnSourcePropertyChanged();
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (DataContext is IPhotoSource)
            {
                if ((_imageState == ImageStates.ScreenSizeThumbnail) &&
                    (_primaryImage.Visibility == Visibility.Visible) &&      // make sure the image is loaded before measuring its size
                    (CurrentImageSizeIsTooSmall(availableSize)))
                {
                    BeginLoadingFullResolutionImage();
                }
            }

            return base.MeasureOverride(availableSize);
        }
        #endregion
    }
}
