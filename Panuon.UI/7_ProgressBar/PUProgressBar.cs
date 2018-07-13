using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Panuon.UI
{
    public class PUProgressBar : ProgressBar
    {
        static PUProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUProgressBar), new FrameworkPropertyMetadata(typeof(PUProgressBar)));
        }

        #region Property
        /// <summary>
        /// 当前的值（相对于Maximuim），默认值为0。
        /// </summary>
        public new double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public new static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(PUProgressBar), new PropertyMetadata((double)0, ValueOnChanged));

        private static void ValueOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pgbar = d as PUProgressBar;
            if (pgbar == null)
                return;
            var anima = new DoubleAnimation()
            {
                To = ((double)e.NewValue / pgbar.Maximum) * pgbar.Width,
                Duration = pgbar.UsingAnimation ? TimeSpan.FromSeconds(0.4) : TimeSpan.FromSeconds(0),
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            pgbar.BeginAnimation(InnerWidthProperty, anima);
            if (pgbar.IsPercentDecimal)
                pgbar.Percent = $"{(((double)e.NewValue / pgbar.Maximum) * 100).ToString("f2")}%";
            else
                pgbar.Percent = $"{(((double)e.NewValue / pgbar.Maximum) * 100).ToString("f0")}%";
        }

        /// <summary>
        /// 进度改变时是否使用动画，默认值为True（使用）。
        /// </summary>
        public bool UsingAnimation
        {
            get { return (bool)GetValue(UsingAnimationProperty); }
            set { SetValue(UsingAnimationProperty, value); }
        }
        public static readonly DependencyProperty UsingAnimationProperty = DependencyProperty.Register("UsingAnimation", typeof(bool), typeof(PUProgressBar), new PropertyMetadata(true));

        /// <summary>
        /// 是否在进度条上显示百分比，默认值为Collapsed（不显示）。可以通过调整Foreground属性的值来改变颜色。
        /// </summary>
        public Visibility ShowPercent
        {
            get { return (Visibility)GetValue(ShowPercentProperty); }
            set { SetValue(ShowPercentProperty, value); }
        }
        public static readonly DependencyProperty ShowPercentProperty = DependencyProperty.Register("ShowPercent", typeof(Visibility), typeof(PUProgressBar), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 显示的百分比文字是否为小数，默认值为True（是小数）。否则为整数百分比。
        /// </summary>
        public bool IsPercentDecimal
        {
            get { return (bool)GetValue(IsPercentDecimalProperty); }
            set { SetValue(IsPercentDecimalProperty, value); }
        }
        public static readonly DependencyProperty IsPercentDecimalProperty = DependencyProperty.Register("IsPercentDecimal", typeof(bool), typeof(PUProgressBar), new PropertyMetadata(true));

        /// <summary>
        /// 是否是竖直的，默认值为False。
        /// </summary>
        public bool IsVertical
        {
            get { return (bool)GetValue(IsVerticalProperty); }
            set { SetValue(IsVerticalProperty, value); }
        }
        public static readonly DependencyProperty IsVerticalProperty = DependencyProperty.Register("IsVertical", typeof(bool), typeof(PUProgressBar), new PropertyMetadata(false));


        /// <summary>
        /// 圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUProgressBar), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 进度颜色，默认值为灰黑色。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUProgressBar), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3E3E3E"))));

        #endregion

        #region Internal Property
        /// <summary>
        /// 进度长度
        /// </summary>
        internal double InnerWidth
        {
            get { return (double)GetValue(InnerWidthProperty); }
            set { SetValue(InnerWidthProperty, value); }
        }
        internal static readonly DependencyProperty InnerWidthProperty = DependencyProperty.Register("InnerWidth", typeof(double), typeof(PUProgressBar), new PropertyMetadata((double)0));


        /// <summary>
        /// 百分比
        /// </summary>
        internal string Percent
        {
            get { return (string)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }
        internal static readonly DependencyProperty PercentProperty = DependencyProperty.Register("Percent", typeof(string), typeof(PUProgressBar), new PropertyMetadata(""));

        #endregion
    }
}
