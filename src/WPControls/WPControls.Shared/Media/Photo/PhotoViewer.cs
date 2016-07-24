using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
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

namespace WPControls.Media.Photo
{
    public class PhotoViewer : Control
    {
        #region Methods
        public PhotoViewer()
        {
            DefaultStyleKey = typeof(PhotoViewer);
            SizeChanged += OnMediaViewerSizeChanged;
        }

        private void OnMediaViewerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
        #endregion
    }
}
