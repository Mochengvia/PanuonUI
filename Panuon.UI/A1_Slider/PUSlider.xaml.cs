using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panuon.UI
{
    /// <summary>
    /// PUSlider.xaml 的交互逻辑
    /// </summary>
    public partial class PUSlider : UserControl
    {
        private double _delta = 0.0;
        private double _totalWidth = 0.0;
        public PUSlider()
        {
            InitializeComponent();
            Foreground = new SolidColorBrush(Colors.LightGray);
            Loaded += delegate
            {
                RecheckSlideBar();
            };
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var left = 0.0;
            if (e.HorizontalChange >= _delta/2)
            {
                var per = (int)(e.HorizontalChange / _delta) + 1;
                left = Canvas.GetLeft(tmbToggle) + _delta * per;
                if (Value < Maximuim - per)
                    Value += per;
                else
                        Value = Maximuim;

                if (left > canvas.ActualWidth - tmbToggle.ActualWidth)
                {
                    left = canvas.ActualWidth - tmbToggle.ActualWidth;
                    Value = Maximuim;
                }
            }
            else if (e.HorizontalChange <= -_delta/2)
            {
                var per = (int)(e.HorizontalChange / -_delta) + 1;

                left = Canvas.GetLeft(tmbToggle) - _delta * per;
                if(Value > Minimuim + per)
                    Value -= per;
                else
                    Value = Minimuim;

                if (left < 0)
                {
                    left = 0;
                    Value = Minimuim;
                }
            }
            else
                return;
            bdrCover.Width = left;
            Canvas.SetLeft(tmbToggle, left);
        }

        #region RoutedEvent
        /// <summary>
        /// 进度改变事件。
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(PUSlider));
        public event RoutedPropertyChangedEventHandler<int> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        internal void OnValueChanged(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, ValueChangedEvent);
            RaiseEvent(arg);
        }
        #endregion

        #region Property
        /// <summary>
        /// 获取或设置滑块覆盖区域（左侧）的颜色。默认值为#696969。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUSlider), new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        /// <summary>
        /// 获取或设置滑块的最大值。默认值为100。
        /// </summary>
        public int Maximuim
        {
            get { return (int)GetValue(MaximuimProperty); }
            set { SetValue(MaximuimProperty, value); }
        }

        public static readonly DependencyProperty MaximuimProperty =
            DependencyProperty.Register("Maximuim", typeof(int), typeof(PUSlider), new PropertyMetadata(100, OnValuesChanged));

        /// <summary>
        /// 获取或设置滑块的最小值。默认值为0。
        /// </summary>
        public int Minimuim
        {
            get { return (int)GetValue(MinimuimProperty); }
            set { SetValue(MinimuimProperty, value); }
        }

        public static readonly DependencyProperty MinimuimProperty =
            DependencyProperty.Register("Minimuim", typeof(int), typeof(PUSlider), new PropertyMetadata(0, OnValuesChanged));

        /// <summary>
        /// 获取或设置滑块当前选择的值。默认值为0。
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(PUSlider), new PropertyMetadata(0,OnValuesChanged));

        private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as PUSlider;
            if (!slider.IsLoaded)
                return;
            if (slider.RecheckSlideBar())
            {
                slider.OnValueChanged((int)e.OldValue, (int)e.NewValue);
            }
        }

        #endregion
        private bool RecheckSlideBar()
        {
            if (Value < Minimuim)
            {
                Value = Minimuim;
                return false;
            }
            if (Value > Maximuim)
            {
                Value = Maximuim;
                return false;
            }

            canvas.Width = this.ActualWidth;
            _totalWidth = (canvas.ActualWidth - tmbToggle.ActualWidth);
            _delta = _totalWidth / (Maximuim - Minimuim);
            bdrCover.Width = (Value - Minimuim) * _delta;
            Canvas.SetLeft(tmbToggle, (Value - Minimuim) * _delta);
            return true;
        }

        private void slider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Minimuim > Maximuim)
                Minimuim = Maximuim - 1;

            RecheckSlideBar();
        }
    }
}
