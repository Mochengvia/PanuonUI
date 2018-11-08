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
        /// 按钮样式，默认值为General。
        /// </summary>
        public TextBoxStyles TextBoxStyle
        {
            get { return (TextBoxStyles)GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }
        public static readonly DependencyProperty TextBoxStyleProperty = 
            DependencyProperty.Register("TextBoxStyle", typeof(TextBoxStyles), typeof(PUTextBox), new PropertyMetadata(TextBoxStyles.General));

        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = 
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUTextBox), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        ///  输入框获得焦点时阴影的颜色，默认值为#33888888。
        /// </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = 
            DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PUTextBox));

        /// <summary>
        ///  水印内容，默认值为空。
        /// </summary>
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        public static readonly DependencyProperty WatermarkProperty = 
            DependencyProperty.Register("Watermark", typeof(string), typeof(PUTextBox), new PropertyMetadata(""));

        /// <summary>
        /// 放置在输入框前的图标。
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
        /// 图标的宽度，默认值为30。
        /// <para>仅当输入框样式为IconGroup时有效。</para>
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        public static readonly DependencyProperty IconWidthProperty = 
            DependencyProperty.Register("IconWidth", typeof(double), typeof(PUTextBox), new PropertyMetadata((double)30));

        #endregion

        #region Enums
        public enum TextBoxStyles
        {
            /// <summary>
            /// 一个标准的输入框。
            /// </summary>
            General = 1,
            /// <summary>
            /// 一个输入框前带图标的输入框。
            /// </summary>
            IconGroup = 2,
        }
        #endregion
    }
}
