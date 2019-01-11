using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PUTextBox : TextBox
    {
        static PUTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUTextBox), new FrameworkPropertyMetadata(typeof(PUTextBox)));
        }

        #region Sys
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AddHandler(PUButton.ClickEvent, new RoutedEventHandler(OnClearButtonClick));
            ScrollViewer scrollViewer = new ScrollViewer();
            if (TextBoxStyle == TextBoxStyles.General)
            {
                    scrollViewer = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 1), 0), 0) as ScrollViewer;
            }
            else if (TextBoxStyle == TextBoxStyles.IconGroup)
            {
                scrollViewer = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this, 0), 1), 0), 1), 0) as ScrollViewer;
            }

            if(scrollViewer != null)
                scrollViewer.MouseWheel += ScrollViewer_MouseWheel;

            PreviewKeyDown += PUTextBox_PreviewKeyDown;
        }

        private void PUTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (TextType == TextTypes.Text)
                return;
            else
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.D0:
                    case System.Windows.Input.Key.D1:
                    case System.Windows.Input.Key.D2:
                    case System.Windows.Input.Key.D3:
                    case System.Windows.Input.Key.D4:
                    case System.Windows.Input.Key.D5:
                    case System.Windows.Input.Key.D6:
                    case System.Windows.Input.Key.D7:
                    case System.Windows.Input.Key.D8:
                    case System.Windows.Input.Key.D9:
                    case System.Windows.Input.Key.NumPad0:
                    case System.Windows.Input.Key.NumPad1:
                    case System.Windows.Input.Key.NumPad2:
                    case System.Windows.Input.Key.NumPad3:
                    case System.Windows.Input.Key.NumPad4:
                    case System.Windows.Input.Key.NumPad5:
                    case System.Windows.Input.Key.NumPad6:
                    case System.Windows.Input.Key.NumPad7:
                    case System.Windows.Input.Key.NumPad8:
                    case System.Windows.Input.Key.NumPad9:
                    case System.Windows.Input.Key.Enter:
                    case System.Windows.Input.Key.Back:
                    case System.Windows.Input.Key.Delete:
                    case System.Windows.Input.Key.Left:
                    case System.Windows.Input.Key.Right:
                    case System.Windows.Input.Key.Up:
                    case System.Windows.Input.Key.Down:
                    case System.Windows.Input.Key.Home:
                    case System.Windows.Input.Key.End:
                    case System.Windows.Input.Key.Tab:
                        break;
                    case System.Windows.Input.Key.OemPeriod:
                        if (TextType != TextTypes.Decimal)
                            e.Handled = true;
                        break;
                    default:
                        e.Handled = true;
                        break;
                }
            }
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            var btnClear = e.OriginalSource as PUButton;
            if (btnClear.Tag == null || btnClear.Tag.ToString() != "Clear")
                return;
            Text = "";
        }

        private void ScrollViewer_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (e.Delta > 0)
                if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                    scrollViewer.LineUp();
                else if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                    scrollViewer.LineLeft();
                else
                    return;
            else
                 if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                scrollViewer.LineDown();
            else if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                scrollViewer.LineRight();
            else
                return;

            if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible || scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                e.Handled = true;
        }
        #endregion

        #region Property
        /// <summary>
        /// 获取或设置文本框的基本样式。默认值为General。
        /// </summary>
        public TextBoxStyles TextBoxStyle
        {
            get { return (TextBoxStyles)GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }
        public static readonly DependencyProperty TextBoxStyleProperty = 
            DependencyProperty.Register("TextBoxStyle", typeof(TextBoxStyles), typeof(PUTextBox), new PropertyMetadata(TextBoxStyles.General));

        /// <summary>
        /// 获取或设置文本框的圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = 
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUTextBox));

        /// <summary>
        ///  获取或设置输入框获得焦点时阴影的颜色，默认值为#66888888。
        /// </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = 
            DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PUTextBox));

        /// <summary>
        ///  获取或设置水印内容。默认值为空。
        /// </summary>
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        public static readonly DependencyProperty WatermarkProperty = 
            DependencyProperty.Register("Watermark", typeof(string), typeof(PUTextBox));

        /// <summary>
        /// 获取或设置i放置在输入框前的图标。
        /// <para>仅当输入框样式为IconGroup时有效。</para>
        /// </summary>
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = 
            DependencyProperty.Register("Icon", typeof(object), typeof(PUTextBox));

        /// <summary>
        /// 获取或设置图标的宽度，默认值为30。
        /// <para>仅当输入框样式为IconGroup时有效。</para>
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        public static readonly DependencyProperty IconWidthProperty = 
            DependencyProperty.Register("IconWidth", typeof(double), typeof(PUTextBox), new PropertyMetadata((double)30));


        /// <summary>
        /// 获取或设置当鼠标悬浮时是否显示清除按钮。默认值为False。
        /// </summary>
        public bool IsClearButtonShow
        {
            get { return (bool)GetValue(IsClearButtonShowProperty); }
            set { SetValue(IsClearButtonShowProperty, value); }
        }

        public static readonly DependencyProperty IsClearButtonShowProperty =
            DependencyProperty.Register("IsClearButtonShow", typeof(bool), typeof(PUTextBox));


        /// <summary>
        /// 获取或设置允许键入的类型。默认值为Text（全部内容）。
        /// </summary>
        public TextTypes TextType
        {
            get { return (TextTypes)GetValue(TextTypeProperty); }
            set { SetValue(TextTypeProperty, value); }
        }

        public static readonly DependencyProperty TextTypeProperty =
            DependencyProperty.Register("TextType", typeof(TextTypes), typeof(PUTextBox), new PropertyMetadata(TextTypes.Text, OnTextTypeChanged));

        private static void OnTextTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as PUTextBox;
            if(textBox.TextType == TextTypes.Text)
            {
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(textBox, true);
            }
            else
            {
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(textBox, false);
            }
        }

        #endregion

    }
}
