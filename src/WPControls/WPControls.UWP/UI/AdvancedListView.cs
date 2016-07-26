using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace WPControls.UWP.UI
{
    public sealed class AdvancedListView : ListView
    {
        #region Fields
        private ScrollViewer scrollViewer;
        private Grid refreshOverlay;
        private Grid container;
        private DispatcherTimer refreshDelayTimer;

        private ISupportIncrementalLoading itemSource;
        #endregion

        #region Properties


        public SolidColorBrush ArrowColor
        {
            get { return (SolidColorBrush)GetValue(ArrowColorProperty); }
            set { SetValue(ArrowColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowColor.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty ArrowColorProperty =
            DependencyProperty.Register("ArrowColor", typeof(SolidColorBrush), typeof(AdvancedListView), new PropertyMetadata(new SolidColorBrush(Colors.White)));



        public string PullText
        {
            get { return (string)GetValue(PullTextProperty); }
            set { SetValue(PullTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PullText.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty PullTextProperty =
            DependencyProperty.Register("PullText", typeof(string), typeof(AdvancedListView), new PropertyMetadata(""));



        public string RefreshText
        {
            get { return (string)GetValue(RefreshTextProperty); }
            set { SetValue(RefreshTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshText.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty RefreshTextProperty =
            DependencyProperty.Register("RefreshText", typeof(string), typeof(AdvancedListView), new PropertyMetadata(""));



        public int RefreshPanelHeight
        {
            get { return (int)GetValue(RefreshPanelHeightProperty); }
            set { SetValue(RefreshPanelHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RefreshPanelHeight.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty RefreshPanelHeightProperty =
            DependencyProperty.Register("RefreshPanelHeight", typeof(int), typeof(AdvancedListView), new PropertyMetadata(30));



        public ICommand PullToRefresh
        {
            get { return (ICommand)GetValue(PullToRefreshProperty); }
            set { SetValue(PullToRefreshProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PullToRefresh.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty PullToRefreshProperty =
            DependencyProperty.Register("PullToRefresh", typeof(ICommand), typeof(AdvancedListView), new PropertyMetadata(null));



        public bool IsPullToRefreshEnabled
        {
            get { return (bool)GetValue(IsPullToRefreshEnabledProperty); }
            set { SetValue(IsPullToRefreshEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPullToRefreshEnabled.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty IsPullToRefreshEnabledProperty =
            DependencyProperty.Register("IsPullToRefreshEnabled", typeof(bool), typeof(AdvancedListView), new PropertyMetadata(false, new PropertyChangedCallback(OnPullToRefreshEnabledChangedCallback)));



        public bool IsPullToRefreshUpdate
        {
            get { return (bool)GetValue(IsPullToRefreshUpdateProperty); }
            set { SetValue(IsPullToRefreshUpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPullToRefreshUpdate.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty IsPullToRefreshUpdateProperty =
            DependencyProperty.Register("IsPullToRefreshUpdate", typeof(bool), typeof(AdvancedListView), new PropertyMetadata(false));



        public bool AllowScrollingToEnd
        {
            get { return (bool)GetValue(AllowScrollingToEndProperty); }
            set { SetValue(AllowScrollingToEndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllowScrollingToEnd.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty AllowScrollingToEndProperty =
            DependencyProperty.Register("AllowScrollingToEnd", typeof(bool), typeof(AdvancedListView), new PropertyMetadata(false));



        public bool IsRefreshing
        {
            get { return (bool)GetValue(IsRefreshingProperty); }
            set { SetValue(IsRefreshingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRefreshing.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty IsRefreshingProperty =
            DependencyProperty.Register("IsRefreshing", typeof(bool), typeof(AdvancedListView), new PropertyMetadata(false));



        public Visibility VisiblePullToUpdatePanel
        {
            get { return (Visibility)GetValue(VisiblePullToUpdatePanelProperty); }
            set { SetValue(VisiblePullToUpdatePanelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisiblePullToUpdatePanel.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty VisiblePullToUpdatePanelProperty =
            DependencyProperty.Register("VisiblePullToUpdatePanel", typeof(Visibility), typeof(AdvancedListView), new PropertyMetadata(Visibility.Collapsed));


        #endregion

        public AdvancedListView()
        {
            this.DefaultStyleKey = typeof(AdvancedListView);
            this.Loaded += AdvancedListView_Loaded;
        }

        private void AdvancedListView_Loaded(object sender, RoutedEventArgs e)
        {
            refreshDelayTimer = new DispatcherTimer();
            refreshDelayTimer.Interval = TimeSpan.FromMilliseconds(500);
            refreshDelayTimer.Tick += RefreshDelayTimer_Tick;

            if (this.ItemsSource is ISupportIncrementalLoading)
            {
                itemSource = this.ItemsSource as ISupportIncrementalLoading;
            }

            if (this.Items.Count > 0 && AllowScrollingToEnd)
            {
                ScrollToVerticalEnd(true);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            scrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");
            refreshOverlay = (Grid)GetTemplateChild("RefreshOverLay");
            container = (Grid)GetTemplateChild("Container");

            if (refreshOverlay != null)
            {
                refreshOverlay.Margin = new Thickness(0, -RefreshPanelHeight, 0, 0);
            }

            scrollViewer.DirectManipulationStarted += ScrollViewer_DirectManipulationStarted;
            scrollViewer.DirectManipulationCompleted += ScrollViewer_DirectManipulationCompleted;
        }

        private void ScrollViewer_DirectManipulationStarted(object sender, object e)
        {
            //Debug.WriteLine("Manipulation Started --- Vertical Offset: " + scrollViewer.VerticalOffset);
            IsPullToRefreshUpdate = false;
            refreshDelayTimer.Start();
        }

        private void ScrollViewer_DirectManipulationCompleted(object sender, object e)
        {
            refreshDelayTimer.Stop();

            //Debug.WriteLine("Manipulation Completed --- Vertical Offset: " + scrollViewer.VerticalOffset);
            VisualStateManager.GoToState(this, "Normal", true);

            if (IsPullToRefreshEnabled && scrollViewer.VerticalOffset < 5)
            {
                if (PullToRefresh != null && PullToRefresh.CanExecute(null))
                {
                    PullToRefresh.Execute(null);
                }

                InvokeUpdate();
            }
        }

        private void RefreshDelayTimer_Tick(object sender, object e)
        {
            refreshDelayTimer.Stop();
            VisualStateManager.GoToState(this, "ReadyToRefresh", true);
        }

        private async void InvokeUpdate()
        {
            if (itemSource != null && itemSource.HasMoreItems && !IsRefreshing)
            {
                IsPullToRefreshUpdate = true;
                IsRefreshing = true;

                LoadMoreItemsResult result = await itemSource.LoadMoreItemsAsync(0);

                if (result.Count >= 0)
                {
                    IsRefreshing = false;
                }
            }
        }

        private static void OnPullToRefreshEnabledChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool isEnablePullToRefresh = (bool)e.NewValue;

            if (isEnablePullToRefresh)
            {
                d.SetValue(AdvancedListView.VisiblePullToUpdatePanelProperty, Visibility.Visible);
            }
            else
            {
                d.SetValue(AdvancedListView.VisiblePullToUpdatePanelProperty, Visibility.Collapsed);
            }
        }

        public void ScrollToVerticalEnd(bool forceScrolling)
        {
            if (this.Items.Count > 0)
            {
                if (scrollViewer != null)
                {
                    double verticalPercentage = scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;

                    if (verticalPercentage > 0.8 || forceScrolling)
                    {
                        scrollViewer.ChangeView(null, scrollViewer.ExtentHeight, null);
                    }
                }
            }
        }
    }
}
