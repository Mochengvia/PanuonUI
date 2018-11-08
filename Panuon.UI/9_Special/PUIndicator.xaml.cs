using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Panuon.UI
{
    /// <summary>
    /// PUIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class PUIndicator : UserControl
    {
        public PUIndicator()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Colors.Transparent);
            MouseEnter += delegate
            {
                var anima = new DoubleAnimation()
                {
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.2),
                };
                this.BeginAnimation(OpacityProperty, anima);
            };
            MouseLeave += delegate
            {
                var anima = new DoubleAnimation()
                {
                    To = 0.8,
                    Duration = TimeSpan.FromSeconds(0.2),
                };
                this.BeginAnimation(OpacityProperty, anima);
            };
        }

        #region RoutedEvent
        /// <summary>
        /// 索引变更事件。
        /// </summary>
        public static readonly RoutedEvent IndexChangedEvent = EventManager.RegisterRoutedEvent("IndexChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(PUIndicator));
        public event RoutedPropertyChangedEventHandler<int> IndexChanged
        {
            add { AddHandler(IndexChangedEvent, value); }
            remove { RemoveHandler(IndexChangedEvent, value); }
        }
        internal void OnIndexChanged(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, IndexChangedEvent);
            RaiseEvent(arg);
        }

        #endregion

        #region Property

        /// <summary>
        /// 排列方式，默认值为Horizontal（横向）。
        /// <para>若排列方式为横向，需要设置控件的Height属性来计算球体大小；若排列方式为纵向，则需要设置控件的Width属性来计算球体大小。</para>
        /// </summary>
        public Orientation Dircetion
        {
            get { return (Orientation)GetValue(DircetionProperty); }
            set { SetValue(DircetionProperty, value); }
        }

        public static readonly DependencyProperty DircetionProperty =
            DependencyProperty.Register("Dircetion", typeof(Orientation), typeof(PUIndicator), new PropertyMetadata(Orientation.Horizontal, OnDircetionDircetionChanged));

        private static void OnDircetionDircetionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var indicator = d as PUIndicator;
            indicator.StkMain.Orientation = indicator.Dircetion;
        }

        /// <summary>
        /// 指示小球的颜色，默认值为灰色。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUIndicator), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"))));

        /// <summary>
        /// 索引总数。 默认值为1，最小值为1。
        /// </summary>
        public int TotalIndex
        {
            get { return (int)GetValue(TotalIndexProperty); }
            set { SetValue(TotalIndexProperty, value); }
        }

        public static readonly DependencyProperty TotalIndexProperty =
            DependencyProperty.Register("TotalIndex", typeof(int), typeof(PUIndicator), new PropertyMetadata(1));


        /// <summary>
        /// 当前索引。默认值为1，最小值为1。
        /// </summary>
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(PUIndicator), new PropertyMetadata(1, OnIndexChanged));

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var indicator = d as PUIndicator;
            indicator.OnIndexChanged((int?)e.OldValue ?? 0, (int?)e.NewValue ?? 0);
            if (!indicator.IsLoaded)
                return;
            indicator.ChangeIndex(true);
        }

        /// <summary>
        /// 左右滑动动画的持续时间（单位：毫秒），若为0，则滑动时不使用动画。默认值为500毫秒。
        /// </summary>
        public int AnimationDuration
        {
            get { return (int)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(int), typeof(PUIndicator), new PropertyMetadata(0));


        #endregion

        #region Private APIs
        private void Draw()
        {
            if (TotalIndex < 1)
                TotalIndex = 1;

            var radius = 0.0;
            if (Dircetion == Orientation.Horizontal)
                radius = this.ActualHeight;
            else
                radius = this.ActualWidth;

            StkMain.Children.Clear();

            EllIndicator.Height = (int)(radius * 0.6);
            EllIndicator.Width = (int)(radius * 0.6);
            EllIndicator.Background = CoverBrush;
            EllIndicator.CornerRadius = new CornerRadius(EllIndicator.Height / 2);
            if (Dircetion == Orientation.Horizontal)
                EllIndicator.Margin = new Thickness((radius - ((int)(radius * 0.6))) / 2, EllIndicator.Height / 2, 0, 0);
            else
                EllIndicator.Margin = new Thickness(EllIndicator.Height / 2, (radius - ((int)(radius * 0.6))) / 2, 0, 0);

            EllIndicator.Opacity = 0.8;
            EllIndicator.MouseEnter += delegate
            {
                var anima = new DoubleAnimation()
                {
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.2),
                };
                EllIndicator.BeginAnimation(OpacityProperty, anima);
            };
            EllIndicator.MouseLeave += delegate
            {
                var anima = new DoubleAnimation()
                {
                    To = 0.8,
                    Duration = TimeSpan.FromSeconds(0.2),
                };
                EllIndicator.BeginAnimation(OpacityProperty, anima);
            };

            for (int i = 1; i <= TotalIndex; i++)
            {
                var ellipse = new Border()
                {
                    Margin = Dircetion == Orientation.Horizontal ? new Thickness(0, 0, (int)(radius * 0.3), 0) : new Thickness(0, 0, 0, (int)(radius * 0.3)),
                    Height = (int)radius,
                    Width = (int)radius,
                    CornerRadius = new CornerRadius(radius / 2),
                    BorderBrush = BorderBrush,
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderThickness = new Thickness(1),
                    Opacity = 0.6,
                    Tag = i,
                    Cursor = Cursors.Hand,
                };
                ellipse.MouseEnter += delegate
                {
                    var anima = new DoubleAnimation()
                    {
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.2),
                    };
                    ellipse.BeginAnimation(OpacityProperty, anima);
                };
                ellipse.MouseLeave += delegate
                {
                    var anima = new DoubleAnimation()
                    {
                        To = 0.6,
                        Duration = TimeSpan.FromSeconds(0.2),
                    };
                    ellipse.BeginAnimation(OpacityProperty, anima);
                };
                ellipse.MouseLeftButtonDown += delegate
                {
                    Index = (int)ellipse.Tag;
                };
                StkMain.Children.Add(ellipse);
            }
        }

        private void ChangeIndex(bool usingAnima)
        {
            var radius = 0.0;
            if (Dircetion == Orientation.Horizontal)
                radius = this.ActualHeight;
            else
                radius = this.ActualWidth;

            var left = (Index - 1) * ((int)(radius * 1.3)) + (radius - ((int)(radius * 0.6))) / 2;
            if (!usingAnima || AnimationDuration == 0)
            {
                if(Dircetion == Orientation.Horizontal)
                    EllIndicator.Margin = new Thickness(left, radius * 0.2, 0, 0);
                else
                    EllIndicator.Margin = new Thickness(radius * 0.2, left, 0, 0);
            }
            else
            {
                if (Dircetion == Orientation.Horizontal)
                {
                    var anima = new ThicknessAnimation()
                    {
                        To = new Thickness(left, radius * 0.2, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                        EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut },
                    };
                    EllIndicator.BeginAnimation(MarginProperty, anima);
                }
                else
                {
                    var anima = new ThicknessAnimation()
                    {
                        To = new Thickness(radius * 0.2, left, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                        EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut },
                    };
                    EllIndicator.BeginAnimation(MarginProperty, anima);
                }
            }
        }
        #endregion

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
            ChangeIndex(false);
        }
    }
}
