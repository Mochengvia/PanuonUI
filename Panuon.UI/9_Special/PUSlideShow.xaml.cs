using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Content = PART_Host.Children;
        }

        #region Property

        public new UIElementCollection Content
        {
            get { return (UIElementCollection)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElementCollection), typeof(PUSlideShow), new PropertyMetadata(OnItemsChanged));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            if (!slideShow.IsLoaded || slideShow.Content == null)
                return;
            slideShow.Draw();
        }

        /// <summary>
        /// 索引，表示当前的位置。
        /// <para>当你试图将Index的值设置为大于Content数量上限 - 1或小于0的数字时，Index会被重设为Content的数量 - 1 或0
        /// （若Recyclable为True，则会被重设为0或Content的数量 - 1）。</para>
        /// </summary>
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(PUSlideShow), new PropertyMetadata(0, OnIndexChanged));

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slideShow = d as PUSlideShow;
            if (!slideShow.IsLoaded || slideShow.Content == null)
                return;
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
            slideShow.PART_Host.Orientation = slideShow.SlideDirection;
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
            DependencyProperty.Register("AnimationDuration", typeof(int), typeof(PUSlideShow), new PropertyMetadata(500));



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


        #endregion


        #region APIs
        private void Draw()
        {
            foreach (var item in Content)
            {
                var grid = item as FrameworkElement;
                grid.Width = ActualWidth;
                grid.Height = ActualHeight;
            }
        }

        private void ChangeIndex(bool isFirstSet)
        {
            if (Index < 0)
            {
                if (!Recyclable)
                    Index = 0;
                else
                    Index = Content.Count - 1;
            }
            else if (Index > Content.Count - 1)
            {
                if (!Recyclable)
                    Index = Content.Count - 1;
                else
                    Index = 0;
            }

            if (Index == 0 && !Recyclable)
                BtnLeft.IsEnabled = false;
            else
                BtnLeft.IsEnabled = true;

            if (Index == Content.Count - 1 && !Recyclable)
                BtnRight.IsEnabled = false;
            else
                BtnRight.IsEnabled = true;


            if (isFirstSet || AnimationDuration == 0)
            {
                if (SlideDirection == Orientation.Horizontal)
                    PART_Host.Margin = new Thickness(-1 * Index * ActualWidth, 0, 0, 0);
                else
                    PART_Host.Margin = new Thickness(0, -1 * Index * ActualHeight, 0, 0);
            }
            else
            {
                if (SlideDirection == Orientation.Horizontal)
                {
                    var anima = new ThicknessAnimation()
                    {
                        To = new Thickness(-1 * Index * ActualWidth, 0, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut },
                    };
                    PART_Host.BeginAnimation(MarginProperty, anima);
                }
                else
                {
                    var anima = new ThicknessAnimation()
                    {
                        To = new Thickness(0, -1 * Index * ActualHeight, 0, 0),
                        Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut },
                    };
                    PART_Host.BeginAnimation(MarginProperty, anima);
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
