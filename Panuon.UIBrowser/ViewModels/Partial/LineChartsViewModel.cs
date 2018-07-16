using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Panuon.UIBrowser.ViewModels.Partial
{
    public class LineChartsViewModel : Screen, IShell
    {
        private int _currentQuantity = 6;
        private int _currentMode = 0;
        private int _currentColor = 0;
        public LineChartsViewModel()
        {
            XAxis = new string[] { "1", "2", "3", "4", "5", "6" };
            YAxis = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            ValueTips = new string[] { "1", "8", "4", "9", "6", "2" };
            Values = new double[] { 0.1, 0.8, 0.4, 0.9, 0.6, 0.2 };
        }

        #region Bindings
        public string[] XAxis
        {
            get { return _xAxis; }
            set { _xAxis = value; NotifyOfPropertyChange(() => XAxis); }
        }
        private string[] _xAxis;

        public string[] YAxis
        {
            get { return _yAxis; }
            set { _yAxis = value; NotifyOfPropertyChange(() => YAxis); }
        }
        private string[] _yAxis;

        public double[] Values
        {
            get { return _values; }
            set { _values = value; NotifyOfPropertyChange(() => Values); }
        }
        private double[] _values;

        public string[] ValueTips
        {
            get { return _valueTips; }
            set { _valueTips = value; NotifyOfPropertyChange(() => ValueTips); }
        }
        private string[] _valueTips;

        public Brush AreaBrush
        {
            get { return _areaBrush; }
            set { _areaBrush = value; NotifyOfPropertyChange(() => AreaBrush); }
        }
        private Brush _areaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAAAAAAA"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#22AAAAAA"), Offset = 1 } }, 90);

        public Brush AxisBrush
        {
            get { return _axisBrush; }
            set { _axisBrush = value; NotifyOfPropertyChange(() => AxisBrush); }
        }
        private Brush _axisBrush = new SolidColorBrush(Colors.DimGray);

        public Brush LineBrush
        {
            get { return _lineBrush; }
            set { _lineBrush = value; NotifyOfPropertyChange(() => LineBrush); }
        }
        private Brush _lineBrush = new SolidColorBrush(Colors.Gray);

        public bool UsingAnimation
        {
            get { return _usingAnimation; }
            set { _usingAnimation = value; NotifyOfPropertyChange(() => UsingAnimation); }
        }
        private bool _usingAnimation = true;


        #endregion

        #region Event
        public void RandomValue()
        {
            if (_currentMode == 0)
            {
                var array = new double[_currentQuantity];
                var tipArray = new string[_currentQuantity];
                for (int i = 0; i < _currentQuantity; i++)
                {
                    array[i] = GetRandomDecimal1();
                    tipArray[i] = (array[i] * 10).ToString("f2");
                }
                ValueTips = (string[])tipArray.Clone();
                Values = (double[])array.Clone();
            }
            else if (_currentMode == 1)
            {
                var temp = new string[] { "等级一", "等级二", "等级三", "等级四", "等级五", "等级六" };
                var array = new double[_currentQuantity];
                var tipArray = new string[_currentQuantity];

                for (int i = 0; i < _currentQuantity; i++)
                {
                    array[i] = GetRandomDecimal2();
                    tipArray[i] = temp[((int)(array[i] * 5))];
                }
                ValueTips = (string[])tipArray.Clone();
                Values = (double[])array.Clone();
            }

        }

        public void ChangeAxis()
        {
            if (_currentMode == 0)
            {
                _currentQuantity = 7;
                XAxis = new string[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天" };
                YAxis = new string[] { "等级一", "等级二", "等级三", "等级四", "等级五", "等级六" };
                _currentMode = 1;
                RandomValue();
            }
            else if (_currentMode == 1)
            {
                _currentQuantity = 6;
                XAxis = new string[] { "1", "2", "3", "4", "5", "6" };
                YAxis = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
                _currentMode = 0;
                RandomValue();
            }
        }

        public void ChangeColor()
        {
            switch (_currentColor)
            {
                case 0:
                    AxisBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF49A9C0"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AA49A9C0"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#5549A9C0"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF49A9C0"));
                    _currentColor = 1;
                    break;
                case 1:
                    AxisBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4A758"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAF4A758"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#55F4A758"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4A758"));
                    _currentColor = 2;
                    break;
                case 2:
                    AxisBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E089B8"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAE089B8"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#55E089B8"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E089B8"));
                    _currentColor = 3;
                    break;
                case 3:
                    AxisBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"));
                    AreaBrush = new LinearGradientBrush(new GradientStopCollection() { new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#AAAAAAAA"), Offset = 0 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#55DDDDDD"), Offset = 1 } }, 90);
                    LineBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AAAAAA"));
                    _currentColor = 0;
                    break;
            }
        }

        public void AnimationSwitch()
        {
            UsingAnimation = !UsingAnimation;
        }
        #endregion

        #region Function
        Random _rand;
        private double GetRandomDecimal1()
        {
            if (_rand == null)
                _rand = new Random(DateTime.Now.Millisecond);
            return _rand.NextDouble();
        }

        private double GetRandomDecimal2()
        {
            var array = new double[6] { 0, 0.2, 0.4, 0.6, 0.8, 1 };
            if (_rand == null)
                _rand = new Random(DateTime.Now.Millisecond);
            return array[_rand.Next(0, 6)];
        }
        #endregion
    }
}
