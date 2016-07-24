using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPControls.Input
{
    [TemplatePart(Name = "HintContentElement", Type = typeof(TextBlock))]
    [TemplatePart(Name = "MainBorder", Type = typeof(Border))]
    [TemplatePart(Name = "RightButtonPresenter", Type = typeof(ContentPresenter))]
    public class AdvancedTextBox : TextBox
    {
        #region Dependencies
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(AdvancedTextBox), new PropertyMetadata(""));

        public SolidColorBrush TextHintForeground
        {
            get { return (SolidColorBrush)GetValue(TextHintForegroundProperty); }
            set { SetValue(TextHintForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextHintForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextHintForegroundProperty =
            DependencyProperty.Register("TextHintForeground", typeof(SolidColorBrush), typeof(AdvancedTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(AdvancedTextBox), new PropertyMetadata(new CornerRadius(0)));

        public DataTemplate RightButtonTemplate
        {
            get { return (DataTemplate)GetValue(RightButtonTemplateProperty); }
            set { SetValue(RightButtonTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RightButtonTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightButtonTemplateProperty =
            DependencyProperty.Register("RightButtonTemplate", typeof(DataTemplate), typeof(AdvancedTextBox), new PropertyMetadata(null));

        public DataTemplate LeftButtonTemplate
        {
            get { return (DataTemplate)GetValue(LeftButtonTemplateProperty); }
            set { SetValue(LeftButtonTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftButtonTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftButtonTemplateProperty =
            DependencyProperty.Register("LeftButtonTemplate", typeof(DataTemplate), typeof(AdvancedTextBox), new PropertyMetadata(null));

        public Visibility VisibleRightButton
        {
            get { return (Visibility)GetValue(VisibleRightButtonProperty); }
            set { SetValue(VisibleRightButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleRightButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleRightButtonProperty =
            DependencyProperty.Register("VisibleRightButton", typeof(Visibility), typeof(AdvancedTextBox), new PropertyMetadata(Visibility.Visible));

        public Visibility VisibleLeftButton
        {
            get { return (Visibility)GetValue(VisibleLeftButtonProperty); }
            set { SetValue(VisibleLeftButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleLeftButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleLeftButtonProperty =
            DependencyProperty.Register("VisibleLeftButton", typeof(Visibility), typeof(AdvancedTextBox), new PropertyMetadata(Visibility.Visible));


        #endregion

        #region Fields
        private TextBlock _hintTextblock;
        private ContentControl _contentElement;
        private Border _mainBorder;
        private Image _rightImage;
        private Border _rightImageContainer;
        #endregion

        #region Methods
        public AdvancedTextBox()
        {
            DefaultStyleKey = typeof(AdvancedTextBox);

            this.GotFocus += AdvancedTextBox_GotFocus;
            this.LostFocus += AdvancedTextBox_LostFocus;
        }

        private void AdvancedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_hintTextblock != null && !string.IsNullOrEmpty(_hintTextblock.Text) && string.IsNullOrEmpty(Text))
            {
                _hintTextblock.Visibility = Visibility.Visible;
            }
        }

        private void AdvancedTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_hintTextblock != null)
            {
                _hintTextblock.Visibility = Visibility.Collapsed;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Get child content
            _hintTextblock = GetTemplateChild("HintContentElement") as TextBlock;
            _contentElement = GetTemplateChild("ContentElement") as ContentControl;
            _mainBorder = GetTemplateChild("MainBorder") as Border;
            _rightImage = GetTemplateChild("RightImage") as Image;
            _rightImageContainer = GetTemplateChild("RightImageContainer") as Border;
            
            //Show Hint
            if (_hintTextblock != null && !string.IsNullOrEmpty(_hintTextblock.Text) && string.IsNullOrEmpty(Text))
            {
                _hintTextblock.Visibility = Visibility.Visible;
            }
            else
            {
                _hintTextblock.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}
