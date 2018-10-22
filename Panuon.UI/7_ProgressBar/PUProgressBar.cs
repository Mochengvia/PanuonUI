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
        /// 获取或设置当百分比改变时是否使用渐进或渐退动画。默认值为True。
        /// </summary>
        public bool UsingAnimation
        {
            get { return (bool)GetValue(UsingAnimationProperty); }
            set { SetValue(UsingAnimationProperty, value); }
        }

        public static readonly DependencyProperty UsingAnimationProperty =
            DependencyProperty.Register("UsingAnimation", typeof(bool), typeof(PUProgressBar));


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
        /// 是否显示百分比。默认值为False。
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


        #endregion

        #region Function
        private void Change()
        {
            var to = ActualWidth * Percent;
            if (Direction == Directions.TopToBottom || Direction == Directions.BottomToTop)
            {
                to = ActualHeight * Percent;
            }

            if (UsingAnimation)
            {
                var anima = new DoubleAnimation()
                {
                    To = to,
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    Duration = TimeSpan.FromSeconds(0.2),
                };
                BeginAnimation(InnerWidthProperty, anima);
            }
            else
            {
                InnerWidth = to;
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

    }
}
