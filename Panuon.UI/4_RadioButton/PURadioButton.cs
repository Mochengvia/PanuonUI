using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Panuon.UI
{
    public class PURadioButton : RadioButton
    {
        static PURadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PURadioButton), new FrameworkPropertyMetadata(typeof(PURadioButton)));
        }

        #region Property
        /// <summary>
        /// 选择框样式，默认值为General。
        /// </summary>
        public RadioButtonStyles RadioButtonStyle
        {
            get { return (RadioButtonStyles)GetValue(RadioButtonStyleProperty); }
            set { SetValue(RadioButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty RadioButtonStyleProperty = DependencyProperty.Register("RadioButtonStyle", typeof(RadioButtonStyles), typeof(PURadioButton), new PropertyMetadata(RadioButtonStyles.General));

        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PURadioButton), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 内部选择框的宽度，默认值为20。
        /// <para>Switch样式下默认值为30。</para>
        /// </summary>
        public double InnerWidth
        {
            get { return (double)GetValue(InnerWidthProperty); }
            set
            { SetValue(InnerWidthProperty, value); }
        }
        public static readonly DependencyProperty InnerWidthProperty = DependencyProperty.Register("InnerWidth", typeof(double), typeof(PURadioButton), new PropertyMetadata((double)20));

        /// <summary>
        /// 内部选择框的高度，默认值为20。
        /// </summary>
        public double InnerHeight
        {
            get { return (double)GetValue(InnerHeightProperty); }
            set { SetValue(InnerHeightProperty, value); }
        }
        public static readonly DependencyProperty InnerHeightProperty = DependencyProperty.Register("InnerHeight", typeof(double), typeof(PURadioButton), new PropertyMetadata((double)20));

        /// <summary>
        /// Check时对号的背景颜色（或前景色），默认值为白色。
        /// <para>仅当按钮样式为General时生效。</para>
        /// </summary>
        public SolidColorBrush CoverBrush
        {
            get { return (SolidColorBrush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(SolidColorBrush), typeof(PURadioButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        public enum RadioButtonStyles
        {
            /// <summary>
            /// 一个标准的RadioButton。
            /// </summary>
            General = 1,
            /// <summary>
            /// 一个开关样式的RadioButton。
            /// </summary>
            Switch = 2,
            /// <summary>
            /// 一个带有左边线的RadioButton。
            /// </summary>
            Branch = 3,
        }

    }
}
