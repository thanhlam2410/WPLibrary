﻿#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml.Media;
#elif WINDOWS_PHONE
using System.Windows.Media;
#endif

namespace WPControls.Common
{
    public interface IImageSource
    {
        Stretch Stretch { get; set; }
        ImageSource ImageSource { get; set; }
    }

    public interface IImageSourceFull : IImageSource
    {
        double ImageWidth { get; set; }
        double ImageHeight { get; set; }
    }
}
