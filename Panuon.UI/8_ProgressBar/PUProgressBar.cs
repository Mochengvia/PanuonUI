using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Panuon.UI
{
    public class PUProgressBar : Control
    {
        static PUProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PUProgressBar), new FrameworkPropertyMetadata(typeof(PUProgressBar)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += delegate
            {
                Change();
            };
        }

        #region Property
        /// <summary>
        /// 获取或设置进度条的样式。默认值为General。
        /// </summary>
        public ProgressBarStyles ProgressBarStyle
        {
            get { return (ProgressBarStyles)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        public static readonly DependencyProperty ProgressBarStyleProperty =
            DependencyProperty.Register("ProgressBarStyle", typeof(ProgressBarStyles), typeof(PUProgressBar), new PropertyMetadata(ProgressBarStyles.General));


        /// <summary>
        /// 获取或设置进度条的填充动画持续时间。默认值为0.4。
        /// </summary>
        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(PUProgressBar), new PropertyMetadata(TimeSpan.FromSeconds(0.6)));



        /// <summary>
        /// 获取或设置进度条的填充颜色。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUProgressBar));


        /// <summary>
        /// 获取或设置圆角大小，默认值为0。
        /// </summary>
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PUProgressBar));

        /// <summary>
        /// 获取或设置进度条填充方向，默认值为LeftToRight。
        /// </summary>
        public Directions Direction
        {
            get { return (Directions)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Directions), typeof(PUProgressBar), new PropertyMetadata(Directions.LeftToRight));

        /// <summary>
        /// 获取或设置是否显示百分比。默认值为False。
        /// </summary>
        public bool IsPercentShow
        {
            get { return (bool)GetValue(IsPercentShowProperty); }
            set { SetValue(IsPercentShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPercentShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPercentShowProperty =
            DependencyProperty.Register("IsPercentShow", typeof(bool), typeof(PUProgressBar));

        /// <summary>
        /// 获取或设置当前进度条的百分比，从0~1的值。默认值为0。
        /// </summary>
        public double Percent
        {
            get { return (double)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        public static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register("Percent", typeof(double), typeof(PUProgressBar), new PropertyMetadata(OnPercentChanged));

        private static void OnPercentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pgBar = d as PUProgressBar;
            if (pgBar.Percent < 0)
            {
                pgBar.Percent = 0;
                return;
            }
            else if (pgBar.Percent > 1)
            {
                pgBar.Percent = 1;
                return;
            }
            pgBar.PercentString = pgBar.Percent * 100 + "%";

            if (!pgBar.IsLoaded)
                return;

            pgBar.Change();
        }



        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public new static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(PUProgressBar), new PropertyMetadata(OnBorderThicknessChanged));

        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as PUProgressBar;
            bar.StrokeThickness = bar.BorderThickness.Left;
        }


        #endregion

        #region Internal Property


        internal string PercentString
        {
            get { return (string)GetValue(PercentStringProperty); }
            set { SetValue(PercentStringProperty, value); }
        }

        internal static readonly DependencyProperty PercentStringProperty =
            DependencyProperty.Register("PercentString", typeof(string), typeof(PUProgressBar));


        internal double InnerWidth
        {
            get { return (double)GetValue(InnerWidthProperty); }
            set { SetValue(InnerWidthProperty, value); }
        }

        internal static readonly DependencyProperty InnerWidthProperty =
            DependencyProperty.Register("InnerWidth", typeof(double), typeof(PUProgressBar));

        internal double InnerPercent
        {
            get { return (double)GetValue(InnerPercentProperty); }
            set { SetValue(InnerPercentProperty, value); }
        }

        internal static readonly DependencyProperty InnerPercentProperty =
            DependencyProperty.Register("InnerPercent", typeof(double), typeof(PUProgressBar));


        internal double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        internal static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(PUProgressBar));


        #endregion

        #region Function
        private void Change()
        {
            if (ProgressBarStyle == ProgressBarStyles.General)
            {
                var toValue = ActualWidth * Percent;
                if (Direction == Directions.TopToBottom || Direction == Directions.BottomToTop)
                {
                    toValue = ActualHeight * Percent;
                }
                var anima = new DoubleAnimation()
                {
                    To = toValue,
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    Duration = AnimationDuration,
                };
                BeginAnimation(InnerWidthProperty, anima);
            }
            else
            {
                var anima = new DoubleAnimation()
                {
                    To = Percent,
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    Duration = AnimationDuration,
                };
                BeginAnimation(InnerPercentProperty, anima);
            }

        }
        #endregion
        public enum Directions
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop,
        }

        public enum ProgressBarStyles
        {
            /// <summary>
            /// 一个标准的进度条。
            /// </summary>
            General,
            /// <summary>
            /// 一个环形的进度条。
            /// </summary>
            Ring
        }
    }
}
