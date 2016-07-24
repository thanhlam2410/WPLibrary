using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WPControls.Media
{
    public interface IPhotoSource
    {
        string FullSizedUrl { get; set; }
        string ThumbnailUrl { get; set; }
        DateTime DateTaken { get; }
    }
}
