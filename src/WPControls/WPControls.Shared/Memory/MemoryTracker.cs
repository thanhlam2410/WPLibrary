using System.Collections.Generic;
using System.Text;
using Windows.System;
using System.Diagnostics;
using System;
using System.Windows;

#if WINDOWS_PHONE
using System.Windows.Threading;
using Microsoft.Phone.Info;
using System.Windows.Controls.Primitives;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace WPControls.Memory
{
    public class MemoryTracker
    {
        #region Fields
        private DispatcherTimer updateTimer;
        private MemoryWatcher memoryWatcher;
        private Popup container;
        private bool _showView;
        private long totalBytes;
        private long currentBytes;
        private long peakBytes;
        #endregion

        #region Properties
        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;

                if (memoryWatcher != null)
                {
                    memoryWatcher.SetMargin(_margin);
                }
            }
        }

        private VerticalAlignment _verticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                _verticalAlignment = value;

                if (memoryWatcher != null)
                {
                    memoryWatcher.SetVerticalAlignment(_verticalAlignment);
                }
            }
        }
        #endregion

        #region Methods
        public void Run(bool showOnView)
        {
            if (updateTimer != null)
            {
                updateTimer.Stop();
                updateTimer.Tick -= UpdateTimer_Tick;
                updateTimer = null;
            }

            _showView = showOnView;

            if (showOnView && memoryWatcher == null)
            {
                memoryWatcher = new MemoryWatcher();
                memoryWatcher.SetMargin(_margin);
                memoryWatcher.SetVerticalAlignment(_verticalAlignment);
            }

            CheckMemory();

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(2);
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, object e)
        {
            CheckMemory();
        }

        private void CheckMemory()
        {
#if WINDOWS_PHONE
            totalBytes = (long)DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit");
            currentBytes = (long)DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
            peakBytes = (long)DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage");
#else
            totalBytes = (long)MemoryManager.AppMemoryUsageLimit;
            currentBytes = (long)MemoryManager.AppMemoryUsage;

            if (peakBytes <= currentBytes)
            {
                peakBytes = currentBytes;
            }
#endif
            string finalValue = string.Format("Use: {0:F2}MB;  Peak: {1:F2}MB;  Limit: {2:F2}MB;", currentBytes / (1024 * 1024.0), peakBytes / (1024 * 1024.0), totalBytes / (1024 * 1024.0));

            if (_showView)
            {
                memoryWatcher.SetValue(finalValue);
                Show();
            }
            else
            {
                Hide();
                Debug.WriteLine(finalValue);
            }
        }

        private void Show()
        {
            if (container == null)
            {
                container = new Popup();

                container.Child = memoryWatcher;
            }

            if (container != null && !container.IsOpen)
            {
                container.IsOpen = true;
            }
        }

        private void Hide()
        {
            if (container != null && container.IsOpen)
            {
                container.IsOpen = false;
            }
        }
        #endregion
    }
}
