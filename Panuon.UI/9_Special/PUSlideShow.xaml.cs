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
        #region Identity
        private bool isFirstTime = true;
        #endregion

        public PUSlideShow()
        {
            InitializeComponent();
            Content = PART_STKMAIN.Children;
            PART_INDICATOR.IndexChanged += delegate
            {
                Index = PART_INDICATOR.Index;
            };

            if (Index == 1 && !Recyclable)
                PART_BTNLEFT.IsEnabled = false;
            else
                PART_BTNLEFT.IsEnabled = true;

            if (Index == Content.Count && !Recyclable)
                PART_BTNRIGHT.IsEnabled = false;
            else
                PART_BTNRIGHT.IsEnabled = true;

            PART_INDICATOR.AnimationDuration = AnimationDuration;
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
            slideShow.PART_INDICATOR.TotalIndex = slideShow.Content.Count;
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
            slideShow.PART_INDICATOR.Index = slideShow.Index;
            slideShow.ChangeIndex();
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
            slideShow.PART_STKMAIN.Orientation = slideShow.SlideDirection;
            if (slideShow.SlideDirection == Orientation.Horizontal)
            {
                slideShow.PART_BTNLEFT.Content = "";
                slideShow.PART_BTNRIGHT.Content = "";
                slideShow.PART_BTNLEFT.VerticalAlignment = VerticalAlignment.Center;
                slideShow.PART_BTNLEFT.HorizontalAlignment = HorizontalAlignment.Left;
                slideShow.PART_BTNRIGHT.VerticalAlignment = VerticalAlignment.Center;
                slideShow.PART_BTNRIGHT.HorizontalAlignment = HorizontalAlignment.Right;
                slideShow.PART_BTNLEFT.Margin = new Thickness(20, 0, 0, 0);
                slideShow.PART_BTNRIGHT.Margin = new Thickness(0, 0, 20, 0);
                slideShow.PART_INDICATOR.Dircetion = Orientation.Horizontal;
                slideShow.PART_INDICATOR.Height = 15;
                slideShow.PART_INDICATOR.Width = double.NaN;
                slideShow.PART_INDICATOR.VerticalAlignment = VerticalAlignment.Bottom;
                slideShow.PART_INDICATOR.HorizontalAlignment = HorizontalAlignment.Center;
                slideShow.PART_INDICATOR.Margin = new Thickness(0, 0, 0, 20);
            }
            else
            {
                slideShow.PART_BTNLEFT.Content = "";
                slideShow.PART_BTNRIGHT.Content = "";
                slideShow.PART_BTNLEFT.VerticalAlignment = VerticalAlignment.Top;
                slideShow.PART_BTNLEFT.HorizontalAlignment = HorizontalAlignment.Center;
                slideShow.PART_BTNRIGHT.VerticalAlignment = VerticalAlignment.Bottom;
                slideShow.PART_BTNRIGHT.HorizontalAlignment = HorizontalAlignment.Center;
                slideShow.PART_BTNLEFT.Margin = new Thickness(0, 20, 0, 0);
                slideShow.PART_BTNRIGHT.Margin = new Thickness(0, 0, 0, 20);
                slideShow.PART_INDICATOR.Dircetion = Orientation.Vertical;
                slideShow.PART_INDICATOR.Height = double.NaN;
                slideShow.PART_INDICATOR.Width =15 ;
                slideShow.PART_INDICATOR.VerticalAlignment = VerticalAlignment.Center;
                slideShow.PART_INDICATOR.HorizontalAlignment = HorizontalAlignment.Left;
                slideShow.PART_INDICATOR.Margin = new Thickness(20, 0, 0, 0);
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
            DependencyProperty.Register("IsSlideButtonShow", typeof(bool), typeof(PUSlideShow), new PropertyMetadata(true, OnIsSlideButtonShowChanged));

        private static void OnIsSlideButtonShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            if (slideShow.IsSlideButtonShow)
            {
                slideShow.PART_BTNLEFT.Visibility = Visibility.Visible;
                slideShow.PART_BTNRIGHT.Visibility = Visibility.Visible;
            }
            else
            {
                slideShow.PART_BTNLEFT.Visibility = Visibility.Collapsed;
                slideShow.PART_BTNRIGHT.Visibility = Visibility.Collapsed;
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
                slideShow.PART_BTNLEFT.Foreground = slideShow.SlideButtonBrush;
                slideShow.PART_BTNLEFT.Background = slideShow.SlideButtonBrush;
                slideShow.PART_BTNLEFT.CoverBrush = slideShow.SlideButtonBrush;
                slideShow.PART_BTNRIGHT.Foreground = slideShow.SlideButtonBrush;
                slideShow.PART_BTNRIGHT.Background = slideShow.SlideButtonBrush;
                slideShow.PART_BTNRIGHT.CoverBrush = slideShow.SlideButtonBrush;
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
            slideShow.PART_INDICATOR.AnimationDuration = slideShow.AnimationDuration;
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
                slide.PART_INDICATOR.Visibility = Visibility.Visible;
            else
                slide.PART_INDICATOR.Visibility = Visibility.Hidden;
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
            slideShow.PART_INDICATOR.BorderBrush = slideShow.IndicatorBrush;
            slideShow.PART_INDICATOR.CoverBrush = slideShow.IndicatorBrush;
        }
        #endregion

        #region APIs
        private void Draw()
        {
            PART_INDICATOR.TotalIndex = Content.Count;
            foreach (var item in Content)
            {
                var grid = item as FrameworkElement;
                grid.Width = ActualWidth;
                grid.Height = ActualHeight;
            }
        }

        private void ChangeIndex()
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
                PART_BTNLEFT.IsEnabled = false;
            else
                PART_BTNLEFT.IsEnabled = true;

            if (Index == Content.Count && !Recyclable)
                PART_BTNRIGHT.IsEnabled = false;
            else
                PART_BTNRIGHT.IsEnabled = true;


            if (isFirstTime || AnimationDuration == 0)
            {
                if (SlideDirection == Orientation.Horizontal)
                    PART_STKMAIN.Margin = new Thickness(-1 * (Index - 1) * ActualWidth, 0, 0, 0);
                else
                    PART_STKMAIN.Margin = new Thickness(0, -1 * (Index - 1) * ActualHeight, 0, 0);
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
                    PART_STKMAIN.BeginAnimation(MarginProperty, anima);
                }
                else
                {
                    var anima = new ThicknessAnimation()
                    {
                        To = new Thickness(0, -1 * (Index - 1) * ActualHeight, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut },
                    };
                    PART_STKMAIN.BeginAnimation(MarginProperty, anima);
                }
            }
            isFirstTime = false;
        }
        #endregion

        #region Sys
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
            ChangeIndex();
        }
        #endregion
    }
}
