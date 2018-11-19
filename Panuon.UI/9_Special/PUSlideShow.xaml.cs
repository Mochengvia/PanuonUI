using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Panuon.UI
{
    /// <summary>
    /// PUSlideShow.xaml 的交互逻辑
    /// </summary>
    [ContentProperty(nameof(Content))]
    public partial class PUSlideShow : UserControl
    {
        public PUSlideShow()
        {
            InitializeComponent();
            Content = StkMain.Children;
            Indicator.IndexChanged += delegate
            {
                Index = Indicator.Index;
            };

            if (Index == 1 && !Recyclable)
                BtnLeft.IsEnabled = false;
            else
                BtnLeft.IsEnabled = true;

            if (Index == Content.Count && !Recyclable)
                BtnRight.IsEnabled = false;
            else
                BtnRight.IsEnabled = true;

            Indicator.AnimationDuration = AnimationDuration;
        }

        #region Property

        public new UIElementCollection Content
        {
            get { return (UIElementCollection)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElementCollection), typeof(PUSlideShow), new PropertyMetadata(OnContentChanged));

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            if (!slideShow.IsLoaded || slideShow.Content == null)
                return;
            slideShow.Draw();
            slideShow.Indicator.TotalIndex = slideShow.Content.Count;
        }

        /// <summary>
        /// 索引，表示当前的位置。从1开始。
        /// <para>当你试图将Index的值设置为大于Content数量上限或小于1的数字时，Index会被重设为Content的数量或1
        /// （若Recyclable为True，则会被重设为1或Content的数量）。</para>
        /// </summary>
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(PUSlideShow), new PropertyMetadata(1, OnIndexChanged));

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            if (!slideShow.IsLoaded || slideShow.Content == null)
                return;
            slideShow.Indicator.Index = slideShow.Index;
            slideShow.ChangeIndex(false);
        }

        /// <summary>
        /// 滑动方向。默认值为Horizontal（横向）。
        /// </summary>
        public Orientation SlideDirection
        {
            get { return (Orientation)GetValue(SlideDirectionProperty); }
            set { SetValue(SlideDirectionProperty, value); }
        }

        public static readonly DependencyProperty SlideDirectionProperty =
            DependencyProperty.Register("SlideDirection", typeof(Orientation), typeof(PUSlideShow), new PropertyMetadata(Orientation.Horizontal, OnSlideDirectionChanged));

        private static void OnSlideDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            slideShow.StkMain.Orientation = slideShow.SlideDirection;
            if (slideShow.SlideDirection == Orientation.Horizontal)
            {
                slideShow.BtnLeft.Content = "";
                slideShow.BtnRight.Content = "";
                slideShow.BtnLeft.VerticalAlignment = VerticalAlignment.Center;
                slideShow.BtnLeft.HorizontalAlignment = HorizontalAlignment.Left;
                slideShow.BtnRight.VerticalAlignment = VerticalAlignment.Center;
                slideShow.BtnRight.HorizontalAlignment = HorizontalAlignment.Right;
                slideShow.BtnLeft.Margin = new Thickness(20, 0, 0, 0);
                slideShow.BtnRight.Margin = new Thickness(0, 0, 20, 0);
                slideShow.Indicator.Dircetion = Orientation.Horizontal;
                slideShow.Indicator.Height = 15;
                slideShow.Indicator.Width = double.NaN;
                slideShow.Indicator.VerticalAlignment = VerticalAlignment.Bottom;
                slideShow.Indicator.HorizontalAlignment = HorizontalAlignment.Center;
                slideShow.Indicator.Margin = new Thickness(0, 0, 0, 20);
            }
            else
            {
                slideShow.BtnLeft.Content = "";
                slideShow.BtnRight.Content = "";
                slideShow.BtnLeft.VerticalAlignment = VerticalAlignment.Top;
                slideShow.BtnLeft.HorizontalAlignment = HorizontalAlignment.Center;
                slideShow.BtnRight.VerticalAlignment = VerticalAlignment.Bottom;
                slideShow.BtnRight.HorizontalAlignment = HorizontalAlignment.Center;
                slideShow.BtnLeft.Margin = new Thickness(0, 20, 0, 0);
                slideShow.BtnRight.Margin = new Thickness(0, 0, 0, 20);
                slideShow.Indicator.Dircetion = Orientation.Vertical;
                slideShow.Indicator.Height = double.NaN;
                slideShow.Indicator.Width =15 ;
                slideShow.Indicator.VerticalAlignment = VerticalAlignment.Center;
                slideShow.Indicator.HorizontalAlignment = HorizontalAlignment.Left;
                slideShow.Indicator.Margin = new Thickness(20, 0, 0, 0);
            }
        }

        /// <summary>
        /// 是否显示左右滑动按钮。
        /// </summary>
        public bool IsSlideButtonShow
        {
            get { return (bool)GetValue(IsSlideButtonShowProperty); }
            set { SetValue(IsSlideButtonShowProperty, value); }
        }

        public static readonly DependencyProperty IsSlideButtonShowProperty =
            DependencyProperty.Register("IsSlideButtonShow", typeof(bool), typeof(PUSlideShow), new PropertyMetadata(false, OnIsSlideButtonShowChanged));

        private static void OnIsSlideButtonShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            if (slideShow.IsSlideButtonShow)
            {
                slideShow.BtnLeft.Visibility = Visibility.Visible;
                slideShow.BtnRight.Visibility = Visibility.Visible;
            }
            else
            {
                slideShow.BtnLeft.Visibility = Visibility.Collapsed;
                slideShow.BtnRight.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 滑动按钮的颜色，默认为灰黑色。
        /// </summary>
        public Brush SlideButtonBrush
        {
            get { return (Brush)GetValue(SlideButtonBrushProperty); }
            set { SetValue(SlideButtonBrushProperty, value); }
        }

        public static readonly DependencyProperty SlideButtonBrushProperty =
            DependencyProperty.Register("SlideButtonBrush", typeof(Brush), typeof(PUSlideShow), new PropertyMetadata(OnSlideButtonBrushChanged));

        private static void OnSlideButtonBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            if (slideShow.SlideButtonBrush != null)
            {
                slideShow.BtnLeft.Foreground = slideShow.SlideButtonBrush;
                slideShow.BtnLeft.Background = slideShow.SlideButtonBrush;
                slideShow.BtnLeft.CoverBrush = slideShow.SlideButtonBrush;
                slideShow.BtnRight.Foreground = slideShow.SlideButtonBrush;
                slideShow.BtnRight.Background = slideShow.SlideButtonBrush;
                slideShow.BtnRight.CoverBrush = slideShow.SlideButtonBrush;
            }
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
            DependencyProperty.Register("AnimationDuration", typeof(int), typeof(PUSlideShow), new PropertyMetadata(500, OnAnimationDurationChanged));

        private static void OnAnimationDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            slideShow.Indicator.AnimationDuration = slideShow.AnimationDuration;
        }



        /// <summary>
        /// 是否允许滑动无限循环。
        /// </summary>
        public bool Recyclable
        {
            get { return (bool)GetValue(RecyclableProperty); }
            set { SetValue(RecyclableProperty, value); }
        }

        public static readonly DependencyProperty RecyclableProperty =
            DependencyProperty.Register("Recyclable", typeof(bool), typeof(PUSlideShow), new PropertyMetadata(false));


        /// <summary>
        /// 是否显示指示器。
        /// </summary>
        public bool IsIndicatorShow
        {
            get { return (bool)GetValue(IsIndicatorShowProperty); }
            set { SetValue(IsIndicatorShowProperty, value); }
        }

        public static readonly DependencyProperty IsIndicatorShowProperty =
            DependencyProperty.Register("IsIndicatorShow", typeof(bool), typeof(PUSlideShow), new PropertyMetadata(true, OnIsIndicatorShowChanged));

        private static void OnIsIndicatorShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slide = d as PUSlideShow;
            if (slide.IsIndicatorShow)
                slide.Indicator.Visibility = Visibility.Visible;
            else
                slide.Indicator.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// 指示器颜色。
        /// </summary>
        public Brush IndicatorBrush
        {
            get { return (Brush)GetValue(IndicatorBrushProperty); }
            set { SetValue(IndicatorBrushProperty, value); }
        }

        public static readonly DependencyProperty IndicatorBrushProperty =
            DependencyProperty.Register("IndicatorBrush", typeof(Brush), typeof(PUSlideShow), new PropertyMetadata(new SolidColorBrush(Colors.DimGray), OnIndicatorBrushChanged));

        private static void OnIndicatorBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            slideShow.Indicator.BorderBrush = slideShow.IndicatorBrush;
            slideShow.Indicator.CoverBrush = slideShow.IndicatorBrush;
        }
        #endregion


        #region APIs
        private void Draw()
        {
            Indicator.TotalIndex = Content.Count;
            foreach (var item in Content)
            {
                var grid = item as FrameworkElement;
                grid.Width = ActualWidth;
                grid.Height = ActualHeight;
            }
        }

        private void ChangeIndex(bool isFirstSet)
        {
            if (Index < 1)
            {
                if (!Recyclable)
                {
                    Index = 1;
                    return;
                }
                else
                {
                    Index = Content.Count;
                    return;
                }
            }
            else if (Index > Content.Count)
            {
                if (!Recyclable)
                {
                    Index = Content.Count;
                    return;
                }
                else
                {
                    Index = 1;
                    return;
                }
            }

            if (Index == 1 && !Recyclable)
                BtnLeft.IsEnabled = false;
            else
                BtnLeft.IsEnabled = true;

            if (Index == Content.Count && !Recyclable)
                BtnRight.IsEnabled = false;
            else
                BtnRight.IsEnabled = true;


            if (isFirstSet || AnimationDuration == 0)
            {
                if (SlideDirection == Orientation.Horizontal)
                    StkMain.Margin = new Thickness(-1 * (Index - 1) * ActualWidth, 0, 0, 0);
                else
                    StkMain.Margin = new Thickness(0, -1 * (Index - 1) * ActualHeight, 0, 0);
            }
            else
            {
                if (SlideDirection == Orientation.Horizontal)
                {
                    var anima = new ThicknessAnimation()
                    {
                        To = new Thickness(-1 * (Index - 1) * ActualWidth, 0, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut },
                    };
                    StkMain.BeginAnimation(MarginProperty, anima);
                }
                else
                {
                    var anima = new ThicknessAnimation()
                    {
                        To = new Thickness(0, -1 * (Index - 1) * ActualHeight, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut },
                    };
                    StkMain.BeginAnimation(MarginProperty, anima);
                }
            }
        }
        #endregion

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            Index--;
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            Index++;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
            ChangeIndex(true);
        }
    }
}
