using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        #region Property
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }

        public static readonly DependencyProperty CoverBrushProperty =
            DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(PUSlider), new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));



        public int Maximuim
        {
            get { return (int)GetValue(MaximuimProperty); }
            set { SetValue(MaximuimProperty, value); }
        }

        public static readonly DependencyProperty MaximuimProperty =
            DependencyProperty.Register("Maximuim", typeof(int), typeof(PUSlider), new PropertyMetadata(100));



        public int Minimuim
        {
            get { return (int)GetValue(MinimuimProperty); }
            set { SetValue(MinimuimProperty, value); }
        }

        public static readonly DependencyProperty MinimuimProperty =
            DependencyProperty.Register("Minimuim", typeof(int), typeof(PUSlider), new PropertyMetadata(0));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(PUSlider), new PropertyMetadata(0,OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as PUSlider;
            if (!slider.IsLoaded)
                return;
            if(slider.Value < slider.Minimuim)
            {
                slider.Value = slider.Minimuim;
                return;
            }
            if(slider.Value > slider.Maximuim)
            {
                slider.Value = slider.Maximuim;
                return;
            }
            slider.bdrCover.Width = slider.Value * slider._delta;
            Canvas.SetLeft(slider.tmbToggle, slider.Value * slider._delta);
        }


        #endregion

        private void slider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Minimuim > Maximuim)
                Minimuim = Maximuim - 1;

            canvas.Width = this.ActualWidth;
            _totalWidth = (canvas.ActualWidth - tmbToggle.ActualWidth);
            _delta = _totalWidth / (Maximuim - Minimuim);

            Canvas.SetLeft(tmbToggle, _delta * Value);
            bdrCover.Width = _delta * Value;
        }
    }
}
